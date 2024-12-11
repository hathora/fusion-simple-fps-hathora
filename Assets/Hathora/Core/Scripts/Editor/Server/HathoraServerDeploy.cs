// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Editor.Common;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using Hathora.Core.Scripts.Runtime.Server;
using Hathora.Core.Scripts.Runtime.Server.ApiWrapper;
using Hathora.Core.Scripts.Runtime.Server.Models;
using HathoraCloud;
using HathoraCloud.Models.Shared;
using UnityEditor;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;

namespace Hathora.Core.Scripts.Editor.Server
{
    public delegate void ZipCompleteHandler();
    public delegate void OnBuildReqComplete(CreatedBuildV3WithMultipartUrls _buildInfo);
    public delegate void OnUploadComplete();

    /// <summary>
    /// Deployment wrapper using a chain of events, gathering data from HathoraServerConfig.
    /// </summary>
    public static class HathoraServerDeploy
    {
        /// <summary>
        /// This nees to be a massive timeout, but still have a timeout to prevent mem leaks.
        /// </summary>
        public const int DEPLOY_TIMEOUT_MINS = 30;

        public static bool IsDeploying =>
            DeploymentStep != DeploymentSteps.Done;

        public enum DeploymentSteps
        {
            Done, // Same as not deployment

            // Init, // Too fast to track
            Zipping,
            RequestingUploadPerm,
            Uploading,
            Deploying,
        }

        private static int maxDeploySteps =>
            Enum.GetValues(typeof(DeploymentSteps)).Length - 1; // Exclude "Done"

        public static DeploymentSteps DeploymentStep { get; private set; }

        /// <summary>TODO: Rename to `OnZipComplete` to conform with sibling styles</summary>
        public static event ZipCompleteHandler OnZipComplete;

        public static event OnBuildReqComplete OnBuildReqComplete;
        public static event OnUploadComplete OnUploadComplete;


        /// <summary>
        /// eg: "(1/4) Zipping...", where (1/4) would be colored.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string GetDeployFriendlyStatus() => DeploymentStep switch
        {
            DeploymentSteps.Done => "Done",

            DeploymentSteps.Zipping => $"{HathoraEditorUtils.StartGreenColor}" +
                $"({(int)DeploymentSteps.Zipping}/{maxDeploySteps})</color> Zipping...",

            DeploymentSteps.RequestingUploadPerm => $"{HathoraEditorUtils.StartGreenColor}" +
                $"({(int)DeploymentSteps.RequestingUploadPerm}/{maxDeploySteps})</color> Requesting Upload Permission...",

            DeploymentSteps.Uploading => $"{HathoraEditorUtils.StartGreenColor}" +
                $"({(int)DeploymentSteps.Uploading}/{maxDeploySteps})</color> Uploading Build...",

            DeploymentSteps.Deploying => $"{HathoraEditorUtils.StartGreenColor}" +
                $"({(int)DeploymentSteps.Deploying}/{maxDeploySteps})</color> Deploying Build...",

            _ => throw new ArgumentOutOfRangeException(),
        };

