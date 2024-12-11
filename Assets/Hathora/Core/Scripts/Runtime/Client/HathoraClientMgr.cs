// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Runtime.Client.ApiWrapper;
using Hathora.Core.Scripts.Runtime.Client.Config;
using Hathora.Core.Scripts.Runtime.Common;
using Hathora.Core.Scripts.Runtime.Common.ApiWrapper;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Security = HathoraCloud.Models.Shared.Security;

namespace Hathora.Core.Scripts.Runtime.Client
{
    /// <summary>
    /// - This is the entry point to call Hathora SDK: Auth, lobby, rooms, etc.
    /// - Opposed to the SDK itself, this gracefully wraps around it with callbacks + events + session tracking.
    /// - Every SDK call from this script caches the result in `hathoraClientSession`.
    /// - To add API scripts: Add to the `apis` serialized field.
    /// - Optimized to optionally be inheritted to separate your logic from Hathora's (easy updates; separation of logic).
    /// - Subscribe to event callbacks like `OnAuthLoginDoneEvent` to handle UI/logic from multiple scripts:
    ///     * OnAuthLoginDoneEvent
    ///     * OnGetActiveConnectionInfoFailEvent
    ///     * OnGetActiveConnectionInfoDoneEvent
    ///     * OnGetActivePublicLobbiesDoneEvent
    /// </summary>
    public class HathoraClientMgr : MonoBehaviour
    {
        public static HathoraClientMgr Singleton { get; private set; }
        
        [Header("(!) Get from Hathora dir; see hover tooltip")]
        [SerializeField, Tooltip("AppId should parity HathoraServerConfig (see top menu Hathora/Configuration")]
        private HathoraClientConfig hathoraClientConfig;
        public HathoraClientConfig HathoraClientConfig => hathoraClientConfig;

        [FormerlySerializedAs("HathoraClientSession")]
        [Header("Session, APIs")]
        [SerializeField, Tooltip("Whenever we use the Hathora Client SDK, we'll cache it in this Session.")]
        private HathoraClientSession hathoraClientSession;
        
        /// <summary>
        /// Whenever we use the Hathora Client SDK, we'll cache it in this Session.
        /// - Reset anew when authenticating.
        /// </summary>
        public HathoraClientSession HathoraClientSession => hathoraClientSession;

        /// <summary>
        /// Container for Hathora Client/Common APIs.
        /// (!) If you skip the high-level wrappers here, hathoraClientSession (cache) will not set!
        /// </summary>
        private HathoraClientApiContainer apis;
        
        /// <summary>Direct SDK access: Inits with info from `HathoraClientConfig`</summary>
        public HathoraCloudSDK HathoraSdk { get; private set; }
        

        #region Public Events
        /// <summary>Event triggers when auth is done (check isSuccess)</summary>
        /// <returns>isSuccess</returns>
        public static event Action<bool> OnAuthLoginDoneEvent;
        
        /// <summary>lobby</summary>
        public static event Action<LobbyV3> OnCreateLobbyDoneEvent;
        
        /// <summary>connectionInfo:ConnectionInfoV2</summary>
        public static event Action<ConnectionInfoV2> OnGetActiveConnectionInfoDoneEvent;
        
        /// <summary>lobbies:List (sorted by Newest @ top)</summary>
        public static event Action<List<LobbyV3>> OnGetActivePublicLobbiesDoneEvent;
        #endregion // Public Events
        
        
        #region Init
        protected virtual void Awake()
        {
            setSingleton();
            initHathoraSdk();
            initApiWrappers();
        }
        
        protected virtual void Start()
        {
        }

        /// <summary>
        /// You want other classes to easily be able to access your ClientMgr
        /// </summary>
        /// <summary>
        /// Set a singleton instance - we'll only ever have one serverMgr.
        /// Children probably want to override this and, additionally, add a Child singleton
        /// </summary>
        private void setSingleton()
        {
            if (Singleton != null)
            {
                Debug.LogError($"[{nameof(HathoraClientMgr)}.{nameof(setSingleton)}] " +
                    "Error: Destroying dupe");
                
                Destroy(gameObject);
                return;
            }
            
            Singleton = this;
        }
        
        private void initHathoraSdk()
        {
            this.HathoraSdk = new HathoraCloudSDK(null, null, hathoraClientConfig.AppId);
        }

