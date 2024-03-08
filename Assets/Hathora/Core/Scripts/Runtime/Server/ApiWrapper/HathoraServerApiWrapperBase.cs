// Created by dylan@hathora.dev

using Hathora.Core.Scripts.Runtime.Common.ApiWrapper;
using HathoraCloud;
using Debug = UnityEngine.Debug;

namespace Hathora.Core.Scripts.Runtime.Server.ApiWrapper
{
    /// <summary>
    /// This allows the API to view UserConfig (eg: AppId), set session and auth tokens.
    /// Server APIs can inherit from this.
    /// Unlike Client API wrappers (since !Mono), we init via Constructor instead of Init().
    /// </summary>
    public abstract class HathoraServerApiWrapperBase : HathoraApiWrapperBase
    {
        #region Vars
        protected HathoraServerConfig HathoraServerConfig { get; }

        /// <summary>Pulls "HathoraDevToken" from HathoraServerConfig</summary>
        protected string HathoraDevToken => 
            HathoraServerConfig.HathoraCoreOpts.DevAuthOpts.HathoraDevToken;

        // Shortcuts
        protected new string AppId
        {
            get {
                string logPrefix = $"[{nameof(HathoraServerApiWrapperBase)}.{nameof(AppId)}.get]";

                if (HathoraServerConfig == null)
                {
                    Debug.LogError($"{logPrefix} !HathoraServerConfig: " +
                        "Did you forget to add init a newly-added API @ HathoraServerMgr.InitApiWrappers()?" +
                        "**For Non-host Clients (or Servers that don't have runtime Server API calls), you may ignore this**");
                    return null;
                }

                if (HathoraServerConfig.HathoraCoreOpts == null)
                {
                    Debug.LogError($"{logPrefix} HathoraServerConfig exists, " +
                        "but !HathoraServerConfig.HathoraCoreOpts");
                    return null;
                }

                if (string.IsNullOrEmpty(HathoraServerConfig.HathoraCoreOpts.AppId))
                {
                    Debug.LogError($"{logPrefix} !HathoraServerConfig.HathoraCoreOpts.AppId: " +
                        "Did you configure your HathoraServerConfig?");
                    return null;
                }

                return HathoraServerConfig.HathoraCoreOpts.AppId;
            }
        }
        #endregion // Vars


        #region Init
        /// <summary>
        /// Server calls use Dev token. Unlike ClientMgr, Server !inherits
        /// from Mono (so we init via constructor).
        /// </summary>
        /// <param name="_hathoraServerConfig">
        /// Find via Unity editor top menu: Hathora >> Find Configs
        /// </param>
        /// <param name="_hathoraSdk"></param>
        protected HathoraServerApiWrapperBase(
            HathoraCloudSDK _hathoraSdk,
            HathoraServerConfig _hathoraServerConfig)
            : base(_hathoraSdk)
        {
            this.HathoraServerConfig = _hathoraServerConfig;
        }
        #endregion // Init
    }
}
