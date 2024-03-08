// Created by dylan@hathora.dev

using System;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Editor.Server.Auth0;
using Hathora.Core.Scripts.Runtime.Server;
using UnityEngine;

namespace Hathora.Core.Scripts.Editor.Server
{
    /// <summary>
    /// Dev auth to get a dev token. Browser will launch to OAuth (via HathoraDevToken) and we'll
    /// tell the server we want a code. After a code, we'll launch a browser for the user
    /// to auth (via UI) and poll every 5s until we are authed.
    /// Timeout is ~5m, supporting cancel tokens. 
    /// </summary>
    public static class HathoraServerAuth
    {
        static HathoraServerAuth()
        {
            AuthCompleteSrc = new TaskCompletionSource<bool>();
        }
        
        public static TaskCompletionSource<bool> AuthCompleteSrc { get; set; }
        public static CancellationTokenSource AuthCancelTokenSrc { get; set; }

        public static bool IsAuthComplete => AuthCompleteSrc?.Task?.IsCompleted ?? false;



        /// <summary>
        /// This is NOT the same as !isSuccess, as this doesn't change
        /// on complete (just "not cancelled"). See AuthCompleteSrc
        /// </summary>
        public static bool HasCancellableAuthToken =>
            AuthCancelTokenSrc?.Token is { CanBeCanceled: true };
        
        /// <summary>
        /// </summary>
        /// <param name="_hathoraServerConfig"></param>
        /// <returns>isSuccess</returns>
        public static async Task<bool> DevAuthLogin(HathoraServerConfig _hathoraServerConfig)
        {
            createNewAuthCancelToken();
            Auth0Login auth = new(); 
            string refreshToken = await auth.GetTokenAsync(cancelToken: AuthCancelTokenSrc.Token);
            
            bool isSuccess = onGetTokenDone(
                _hathoraServerConfig, 
                refreshToken);
            
            return isSuccess;
        }

        /// <summary>
        /// </summary>
        /// <param name="_hathoraServerConfig"></param>
        /// <param name="_refreshToken"></param>
        /// <returns>isSuccess</returns>
        private static bool onGetTokenDone(
            HathoraServerConfig _hathoraServerConfig,
            string _refreshToken)
        {
            if (string.IsNullOrEmpty(_refreshToken))
            {
                // Fail >>
                if (AuthCancelTokenSrc != null && HasCancellableAuthToken)
                    onGetTokenCancelled();

                return false; // !isSuccess
            }
            
            SetAuthToken(_hathoraServerConfig, _refreshToken);
            return true; // isSuccess
        }

        private static void onGetTokenCancelled() =>
            AuthCancelTokenSrc?.Cancel();

        private static void createNewAuthCancelToken()
        {
            // Cancel an old op 1st
            if (AuthCancelTokenSrc != null && AuthCancelTokenSrc.Token.CanBeCanceled)
                AuthCancelTokenSrc.Cancel();
 
            AuthCancelTokenSrc = new CancellationTokenSource(
                TimeSpan.FromMinutes(Auth0Login.PollTimeoutMins));
        }

        /// <summary>Shortcut to set dev auth token with log</summary>
        /// <param name="_hathoraServerConfig"></param>
        /// <param name="_token">You probably want the refreshToken</param>
        public static void SetAuthToken(HathoraServerConfig _hathoraServerConfig, string _token)
        {
            _hathoraServerConfig.HathoraCoreOpts.DevAuthOpts.HathoraDevToken = _token;
            Debug.Log("[HathoraServerBuild] Dev HathoraDevToken login successful: " +
                "Token set @ HathoraServerConfig");
        }
    }
}
