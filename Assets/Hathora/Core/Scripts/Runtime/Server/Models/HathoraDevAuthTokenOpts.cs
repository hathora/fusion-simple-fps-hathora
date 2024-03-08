// Created by dylan@hathora.dev

using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hathora.Core.Scripts.Runtime.Server.Models
{
    [Serializable]
    public class HathoraDevAuthTokenOpts
    {
        [FormerlySerializedAs("_devAuthToken")]
        [SerializeField]
        private string _hathoraDevToken;

        public string HathoraDevToken
        {
            get => _hathoraDevToken;
            set => _hathoraDevToken = value;
        }

        [SerializeField, Tooltip("Deletes an existing refresh_token, if exists from cached file")]
        private bool _forceNewToken = false;
        public bool ForceNewToken
        {
            get => _forceNewToken;
            set => _forceNewToken = value;
        }
        
        /// <summary>Temp var that triggers refreshing apps list after init auth</summary>
        public bool RecentlyAuthed { get; set; }
        
        
        // Public utils
        public bool HasAuthToken => !string.IsNullOrEmpty(_hathoraDevToken);
    }
}
