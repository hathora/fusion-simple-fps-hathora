// Created by dylan@hathora.dev

using System;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Runtime.Common.ApiWrapper;
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;
using UnityEngine;

namespace Hathora.Core.Scripts.Runtime.Client.ApiWrapper
{
    /// <summary>
    /// Client API calls for Auth.
    /// Operations that allow you to generate a Hathora-signed JSON web token (JWT) for player authentication.
    /// JWT Concept | https://jwt.io/
    /// Auth Concept | https://hathora.dev/docs/lobbies-and-matchmaking/auth-service
    /// API Docs | https://hathora.dev/api#tag/AuthV1
    /// </summary>
    public class HathoraClientAuthApiWrapper : HathoraApiWrapperBase
    {
        protected AuthV1 AuthApi { get; }

        public HathoraClientAuthApiWrapper(HathoraCloudSDK _hathoraSdk)
            : base(_hathoraSdk)
        {
            Debug.Log($"[{nameof(HathoraClientAuthApiWrapper)}.Constructor] " +
                "Initializing Client API...");
            
            this.AuthApi = _hathoraSdk.AuthV1 as AuthV1;
        }


        #region Client Auth Async Hathora SDK Calls
        /// <param name="_cancelToken">TODO</param>
        /// <returns>Returns AuthResult on success</returns>
        public async Task<PlayerTokenObject> ClientAuthAsync(CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraClientAuthApiWrapper)}.{nameof(ClientAuthAsync)}]"; 
            Debug.Log($"{logPrefix} Start");

            LoginAnonymousRequest anonLoginRequest = new() { AppId = base.AppId }; 
            LoginAnonymousResponse loginAnonResponse = null;
            
            try
            {
                loginAnonResponse = await AuthApi.LoginAnonymousAsync(anonLoginRequest);
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(AuthApi.LoginAnonymousAsync)} => Error: {e.Message}");
                return null; // fail
            }

            string clientAuthToken = loginAnonResponse.PlayerTokenObject?.Token; 
            bool isAuthed = !string.IsNullOrEmpty(clientAuthToken); 
            
            
#if UNITY_EDITOR
            // For security, we probably only want to log this in the editor
            Debug.Log($"{logPrefix} <color=yellow>{nameof(isAuthed)}: {isAuthed}, " +
                $"{nameof(loginAnonResponse.PlayerTokenObject)}: {ToJson(loginAnonResponse.PlayerTokenObject)}</color>");
#else
            Debug.Log($"{logPrefix} {nameof(isAuthed)}: {isAuthed}");
#endif


            loginAnonResponse.RawResponse?.Dispose(); // Prevent mem leaks
            return loginAnonResponse.PlayerTokenObject;
        }
        #endregion // Server Auth Async Hathora SDK Calls
    }
}
