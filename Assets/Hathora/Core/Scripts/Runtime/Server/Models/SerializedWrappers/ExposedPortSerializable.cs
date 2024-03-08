// Created by dylan@hathora.dev

using System;
using HathoraCloud.Models.Shared;
using Newtonsoft.Json;
using UnityEngine;

namespace Hathora.Core.Scripts.Runtime.Server.Models.SerializedWrappers
{
    /// <summary>
    /// Hathora SDK model wrapper to allow serializable class/fields.
    /// 
    /// Set transport configurations for where the server will listen.
    /// --- 
    /// This is a wrapper for Hathora SDK's `ExposedPort` model.
    /// TODO: Upgrade SDK models to natively support serialization
    /// </summary>
    [Serializable]
    public class ExposedPortSerializable : ContainerPortSerializable
    {
        [SerializeField, JsonProperty("host")]
        private string _host;
        public string Host
        {
            get => _host;
            set => _host = value;
        }

        
        public ExposedPortSerializable(ExposedPort _exposedPort)
            : base(_exposedPort)
        {
            if (_exposedPort == null)
                return;
            
            this.Host = _exposedPort.Host;
        }
        
        public virtual ExposedPort ToExposedPortType()
        {
            string containerName = this.GetTransportNickname(); // Should be "default"
            ExposedPort exposedPort = new()
            {
                TransportType = this.TransportType,
                Port = this.Port,
                Host = this.Host,
                Name = containerName,
            };

            return exposedPort;
        }

    }
}
