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
        protected AppV1 AppApi { get; }

        public HathoraServerAppApiWrapper(
            HathoraCloudSDK _hathoraSdk,
            HathoraServerConfig _hathoraServerConfig)
            : base(_hathoraSdk, _hathoraServerConfig)
        {
            Debug.Log($"[{nameof(HathoraServerAppApiWrapper)}.Constructor] " +
                "Initializing Server API...");
            
            this.AppApi = _hathoraSdk.AppV1 as AppV1;
        }
        
        
        #region Server App Async Hathora SDK Calls
        /// <summary>
        /// Wrapper for `CreateAppAsync` to upload and app a cloud app to Hathora.
        /// </summary>
        /// <param name="_cancelToken">TODO: This may be implemented in the future</param>
        /// <returns>Returns App on success</returns>
        public async Task<List<ApplicationWithDeployment>> GetAppsAsync(
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerAppApiWrapper)}.{nameof(GetAppsAsync)}]";

            // Get response async => 
            GetAppsResponse getAppsResponse = null;
            
            try
            {
                getAppsResponse = await AppApi.GetAppsAsync(); 
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(AppApi.GetAppsAsync)} => Error: {e.Message}");
                return null; // fail
            }

            // Get inner response to return -> Log/Validate
            List<ApplicationWithDeployment> applicationWithDeployment = getAppsResponse.Classes;
            Debug.Log($"{logPrefix} num: '{applicationWithDeployment?.Count ?? 0}'");
            
            getAppsResponse.RawResponse?.Dispose(); // Prevent mem leaks
            return applicationWithDeployment;
        }
        #endregion // Server App Async Hathora SDK Calls
    }
}
