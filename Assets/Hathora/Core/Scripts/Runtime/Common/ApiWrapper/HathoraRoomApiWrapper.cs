// Created by dylan@hathora.dev

using System;
using System.Threading;
using System.Threading.Tasks;
using HathoraCloud;
using HathoraCloud.Models.Shared;
using UnityEngine;

namespace Hathora.Core.Scripts.Runtime.Common.ApiWrapper
{
    /// <summary>
    /// Common+Client API calls for Room.
    /// Operations to create, manage, and connect to rooms.
    /// Rooms Concept | https://hathora.dev/docs/concepts/hathora-entities#room 
    /// API Docs | https://hathora.dev/api#tag/RoomV1 
    /// </summary>
    public class HathoraRoomApiWrapper : HathoraApiWrapperBase
    {
        protected IRoomsV2 RoomApi { get; }

        public HathoraRoomApiWrapper(HathoraCloudSDK _hathoraSdk)
        : base(_hathoraSdk)
        {
            Debug.Log($"[{nameof(HathoraRoomApiWrapper)}.Constructor] " +
                "Initializing Common API...");
            
            this.RoomApi = _hathoraSdk.RoomsV2;
        }

        
        #region Common Room Async Hathora SDK Calls
        /// <summary>
        /// Gets connection info, like ip:port.
        /// (!) We'll poll until we have an `Active` Status: Be sure to await!
        /// </summary>
        /// <param name="_roomId">Get this from NetHathoraClientLobbyApi join/create</param>
        /// <param name="_pollIntervalSecs"></param>
        /// <param name="_pollTimeoutSecs"></param>
        /// <param name="_cancelToken"></param>
        /// <returns>Room on success</returns>
        public virtual async Task<ConnectionInfoV2> GetConnectionInfoAsync(
            string _roomId, 
            int _pollIntervalSecs = 1, 
            int _pollTimeoutSecs = 120,
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraRoomApiWrapper)}.{nameof(GetConnectionInfoAsync)}]";
            
            // Prep request
            HathoraCloud.Models.Operations.GetConnectionInfoRequest getConnectionInfoRequest = new()
            {
                RoomId = _roomId,
            };

            // Poll until we get the `Active` status.
            int pollSecondsTicked; // Duration to be logged later
            HathoraCloud.Models.Operations.GetConnectionInfoResponse getConnectionInfoResponse = null;
            
            for (pollSecondsTicked = 0; pollSecondsTicked < _pollTimeoutSecs; pollSecondsTicked++)
            {
                _cancelToken.ThrowIfCancellationRequested();
                
                try
                {
                    getConnectionInfoResponse = await RoomApi.GetConnectionInfoAsync(getConnectionInfoRequest);
                }
                catch(Exception e)
                {
                    Debug.LogError($"{logPrefix} {e.Message}");
                    return null; // fail
                }

                if (getConnectionInfoResponse.ConnectionInfoV2?.Status == RoomReadyStatus.Active)
                    break;
                
                await Task.Delay(TimeSpan.FromSeconds(_pollIntervalSecs), _cancelToken);
            }

            // -----------------------------------------
            // We're done polling -- sucess or timeout?
            ConnectionInfoV2 connectionInfo = getConnectionInfoResponse?.ConnectionInfoV2;

            if (connectionInfo?.Status != RoomReadyStatus.Active)
            {
                Debug.LogError($"{logPrefix} Error: Timed out");
                return null;
            }

            // Success
            Debug.Log($"{logPrefix} Success (after {pollSecondsTicked}s polling): <color=yellow>" +
                $"[{getConnectionInfoResponse.StatusCode}] {nameof(getConnectionInfoResponse.ConnectionInfoV2)}: " +
                $"{ToJson(getConnectionInfoResponse.ConnectionInfoV2)}</color>");

            getConnectionInfoResponse.RawResponse?.Dispose(); // Prevent mem leaks
            return connectionInfo;
        }
        #endregion // Common Room Async Hathora SDK Calls
    }
}
