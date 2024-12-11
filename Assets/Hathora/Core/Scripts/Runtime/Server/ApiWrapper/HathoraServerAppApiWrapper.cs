// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;
using Debug = UnityEngine.Debug;

namespace Hathora.Core.Scripts.Runtime.Server.ApiWrapper
{
    /// <summary>
    /// Operations that allow you manage your applications.
    /// Apps Concept | https://hathora.dev/docs/concepts/hathora-entities#application
    /// API Docs | https://hathora.dev/api#tag/AppV1
    /// </summary>
    public class HathoraServerAppApiWrapper : HathoraServerApiWrapperBase
    {
        protected IAppsV2 AppApi { get; }

        public HathoraServerAppApiWrapper(
            HathoraCloudSDK _hathoraSdk,
            HathoraServerConfig _hathoraServerConfig)
            : base(_hathoraSdk, _hathoraServerConfig)
        {
            Debug.Log($"[{nameof(HathoraServerAppApiWrapper)}.Constructor] " +
                "Initializing Server API...");
            
            this.AppApi = _hathoraSdk.AppsV2;
        }
        
        
        #region Server App Async Hathora SDK Calls
        /// <summary>
        /// Wrapper for `CreateAppAsync` to upload and app a cloud app to Hathora.
        /// </summary>
        /// <param name="_cancelToken">TODO: This may be implemented in the future</param>
        /// <returns>Returns App on success</returns>
        public async Task<List<ApplicationWithLatestDeploymentAndBuild>> GetAppsAsync(
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerAppApiWrapper)}.{nameof(GetAppsAsync)}]";
            Debug.Log($"{logPrefix} making GetAppsAsync request");

            // Get response async => 
            GetAppsResponse getAppsResponse = null;
            
            try
            {
                getAppsResponse = await AppApi.GetAppsAsync(new GetAppsRequest()); 
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(AppApi.GetAppsAsync)} => Error: {e.Message}");
                return null; // fail
            }

            // Get inner response to return -> Log/Validate
            List<ApplicationWithLatestDeploymentAndBuild> applicationWithDeployment = getAppsResponse.ApplicationsPage != null ? getAppsResponse.ApplicationsPage.Applications : new List<ApplicationWithLatestDeploymentAndBuild>();
            Debug.Log($"{logPrefix} num: '{applicationWithDeployment?.Count ?? 0}'");
            
            getAppsResponse.RawResponse?.Dispose(); // Prevent mem leaks
            return applicationWithDeployment;
        }
        #endregion // Server App Async Hathora SDK Calls
    }
}
