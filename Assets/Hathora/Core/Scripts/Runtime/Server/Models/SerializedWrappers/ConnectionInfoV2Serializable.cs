// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using HathoraCloud.Models.Shared;
using Newtonsoft.Json;
using UnityEngine;

namespace Hathora.Core.Scripts.Runtime.Server.Models.SerializedWrappers
{
    /// <summary>
    /// Hathora SDK model wrapper to allow serializable class/fields.
    /// 
    /// This is a wrapper for Hathora SDK's `ConnectionInfoV2` model.
    /// TODO: Upgrade SDK models to natively support serialization
    /// </summary>
    [Serializable]
    public class ConnectionInfoV2Serializable
    {
        [SerializeField, JsonProperty("_status")]
        private RoomReadyStatus _status;
        public RoomReadyStatus Status
        {
            get => _status;
            set => _status = value;
        }
        
        [SerializeField, JsonProperty("exposedPort")]
        private ExposedPortSerializable _exposedPort;
        public ExposedPort ExposedPort
        {
            get => _exposedPort.ToExposedPortType();
            set => _exposedPort = new ExposedPortSerializable(value);
        }
        
        [SerializeField, JsonProperty("additionalExposedPorts")]
        private List<ExposedPortSerializable> _additionalExposedPorts;
        public List<ExposedPort> AdditionalExposedPorts
        {
            get => _additionalExposedPorts?.ConvertAll(wrapper => 
                wrapper.ToExposedPortType());
            
            set => _additionalExposedPorts = value?.ConvertAll(val => 
                new ExposedPortSerializable(val));
        }
        
        [SerializeField, JsonProperty("roomId")]
        private string _roomId;
        public string RoomId
        {
            get => _roomId;
            set => _roomId = value;
        }


        public ConnectionInfoV2Serializable(ConnectionInfoV2 _connectionInfoV2)
        {
            if (_connectionInfoV2 == null)
                return;
            
            this.AdditionalExposedPorts = _connectionInfoV2.AdditionalExposedPorts;
            this.ExposedPort = _connectionInfoV2.ExposedPort;
            this.Status = _connectionInfoV2.Status;
            this.RoomId = _connectionInfoV2.RoomId;
        }

        public ConnectionInfoV2 ToConnectionInfoV2Type()
        {
            ConnectionInfoV2 connectionInfo = null;
            {
                connectionInfo = new ConnectionInfoV2
                {
                    AdditionalExposedPorts = this.AdditionalExposedPorts,
                    ExposedPort = this.ExposedPort,
                    Status = this.Status,
                    RoomId = this.RoomId,
                };
            }
            
            return connectionInfo;
        }
    }
}
