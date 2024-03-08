// Created by dylan@hathora.dev

using System;
using HathoraCloud.Models.Shared;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hathora.Core.Scripts.Runtime.Server.Models.SerializedWrappers
{
    /// <summary>
    /// Hathora SDK model wrapper to allow serializable class/fields.
    /// 
    /// Set transport configurations for where the server will listen.
    /// --- 
    /// This is a wrapper for Hathora SDK's `ContainerPortSerializable` model.
    /// TODO: Upgrade SDK models to natively support serialization
    /// </summary>
    [Serializable]
    public class ContainerPortSerializable
    {
        /// <summary>(!) Hathora SDK Enums starts at index 1; not 0: Care of indexes</summary>
        [SerializeField, JsonProperty("transportType")]
        private TransportType _transportType = TransportType.Udp;
       
        /// <summary>(!) Hathora SDK Enums starts at index 1; not 0: Care of indexes</summary>
        public TransportType TransportType
        {
            get => _transportType;
            set => _transportType = value;
        }    
        

        [FormerlySerializedAs("_portNumber")]
        [SerializeField, Range(1024, 65535), JsonProperty("portNumber")]
        private int port = 7777;
        public int Port
        {
            get => port;
            set => port = value;
        }
        
        public ContainerPortSerializable()
        {
        }

        /// <summary>Handle "Host" in child.</summary>
        /// <param name="_exposedPort"></param>
        public ContainerPortSerializable(ExposedPort _exposedPort)
        {
            if (_exposedPort == null)
                return;
            
            this._transportType = _exposedPort.TransportType;
            this.port = (int)_exposedPort.Port;
            // this.Nickname = _exposedPort.Name; // Always "default": See GetTransportNickname()
        }
        
        /// <summary>Handle Nickname in child.</summary>
        /// <param name="_containerPort"></param>
        public ContainerPortSerializable(ContainerPort _containerPort)
        {
            if (_containerPort == null)
                return;
            
            this._transportType = _containerPort.TransportType;
            this.port = _containerPort.Port;
        }
        
        /// <summary>
        /// Override this if you want the name to be custom. 
        /// </summary>
        /// <returns></returns>
        public virtual string GetTransportNickname() => "default";

        public virtual ContainerPort ToContainerPortType()
        {
            string containerName = this.GetTransportNickname();
            
            ContainerPort containerPort = new()
            {
                TransportType = this.TransportType,
                Port = this.Port,
                Name = containerName,
            };

            return containerPort;
        }

    }
}
