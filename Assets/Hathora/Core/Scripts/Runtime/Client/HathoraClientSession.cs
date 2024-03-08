// Created by dylan@hathora.dev

using System.Collections.Generic;
using HathoraCloud;
using HathoraCloud.Models.Shared;
using UnityEngine;

namespace Hathora.Core.Scripts.Runtime.Client
{
    /// <summary>
    /// Cached Client net session. Eg: Auth token, last room joined.
    /// API wrappers will cache here on success from the HathoraClientMgr.
    /// New authentications will wipe these settings via InitNewSession().
    /// </summary>
    public class HathoraClientSession : MonoBehaviour
    {
        public static HathoraClientSession Singleton { get; private set; }
        
        /// <summary>
        /// Client: From AuthV1 via NetHathoraPlayer.
        /// </summary>
        public string PlayerAuthToken { get; private set; }
        public bool IsAuthed => !string.IsNullOrEmpty(PlayerAuthToken);

        /// <summary>The last known Lobby.</summary>
        public LobbyV3 Lobby { get; set; }
        
        /// <summary>The last known List of Lobby for a server browser. </summary>
        public List<LobbyV3> Lobbies { get; set; }
        public string RoomId => Lobby?.RoomId;

        /// <summary>
        /// The last known ServerConnectionInfo (ip/host, port, protocol).
        /// Unity ClientAddress == ExposedPort.Host
        /// </summary>
        public ConnectionInfoV2 ServerConnectionInfo { get; set; }

        /// <summary>Validates host + port</summary>
        /// <returns></returns>
        public bool CheckIsValidServerConnectionInfo() => 
            !string.IsNullOrEmpty(ServerConnectionInfo?.ExposedPort?.Host) && 
            ServerConnectionInfo?.ExposedPort?.Port > 0;
        
        public string GetServerInfoIpPort() => 
            $"{ServerConnectionInfo?.ExposedPort.Host}:{ServerConnectionInfo?.ExposedPort.Port}";

        
        #region Init
        public void Awake()
        {
            setSingleton();
        }
        
        private void setSingleton()
        {
            if (Singleton != null)
            {
                Debug.LogError("[HathoraClientSession]**ERR @ setSingleton: Destroying dupe");
                Destroy(gameObject);
                return;
            }

            Singleton = this;
        }

        /// <summary>For a new Session, we simply update the PlayerAuthToken.</summary>
        /// <param name="_playerAuthToken"></param>
        public void InitNetSession(string _playerAuthToken) =>
            this.PlayerAuthToken = _playerAuthToken;
        #endregion // Init
    }
}
