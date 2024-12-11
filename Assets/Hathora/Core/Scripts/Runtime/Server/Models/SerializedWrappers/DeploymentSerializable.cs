// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Globalization;
using HathoraCloud.Models.Shared;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hathora.Core.Scripts.Runtime.Server.Models.SerializedWrappers
{
    /// <summary>
    /// Hathora SDK model wrapper to allow serializable class/fields.
    /// 
    /// This is a wrapper for Hathora SDK's `Deployment` model.
    /// TODO: Upgrade SDK models to natively support serialization
    /// </summary>
    [Serializable]
    public class DeploymentSerializable
    {
        // [SerializeField, JsonProperty("planName")]
        // private PlanName _planName;
        // public PlanName PlanName 
        // { 
        //     get => _planName;
        //     set => _planName = value;
        // }
        
        [SerializeField, JsonProperty("env")] // TODO
        private List<ApplicationWithLatestDeploymentAndBuildEnv> _env;
        public List<ApplicationWithLatestDeploymentAndBuildEnv> Env
        {
            get => _env;
            set => _env = value;

        }
        
        [SerializeField, JsonProperty("roomsPerProcess")]
        private int _roomsPerProcess;
        public int RoomsPerProcess 
        { 
            get => _roomsPerProcess;
            set => _roomsPerProcess = value;
        }
        
        
        /// <summary>Parses from List of ContainerPortSerializable</summary>
        [SerializeField, JsonProperty("additionalContainerPorts")]
        private List<AdditionalContainerPortSerializable> _additionalContainerPorts;

        /// <summary>Parses from List of ContainerPortSerializable</summary>
        public List<ContainerPort> AdditionalContainerPorts
        {
            get => _additionalContainerPorts?.ConvertAll(wrapper => 
                wrapper.ToContainerPortType());
            
            set => _additionalContainerPorts = value?.ConvertAll(val => 
                new AdditionalContainerPortSerializable(val));
        }
        

        /// <summary>Parses from ContainerPortSerializable</summary>
        [FormerlySerializedAs("_defaultContainerPortWrapper")]
        [SerializeField, JsonProperty("defaultContainerPort")]
        private ContainerPortSerializable defaultContainerPortSerializable = new();
        public ContainerPort DefaultContainerPort 
        { 
            get => defaultContainerPortSerializable?.ToContainerPortType();
            set => defaultContainerPortSerializable = new ContainerPortSerializable(value);
        }
        
        
        /// <summary>Serialized from `DateTime`.</summary>
        [SerializeField, JsonProperty("createdAt")]
        private string _createdAtWrapper;
        
        /// <summary>Serialized from `DateTime`.</summary>
        public DateTime CreatedAt
        {
            get => DateTime.TryParse(_createdAtWrapper, out DateTime parsedDateTime) 
                ? parsedDateTime 
                : DateTime.MinValue;
            
            set => _createdAtWrapper = value.ToString(CultureInfo.InvariantCulture);
        }
        
        
        [SerializeField, JsonProperty("createdBy")]
        private string _createdBy;
        public string CreatedBy
        {
            get => _createdBy ?? "";
            set => _createdBy = value;
        }
        
        [SerializeField, JsonProperty("requestedMemoryMB")]
        private double _requestedMemoryMB;
        public double RequestedMemoryMB 
        { 
            get => _requestedMemoryMB;
            set => _requestedMemoryMB = value;
        }
        
        [SerializeField, JsonProperty("requestedCPU")]
        private double _requestedCPU;
        public double RequestedCPU 
        { 
            get => _requestedCPU;
            set => _requestedCPU = value;
        }
        
        [SerializeField, JsonProperty("deploymentId")]
        private string _deploymentId;
        public string DeploymentId 
        { 
            get => _deploymentId;
            set => _deploymentId = value;
        }
        
        [SerializeField, JsonProperty("buildId")]
        private string _buildId;
        public string BuildId 
        { 
            get => _buildId;
            set => _buildId = value;
        }
        
        [SerializeField, JsonProperty("appId")]
        private string _appId;

        public string AppId 
        { 
            get => _appId;
            set => _appId = value;
        }
        
        
        public DeploymentSerializable(Deployment _deployment)
        {
            if (_deployment == null)
                return;

            // this.PlanName = _deployment.PlanName;
            this.RoomsPerProcess = _deployment.RoomsPerProcess;
            this.defaultContainerPortSerializable = new ContainerPortSerializable(_deployment.DefaultContainerPort);
            this.AdditionalContainerPorts = _deployment.AdditionalContainerPorts;
            this.CreatedAt = _deployment.CreatedAt;
            this.CreatedBy = _deployment.CreatedBy;
            this.RequestedMemoryMB = _deployment.RequestedMemoryMB;
            this.RequestedCPU = _deployment.RequestedCPU;
            this.DeploymentId = _deployment.DeploymentId;
            this.BuildId = _deployment.BuildId;
            this.AppId = _deployment.AppId;
            this.Env = _deployment.Env;
        }

        public Deployment ToDeploymentType()
        {
            Deployment deployment = new()
            {
                // PlanName = this.PlanName,
                RoomsPerProcess = this.RoomsPerProcess,
                DefaultContainerPort = this.DefaultContainerPort,
                AdditionalContainerPorts = this.AdditionalContainerPorts,
                CreatedAt = this.CreatedAt,
                CreatedBy = this.CreatedBy,
                RequestedMemoryMB = this.RequestedMemoryMB,
                RequestedCPU = this.RequestedCPU,
                DeploymentId = this.DeploymentId,
                BuildId = this.BuildId,
                AppId = this.AppId,
                Env = this.Env,
            };

            return deployment;
        }
    }
}
