// Created by dylan@hathora.dev

using System;
using HathoraCloud.Models.Shared;

namespace Hathora.Core.Scripts.Runtime.Server.Models.SerializedWrappers
{
    /// <summary>
    /// Hathora SDK model wrapper to allow serializable class/fields.
    /// 
    /// This is a wrapper for Hathora SDK's `ApplicationWithDeployment` model.
    /// TODO: Upgrade SDK models to natively support serialization
    /// </summary>
    [Serializable]
    public class ApplicationAuthConfigurationSerializable
    {
        // [SerializeField, JsonProperty("_google")] // TODO
        // private ApplicationAuthConfigurationGoogleWrapper _googleWrapper;
        // public ApplicationAuthConfigurationGoogle Google 
        // { 
        //     get => _google; 
        //     set => _google = value;
        // }
        
        
        // /// <summary>"Construct a type with a set of properties K of type T" --from SDK</summary>
        // [SerializeField, JsonProperty("nickname")] // TODO
        // private string _nicknameWrapper;
        //
        // /// <summary>"Construct a type with a set of properties K of type T" --from SDK</summary>
        // public System.Object Nickname // TODO: What's expected in this object, if not a string? 
        // { 
        //     get => _nicknameWrapper; 
        //     set => _nicknameWrapper = value;
        // }
        
        
        // /// <summary>"Construct a type with a set of properties K of type T" --from SDK</summary>
        // [SerializeField, JsonProperty("anonymous")]
        // private System.Object _anonymous;
        //
        // /// <summary>"Construct a type with a set of properties K of type T" --from SDK</summary>
        // public System.Object Anonymous 
        // { 
        //     get => _anonymous; 
        //     set => _anonymous = value;
        // }
        

        public ApplicationAuthConfigurationSerializable(AuthConfiguration _appAuthConfig)
        {
            if (_appAuthConfig == null)
                return;

            // this.Google = _appAuthConfig.Google; // TODO
            // this.Nickname = _appAuthConfig.Nickname; // TODO
            // this.Anonymous = _appAuthConfig.Anonymous; // TODO
        }

        public AuthConfiguration ToApplicationAuthConfigurationType()
        {
            AuthConfiguration appAuthConfig = null;
            try
            {
                appAuthConfig = new()
                {
                    // Optional >>
                    // Google = this.Google, // TODO
                    // Nickname = this.Nickname, // TODO
                    // Anonymous = this.Anonymous, // TODO
                };
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Error: {e}");
                throw;
            }

            return appAuthConfig;
        }
    }
}
