// Created by dylan@hathora.dev

using System;
using System.Globalization;
using HathoraCloud.Models.Shared;
using Newtonsoft.Json;
using UnityEngine;

namespace Hathora.Core.Scripts.Runtime.Server.Models.SerializedWrappers
{
    /// <summary>
    /// Hathora SDK model wrapper to allow serializable class/fields.
    /// 
    /// This is a wrapper for Hathora SDK's `RoomCurrentAllocationSerializable` model.
    /// TODO: Upgrade SDK models to natively support serialization
    /// </summary>
    [Serializable]
    public class RoomCurrentAllocationSerializable
    {
        /// <summary>
        /// (!) Originally a nullable DateTime, but Unity's SerializeField
        /// doesn't support nullable types.
        /// </summary>
        [SerializeField, JsonProperty("unscheduledAt")]
        private string _unscheduledAtDateTimeWrapper;
        public DateTime UnscheduledAt
        { 
            get => DateTime.TryParse(_unscheduledAtDateTimeWrapper, out DateTime parsedDateTime) 
                ? parsedDateTime 
                : DateTime.MinValue;

            set => _unscheduledAtDateTimeWrapper = value.ToString(CultureInfo.InvariantCulture);
        }

        [SerializeField, JsonProperty("scheduledAt")]
        private string _scheduledAtDateTimeWrapper;
        public DateTime ScheduledAt
        { 
            get => DateTime.TryParse(_scheduledAtDateTimeWrapper, out DateTime parsedDateTime) 
                ? parsedDateTime 
                : DateTime.MinValue;

            set => _scheduledAtDateTimeWrapper = value.ToString(CultureInfo.InvariantCulture);
        }

        [SerializeField, JsonProperty("processId")]
        private string _processId;
        public string ProcessId
        { 
            get => _processId;
            set => _processId = value;
        }

        [SerializeField, JsonProperty("roomAllocationId")]
        private string _roomAllocationId;
        public string RoomAllocationId
        { 
            get => _roomAllocationId;
            set => _roomAllocationId = value;
        }

        public RoomCurrentAllocationSerializable(CurrentAllocation _roomAllocation)
        {
            if (_roomAllocation == null)
                return;
            
            this.UnscheduledAt = _roomAllocation.UnscheduledAt ?? DateTime.MaxValue;
            this.ScheduledAt = _roomAllocation.ScheduledAt;
            this.ProcessId = _roomAllocation.ProcessId;
            this.RoomAllocationId = _roomAllocation.RoomAllocationId;
        }

        public CurrentAllocation ToRoomCurrentAllocationType()
        {
            CurrentAllocation roomCurrentAllocation = new()
            {
                UnscheduledAt = UnscheduledAt,
                ScheduledAt = ScheduledAt,
                ProcessId = ProcessId,
                RoomAllocationId = RoomAllocationId,
            };
            
            return roomCurrentAllocation;
        }
    }
}