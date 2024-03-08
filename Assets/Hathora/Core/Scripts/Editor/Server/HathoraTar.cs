// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Editor.Common;
using Hathora.Core.Scripts.Runtime.Server.Models;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;

namespace Hathora.Core.Scripts.Editor.Server
{
    public static class HathoraTar
    {
        /// <summary>Overwrites any existing</summary>
        /// <param name="_paths"></param>
        /// <returns>pathToCopiedDockerfile</returns>
        private static string copyDockerfileTo(HathoraServerPaths _paths, string _destPath)
        {
            string pathToBuildDirDockerfile = $"{_destPath}/Dockerfile"; 
            
            File.Copy(
                _paths.PathToDotHathoraDockerfile,
                pathToBuildDirDockerfile,
                overwrite: true);

            return pathToBuildDirDockerfile;
        }

        /// <summary>
        /// Archives the build + Dockerfile into a .tar.gz file.
        /// - Excludes "*_DoNotShip" and "*_ButDontShipItWithYourGame" dirs.
        /// - Eg: "tar -czvf archive.tar.gz --exclude='*_DoNotShip' -C /path/to/dir ."
        /// </summary>
        /// <param name="_paths"></param>
        /// <param name="_cancelToken"></param>
        public static async Task ArchiveFilesAsTarGzToDotHathoraDir(
            HathoraServerPaths _paths, 
            CancellationToken _cancelToken)
        {
            string outputArchiveNameTarGz = $"{_paths.ExeBuildName}.tar.gz";
            string initWorkingDir = _paths.PathToDotHathoraDir; // .tar.gz will appear here
            string pathToOutputTarGz = $"{initWorkingDir}/{outputArchiveNameTarGz}";
            
            HathoraEditorUtils.ValidateCreateDotHathoraDir();
            HathoraEditorUtils.DeleteFileIfExists(pathToOutputTarGz);
            
            // (!) We temporarily copy the Dockerfile to project root so it's included at top-level of archive
            // This is due to the way the `tar` cmd literally handles the exact structure for things you add.
            string pathToCopiedDockerfile = copyDockerfileTo(_paths, _destPath: _paths.PathToUnityProjRoot);
            
            // ####################################################################################
            // Start from .hathora as working dir; use relative paths
            // -czvf:
            // c: Create a new archive.
            // z: Compress the archive with gzip. 
            // v: Verbosely list the files processed.
            // p: Permissions
            // f: Use archive file or device archive; eg: "{archiveNameWithoutExt}.tar.gz"
            // -C: Change to the specified directory before performing any operations.
            // -transform 's,^: Wraps the contents of the archive in a subdir
            // ####################################################################################
            // pwd 1st so the logs show where our working dir started
            const string cmd = "tar";
            
            // We don't use -v since it's too spammy; logs get truncated and you don't see the result
            string tarArgs = $"-czpf {outputArchiveNameTarGz} " +
                "--exclude \"*_DoNotShip\" " +
                "--exclude \"*_ButDontShipItWithYourGame\" " +
                "-C .. " + // Set working dir at parent of .hathora (unity proj root)
                $"{_paths.ExeBuildDirName} " + // Add build dir from proj root
                "Dockerfile"; // Add copied Dockerfile from proj root

            string cmdWithArgs = $"{cmd} {tarArgs}";
            (Process process, string resultLog) output = default;

            try
            {
                // Archive the build dir + Dockerfile together, excluding "*_DoNotShip"
                output = await HathoraEditorUtils.ExecuteCrossPlatformShellCmdAsync(
                    _workingDirPath: initWorkingDir,
                    cmdWithArgs,
                    _printLogs: true,
                    _cancelToken);
            }
            catch (TaskCanceledException)
            {
                Debug.Log($"[HathoraTar.ArchiveFilesAsTarGzToDotHathoraDir] Task cancelled.");
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"[HathoraTar.ArchiveFilesAsTarGzToDotHathoraDir] Error " +
                    $"awaiting {nameof(HathoraEditorUtils.ExecuteCrossPlatformShellCmdAsync)}: " +
                    $"<color=yellow>{e}</color>");
                throw;
            }
            
            // Delete the Dockerfile we copied to project root
            HathoraEditorUtils.DeleteFileIfExists(pathToCopiedDockerfile);
            
            // Assert success
            Assert.AreEqual(0, output.process.ExitCode, 
                "Error in `tar` cmd; check logs for details.");
        }
    }
}
