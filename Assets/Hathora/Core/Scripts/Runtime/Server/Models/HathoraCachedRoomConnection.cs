// dylan@hathora.dev

using System;
using System.Globalization;
using Hathora.Core.Scripts.Runtime.Common.Extensions;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using Hathora.Core.Scripts.Runtime.Server.Models.SerializedWrappers;
using HathoraCloud.Models.Shared;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hathora.Core.Scripts.Runtime.Server.Models
{
    /// <summary>
    /// We just created a Room from ServerConfig, containing Room + ConnectionInfo (v2+).
    /// </summary>
    [Serializable]
    public class HathoraCachedRoomConnection
    {
        [SerializeField]
        private Region _hathoraRegion = HathoraUtils.DEFAULT_REGION;
        public Region HathoraRegion
        {
            get => _hathoraRegion;
            set => _hathoraRegion = value;
        }

        /// <summary>Washington_DC => "Washington DC"</summary>
        public string GetFriendlyRegionStr() => 
            Enum.GetName(typeof(Region), _hathoraRegion)?.SplitPascalCase();
        
        [SerializeField]
        private RoomSerializable _roomSerializable;
        public Room Room
        {
            get => _roomSerializable.ToRoomType();
            set => _roomSerializable = new RoomSerializable(value);
        }
        
        [FormerlySerializedAs("_connectionInfoV2")]
        [FormerlySerializedAs("_connectionInfoV2Wrapper")]
        [SerializeField]
        private ConnectionInfoV2Serializable _connectionInfoV2Serializable;
        public ConnectionInfoV2 ConnectionInfoV2
        {
            get => _connectionInfoV2Serializable.ToConnectionInfoV2Type();
            set => _connectionInfoV2Serializable = new ConnectionInfoV2Serializable(value);
        }

        public bool IsError { get; set; }
        public string ErrReason { get; set; }

        public HathoraCachedRoomConnection(
            Region _region,
            Room _room, 
            ConnectionInfoV2 _connectionInfoV2)
        {
            this.HathoraRegion = _region;
            this.Room = _room;
            this.ConnectionInfoV2 = _connectionInfoV2;
        }
        
        /// <summary>
        /// Use this to mock the obj for a failure.
        /// </summary>
        public HathoraCachedRoomConnection()
        {
        }

        /// <summary>
        /// Returns a prettified "host:port".
        /// </summary>
        /// <returns></returns>
        public string GetConnInfoStr()
        {
            string hostStr = _connectionInfoV2Serializable == null
                ? "<MissingHost>"
                : _connectionInfoV2Serializable?.ExposedPort?.Host ?? "<MissingHost>";

            double portDbl = _connectionInfoV2Serializable == null
                ? 0
                : _connectionInfoV2Serializable?.ExposedPort?.Port ?? 0;
            
            string portStr = portDbl > 0 
                ? portDbl.ToString(CultureInfo.InvariantCulture)
                : "<MissingPort>";

            return $"{hostStr}:{portStr}";
        }
    }
}
