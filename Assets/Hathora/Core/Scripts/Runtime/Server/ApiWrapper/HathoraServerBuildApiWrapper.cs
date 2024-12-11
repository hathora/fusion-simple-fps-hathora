// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;
using HathoraCloud.Utils;
using UnityEngine;
using UnityEngine.Networking;
using File = System.IO.File;

namespace Hathora.Core.Scripts.Runtime.Server.ApiWrapper
{
    /// <summary>
    /// Operations that allow you create and manage your builds.
    /// Build Concept | https://hathora.dev/docs/concepts/hathora-entities#build
    /// API Docs | https://hathora.dev/api#tag/BuildV1
    /// </summary>
    public class HathoraServerBuildApiWrapper : HathoraServerApiWrapperBase
    {
        protected IBuildsV3 BuildApi { get; }
        private volatile bool uploading;
        private volatile bool runningBuild;

        public HathoraServerBuildApiWrapper(
            HathoraCloudSDK _hathoraSdk,
            HathoraServerConfig _hathoraServerConfig)
            : base(_hathoraSdk, _hathoraServerConfig)
        {
            Debug.Log($"[{nameof(HathoraServerBuildApiWrapper)}.Constructor] " +
                "Initializing Server API...");
            
            this.BuildApi = _hathoraSdk.BuildsV3;
        }
        
        
        #region Server Build Async Hathora SDK Calls
        /// <summary>
        /// Wrapper for `CreateBuildAsync` to request an cloud build (_tarball upload).
        /// </summary>
        /// <param name="_buildTag">
        /// Build tag to associate a version with a build. It is accessible via getBuildInfo().
        /// </param>
        /// <param name="_cancelToken">TODO</param>
        /// <returns>Returns Build on success >> Pass this info to RunCloudBuildAsync()</returns>
        public async Task<CreatedBuildV3WithMultipartUrls> CreateBuildAsync(
            double _buildSizeBytes,
            string _buildTag = null,
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerBuildApiWrapper)}.{nameof(CreateBuildAsync)}]";
            
            // Prep request
            CreateMultipartBuildParams createBuildParams = new()
            {
                BuildTag = _buildTag,
                BuildSizeInBytes = _buildSizeBytes,
            };

            CreateBuildRequest createBuildRequestWrapper = new()
            {
                CreateMultipartBuildParams = createBuildParams,
            };
            
            // Get response async =>
            CreateBuildResponse createBuildResponse = null;
            
            try
            {
                createBuildResponse = await BuildApi.CreateBuildAsync(createBuildRequestWrapper);
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(BuildApi.CreateBuildAsync)} => Error: {e.Message}");
                return null; // fail
            }

            Debug.Log($"{logPrefix} Success: <color=yellow>{nameof(createBuildResponse)}: " +
                $"{ToJson(createBuildResponse.StatusCode)}</color>");
            
            createBuildResponse.RawResponse?.Dispose(); // Prevent mem leaks
            return createBuildResponse.CreatedBuildV3WithMultipartUrls;
        }

        /// <summary>
        /// Wrapper for `RunBuildAsync` to upload the _tarball after calling CreateBuildAsync().
        /// (!) Temporarily sets the Timeout to 15min (900k ms) to allow for large builds.
        /// (!) After this is done, you probably want to call GetBuildInfoAsync().
        /// </summary>
        /// <param name="_buildId"></param>
        /// <param name="_createBuildResponse">Response from CreateBuild request</param>
        /// <param name="_pathToTarGzBuildFile">Ensure path is normalized</param>
        /// <param name="_cancelToken">TODO</param>
        /// <returns>Returns streamLogs (List of chunks) on success</returns>
        public async Task<List<string>> UploadAndRunCloudBuildAsync(
            string _buildId, 
            CreatedBuildV3WithMultipartUrls _createBuildResponse,
            string filePath,
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerBuildApiWrapper)}.{nameof(UploadAndRunCloudBuildAsync)}]";
            
            // Multipart upload requests
            try
            {
                Debug.Log("Starting multipart upload");
                uploading = true;
                _ = startUploadProgressNoticeAsync(); // !await
                await UploadToMultipartUrl(
                    _createBuildResponse.UploadParts,
                    _createBuildResponse.MaxChunkSize,
                    _createBuildResponse.CompleteUploadPostRequestUrl,
                    filePath
                );
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(UploadToMultipartUrl)} => Error: {e.Message}");
                return null; // fail
            }
            finally
            {
                uploading = false;
            }
            
            RunBuildRequest runBuildRequest = new()
            {
                BuildId = _buildId
            };
                
            // Get response async =>
            RunBuildResponse runBuildResponse = null;
            runningBuild = true;

            try
            {
                _ = startRunBuildProgressNoticeAsync(); // !await
                runBuildResponse = await BuildApi.RunBuildAsync(runBuildRequest);
            }
            catch (TaskCanceledException)
            {
                Debug.Log($"{logPrefix} Task Cancelled || timed out");
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(BuildApi.RunBuildAsync)} => Error: {e.Message}");
                return null; // fail
            }
            finally
            {
                runningBuild = false;
            }

            Debug.Log($"{logPrefix} Done - to know if success, call BuildApi.RunBuild " +
                "(or see `HathoraServerConfig` logs at bottom)");

            // (!) Unity, by default, truncates logs to 1k chars (callstack-inclusive).
            string logs = runBuildResponse?.RawResponse.downloadHandler.text;

            if (string.IsNullOrEmpty(logs))
            {
                Debug.LogError($"{logPrefix} Error: Expected {nameof(runBuildResponse)} response text");
                return null;
            }
            
            List<string> logChunks = onRunCloudBuildDone(logs);
            
            runBuildResponse?.RawResponse?.Dispose(); // Prevent mem leaks
            return logChunks;  // streamLogs 
        }

        private async Task startUploadProgressNoticeAsync()
        {
            TimeSpan delayTimespan = TimeSpan.FromSeconds(5);
            StringBuilder sb = new("...");
            
            while (uploading)
            {
                Debug.Log($"[HathoraServerBuild] Uploading {sb}");
                
                await Task.Delay(delayTimespan);
                sb.Append(".");
            }
        }
        private async Task startRunBuildProgressNoticeAsync()
        {
            TimeSpan delayTimespan = TimeSpan.FromSeconds(5);
            StringBuilder sb = new("...");
            
            while (runningBuild)
            {
                Debug.Log($"[HathoraServerBuild] Running build {sb}");
                
                await Task.Delay(delayTimespan);
                sb.Append(".");
            }
        }

        /// <summary>
        /// DONE - not necessarily success. Log _stream every 500 lines
        /// (!) Unity, by default, truncates logs to 1k chars (including callstack).
        /// </summary>
        /// <param name="_cloudRunBuildResultLogsStr"></param>
        /// <returns>List of log chunks</returns>
        private static List<string> onRunCloudBuildDone(string _cloudRunBuildResultLogsStr)
        {
            // Split string into lines
            string[] linesArr = _cloudRunBuildResultLogsStr.Split(new[] 
                { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            List<string> lines = new (linesArr);

            // Group lines into chunks of 500
            const int chunkSize = 500;
            for (int i = 0; i < lines.Count; i += chunkSize)
            {
                IEnumerable<string> chunk = lines.Skip(i).Take(chunkSize);
                string chunkStr = string.Join("\n", chunk);
                Debug.Log($"[HathoraServerBuildApiWrapper.onRunCloudBuildDone - RunBuild logs] result == chunk starting at line {i}: " +
                    $"\n<color=yellow>{chunkStr}</color>");
            }

            return lines;
        }

        /// <summary>
        /// Wrapper for `RunBuildAsync` to upload the _tarball after calling 
        /// </summary>
        /// <param name="_buildId"></param>
        /// <param name="_cancelToken">TODO</param>
        /// <returns>Returns byte[] on success</returns>
        public async Task<BuildV3> GetBuildInfoAsync(
            string _buildId,
            CancellationToken _cancelToken)
        {
            string logPrefix = $"[{nameof(HathoraServerBuildApiWrapper)}.{nameof(GetBuildInfoAsync)}]";

            // Prepare request
            GetBuildRequest getBuildInfoRequest = new()
            {
                BuildId = _buildId,
            };
            
            // Get response async =>
            GetBuildResponse getBuildInfoResponse = null;
            
            try
            {
                getBuildInfoResponse = await BuildApi.GetBuildAsync(getBuildInfoRequest);
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(BuildApi.GetBuildAsync)} => Error: {e.Message}");
                return null; // fail
            }

            BuildV3 build = getBuildInfoResponse.BuildV3;
            bool isSuccess = build is { Status: BuildStatus.Succeeded };
            
            Debug.Log($"{logPrefix} Success? {isSuccess}, <color=yellow>" +
                $"{nameof(getBuildInfoResponse)}: {ToJson(getBuildInfoResponse.BuildV3)}</color>");

            getBuildInfoResponse.RawResponse?.Dispose(); // Prevent mem leaks
            return build;
        }
        #endregion // Server Build Async Hathora SDK Calls
        
        
        #region Utils

        public async Task UploadToMultipartUrl(List<BuildPart> multipartUploadParts, double maxChunkSize, string completeUploadPostRequestUrl, string filePath)
        {
            Debug.Log($"~~~~~~{multipartUploadParts.Count} parts, maxChunkSize: {maxChunkSize}");
            string logPrefix = $"[{nameof(HathoraServerBuildApiWrapper)}.{nameof(UploadToMultipartUrl)}]";
            try
            {
                List<Task<PartResult>> uploadTasks = new List<Task<PartResult>>();

                foreach (var part in multipartUploadParts)
                {
                    uploadTasks.Add(UploadPart(part, filePath, maxChunkSize));
                }

                var uploadedParts = await Task.WhenAll(uploadTasks);

                string xmlParts = "";
                foreach (var part in uploadedParts)
                {
                    xmlParts += $"<Part><PartNumber>{part.PartNumber}</PartNumber><ETag>{part.ETag}</ETag></Part>";
                }

                string xmlBody = $"<CompleteMultipartUpload>{xmlParts}</CompleteMultipartUpload>";
                byte[] xmlBodyBytes = Encoding.UTF8.GetBytes(xmlBody);

                using (UnityWebRequest postRequest = new UnityWebRequest(completeUploadPostRequestUrl, "POST"))
                {
                    postRequest.uploadHandler = new UploadHandlerRaw(xmlBodyBytes);
                    postRequest.downloadHandler = new DownloadHandlerBuffer();
                    postRequest.SetRequestHeader("Content-Type", "application/xml");

                    await postRequest.SendWebRequest();

                    if (postRequest.result == UnityWebRequest.Result.Success)
                    {
                        Debug.Log($"{logPrefix} Upload fully complete with status: {postRequest.responseCode}");
                    }
                    else
                    {
                        Debug.LogError($"{logPrefix} Error completing multipart upload. Status code: {postRequest.responseCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"{logPrefix} Error occurred uploading file: {ex.Message}");
            }
        }

        private async Task<PartResult> UploadPart(BuildPart part, string filePath, double maxChunkSize)
        {
            string logPrefix = $"[{nameof(HathoraServerBuildApiWrapper)}.{nameof(UploadToMultipartUrl)}]";
            try
            {
                double startByteForPart = (part.PartNumber - 1) * maxChunkSize;
                double endByteForPart = Math.Min(part.PartNumber * maxChunkSize, new FileInfo(filePath).Length);
                double partSize = endByteForPart - startByteForPart;
                byte[] fileChunk = new byte[(int)partSize];
                
                Debug.Log($"~~~~~{part.PartNumber} parts, chunkSize: {partSize}, arrayMaxLength: {fileChunk.Length}");
                // Open the file and read the specific chunk
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    fileStream.Seek((long)startByteForPart, SeekOrigin.Begin);
                    int bytesRead = await fileStream.ReadAsync(fileChunk, 0, (int)partSize);

                    if (bytesRead != (int)partSize)
                    {
                        throw new Exception($"Failed to read the expected number of bytes for part {part.PartNumber}");
                    }
                }

                using (UnityWebRequest putRequest = new (part.PutRequestUrl, UnityWebRequest.kHttpVerbPUT))
                {
                    putRequest.uploadHandler = new UploadHandlerRaw(fileChunk);
                    putRequest.SetRequestHeader("Content-Type", "application/octet-stream");

                    await putRequest.SendWebRequest();

                    if (putRequest.result != UnityWebRequest.Result.Success)
                    {
                        throw new Exception($"Failed to upload part {part.PartNumber}, status: {putRequest.responseCode}");
                    }

                    string eTag = putRequest.GetResponseHeader("ETag");
                    if (string.IsNullOrEmpty(eTag))
                    {
                        throw new Exception($"ETag not found in response headers for part {part.PartNumber}");
                    }

                    Debug.Log($"{logPrefix} Upload part {part.PartNumber} complete with status: {putRequest.responseCode}");
                    return new PartResult { ETag = eTag, PartNumber = part.PartNumber };
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"{logPrefix} Error uploading part {part.PartNumber}: {ex.Message}");
                throw;
            }
        }

        public class PartResult
        {
            public string ETag { get; set; }
            public double PartNumber { get; set; }
        }
        private static byte[] toByteArray(Stream _stream)
        {
            _stream.Position = 0;
            byte[] buffer = new byte[_stream.Length];
            
            for (int totalBytesCopied = 0; totalBytesCopied < _stream.Length;)
                totalBytesCopied += _stream.Read(
                    buffer, 
                    offset: totalBytesCopied, 
                    count: Convert.ToInt32(_stream.Length) - totalBytesCopied);
            
            return buffer;
        }
        #endregion // Utils
    }
}
