// Created by dylan@hathora.dev

using System;
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
    /// This is a wrapper for Hathora SDK's `ApplicationWithDeployment` model.
    /// TODO: Upgrade SDK models to natively support serialization
    /// </summary>
    [Serializable]
    public class ApplicationWithDeploymentSerializable
    {
        [SerializeField, JsonProperty("appId")]
        private string _appId;
        public string AppId 
        { 
            get => _appId;
            set => _appId = value;
        }
        
        /// <summary>Serialized from `DateTime`.</summary>
        [SerializeField, JsonProperty("createdAt")]
        private string _createdAtWrapper;
        
        /// <summary>Serialized from `DateTime`.</summary>
        public DateTime CreatedAt
        {
            get => DateTime.Parse(_createdAtWrapper); 
            set => _createdAtWrapper = value.ToString(CultureInfo.InvariantCulture);
        }
        
        [SerializeField, JsonProperty("createdBy")]
        private string _createdBy;
        public string CreatedBy 
        { 
            get => _createdBy;
            set => _createdBy = value;
        }
        
        [SerializeField, JsonProperty("deletedBy")]
        private string _deletedBy;
        public string DeletedBy 
        { 
            get => _deletedBy;
            set => _deletedBy = value;
        }


        /// <summary>Serialized from `DateTime`.</summary>
        [SerializeField, JsonProperty("deletedAt")]
        private string _deletedAtWrapper;
        
        /// <summary>Serialized from `DateTime`.</summary>
        public DateTime? DeletedAt
        {
            get => DateTime.Parse(_deletedAtWrapper);
            set => _deletedAtWrapper = value.ToString();
        }
 
        [SerializeField, JsonProperty("orgId")]
        private string _orgId;
        public string OrgId 
        { 
            get => _orgId;
            set => _orgId = value;
        }
        
        [FormerlySerializedAs("_authConfigurationWrapper")]
        [SerializeField, JsonProperty("authConfiguration")]
        private ApplicationAuthConfigurationSerializable authConfigurationSerializable;
        public AuthConfiguration AuthConfiguration 
        { 
            get => authConfigurationSerializable.ToApplicationAuthConfigurationType();
            set => authConfigurationSerializable = new ApplicationAuthConfigurationSerializable(value);
        }
        
        [SerializeField, JsonProperty("appSecret")]
        private string _appSecret;
        public string AppSecret 
        { 
            get => _appSecret;
            set => _appSecret = value;
        }
        
        [SerializeField, JsonProperty("appName")]
        private string _appName;
        public string AppName 
        { 
            get => _appName;
            set => _appName = value;
        }
        
         
        [FormerlySerializedAs("_deploymentWrapper")]
        [SerializeField, JsonProperty("deployment")]
        private DeploymentSerializable deploymentSerializable;
        public Deployment Deployment 
        { 
            get
            {
                // Ensure !null and minimally validate
                if (string.IsNullOrEmpty(deploymentSerializable?.AppId))
                    return null;
                
                return deploymentSerializable.ToDeploymentType();
            }
            
            set => deploymentSerializable = value == null 
                ? null 
                : new DeploymentSerializable(value);
        }

        
        public ApplicationWithDeploymentSerializable(ApplicationWithLatestDeploymentAndBuild _appWithDeployment)
        {
            if (_appWithDeployment == null)
                return;

            this.AppId = _appWithDeployment.AppId;
            this.AppName = _appWithDeployment.AppName;
            this.CreatedAt = _appWithDeployment.CreatedAt;
            this.DeletedBy = _appWithDeployment.DeletedBy; 
            this.DeletedAt = _appWithDeployment.DeletedAt;
            this.AppSecret = _appWithDeployment.AppSecret;
            this.OrgId = _appWithDeployment.OrgId;
            this.Deployment = _appWithDeployment.Deployment;
            this.AuthConfiguration = _appWithDeployment.AuthConfiguration;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public ApplicationWithLatestDeploymentAndBuild ToApplicationWithDeploymentType()
        {
            ApplicationWithLatestDeploymentAndBuild appWithDeploy = new()
            {
                DeletedBy = DeletedBy,
                DeletedAt = DeletedAt,
                CreatedAt = CreatedAt,
                CreatedBy = CreatedBy,
                OrgId = OrgId,
                AuthConfiguration = AuthConfiguration,
                AppSecret = AppSecret,
                AppId = AppId,
                AppName = AppName,
                Deployment = Deployment,
            };
            
            return appWithDeploy;
        }
    }
}