        /// <summary>
        /// Deploys with HathoraServerConfig opts. Optionally sub to events:
        /// - OnZipComplete
        /// - OnBuildReqComplete
        /// - OnUploadComplete
        /// </summary>
        /// <param name="_serverConfig">Find via menu `Hathora/Find UserConfig(s)`</param>
        /// <param name="_cancelToken"></param>
        public static async Task<DeploymentV3> DeployToHathoraAsync(
            HathoraServerConfig _serverConfig,
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerDeploy)}.{nameof(HathoraServerDeploy)}]";

            // Prep logs cache
            Debug.Log($"{logPrefix} <color=yellow>Starting...</color>");

            Assert.IsNotNull(_serverConfig, $"{logPrefix} " +
                "Cannot find HathoraServerConfig ScriptableObject");

            #if UNITY_WEBGL
            bool isTcpTransport = _serverConfig.HathoraDeployOpts.TransportType == TransportType.Tcp;
            string selectedTransportTypeStr = Enum.GetName(typeof (TransportType), _serverConfig.HathoraDeployOpts.TransportType);
            if (!isTcpTransport)
            {
                Debug.LogWarning($"{logPrefix} (!) Do you plan for your clients to connect with WebGL (TCP/WS)? " +
                    "Your HathoraServerConfig's transport type should probably be set to `Tcp`. " +
                    $"Currently, the selected transport type is set to: `{selectedTransportTypeStr}`");
            }
            #endif

            StringBuilder strb = _serverConfig.HathoraDeployOpts.LastDeployLogsStrb;
            DateTime startTime = DateTime.Now;
            strb.Clear()
                .AppendLine($"{HathoraUtils.GetFriendlyDateTimeShortStr(startTime)} (Local Time)")
                .AppendLine("Preparing remote application deployment...")
                .AppendLine();

            try
            {
                // Prepare paths and file names that we didn't get from UserConfig  
                HathoraServerPaths serverPaths = new(_serverConfig);

                // Prepare APIs

                Security security = new()
                {
                    HathoraDevToken = _serverConfig.HathoraCoreOpts.DevAuthOpts.HathoraDevToken,
                };
                
                HathoraCloudSDK sdk = new(security, null, _serverConfig.HathoraCoreOpts.AppId);
                
                HathoraServerBuildApiWrapper buildApiWrapper = new(
                    sdk,
                    _serverConfig);

                HathoraServerDeployApiWrapper deployApiWrapper = new(
                    sdk,
                    _serverConfig);


                #region Dockerfile >> Compress to .tar.gz
                // ----------------------------------------------
                DeploymentStep = DeploymentSteps.Zipping;
                strb.AppendLine(GetDeployFriendlyStatus());

                // Compress build into .tar.gz (gzipped tarball)
                try
                {
                    await HathoraTar.ArchiveFilesAsTarGzToDotHathoraDir(
                        serverPaths,
                        _cancelToken);
                }
                catch (TaskCanceledException e)
                {
                    Debug.Log($"{logPrefix} ArchiveFilesAsTarGzToDotHathoraDir => Cancelled {e.Message}");

                    throw;
                }
                catch (Exception e)
                {
                    Debug.LogError($"{logPrefix} Error: {e.Message}");

                    throw;
                }

                OnZipComplete?.Invoke();
                #endregion // Dockerfile >> Compress to .tar.gz


                #region Request to build
                // ----------------------------------------------
                // Get all Deployments -> Get the most recent -> Use their env vars / additional ports
                List<DeploymentV3> oldDeployments = null;

                try
                {
                    oldDeployments = await deployApiWrapper.GetDeploymentsAsync(_cancelToken);
                }
                catch (TaskCanceledException e)
                {
                    Debug.Log($"{logPrefix} GetDeploymentsAsync => Cancelled {e.Message}");

                    throw;
                }
                catch (Exception e)
                {
                    Debug.LogError($"{logPrefix} {e.Message}");

                    return null;
                }

                // Get the most-recent deployments env vars + additional ports, if any
                List<DeploymentV3Env> envVars = null;
                List<ContainerPort> additionalContainerPorts = null;
                Boolean idleTimeoutEnabledSetting = true;

                if (oldDeployments?.Count > 0)
                {
                    // The order is unknown - sort by create date and get the latest one
                    DeploymentV3 lastDeployment = oldDeployments.OrderByDescending(item =>
                        item.CreatedAt).FirstOrDefault();

                    // Get the latest deployments env vars + additional ports
                    // to prevent the new deployment from overriding them.
                    envVars = lastDeployment?.Env;
                    additionalContainerPorts = lastDeployment?.AdditionalContainerPorts;
                    idleTimeoutEnabledSetting = lastDeployment?.IdleTimeoutEnabled ?? true;
                }

                // ----------------------------------------------
                // Create a Hathora build (request a buildId)
                DeploymentStep = DeploymentSteps.RequestingUploadPerm;
                strb.AppendLine(GetDeployFriendlyStatus());

                // Get a buildId from Hathora
                CreatedBuildV3WithMultipartUrls createBuildResponse = null;

                // Get file byte size
                string tarGzFileName = $"{serverPaths.ExeBuildName}.tar.gz";
                string normalizedPathToTarball = Path.GetFullPath(
                    $"{serverPaths.PathToDotHathoraDir}/{tarGzFileName}");

                if (!File.Exists(normalizedPathToTarball))
                {
                    Debug.Log($"File not found: {normalizedPathToTarball}");
                    return null;
                }

                // Get file size
                FileInfo fileInfo = new (normalizedPathToTarball);
                long fileSize = fileInfo.Length;
                // fileContents = readFileAsBinaryBlob(normalizedPathToTarball);
                Debug.Log($"'{tarGzFileName}' file read successfully. Size: " + fileSize);

                if (fileSize < 1)
                {
                    Debug.Log($"Server build file ('{tarGzFileName}') can't be empty.");

                    return null;
                }

                try
                {
                    // TODO: Ask for buildTag in parent arg - passing `null`, for now
                    createBuildResponse = await buildApiWrapper.CreateBuildAsync(
                        fileSize,
                        _buildTag: null,
                        _cancelToken);
                }
                catch (TaskCanceledException e)
                {
                    Debug.Log($"{logPrefix} CreateBuildAsync => Cancelled {e.Message}");

                    throw;
                }
                catch (Exception e)
                {
                    Debug.LogError($"{logPrefix} Error: {e.Message}");

                    return null;
                }

                Assert.IsNotNull(createBuildResponse, $"{logPrefix} Expected createBuildResponse");

                // Building seems to unselect Hathora _serverConfig on success
                HathoraServerConfigFinder.SelectLastKnownServerConfig();

                OnBuildReqComplete?.Invoke(createBuildResponse);
                _cancelToken.ThrowIfCancellationRequested();
                #endregion // Request to build


                #region Upload Build
                // ----------------------------------------------
                DeploymentStep = DeploymentSteps.Uploading;
                strb.AppendLine(GetDeployFriendlyStatus());

                // Upload the build to Hathora
                (BuildV3 build, List<string> logChunks) buildWithLogs = default;

                try
                {
                    buildWithLogs = await uploadAndVerifyBuildAsync(
                        _serverConfig,
                        buildApiWrapper,
                        createBuildResponse,
                        normalizedPathToTarball,
                        serverPaths,
                        _cancelToken);
                }
                catch (TaskCanceledException e)
                {
                    Debug.Log($"{logPrefix} uploadAndVerifyBuildAsync => Cancelled {e.Message}");

                    throw;
                }
                catch (Exception e)
                {
                    Debug.LogError($"{logPrefix} {e.Message}");

                    return null;
                }

                // Logs from server
                appendServerLogOutput(strb, buildWithLogs);

                OnUploadComplete?.Invoke();
                _cancelToken.ThrowIfCancellationRequested();
                #endregion // Upload Build


                #region Deploy Build
                // ----------------------------------------------
                // Deploy the build
                DeploymentStep = DeploymentSteps.Deploying;
                strb.AppendLine(GetDeployFriendlyStatus());

                DeploymentV3 deployment = null;

                try
                {
                    deployment = await deployApiWrapper.CreateDeploymentAsync(
                        createBuildResponse.BuildId,
                        idleTimeoutEnabledSetting,
                        envVars,
                        additionalContainerPorts,
                        _cancelToken);
                }
                catch (TaskCanceledException e)
                {
                    Debug.Log($"{logPrefix} CreateDeploymentAsync => Cancelled {e.Message}");

                    throw;
                }
                catch (Exception e)
                {
                    Debug.LogError($"{logPrefix} {e.Message}");

                    return null;
                }

                Assert.IsTrue(
                    !string.IsNullOrEmpty(deployment?.BuildId),
                    "[HathoraServerBuild.DeployToHathoraAsync] Expected deployment");
                #endregion // Deploy Build


                DeploymentStep = DeploymentSteps.Done;
                DateTime endTime = DateTime.Now;
                strb.AppendLine()
                    .Append($"{HathoraEditorUtils.StartGreenColor}Completed</color> ")
                    .Append(HathoraUtils.GetFriendlyDateTimeShortStr(endTime)) // "{date} {time}"
                    .Append(" (in ")
                    .Append(HathoraUtils.GetFriendlyDateTimeDiff( // "{hh}h:{mm}m:{ss}s"; strips 0
                        startTime,
                        endTime,
                        exclude0: true))
                    .AppendLine(")")
                    .AppendLine("DEPLOYMENT DONE");

                // #########################################################
                // {green}Completed{/green} {date} {time} (in {hh}h:{mm}m:{ss}s)
                // BUILD DONE
                // #########################################################

                return deployment;
            }
            catch (TaskCanceledException e)
            {
                Debug.Log($"{logPrefix} Cancelled {e.Message}");
                strb.AppendLine().AppendLine($"<color={HathoraEditorUtils.HATHORA_PINK_CANCEL_COLOR_HEX}>" +
                    "(!) Cancelled by user</color>");

                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {e.Message}");
                strb.AppendLine($"<color={HathoraEditorUtils.HATHORA_PINK_CANCEL_COLOR_HEX}>" +
                    $"<b>(!) Error:</b>\n{e.Message}</color>");

                throw;
            }
            finally
            {
                Debug.Log($"{logPrefix} Done");
                DeploymentStep = DeploymentSteps.Done;
            }
        }

        private static void appendServerLogOutput(
            StringBuilder _strb,
            (BuildV3 build, List<string> logChunks) _buildWithLogs)
        {
            _strb.AppendLine("<color=white>");
            _strb.AppendLine("<b>===== [Server response START] =====</b>");
            _buildWithLogs.logChunks.ForEach(
                log =>
                {
                    // Make an error stand out
                    bool hasErr = log.StartsWith("Error");
                    if (hasErr)
                        _strb.Append($"<color={HathoraEditorUtils.HATHORA_PINK_CANCEL_COLOR_HEX}>");

                    // No matter what, add the log here
                    _strb.AppendLine(log);

                    if (hasErr)
                        _strb.Append("</color>");
                });
            _strb.AppendLine("<b>===== [Server response END] =====</b>")
                .AppendLine("</color>");

            Assert.AreEqual(
                _buildWithLogs.build?.Status,
                BuildStatus.Succeeded,
                $"[{nameof(HathoraServerDeploy)}.{nameof(appendServerLogOutput)}] " +
                "buildWithLogs.build?.Status != Succeeded");
        }

        /// <summary>
        /// High-level func, running 2 Tasks:
        /// 1. RunCloudBuildAsync
        /// 2. getBuildInfo (since File streaming may have failed)
        /// </summary>
        /// <param name="_serverConfig"></param>
        /// <param name="_buildApiWrapper"></param>
        /// <param name="_buildId"></param>
        /// <param name="_serverPaths"></param>
        /// <param name="_cancelToken">Optional</param>
        /// <returns>streamingLogs</returns>
        private static async Task<(BuildV3 build, List<string> logChunks)> uploadAndVerifyBuildAsync(
            HathoraServerConfig _serverConfig,
            HathoraServerBuildApiWrapper _buildApiWrapper,
            CreatedBuildV3WithMultipartUrls _createBuildResponse,
            string normalizedPathToTarball,
            HathoraServerPaths _serverPaths,
            CancellationToken _cancelToken = default)
        {

            Debug.Log($"[HathoraServerDeploy.uploadAndVerifyBuildAsync] " +
                $"Uploading local server build to Hathora...");

            BuildV3 build = null;
            List<string> logChunks = null;

            try
            {
                logChunks = await _buildApiWrapper.UploadAndRunCloudBuildAsync(
                    _createBuildResponse.BuildId,
                    _createBuildResponse,
                    normalizedPathToTarball,
                    _cancelToken);


                Security security = new()
                {
                    HathoraDevToken = _serverConfig.HathoraCoreOpts.DevAuthOpts.HathoraDevToken,
                };
                
                HathoraServerBuildApiWrapper buildApiWrapper = new(
                    new HathoraCloudSDK(security, null, _serverConfig.HathoraCoreOpts.AppId),
                    _serverConfig);

                build = await buildApiWrapper.GetBuildInfoAsync(_createBuildResponse.BuildId, _cancelToken);
            }
            catch (TaskCanceledException e)
            {
                Debug.Log($"[HathoraServerDeploy.uploadAndVerifyBuildAsync] Cancelled {e.Message}");

                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"[HathoraServerDeploy.uploadAndVerifyBuildAsync] Error: {e.Message}");

                throw;
            }

            return (build, logChunks);
        }

        public static MemoryStream readFileAsBinaryBlob(string filePath, int chunkSize = 1073741824) // 1GB chunk size
        {
            MemoryStream memoryStream = new MemoryStream(); // The binary blob we will return
        
            byte[] buffer = new byte[chunkSize];

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, bytesRead); // Write chunk data to the memory stream
                }
            }

            memoryStream.Position = 0; // Reset the stream position for reading
            return memoryStream; // Return the binary blob as a MemoryStream
        }
    }

}