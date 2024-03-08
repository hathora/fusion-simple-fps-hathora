// Created by dylan@hathora.dev

using System;
using Newtonsoft.Json;

namespace Hathora.Core.Scripts.Runtime.Common.Models
{
    /// <summary>
    /// Used with HathoraServerDeployApi + HathoraDeployOpts.
    /// </summary>
    [Serializable]
    public struct HathoraEnvVars
    {
        [JsonProperty("name")]
        public string Key;
        
        [JsonProperty("value")]
        public string StrVal;
        
        
        public HathoraEnvVars(string _key, string _strVal)
        {
            this.Key = _key;
            this.StrVal = _strVal;
        }
        
        public string ToJson() =>
            JsonConvert.SerializeObject(this);
    }
}
