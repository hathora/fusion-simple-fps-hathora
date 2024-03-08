// Created by dylan@hathora.dev

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using HathoraCloud.Models.Shared;
using Newtonsoft.Json;
using UnityEngine;
using Application = UnityEngine.Application;

namespace Hathora.Core.Scripts.Runtime.Common.Utils
{
    public static class HathoraUtils
    {
        public const Region DEFAULT_REGION = Region.Seattle;

        /// <summary>
        /// eg: "E1HKfn68Pkms5zsZsvKONw=="
        /// https://stackoverflow.com/a/9279005 
        /// </summary>
        /// <returns></returns>
        public static string GenerateShortUid(bool omitEndDblEquals)
        {
            string shortId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            
            if (omitEndDblEquals && shortId.EndsWith("=="))
                shortId = shortId[..^2]; // Exclude the last 2 chars

            Debug.Log($"[HathoraUtils] ShortId Generated: {shortId}");

            return shortId;
        }

        public static string NormalizePath(string _path) =>
            Path.GetFullPath(_path);

        /// <summary>
        /// Gets path to Unity proj root, then normalizes the/path/slashes.
        /// </summary>
        /// <returns></returns>
        public static string GetNormalizedPathToProjRoot()
        {
            string dirtyPathToUnityProjRoot = Directory.GetParent(Application.dataPath)?.ToString();
            return dirtyPathToUnityProjRoot == null 
                ? null 
                : NormalizePath(dirtyPathToUnityProjRoot);
        }

        /// <summary>Returns null on null || MinValue</summary>
        public static string GetFriendlyDateTimeShortStr(DateTime? _dateTime)
        {
            if (_dateTime == null || _dateTime == DateTime.MinValue)
                return null;

            return $"{_dateTime.Value.ToShortDateString()} {_dateTime.Value.ToShortTimeString()}";
        }

        /// <summary>Returns null on null || MinValue</summary>
        public static string GetFriendlyDateTimeDiff(
            TimeSpan _duration, 
            bool _exclude0)
        {
            int totalHours = (int)_duration.TotalHours;
            int totalMinutes = (int)_duration.TotalMinutes % 60;
            int totalSeconds = (int)_duration.TotalSeconds % 60;
            
            if (totalHours > 0 || !_exclude0)
                return $"{totalHours}h:{totalMinutes}m:{totalSeconds}s";
            
            return totalMinutes > 0 
                ? $"{totalMinutes}m:{totalSeconds}s" 
                : $"{totalSeconds}s";
        }


        /// <summary>
        /// </summary>
        /// <param name="_startTime"></param>
        /// <param name="_endTime"></param>
        /// <param name="exclude0">If 0, </param>
        /// <returns>hh:mm:ss</returns>
        public static string GetFriendlyDateTimeDiff(
            DateTime _startTime, 
            DateTime _endTime,
            bool exclude0)
        {
            TimeSpan duration = _endTime - _startTime;

            return GetFriendlyDateTimeDiff(duration, exclude0);
        }
        
        /// <summary>
        /// Useful for creating a deep copy of a class obj. For example, with the Hathora Sdk Config.
        /// JSON serialization: Similar to binary serialization, but uses JSON as an intermediary format.
        /// It's simpler and doesn't require [Serializable] attribute but might be slower and
        /// has limitations with some complex types.
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DeepCopy<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>This can return more than 1 IP, but we just return the 1st</summary>
        /// <param name="_host"></param>
        /// <returns></returns>
        public static async Task<IPAddress> ConvertHostToIpAddress(string _host)
        {
            IPAddress[] ips = await Dns.GetHostAddressesAsync(_host);
            return ips.FirstOrDefault();
        }

        /// <summary>
        /// eg: "127.0.0.1:7777", "localhost:7777", "1.proxy.hathora.dev:7777",
        /// </summary>
        /// <returns></returns>
        public static string GetHostIpPortPatternStr() =>
            @"^((localhost:[0-9]{1,5})|(([\w-]+(\.\w+)*\.[a-zA-Z]{2,})|(\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b)):[0-9]{1,5})$";
    }
}