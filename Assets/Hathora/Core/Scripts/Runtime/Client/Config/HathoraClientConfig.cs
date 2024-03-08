// Created by dylan@hathora.dev

using UnityEngine;

namespace Hathora.Core.Scripts.Runtime.Client.Config
{
    /// <summary>Hathora Client Config ScriptableObject, containing AppId.</summary>
    [CreateAssetMenu(fileName = nameof(HathoraClientConfig), menuName = "Hathora/Client Config File")]
    public class HathoraClientConfig : ScriptableObject
    {
        #region Vars
        [Header("These should match your `HathoraServerConfig`")]
        [SerializeField, Tooltip("Get from your Hathora dashboard, or copy from " +
             "Hathora Server Config (t:HathoraServerConfig)")]
        private string _appId;

        /// <summary>
        /// Get from your Hathora dashboard, or copy from Hathora
        /// Server Config (t:HathoraServerConfig).
        /// </summary>
        public string AppId
        {
            get => _appId;
            set => _appId = value;
        }
        
        public bool HasAppId => !string.IsNullOrEmpty(_appId);
        #endregion // Vars


        /// <summary>(!) Don't use OnEnable for ScriptableObjects</summary>
        private void OnValidate()
        {
        }
    }
}
