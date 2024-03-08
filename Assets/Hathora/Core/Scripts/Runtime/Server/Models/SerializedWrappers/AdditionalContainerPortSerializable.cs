// Created by dylan@hathora.dev

using System;
using HathoraCloud.Models.Shared;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hathora.Core.Scripts.Runtime.Server.Models.SerializedWrappers
{
    /// <summary>
    /// Hathora SDK model wrapper to allow serializable class/fields.
    /// 
    /// Set transport configurations for where the server will listen.
    /// Unlike ContainerPortSerializable, here, you can customize the nickname (instead of "default").
    /// Leave the nickname null and we'll ignore this class.
    /// ---
    /// This is a wrapper for Hathora SDK's `ApplicationWithDeployment` model.
    /// TODO: Upgrade SDK models to natively support serialization
    /// </summary>
    [Serializable]
    public class AdditionalContainerPortSerializable : ContainerPortSerializable
    {
        [SerializeField, JsonProperty("name")]
        private string _transportNickname;
        public string TransportNickname
        {
            get => GetTransportNickname();
            set => _transportNickname = value;
        }
        
        public AdditionalContainerPortSerializable()
        {
        }

        public AdditionalContainerPortSerializable(ContainerPort _containerPort)
            : base(_containerPort)
        {
            this._transportNickname = _containerPort.Name;
        }

        /// <summary>
        /// Override this if you want the name to be custom
        /// </summary>
        /// <returns></returns>
        public override string GetTransportNickname()
        {
            Assert.IsFalse(_transportNickname == "default", 
                "Extra Transport nickname cannot be 'default' (reserved)");
            
            return _transportNickname;   
        }

        public override ContainerPort ToContainerPortType()
        {
            ContainerPort containerPort = base.ToContainerPortType();
            containerPort.Name = GetTransportNickname();

            return containerPort;
        }
    }
}
