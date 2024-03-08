// Created by dylan@hathora.dev

using System.IO;
using Hathora.Core.Scripts.Runtime.Common.Utils;

namespace Hathora.Core.Scripts.Runtime.Server.Models
{
    /// <summary>
    /// Container for all the many paths to prep/upload/deploy a Hathora server build.
    /// </summary>
    public class HathoraServerPaths
    {
        public const string HathoraConsoleAppBaseUrl = "https://console.hathora.dev/application/";
        
        public readonly HathoraServerConfig UserConfig;
        public readonly string DotHathoraDirName;
        public readonly string PathToUnityProjRoot;
        public readonly string PathToDotHathoraDir;
        public readonly string PathToBuildExe;
        public readonly string PathToBuildDir;
        public readonly string PathToDotHathoraDockerfile;
        public readonly string PathToDotHathoraTarGz;

        public string ExeBuildName => UserConfig.LinuxHathoraAutoBuildOpts.ServerBuildExeName;
        public string ExeBuildDirName => UserConfig.LinuxHathoraAutoBuildOpts.ServerBuildDirName;

        public HathoraServerPaths(HathoraServerConfig userConfig)
        {
            this.UserConfig = userConfig;
            this.DotHathoraDirName = ".hathora";
            this.PathToUnityProjRoot = HathoraUtils.GetNormalizedPathToProjRoot(); // Path slashes normalized
            this.PathToDotHathoraDir = HathoraUtils.NormalizePath(Path.Combine(PathToUnityProjRoot, DotHathoraDirName));
            this.PathToBuildExe = UserConfig.GetNormalizedPathToBuildExe();
            this.PathToBuildDir = UserConfig.GetNormalizedPathToBuildDir();
            
            this.PathToDotHathoraDockerfile = HathoraUtils.NormalizePath(Path.Join(
                PathToDotHathoraDir, 
                "Dockerfile"));
            
            PathToDotHathoraTarGz = HathoraUtils.NormalizePath(Path.Join(
                PathToDotHathoraDir,
                $"{ExeBuildName}.tar.gz"));
        }
    }
}