        /// <summary>Init all Client API wrappers, passing HathoraSdk instance.</summary>
        private void initApiWrappers()
        {
            if (!CheckIsValidToInitApis())
                return;
            
            apis = new HathoraClientApiContainer(HathoraSdk);
        }

        /// <returns>isValid</returns>
        private bool CheckIsValidToInitApis()
        {
            string logPrefix = $"[{nameof(HathoraClientMgr)}.{nameof(CheckIsValidToInitApis)}]";

            if (hathoraClientConfig == null)
            {
                Debug.LogError($"{logPrefix} !hathoraClientConfig: " +
                    $"Serialize to {gameObject.name}.{nameof(HathoraClientMgr)} to " +
                    "call Client + Common API calls");
                return false;
            }

            return true; // isValid
        }
        #endregion // Init
        
        
        #region Interactions from UI
        /// <summary>
        /// Auths anonymously => Creates new hathoraClientSession.
        /// - Resets cache completely on done (not necessarily success)
        /// - Sets `PlayerAuthToken` cache
        /// - Callback @ virtual OnAuthLoginComplete(isSuccess)
        /// </summary>
        public async Task<PlayerTokenObject> AuthLoginAsync(CancellationToken _cancelToken = default)
        {
            PlayerTokenObject authResult = await apis.ClientAuthApiWrapper.ClientAuthAsync(_cancelToken);
            bool isSuccess = !string.IsNullOrEmpty(authResult.Token);
            
            hathoraClientSession.InitNetSession(authResult.Token);
            OnAuthLoginDone(isSuccess);

            return authResult;
        }

        /// <summary>
        /// Creates lobby => caches Lobby info @ hathoraClientSession.
        /// - Sets `Lobby` cache on done (not necessarily success)
        /// - Callback @ virtual OnCreateLobbyCompleteAsync(lobby)
        /// - Asserts IsAuthed
        /// </summary>
        /// <param name="_region">Leaving null will pass `HathoraUtils.DEFAULT_REGION`</param>
        /// <param name="_visibility"></param>
        /// <param name="_roomConfigSerializable">
        /// Pass your own model OR stringified json, minimally "{}"
        /// </param>
        /// <param name="_shortCode">
        /// Ideal for user-defined identifiers for lobbies, if null will default to roomId
        /// </param>
        /// <param name="_roomId">
        /// Leave null to auto-generate a globally unique roomId (recommended). Should only be overwritten to integrate with matchmakers, etc.
        /// </param>
        /// <param name="_cancelToken"></param>
        public async Task<LobbyV3> CreateLobbyAsync(
            object _roomConfigSerializable,
            Region _region = HathoraUtils.DEFAULT_REGION,
            string _shortCode = null,
            string _roomId = null,
            LobbyVisibility _visibility = LobbyVisibility.Public,
            CancellationToken _cancelToken = default)
        {
            Assert.IsTrue(hathoraClientSession.IsAuthed, 
                "expected hathoraClientSession.IsAuthed");

            LobbyV3 lobby = await apis.LobbyApiWrapper.CreateLobbyAsync(
                hathoraClientSession.PlayerAuthToken,
                _roomConfigSerializable,
                _region,
                _visibility,
                _shortCode,
                _roomId,
                _cancelToken);
            
            hathoraClientSession.Lobby = lobby;
            OnCreateLobbyDoneAsync(lobby);

            return lobby;
        }

        /// <summary>
        /// Gets lobby info by roomId.
        /// - Asserts IsAuthed
        /// - Sets `Lobby` cache on done (not necessarily success)
        /// - Callback @ virtual OnCreateLobbyCompleteAsync(lobby)
        /// </summary>
        public async Task<LobbyV3> GetLobbyInfoAsync(
            string _roomId, 
            CancellationToken _cancelToken = default)
        {
            Assert.IsTrue(hathoraClientSession.IsAuthed, 
                "expected hathoraClientSession.IsAuthed");

            LobbyV3 lobby = await apis.LobbyApiWrapper.GetLobbyInfoByRoomIdAsync(
                _roomId,
                _cancelToken);
        
            hathoraClientSession.Lobby = lobby;
            OnCreateLobbyDoneAsync(lobby);

            return lobby;
        }

