// Created by dylan@hathora.dev

using System;
using Hathora.Core.Scripts.Runtime.Client.ApiWrapper;
using Hathora.Core.Scripts.Runtime.Common;
using Hathora.Core.Scripts.Runtime.Common.ApiWrapper;
using HathoraCloud;

namespace Hathora.Core.Scripts.Runtime.Client
{
    /// <summary>
    /// Client API wrapper container for HathoraClientMgr
    /// </summary>
    [Serializable]
    public class HathoraClientApiContainer : HathoraCommonApiContainer
    {
        public HathoraClientAuthApiWrapper ClientAuthApiWrapper { get; set; }

        
        /// <summary>Initializes Client + Commmon Hathora APIs</summary>
        /// <param name="_hathoraSdk"></param>
        /// <param name="_initAuthApiWrapper"></param>
        /// <param name="_initRoomApiWrapper"></param>
        public HathoraClientApiContainer(
            HathoraCloudSDK _hathoraSdk,
            bool _initAuthApiWrapper = true,
            bool _initRoomApiWrapper = true)
            : base(_hathoraSdk)
        {
            // Init Client API Wrappers
            if (_initAuthApiWrapper)
                ClientAuthApiWrapper = new HathoraClientAuthApiWrapper(_hathoraSdk);
        }
    }
}
