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
    /// This is a wrapper for Hathora SDK's `Room` model.
    /// We'll eventually upgrade the SDK model to natively support this.
    /// </summary>
    [Serializable]
    public class RoomSerializable
    {
        [SerializeField, JsonProperty("appId")]
        private string _appId;
        public string AppId
        {
            get => _appId;
            set => _appId = value;
        }
        
        [SerializeField, JsonProperty("status")]
        private RoomStatus _status = RoomStatus.Destroyed;
        public RoomStatus Status
        {
            get => _status;
            set => _status = value;
        }
        
        [SerializeField, JsonProperty("currentAllocation")]
        private RoomCurrentAllocationSerializable _currentCurrentAllocationSerializable;
        public CurrentAllocation CurrentAllocation
        {
            get => _currentCurrentAllocationSerializable?.ToRoomCurrentAllocationType();
            set => _currentCurrentAllocationSerializable = new RoomCurrentAllocationSerializable(value);
        }

        [SerializeField, JsonProperty("allocations")]
        private List<RoomAllocationSerializable> _allocationsWrapper = new();
        public List<RoomAllocation> Allocations
        {
            get => _allocationsWrapper?.ConvertAll(wrapper => 
                wrapper.ToRoomAllocationType());
            
            set => _allocationsWrapper = value?.ConvertAll(val => 
                new RoomAllocationSerializable(val));
        }

        [SerializeField, JsonProperty("roomId")]
        private string _roomId;
        public string RoomId
        {
            get => _roomId;
            set => _roomId = value;
        }


        public RoomSerializable(Room _room)
        {
            if (_room == null)
                return;
            
            this.AppId = _room.AppId;
            this.Status = _room.Status;
            this.CurrentAllocation = _room.CurrentAllocation;
            this.Allocations = _room.Allocations;
            this.RoomId = _room.RoomId;
        }

        public Room ToRoomType()
        {
            Room room = new()
            {
                CurrentAllocation = CurrentAllocation,
                Status = Status,
                Allocations = Allocations,
                RoomId = RoomId,
                AppId = AppId,
            };
         
            return room;
        }
    }
}
