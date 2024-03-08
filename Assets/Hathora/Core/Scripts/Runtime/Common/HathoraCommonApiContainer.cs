// Created by dylan@hathora.dev

using System;
using Hathora.Core.Scripts.Runtime.Common.ApiWrapper;
using HathoraCloud;

namespace Hathora.Core.Scripts.Runtime.Common
{
    /// <summary>
    /// Common API wrapper container for HathoraClientMgr / HathoraServerMgr
    /// </summary>
    [Serializable]
    public class HathoraCommonApiContainer
    {
        public HathoraLobbyApiWrapper LobbyApiWrapper { get; set; }
        public HathoraRoomApiWrapper RoomApiWrapper { get; set; }

        
        /// <summary>Initializes Commmon Hathora APIs</summary>
        /// <param name="_hathoraSdk"></param>
        /// <param name="_initLobbyApiWrapper"></param>
        /// <param name="_initRoomApiWrapper"></param>
        protected HathoraCommonApiContainer(
            HathoraCloudSDK _hathoraSdk,
            bool _initLobbyApiWrapper = true,
            bool _initRoomApiWrapper = true)
        {
            // Init Common API Wrappers
            if (_initLobbyApiWrapper)
                LobbyApiWrapper = new HathoraLobbyApiWrapper(_hathoraSdk);
            
            if (_initRoomApiWrapper)
                RoomApiWrapper = new HathoraRoomApiWrapper(_hathoraSdk);
        }
    }
}