        /// <summary>
        /// Gets Public+active lobbies.
        /// - Asserts IsAuthed
        /// - Sets `Lobbies` cache on done (not necessarily success)
        /// - Callback @ virtual OnViewPublicLobbiesComplete(lobbies)
        /// </summary>
        /// <param name="_listActivePublicLobbiesRequest">Null region returns all Regions</param>
        /// <param name="_cancelToken"></param>
        public async Task<List<LobbyV3>> GetActivePublicLobbiesAsync(
            ListActivePublicLobbiesRequest _listActivePublicLobbiesRequest,
            CancellationToken _cancelToken = default)
        {
            Assert.IsTrue(hathoraClientSession.IsAuthed, 
                "expected hathoraClientSession.IsAuthed");
            
            List<LobbyV3> lobbies = await apis.LobbyApiWrapper.ListPublicLobbiesAsync(
                _listActivePublicLobbiesRequest,
                _cancelToken);

            hathoraClientSession.Lobbies = lobbies;
            OnGetActivePublicLobbiesDone(lobbies);

            return lobbies;
        }
        
        /// <summary>
        /// Gets ip:port (+transport type) info so we can connect the Client
        /// via the selected transport (eg: Fishnet).
        /// - Asserts IsAuthed
        /// - Polls until status is `Active`: May take a bit!
        /// - Sets `ServerConnectionInfo` cache on done (not necessarily success)
        /// - Callback @ virtual OnGetActiveConnectionInfoComplete(connectionInfo)
        /// </summary>
        public async Task<ConnectionInfoV2> GetActiveConnectionInfo(
            string _roomId, 
            CancellationToken _cancelToken = default)
        {
            Assert.IsTrue(hathoraClientSession.IsAuthed, 
                "expected hathoraClientSession.IsAuthed");
            
            ConnectionInfoV2 connectionInfo = null;
            
            try
            {
                connectionInfo = await apis.RoomApiWrapper.GetConnectionInfoAsync(
                    _roomId,
                    _cancelToken: _cancelToken);
            }
            catch (Exception e)
            {
                Debug.LogError(
                    $"[{nameof(HathoraClientMgr)}.{nameof(GetActiveConnectionInfo)}] " +
                    $"GetConnectionInfoAsync => Error: {e}");

                throw;
            }
            finally
            {
                hathoraClientSession.ServerConnectionInfo = connectionInfo;
                OnGetActiveConnectionInfoDone(connectionInfo);
            }
            
            return connectionInfo;
        }
        #endregion // Interactions from UI
        
        
        #region Virtual callbacks w/Events
        /// <summary>AuthLogin() callback.</summary>
        /// <param name="_isSuccess"></param>
        protected virtual void OnAuthLoginDone(bool _isSuccess) =>
            OnAuthLoginDoneEvent?.Invoke(_isSuccess);
        
        /// <summary>
        /// GetActiveConnectionInfo() done callback (not necessarily successful).
        /// </summary>
        protected virtual void OnGetActiveConnectionInfoDone(ConnectionInfoV2 _connectionInfo) =>
            OnGetActiveConnectionInfoDoneEvent?.Invoke(_connectionInfo);

        /// <summary>GetActivePublicLobbies() callback.</summary>
        /// <param name="_lobbies"></param>
        protected virtual void OnGetActivePublicLobbiesDone(List<LobbyV3> _lobbies)
        {
            // Sort lobbies by create date -> Pass to UI
            List<LobbyV3> sortedFromNewestToOldest = _lobbies.OrderByDescending(lobby => 
                lobby.CreatedAt).ToList();
            
            OnGetActivePublicLobbiesDoneEvent?.Invoke(sortedFromNewestToOldest);
        }
        
        /// <summary>
        /// On success, most users will want to call GetActiveConnectionInfo().
        /// </summary>
        /// <param name="_lobby"></param>
        protected virtual void OnCreateLobbyDoneAsync(LobbyV3 _lobby) => 
            OnCreateLobbyDoneEvent?.Invoke(_lobby);
        #endregion // Virtual callbacks w/Events
        
        
        #region Utils
        /// <summary>
        /// We just need HathoraClientConfig serialized to a
        /// scene NetworkManager, with `AppId` set.
        /// - Does not throw, so you can properly handle UI on err.
        /// </summary>
        /// <returns>isValid</returns>
        public bool CheckIsValidToAuth() =>
            hathoraClientConfig != null && hathoraClientConfig.HasAppId;
        #endregion // Utils
    }
}
