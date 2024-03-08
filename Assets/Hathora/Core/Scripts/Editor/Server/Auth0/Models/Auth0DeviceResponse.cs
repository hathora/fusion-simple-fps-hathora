// Created by dylan@hathora.dev

using System;
using Newtonsoft.Json;

namespace Hathora.Core.Scripts.Editor.Server.Auth0.Models
{
    /// <summary>
    /// Take the UserCode and send it to the HathoraDevToken API to get a token.
    /// </summary>
    [Serializable]
    public class Auth0DeviceResponse
    {
        [JsonProperty("device_code")]
        public string DeviceCode { get; set; }

        /// <summary>
        /// (!) Not to be confused with a token. We exchange this code for a token.
        /// </summary>
        [JsonProperty("user_code")]
        public string UserCode { get; set; }

        [JsonProperty("verification_uri")]
        public string VerificationUri { get; set; }

        [JsonProperty("verification_uri_complete")]
        public string VerificationUriComplete { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("interval")]
        public int Interval { get; set; }
    }
}
