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
    public class ContainerSize
    {
        public double Cpu { get; set; }
        public double Memory { get; set; }
    }

    public static class ContainerSizes
    {
        public static readonly Dictionary<PlanName, ContainerSize> Sizes = new Dictionary<PlanName, ContainerSize>
        {
            { PlanName.Tiny, new ContainerSize { Cpu = 0.5, Memory = 1024 } },
            { PlanName.Small, new ContainerSize { Cpu = 1, Memory = 2048 } },
            { PlanName.Medium, new ContainerSize { Cpu = 2, Memory = 4096 } },
            { PlanName.Large, new ContainerSize { Cpu = 4, Memory = 8192 } }
        };
    }
    /// <summary>
    /// Operations that allow you configure and manage an application's build at runtime.
    /// Build Concept | https://hathora.dev/docs/concepts/hathora-entities#build
    /// API Docs | https://hathora.dev/api#tag/DeploymentV1
    /// </summary>
    public class HathoraServerDeployApiWrapper : HathoraServerApiWrapperBase
    {
        protected IDeploymentsV3 DeployApi { get; }
        private HathoraDeployOpts deployOpts => HathoraServerConfig.HathoraDeployOpts;
        private volatile bool uploading;

        public HathoraServerDeployApiWrapper(
            HathoraCloudSDK _hathoraSdk,
            HathoraServerConfig _hathoraServerConfig)
            : base(_hathoraSdk, _hathoraServerConfig)
        {
            Debug.Log($"[{nameof(HathoraServerDeployApiWrapper)}.Constructor] " +
                "Initializing Server API...");
            
            this.DeployApi = _hathoraSdk.DeploymentsV3;
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
        public async Task<DeploymentV3> CreateDeploymentAsync(
            string _buildId,
            Boolean _idleTimeoutEnabledSetting,
            List<DeploymentV3Env> _env = null,
            List<ContainerPort> _additionalContainerPorts = null,
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerDeployApiWrapper)}.{nameof(CreateDeploymentAsync)}]";
            
            // Prepare request
            TransportType selectedTransportType = deployOpts.TransportType;
            
            #region DeploymentEnvConfigInner Workaround
            // #######################################################################################
            // (!) Hathora SDK's DeploymentEnv is identical to DeploymentConfigEnv >> Port it over
            List<DeploymentConfigV3Env> envWorkaround = _env?.Select(envVar => 
                new DeploymentConfigV3Env
                {
                    Name = envVar.Name, 
                    Value = envVar.Value,
                }).ToList();
            // #######################################################################################
            #endregion DeploymentEnvConfigInner Workaround
            
            // Map Plan Name to requested hardware
            double requestedCPU = 1;
            double requestedMemory = 2048;
            if (ContainerSizes.Sizes.TryGetValue(deployOpts.PlanName, out ContainerSize size))
            {
                requestedCPU = size.Cpu;
                requestedMemory = size.Memory;
            }
            else
            {
                Console.WriteLine("Plan not found, defaulting to 1 CPU, 2048 MB plan size");
            }
            
            DeploymentConfigV3 deployConfig = new()
            {
                BuildId = _buildId,
                Env = envWorkaround ?? new List<DeploymentConfigV3Env>(),
                RoomsPerProcess = deployOpts.RoomsPerProcess, 
                RequestedCPU = requestedCPU,
                RequestedMemoryMB = requestedMemory,
                IdleTimeoutEnabled = _idleTimeoutEnabledSetting,
                AdditionalContainerPorts = _additionalContainerPorts ?? new List<ContainerPort>(),
                TransportType = selectedTransportType,
                ContainerPort = deployOpts.ContainerPortSerializable.Port,
            };

            CreateDeploymentRequest createDeploymentRequest = new()
            {
                DeploymentConfigV3 = deployConfig,
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

            Debug.Log($"{logPrefix} <color=yellow>{nameof(createDeploymentResponse.DeploymentV3)}: " +
                $"{ToJson(createDeploymentResponse.DeploymentV3)}</color>");
            
            createDeploymentResponse.RawResponse?.Dispose(); // Prevent mem leaks
            return createDeploymentResponse.DeploymentV3;
        }

        /// <summary>
        /// Wrapper for `CreateDeploymentDeprecatedAsync` to upload and deploy a cloud deploy to Hathora.
        /// </summary>
        /// <param name="_cancelToken">TODO</param>
        /// <returns>Returns Deployment on success</returns>
        public async Task<List<DeploymentV3>> GetDeploymentsAsync(
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
            List<DeploymentV3> deployments = getDeploymentsResponse.DeploymentsV3Page != null ? getDeploymentsResponse.DeploymentsV3Page.Deployments : new List<DeploymentV3>();
            Debug.Log($"{logPrefix} <color=yellow>num {nameof(deployments)}: '{deployments?.Count}'</color>");
            
            getDeploymentsResponse.RawResponse?.Dispose(); // Prevent mem leaks
            return deployments;
        }
        #endregion // Server Deploy Async Hathora SDK Calls
    }
}
