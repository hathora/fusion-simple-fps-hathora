// Created by dylan@hathora.dev

using System;
using Hathora.Core.Scripts.Runtime.Common;
using Hathora.Core.Scripts.Runtime.Server.ApiWrapper;
using HathoraCloud;

namespace Hathora.Core.Scripts.Runtime.Server.Models
{
    /// <summary>
    /// Server [runtime] API wrapper container. Requires Init()
    /// with a new() constructor since they don't inherit from Mono.
    /// 
    /// -> Have a new Hathora API to add?
    /// 1. Serialize it here, and add to `HathoraClientMgrBase.InitApiWrappers()`
    /// 2. Open scene `HathoraManager` GameObj (not prefab)
    /// 3. Add the new script component to HathoraManager.Hathora{Platform}ClientMgr.ClientApis
    /// 4. Add a new script[] to ClientApis container -> drag the script into the serialized field
    /// </summary>
    [Serializable]
    public class ServerApiContainer : HathoraCommonApiContainer
    {
        public HathoraServerAppApiWrapper ServerAppApiWrapper { get; set; }
        public HathoraServerProcessApiWrapper ServerProcessApiWrapper { get; set; }
        
        public HathoraServerRoomApiWrapper ServerRoomApiWrapper { get; set; }
        public HathoraServerBuildApiWrapper ServerBuildApiWrapper { get; set; } // Not generally used for Runtime scripts

        
        /// <summary>Initializes Server+Commmon Hathora APIs</summary>
        /// <param name="_hathoraSdk"></param>
        /// <param name="_hathoraServerConfig"></param>
        /// <param name="_initServerAppApi"></param>
        /// <param name="_initServerProcessApi"></param>
        /// <param name="_initServerRoomApiWrapper"></param>
        /// <param name="_initServerBuildApi">Not generally used for Runtime scripts</param>
        public ServerApiContainer(
            HathoraCloudSDK _hathoraSdk,
            HathoraServerConfig _hathoraServerConfig,
            bool _initServerAppApi = true,
            bool _initServerProcessApi = true,
            bool _initServerRoomApiWrapper = true,
            bool _initServerBuildApi = false)
            : base(_hathoraSdk)
        {
            // Init Server API Wrappers
            if (_initServerAppApi)
                ServerAppApiWrapper = new HathoraServerAppApiWrapper(_hathoraSdk, _hathoraServerConfig);
            
            if (_initServerAppApi)
                ServerProcessApiWrapper = new HathoraServerProcessApiWrapper(_hathoraSdk, _hathoraServerConfig);
            
            if (_initServerRoomApiWrapper)
                ServerRoomApiWrapper = new HathoraServerRoomApiWrapper(_hathoraSdk, _hathoraServerConfig);
            
            if (_initServerBuildApi)
                ServerBuildApiWrapper = new HathoraServerBuildApiWrapper(_hathoraSdk, _hathoraServerConfig);
        }
    }
}
