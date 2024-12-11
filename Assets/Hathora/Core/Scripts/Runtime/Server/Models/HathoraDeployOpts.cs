// dylan@hathora.dev

using System;
using System.Text;
using Hathora.Core.Scripts.Runtime.Server.Models.SerializedWrappers;
using HathoraCloud.Models.Shared;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hathora.Core.Scripts.Runtime.Server.Models
{
    [Serializable]
    public class HathoraDeployOpts
    {
        #region Rooms Per Process
        /// <summary>Default: 1. How many rooms do you want to support per server?</summary>
        [SerializeField]
        private int _roomsPerProcess = 1;

        /// <summary>Default: 1. How many rooms do you want to support per server?</summary>
        public int RoomsPerProcess
        {
            get => _roomsPerProcess;
            set => _roomsPerProcess = value;
        }
        #endregion // Rooms Per Process
        
        
        #region Plan Name
        [SerializeField]
        private int _planNameIndexIndex = (int)PlanName.Tiny;
        public int PlanNameIndex
        {
            get => _planNameIndexIndex;
            set => _planNameIndexIndex = value;
        }

        public PlanName PlanName => 
            (PlanName)_planNameIndexIndex;
        #endregion // Plan Name
        
        
        #region Container Port
        /// <summary>Default: Tiny. Billing Option: You only get charged for active rooms.</summary>
        [SerializeField]
        private ContainerPortSerializable _containerPortSerializableSerializable = new();

        /// <summary>Default: Tiny. Billing Option: You only get charged for active rooms.</summary>
        public ContainerPortSerializable ContainerPortSerializable
        {
            get => _containerPortSerializableSerializable;
            set => _containerPortSerializableSerializable = value;
        }
        #endregion // Container Port


        #region Transport Type 
        [SerializeField]
        private int _transportTypeIndex = (int)TransportType.Udp;
        public int TransportTypeIndex
        {
            get => _transportTypeIndex;
            set => _transportTypeIndex = value;
        }
        
        public TransportType TransportType => 
            (TransportType)_transportTypeIndex;
        #endregion // Transport Type 

        
        #region Last Deployment
        /// <summary>If you deployed something, we set the cached result</summary>
        // [SerializeField] // TODO: Make serializable. For now, this won't persist between Unity sessions.
        private DeploymentV3 _lastDeployment;
        
        /// <summary>If you deployed something, we set the cached result</summary>
        public DeploymentV3 LastDeployment
        {
            get => _lastDeployment;
            set => _lastDeployment = value;
        }
        
        private StringBuilder _lastDeployLogsStrb = new();
        public StringBuilder LastDeployLogsStrb
        {
            get => _lastDeployLogsStrb; 
            set => _lastDeployLogsStrb = value;
        }
        public bool HasLastDeployLogsStrb => 
            LastDeployLogsStrb?.Length > 0;
        #endregion // Last Deployment
    }
}
