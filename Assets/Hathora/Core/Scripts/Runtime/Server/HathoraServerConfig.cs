// Created by dylan@hathora.dev

using System.IO;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using Hathora.Core.Scripts.Runtime.Server.Models;
using UnityEngine;

namespace Hathora.Core.Scripts.Runtime.Server
{
    /// <summary>
    /// The primary config file for Hathora Server, including AppId and dev auth token.
    /// - See top menu `Hathora/ConfigFInder` to create or find a config, or create a new one in Project right-click menu.
    /// - The default ".template" file is included, but with a warning to duplicate and .gitignore it.
    /// - There can be multiple configs; the recommended use is 1-per-stage. Eg: "dev", "staging", "prod".
    /// - Sensitive info will not be included in Client builds.
    /// - For meta objects (like the banner and btns), see `HathoraConfigUI`.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(HathoraServerConfig), menuName = "Hathora/ServerConfig File")]
    public class HathoraServerConfig : ScriptableObject
    {
        #region Vars
        // ----------------------------------------
        [SerializeField]
        private HathoraCoreOpts _hathoraCoreOpts = new();
        public HathoraCoreOpts HathoraCoreOpts
        {
            get => _hathoraCoreOpts;
            set => _hathoraCoreOpts = value;
        }

        [SerializeField]
        private HathoraAutoBuildOpts _linuxHathoraAutoBuildOpts = new();
        public HathoraAutoBuildOpts LinuxHathoraAutoBuildOpts
        {
            get => _linuxHathoraAutoBuildOpts;
            set => _linuxHathoraAutoBuildOpts = value;
        }

        [SerializeField] 
        private HathoraDeployOpts _hathoraDeployOpts = new();
        public HathoraDeployOpts HathoraDeployOpts
        {
            get => _hathoraDeployOpts;
            set => _hathoraDeployOpts = value;
        }
        
        [SerializeField]
        private HathoraLobbyRoomOpts _hathoraLobbyRoomOpts = new();
        public HathoraLobbyRoomOpts HathoraLobbyRoomOpts
        {
            get => _hathoraLobbyRoomOpts;
            set => _hathoraLobbyRoomOpts = value;
        }
        #endregion // Vars


        /// <summary>(!) Don't use OnEnable for ScriptableObjects</summary>
        private void OnValidate()
        {
        }

        public bool MeetsBuildBtnReqs() =>
            !string.IsNullOrEmpty(_linuxHathoraAutoBuildOpts.ServerBuildDirName) &&
            !string.IsNullOrEmpty(_linuxHathoraAutoBuildOpts.ServerBuildExeName);
                                                          
        public bool MeetsDeployBtnReqs() =>
            !string.IsNullOrEmpty(_hathoraCoreOpts.AppId) &&
            _hathoraCoreOpts.DevAuthOpts.HasAuthToken &&
            !string.IsNullOrEmpty(_linuxHathoraAutoBuildOpts.ServerBuildDirName) &&
            !string.IsNullOrEmpty(_linuxHathoraAutoBuildOpts.ServerBuildExeName) &&
            _hathoraDeployOpts.ContainerPortSerializable.Port >= 1024;
        
        /// <summary>
        /// For Editor only: You may also want to check if you are !HathoraServerDeploy.IsDeploying
        /// </summary>
        /// <returns></returns>
        public bool MeetsBuildAndDeployBtnReqs() =>
            MeetsBuildBtnReqs() &&
            MeetsDeployBtnReqs();

        /// <returns>meetsCreateRoomBtnReqs</returns>
        public bool MeetsCreateRoomBtnReqs() =>
            HathoraCoreOpts.HasAppId;

        /// <summary>
        /// Combines path, then normalizes
        /// </summary>
        /// <returns></returns>
        public string GetNormalizedPathToBuildExe() => Path.GetFullPath(Path.Combine(
            GetNormalizedPathToBuildDir(), 
            _linuxHathoraAutoBuildOpts.ServerBuildExeName));

        public string GetNormalizedPathToBuildDir() => Path.GetFullPath(Path.Combine(
            HathoraUtils.GetNormalizedPathToProjRoot(), 
            _linuxHathoraAutoBuildOpts.ServerBuildDirName));
    }
}