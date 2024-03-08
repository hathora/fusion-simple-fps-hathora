// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Runtime.Server.Models;
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;
using UnityEngine;

namespace Hathora.Core.Scripts.Runtime.Server.ApiWrapper
{
    /// <summary>
    /// Operations that allow you configure and manage an application's build at runtime.
    /// Build Concept | https://hathora.dev/docs/concepts/hathora-entities#build
    /// API Docs | https://hathora.dev/api#tag/DeploymentV1
    /// </summary>
    public class HathoraServerDeployApiWrapper : HathoraServerApiWrapperBase
    {
        protected DeploymentV1 DeployApi { get; }
        private HathoraDeployOpts deployOpts => HathoraServerConfig.HathoraDeployOpts;
        private volatile bool uploading;

        public HathoraServerDeployApiWrapper(
            HathoraCloudSDK _hathoraSdk,
            HathoraServerConfig _hathoraServerConfig)
            : base(_hathoraSdk, _hathoraServerConfig)
        {
            Debug.Log($"[{nameof(HathoraServerDeployApiWrapper)}.Constructor] " +
                "Initializing Server API...");
            
            this.DeployApi = _hathoraSdk.DeploymentV1 as DeploymentV1;
        }
        
        
        #region Server Deploy Async Hathora SDK Calls
        /// <summary>
        /// Wrapper for `CreateDeploymentAsync` to upload and deploy a cloud deploy to Hathora.
        /// </summary>
        /// <param name="_buildId"></param>
        /// <param name="_env">
        /// Optional - env vars. We recommend you pass the ones from your previous
        /// build, via `DeployApi.GetDeploymentsAsync()`.
        /// </param>
        /// <param name="_additionalContainerPorts">
        /// Optional - For example, you may want to expose 2 ports to support both UDP and
        /// TLS transports simultaneously (eg: FishNet's `Multipass` transport)
        /// </param>
        /// <param name="_cancelToken">TODO</param>
        /// <returns>Returns Deployment on success</returns>
        public async Task<Deployment> CreateDeploymentAsync(
            int _buildId,
            List<Env> _env = null,
            List<ContainerPort> _additionalContainerPorts = null,
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerDeployApiWrapper)}.{nameof(CreateDeploymentAsync)}]";
            
            // Prepare request
            TransportType selectedTransportType = deployOpts.TransportType;
            
            #region DeploymentEnvConfigInner Workaround
            // #######################################################################################
            // (!) Hathora SDK's DeploymentEnv is identical to DeploymentConfigEnv >> Port it over
            List<DeploymentConfigEnv> envWorkaround = _env?.Select(envVar => 
                new DeploymentConfigEnv
                {
                    Name = envVar.Name, 
                    Value = envVar.Value,
                }).ToList();
            // #######################################################################################
            #endregion DeploymentEnvConfigInner Workaround
            
            DeploymentConfig deployConfig = new()
            {
                Env = envWorkaround ?? new List<DeploymentConfigEnv>(),
                RoomsPerProcess = deployOpts.RoomsPerProcess, 
                PlanName = deployOpts.PlanName, 
                AdditionalContainerPorts = _additionalContainerPorts ?? new List<ContainerPort>(),
                TransportType = selectedTransportType,
                ContainerPort = deployOpts.ContainerPortSerializable.Port,
            };

            CreateDeploymentRequest createDeploymentRequest = new()
            {
                DeploymentConfig = deployConfig,
                BuildId = _buildId,
            };
            
            Debug.Log($"{logPrefix} <color=yellow>Request {nameof(deployConfig)}: {ToJson(deployConfig)}</color>");

            // Get response async =>
            CreateDeploymentResponse createDeploymentResponse = null;
            
            try
            {
                createDeploymentResponse = await DeployApi.CreateDeploymentAsync(createDeploymentRequest);
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(DeployApi.CreateDeploymentAsync)} => Error: {e.Message}");
                return null; // fail
            }

            Debug.Log($"{logPrefix} <color=yellow>{nameof(createDeploymentResponse.Deployment)}: " +
                $"{ToJson(createDeploymentResponse.Deployment)}</color>");
            
            createDeploymentResponse.RawResponse?.Dispose(); // Prevent mem leaks
            return createDeploymentResponse.Deployment;
        }

        /// <summary>
        /// Wrapper for `CreateDeploymentAsync` to upload and deploy a cloud deploy to Hathora.
        /// </summary>
        /// <param name="_cancelToken">TODO</param>
        /// <returns>Returns Deployment on success</returns>
        public async Task<List<Deployment>> GetDeploymentsAsync(
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerDeployApiWrapper)}.{nameof(CreateDeploymentAsync)}]";

            // Prepare request
            GetDeploymentsRequest getDeploymentsRequest = new()
            {
            };

            // Get response async =>
            GetDeploymentsResponse getDeploymentsResponse = null;
            
            try
            {
                getDeploymentsResponse = await DeployApi.GetDeploymentsAsync(getDeploymentsRequest);
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(DeployApi.GetDeploymentsAsync)} => Error: {e.Message}");
                return null; // fail
            }

            // Process response
            List<Deployment> deployments = getDeploymentsResponse.Classes;
            Debug.Log($"{logPrefix} <color=yellow>num {nameof(deployments)}: '{deployments?.Count}'</color>");
            
            getDeploymentsResponse.RawResponse?.Dispose(); // Prevent mem leaks
            return deployments;
        }
        #endregion // Server Deploy Async Hathora SDK Calls
    }
}
