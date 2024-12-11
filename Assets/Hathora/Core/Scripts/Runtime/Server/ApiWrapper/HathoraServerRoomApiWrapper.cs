// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Runtime.Common.ApiWrapper;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hathora.Core.Scripts.Runtime.Server.ApiWrapper
{
    /// <summary>
    /// Operations to create, manage, and connect to rooms.
    /// Rooms Concept | https://hathora.dev/docs/concepts/hathora-entities#process
    /// API Docs | https://hathora.dev/api#tag/RoomV2
    /// </summary>
    public class HathoraServerRoomApiWrapper : HathoraRoomApiWrapper
    {
        private HathoraServerConfig hathoraServerConfig;
        private string auth0DevToken => hathoraServerConfig.HathoraCoreOpts.DevAuthOpts.HathoraDevToken; 
        public bool IsPollingForActiveConnInfo { get; private set; }
        
        public HathoraServerRoomApiWrapper(
            HathoraCloudSDK _hathoraSdk,
            HathoraServerConfig _hathoraServerConfig)
            : base(_hathoraSdk)
        {
            Debug.Log($"[{nameof(HathoraServerRoomApiWrapper)}.Constructor] " +
                "Initializing Server API...");
            
            hathoraServerConfig = _hathoraServerConfig;
        }
        
        
        #region Server Room Async Hathora SDK Calls
        /// <summary>
        /// This is a HIGH-LEVEL func that does multiple things:
        /// </summary>
        /// 
        /// <list type="number">
        /// <item>Wrapper for `ServerCreateRoomAwaitActiveAsync` to create a new room in Hathora.</item>
        /// <item>Poll GetConnectionInfoV2Async(roomId): takes ~5s, until Status is `Active`.</item>
        /// <item>Once Active Status, get Room info.</item>
        /// </list>
        /// <param name="_region">Leave empty to use the default value via `HathoraUtils.DEFAULT_REGION`.</param>
        /// <param name="_roomConfig">
        /// Optional configuration parameters for the room. Can be any string including stringified JSON.
        /// It is accessible from the room via [`GetRoomInfo()`](https://hathora.dev/api#tag/RoomV2/operation/GetRoomInfo).
        /// </param>
        /// <param name="_customCreateRoomId">Recommended to leave null to prevent potential dupes.</param>
        /// <param name="_cancelToken">TODO</param>
        /// <returns>both Room + [ACTIVE]ConnectionInfoV2 (ValueTuple) on success</returns>
        public async Task<(Room room, ConnectionInfoV2 connInfo)> ServerCreateRoomAwaitActiveAsync(
            Region _region = HathoraUtils.DEFAULT_REGION,
            string _roomConfig = null,
            string _customCreateRoomId = null,
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerRoomApiWrapper)}.{nameof(ServerCreateRoomAwaitActiveAsync)}]";
            
            // (1/3) Create Room
            RoomConnectionData createdRoomConnectionInfo = null;
            
            try
            {
                CreateRoomParams createRoomParams = new()
                {
                    Region = _region,
                    RoomConfig = _roomConfig,
                };
                  
                createdRoomConnectionInfo = await ServerCreateRoomAsync(
                    createRoomParams, 
                    _customCreateRoomId, 
                    _cancelToken);
            }
            catch (TaskCanceledException e)
            {
                Debug.Log($"{logPrefix} Cancelled {e.Message}");
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} ServerCreateRoomAsync => Error: {e.Message}");
                throw;
            }

            string newlyCreatedRoomId = createdRoomConnectionInfo.RoomId;
            
            // ----------
            // (2/3) Poll until `Active` connection Status (or timeout)
            ConnectionInfoV2 activeConnectionInfo = null;

            try
            {
                activeConnectionInfo = await PollConnectionInfoUntilActiveAsync(
                    newlyCreatedRoomId,
                    _cancelToken);

                Assert.IsTrue(activeConnectionInfo?.Status == RoomReadyStatus.Active,
                    $"{logPrefix} {nameof(activeConnectionInfo)} Expected `Active` status, since Room is Active");
            }
            catch (TaskCanceledException e)
            {
                Debug.Log($"{logPrefix} Cancelled {e.Message}");
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(PollConnectionInfoUntilActiveAsync)} => Error: {e.Message} " +
                    "(Check console.hathora.dev logs for +info)");
                throw;
            }
            
            // ----------
            // (3/3) Once Connection Status is `Active`, get Room info
            Room activeRoom = null;
            try
            {
                activeRoom = await ServerGetRoomInfoAsync(
                    newlyCreatedRoomId, 
                    _cancelToken);
                
                Assert.IsTrue(activeRoom?.Status == RoomStatus.Active, 
                    $"{logPrefix} Expected activeRoom !Active status");
            }
            catch (TaskCanceledException e)
            {
                Debug.Log($"{logPrefix} Cancelled {e.Message}");
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(ServerGetRoomInfoAsync)} => Error: {e.Message} " +
                    "(Check console.hathora.dev logs for +info)");
                throw;
            }

            // ----------
            // Success
            Debug.Log($"{logPrefix} Success: <color=yellow>{nameof(activeConnectionInfo)}: " +
                $"{ToJson(activeConnectionInfo)}</color>");

            return (activeRoom, activeConnectionInfo);
        }

        /// <summary>
        /// Wrapper for `ServerCreateRoomAwaitActiveAsync` to create a new room in Hathora.
        /// </summary>
        /// <param name="_createRoomReq">Region, RoomConfig</param>
        /// <param name="_customRoomId"></param>
        /// <param name="_cancelToken">TODO</param>
        /// <returns></returns>
        private async Task<RoomConnectionData> ServerCreateRoomAsync(
            CreateRoomParams _createRoomParams,
            string _customRoomId = null,
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerRoomApiWrapper)}.{nameof(ServerCreateRoomAsync)}]";
            
            // Prep request data
            CreateRoomRequest createRoomRequestWrapper = new()
            {
                RoomId = _customRoomId,
                CreateRoomParams = _createRoomParams,
            };
            
            Debug.Log($"{logPrefix} <color=yellow>{nameof(createRoomRequestWrapper)}: " +
                $"{ToJson(createRoomRequestWrapper)}</color>");

            // Request call async =>
            CreateRoomResponse createRoomResponse = null;
            
            try
            {
                // BUG: ExposedPort prop will always be null here; prop should be removed for CreateRoom.
                // To get the ExposedPort, we need to poll until Room Status is Active
                createRoomResponse = await RoomApi.CreateRoomAsync(createRoomRequestWrapper);
            }
            catch (TaskCanceledException)
            {
                // The user explicitly cancelled, or the Task timed out
                Debug.Log($"{logPrefix} Task cancelled");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(RoomApi.CreateRoomAsync)} => Error: {e.Message}");
                return null; // fail
            }
            
            // Process response
            Debug.Log($"{logPrefix} Success: <color=yellow>{nameof(createRoomResponse.RoomConnectionData)}: " +
                $"{ToJson(createRoomResponse.RoomConnectionData)}</color>");

            // Everything else in this result object is currently irrelevant except the RoomId
            createRoomResponse.RawResponse?.Dispose(); // Prevent mem leaks
            return createRoomResponse.RoomConnectionData;
        }
        
        /// <summary>
        /// When you get the result, check Status for Active.
        /// (!) If !Active, getting the ConnectionInfoV2 will result in !ExposedPort.
        /// </summary>
        /// <param name="_roomId"></param>
        /// <param name="_cancelToken"></param>
        /// <returns></returns>
        public async Task<Room> ServerGetRoomInfoAsync(
            string _roomId, 
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerRoomApiWrapper)}.{nameof(ServerGetRoomInfoAsync)}]";
            
            // Prepare request
            GetRoomInfoRequest getRoomInfoRequest = new()
            {
                RoomId = _roomId,
            };
            
            // Get response async =>
            GetRoomInfoResponse getRoomInfoResponse = null;

            try
            {
                getRoomInfoResponse = await RoomApi.GetRoomInfoAsync(getRoomInfoRequest);
            }
            catch (TaskCanceledException)
            {
                // The user explicitly cancelled, or the Task timed out
                Debug.Log($"{logPrefix} Task cancelled");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(RoomApi.GetRoomInfoAsync)} => Error: {e.Message}");
                return null; // fail
            }

            Debug.Log($"{logPrefix} Success: <color=yellow>{nameof(getRoomInfoResponse.Room)}: " +
                $"{ToJson(getRoomInfoResponse.Room)}</color>");

            getRoomInfoResponse.RawResponse?.Dispose(); // Prevent mem leaks
            return getRoomInfoResponse.Room;
        }
        
        /// <summary>
        /// Get all active rooms for a given process using `appId + `processId`.
        /// API Doc | https://hathora.dev/api#tag/RoomV2/operation/GetActiveRoomsForProcess 
        /// </summary>
        /// <param name="_processId">
        /// System generated unique identifier to a runtime instance of your game server.
        /// Example: "cbfcddd2-0006-43ae-996c-995fff7bed2e"
        /// </param>
        /// <param name="_cancelToken"></param>
        /// <returns></returns>
        public async Task<List<RoomWithoutAllocations>> ServerGetActiveRoomsForProcessAsync(
            string _processId, 
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerRoomApiWrapper)}].{nameof(ServerGetActiveRoomsForProcessAsync)}]";
            
            // Prepare request
            GetActiveRoomsForProcessRequest getActiveRoomsForProcessRequest = new()
            {
                ProcessId = _processId,
            };
            
            // Get response async =>
            GetActiveRoomsForProcessResponse getActiveRoomsForProcessResponse = null;

            try
            {
                getActiveRoomsForProcessResponse = await RoomApi.GetActiveRoomsForProcessAsync(
                    getActiveRoomsForProcessRequest);
            }
            catch (TaskCanceledException)
            {
                // The user explicitly cancelled, or the Task timed out
                Debug.Log($"{logPrefix} Task cancelled");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} {nameof(RoomApi.GetActiveRoomsForProcessAsync)} => Error: {e.Message}");
                return null; // fail
            }

            // Process result
            List<RoomWithoutAllocations> activeRooms = getActiveRoomsForProcessResponse.Classes;
            Debug.Log($"{logPrefix} Success: <color=yellow>" +
                $"getActiveRoomsResultList count: {activeRooms.Count}</color>");

            if (activeRooms.Count > 0)
            {
                Debug.Log($"{logPrefix} Success: <color=yellow>{nameof(activeRooms)}[0]: " +
                    $"{ToJson(activeRooms[0])}</color>");
            }

            getActiveRoomsForProcessResponse.RawResponse?.Dispose(); // Prevent mem leaks
            return activeRooms;
        }
        #endregion // Server Room Async Hathora SDK Calls

        
        #region Utils
        /// <summary>
        /// ETA 5 seconds; poll once/second.
        /// </summary>
        /// <param name="_roomId"></param>
        /// <param name="_cancelToken"></param>
        /// <returns></returns>
        public async Task<ConnectionInfoV2> PollConnectionInfoUntilActiveAsync(
            string _roomId,
            CancellationToken _cancelToken = default)
        {
            string logPrefix = $"[{nameof(HathoraServerRoomApiWrapper)}.{nameof(PollConnectionInfoUntilActiveAsync)}]";
            
            ConnectionInfoV2 connectionInfo = null;
            int attemptNum = 0;
            IsPollingForActiveConnInfo = true;
            
            while (connectionInfo is not { Status: RoomReadyStatus.Active })
            {
                // Check for cancel + await 1s
                if (_cancelToken.IsCancellationRequested)
                {
                    IsPollingForActiveConnInfo = false;
                    _cancelToken.ThrowIfCancellationRequested();
                }
                
                await Task.Delay(TimeSpan.FromSeconds(1), _cancelToken);
        
                // ---------------
                // Try again
                attemptNum++;
                Debug.Log($"{logPrefix} Attempt #{attemptNum} ...");
                
                connectionInfo = await GetConnectionInfoAsync(
                    _roomId,
                    _cancelToken: _cancelToken);
                
                // ---------------
                if (connectionInfo?.Status == RoomReadyStatus.Active)
                    break; // Success
                
                // ---------------
                // Still !Active -- log, then try again
                Debug.LogWarning($"{logPrefix} Room !Active (yet) - attempting to poll again ...");
            }
            
            Debug.Log($"{logPrefix} Success: <color=yellow>{nameof(connectionInfo)}: " +
                $"{ToJson(connectionInfo)}</color>");
        
            IsPollingForActiveConnInfo = false;
            return connectionInfo;
        }
        #endregion //Utils
    }
}