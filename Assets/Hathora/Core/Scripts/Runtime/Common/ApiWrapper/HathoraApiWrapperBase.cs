// Created by dylan@hathora.dev

using HathoraCloud;
using HathoraCloud.Utils;
using Newtonsoft.Json;

namespace Hathora.Core.Scripts.Runtime.Common.ApiWrapper
{
    /// <summary>
    /// Common high-level helper of common utils for Client/Server API wrappers.
    /// - Stores HathoraCloudSDK
    /// - Shortcut to commonly-used AppId
    /// - ToJson helper to serialize [+prettify] API requests/results. 
    /// </summary>
    public abstract class HathoraApiWrapperBase
    {
        #region Vars
        private HathoraCloudSDK HathoraSdk { get; set; }

        /// <summary>Common shortcut to HathoraSdk.Config.AppId</summary>
        protected string AppId => HathoraSdk.SDKConfiguration.AppId;

        /// <summary>Works around UnityWebRequest serialization errs for disposable objs</summary>
        private JsonSerializerSettings jsonSerializerSettings;
        #endregion // Vars
        

        #region Init
        /// <summary>
        /// Init anytime before calling an API to ensure AppId + Auth is set.
        /// - Clients auth via Client Auth API
        /// - Servers auth via Auth0 HathoraDevToken
        /// </summary>
        /// <param name="_hathoraSdk">Leave null to get default from ClientMgr</param>
        protected HathoraApiWrapperBase(HathoraCloudSDK _hathoraSdk)
        {
            this.HathoraSdk = _hathoraSdk;
        }
        #endregion // Init
        
        
        #region Utils
        /// <summary>
        /// This uses the identical Json serializer that Hathora SDK uses:
        /// Deserialize requests, for example, to see exactly what's being sent.
        /// - BUG: UnityWebRequest serialization errs for disposable objs
        /// - BUG: Enums are serializing as numbers; server expects camelCase "strings" 
        /// </summary>
        /// <param name="_serializableObj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected string ToJson<T>(T _serializableObj) =>
            Utilities.SerializeJSON(_serializableObj);
        #endregion // Utils
    }
}
