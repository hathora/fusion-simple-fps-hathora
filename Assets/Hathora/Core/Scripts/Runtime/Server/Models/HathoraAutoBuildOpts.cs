// Created by dylan@hathora.dev

using System;
using System.Text;
using HathoraCloud.Models.Shared;
using Newtonsoft.Json;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Build.Reporting;
#endif // UNITY_EDITOR

namespace Hathora.Core.Scripts.Runtime.Server.Models
{
    /// <summary>
    /// This is technically an editor script, but we #if directive UNITY_EDITOR for those parts
    /// </summary>
    [Serializable]
    public class HathoraAutoBuildOpts
    {
        #region Persisted
        // Private Serialized
        /// <summary>Default: Build-Server</summary>
        [SerializeField]
        private string _serverBuildDirName = "Build-Server";

        /// <summary>Default: Build-Server</summary>
        public string ServerBuildDirName
        {
            get => _serverBuildDirName;
            set => _serverBuildDirName = value;
        }
        
        public bool HasServerBuildDirName =>
           !string.IsNullOrEmpty(ServerBuildDirName);

        /// <summary>Default: Hathora-Unity-LinuxServer.x86_64</summary>
        [SerializeField]
        private string _serverBuildExeName = "Hathora-Unity_LinuxServer.x86_64";
        
        /// <summary>Default: Hathora-Unity-LinuxServer.x86_64</summary>
        public string ServerBuildExeName
        {
            get => _serverBuildExeName;
            set => _serverBuildExeName = value;
        }
        

        public bool HasServerBuildExeName =>
            !string.IsNullOrEmpty(ServerBuildExeName);

        
        /// <summary>The same as checking 'Developer Build' in build opts</summary>
        [SerializeField]
        private bool _isDevBuild = true;
        
        /// <summary>The same as checking 'Developer Build' in build opts</summary>
        public bool IsDevBuild
        {
            get => _isDevBuild;
            set => _isDevBuild = value;
        }
        
        
        /// <summary>If an old build exists, first delete this dir?</summary>
        [SerializeField]
        private bool _cleanBuildDir = true;

        /// <summary>If an old build exists, first delete this dir?</summary>
        public bool CleanBuildDir
        {
            get => _cleanBuildDir;
            set => _cleanBuildDir = value;
        }
        
        
        [SerializeField]
        private bool _overwriteDockerfile = true;
        
        /// <summary>
        /// If you have edited the generated Dockerfile or need to use a
        /// custom Dockerfile, this should be set to 'false'
        /// </summary>
        public bool OverwriteDockerfile
        {
            get => _overwriteDockerfile;
            set => _overwriteDockerfile = value;
        }
        #endregion // Persisted
        
        public enum ScriptingBackend
        {
            [JsonProperty("mono")]
            Mono,
            [JsonProperty("il2cpp")]
            IL2CPP,
        }
        #region Scripting Backend
        [SerializeField]
        private int _scriptingBackendIndex = (int)ScriptingBackend.Mono;
        public int ScriptingBackendIndex
        {
            get => _scriptingBackendIndex;
            set => _scriptingBackendIndex = value;
        }

        public ScriptingBackend SelectedScriptingBackend => 
            (ScriptingBackend)_scriptingBackendIndex;
        #endregion // Scripting Backend

        
        #region Session Only (!Persistence)
        #if UNITY_EDITOR
        private BuildReport _lastBuildReport;
        public BuildReport LastBuildReport
        {
            get => _lastBuildReport;
            set => _lastBuildReport = value;
        }
        #endif // UNITY_EDITOR

        
        private StringBuilder _lastBuildLogsStrb = new();
        public StringBuilder LastBuildLogsStrb
        {
            get => _lastBuildLogsStrb; 
            set => _lastBuildLogsStrb = value;
        }
        
        [SerializeField]
        private string _lastBuildLogsStr = "";
        public string LastBuildLogsStr
        {
            get => _lastBuildLogsStr; 
            set => _lastBuildLogsStr = value;
        }

        public bool HasLastBuildLogsStrb => 
            LastBuildLogsStrb?.Length > 0 || LastBuildLogsStr?.Length > 0;
        #endregion // Session Only (!Persistence)
    }
}
