// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;
using Newtonsoft.Json;
using UnityEngine;

namespace Hathora.Core.Scripts.Runtime.Server.Models
{
    /// <summary>
    /// Result model for HathoraServerMgr.GetHathoraServerContextAsync().
    /// - Can get Hathora Process, Room, [Lobby].
    /// - Can quick check if valid via `CheckIsValid`.
    /// - Contains utils to get "host:port" || "ip:port".
    /// </summary>
    public class HathoraServerContext
    {
        #region Vars
        public string EnvVarProcessId { get; private set; }
        public Region EnvVarRegion { get; private set; }
        public ProcessV3 ProcessInfo { get; set; }
        public LobbyV3 Lobby { get; set; }
        public List<RoomWithoutAllocations> ActiveRoomsForProcess { get; set; }
        #endregion // Vars
        

        #region Utils
        public bool HasPort => ProcessInfo?.ExposedPort?.Port > 0;
        
        /// <summary>
        /// Return host:port sync (opposed to GetHathoraServerIpPort async).
        /// </summary>
        /// <returns></returns>
        public (string host, ushort port) GetHathoraServerHostPort()
        {
            ProcessV3ExposedPort connectInfo = ProcessInfo?.ExposedPort;

            if (connectInfo == null)
                return default;

            ushort port = (ushort)connectInfo.Port;
            return (connectInfo.Host, port);
        }
        
        /// <summary>
        /// Async since we use Dns to translate the Host to IP.
        /// </summary>
        /// <returns></returns>
        public async Task<(IPAddress ip, ushort port)> GetHathoraServerIpPortAsync()
        {
            string logPrefix = $"[{nameof(HathoraServerContext)}.{nameof(GetHathoraServerIpPortAsync)}]";
            
            (IPAddress ip, ushort port) ipPort;
            
            ProcessV3ExposedPort connectInfo = ProcessInfo?.ExposedPort;

            if (connectInfo == null)
            {
                Debug.LogError($"{logPrefix} !connectInfo from ProcessInfo.ExposedPort");
                return default;
            }
            
            ipPort.port = (ushort)connectInfo.Port;

            try
            {
                ipPort.ip = await HathoraUtils.ConvertHostToIpAddress(connectInfo.Host);
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} ConvertHostToIpAddress => Error: {e}");
                throw;
            }

            return ipPort;
        }
        
        public RoomWithoutAllocations FirstActiveRoomForProcess => 
            ActiveRoomsForProcess?.FirstOrDefault();

        /// <summary>Checks for ( Process, Room, [Lobby] ) != null and Process !stopped.</summary>
        /// <returns>isValid</returns>
        public bool CheckIsValid(bool _expectingLobby) =>
            ProcessInfo != null &&
            ProcessInfo.StoppingAt != default &&
            FirstActiveRoomForProcess != null &&
            (!_expectingLobby || Lobby != null);

        /// <summary>Parse the RoomConfig into your own model.</summary>
        /// <typeparam name="TRoomConfig">
        /// Your serializable model; the same model passed when the room was created. 
        /// </typeparam>
        /// <returns></returns>
        public TRoomConfig ParseRoomConfig<TRoomConfig>()
        {
            string logPrefix = $"[{nameof(HathoraServerContext)}.{nameof(ParseRoomConfig)}]";

            string roomConfigJsonStr = Lobby?.RoomConfig;
            if (string.IsNullOrEmpty(roomConfigJsonStr))
            {
                Debug.LogError($"{logPrefix} !{nameof(roomConfigJsonStr)}");
                return default;
            }

            try
            {
                TRoomConfig parsedRoomConfig = JsonConvert.DeserializeObject<TRoomConfig>(roomConfigJsonStr);
                return parsedRoomConfig;
            }
            catch (Exception e)
            {
                Debug.LogError($"{logPrefix} Error parsing {nameof(roomConfigJsonStr)}: {e}");
                throw;
            }
        }
        #endregion // Utils

        
        #region Constructors
        public HathoraServerContext(string _envVarProcessId, Region _envVarRegion)
        {
            this.EnvVarProcessId = _envVarProcessId;
            this.EnvVarRegion = _envVarRegion;
        }

        public HathoraServerContext(
            string _envVarProcessId,
            Region _envVarRegion,
            ProcessV3 _processInfo,
            List<RoomWithoutAllocations> _activeRoomsForProcess,
            LobbyV3 _lobby)
        {
            this.EnvVarProcessId = _envVarProcessId;
            this.EnvVarRegion = _envVarRegion;
            this.ProcessInfo = _processInfo;
            this.ActiveRoomsForProcess = _activeRoomsForProcess;
            this.Lobby = _lobby;
        }
        #endregion // Constructors
    }
}
