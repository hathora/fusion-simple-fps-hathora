// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Runtime.Server;
using Hathora.Core.Scripts.Runtime.Server.Models;
using HathoraCloud.Models.Shared;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hathora.Core.Scripts.Editor.Server.ConfigStyle.PostAuth
{
    public class HathoraConfigPostAuthBodyBuildUI : HathoraConfigUIBase
    {
        #region Vars
        // Foldouts
        private bool isBuildFoldout;
        private CancellationTokenSource cancelBuildTokenSrc;
        
        /// <summary>For state persistence on which dropdown group was last clicked</summary>
        protected const string SERVER_BUILD_SETTINGS_FOLDOUT_STATE_KEY = "ServerBuildSettingsFoldoutState";
        #endregion // Vars


        #region Init
        public HathoraConfigPostAuthBodyBuildUI(
            HathoraServerConfig _serverConfig, 
            SerializedObject _serializedConfig)
            : base(_serverConfig, _serializedConfig)
        {
        }
        #endregion // Init
        
        
        #region UI Draw
        public void Draw()
        {
            if (!IsAuthed)
                return; // You should be calling HathoraConfigPreAuthBodyUI.Draw()

            insertServerBuildSettingsFoldout();
        }

        private void insertServerBuildSettingsFoldout()
        {
            // Retrieve the saved foldout state from EditorPrefs
            isBuildFoldout = EditorPrefs.GetBool(
                SERVER_BUILD_SETTINGS_FOLDOUT_STATE_KEY, 
                defaultValue: false);
            
            // Create the foldout
            isBuildFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(
                isBuildFoldout,
                "Server Build Settings");
            
            // Save the new foldout state to EditorPrefs
            EditorPrefs.SetBool(
                SERVER_BUILD_SETTINGS_FOLDOUT_STATE_KEY, 
                isBuildFoldout);

            if (!isBuildFoldout)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }
            
            EditorGUI.indentLevel++;
            InsertSpace2x();
            
            insertBuildSettingsFoldoutComponents();
            
            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void insertBuildSettingsFoldoutComponents()
        {
            insertBuildDirNameHorizGroup();
            insertBuildFileExeNameHorizGroup();
            insertScriptingBackendHorizGroup();
            insertOverwriteDockerfileToggleHorizGroup();

            InsertSpace2x();
            
            bool enableBuildBtn = ServerConfig.MeetsBuildBtnReqs();
            if (!enableBuildBtn && !HathoraServerDeploy.IsDeploying)
                insertGenerateServerBuildBtnHelpboxOnMissingReqs();
            else
                insertGenerateServerBuildBtnInfoHelpbox();

            insertGenerateServerBuildBtn(enableBuildBtn); // !await
            insertOpenGeneratedDockerfileLinkLabel();
        }

        private void insertOverwriteDockerfileToggleHorizGroup()
        {
            bool overwriteDockerfile = ServerConfig.LinuxHathoraAutoBuildOpts.OverwriteDockerfile;
            
            bool inputBool = base.InsertHorizLabeledCheckboxField(
                _labelStr: "Overwrite Dockerfile", 
                _tooltip: "If you have edited the generated Dockerfile or need to use a " +
                    "custom Dockerfile, this should be set to 'false'\n\nDefault: true",
                _val: overwriteDockerfile,
                _alignCheckbox: GuiAlign.SmallRight);
            
            bool isChanged = inputBool != overwriteDockerfile;
            if (isChanged)
                onOverwriteDockerfileChanged(inputBool);
            
            InsertSpace1x();
        }

        /// <summary>Only if exists. (!) RESOURCE INTENSIVE</summary>
        private void insertOpenGeneratedDockerfileLinkLabel()
        {
            // USER INPUT >> Calls back onClick
            InsertLinkLabel(
                "Open Dockerfile",
                _url: null,
                _centerAlign: true,
                onClick: onOpenDockerfileBtnClick);
            
            InsertSpace1x();
        }

        /// <summary>
        /// Generally used for helpboxes to explain why a button is disabled.
        /// </summary>
        /// <param name="_serverConfig"></param>
        /// <param name="_includeMissingReqsFieldsPrefixStr">Useful if you had a combo of this </param>
        /// <returns></returns>
        public static StringBuilder GetCreateBuildMissingReqsStrb(
            HathoraServerConfig _serverConfig,
            bool _includeMissingReqsFieldsPrefixStr = true)
        {
            StringBuilder helpboxLabelStrb = new(_includeMissingReqsFieldsPrefixStr 
                ? "Missing required fields: " 
                : ""
            );
            
            if (!_serverConfig.HathoraCoreOpts.HasAppId)
                helpboxLabelStrb.Append("`AppId` ");
            
            if (!_serverConfig.LinuxHathoraAutoBuildOpts.HasServerBuildDirName)
                helpboxLabelStrb.Append("`Server Build Dir Name` ");
                
            if (!_serverConfig.LinuxHathoraAutoBuildOpts.HasServerBuildExeName)
                helpboxLabelStrb.Append("`Server Build Exe Name`");

            return helpboxLabelStrb;
        }
        
        private static void insertGenerateServerBuildBtnInfoHelpbox()
        {
            // Post the help box *before* we disable the button so it's easier to see (if toggleable)
            string labelStr = "This will generate a Linux Server Build for your game. " +
                "It will also generate a Dockerfile. NOTE: this action will update your Build Settings to be 'Dedicated Server - Linux'";
            
            
            EditorGUILayout.HelpBox(labelStr, MessageType.Info);
        }

        private void insertGenerateServerBuildBtnHelpboxOnMissingReqs()
        {
            StringBuilder helpboxLabelStrb = GetCreateBuildMissingReqsStrb(ServerConfig);

            // Post the help box *before* we disable the button so it's easier to see (if toggleable)
            EditorGUILayout.HelpBox(helpboxLabelStrb.ToString(), MessageType.Error);
        }

        private void insertGenerateServerBuildBtn(bool _enableBuildBtn)
        {
            EditorGUI.BeginDisabledGroup(disabled: !_enableBuildBtn);
            
            // USER INPUT >>
            bool clickedBuildBtn = InsertLeftGeneralBtn("Generate Server Build");
            
            EditorGUI.EndDisabledGroup();
            InsertSpace1x();

            if (clickedBuildBtn)
                OnGenerateServerBuildBtnClick();
        }

        private void insertBuildDirNameHorizGroup()
        {
            string inputStr = base.InsertHorizLabeledTextField(
                _labelStr: "Build directory",
                _tooltip: "Parent directory to generate build into.\n\n" +
                "Default: `Build-Server`",
                _val: ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildDirName,
                _alignTextField: GuiAlign.SmallRight);

            bool isChanged = inputStr != ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildDirName;
            if (isChanged)
                onServerBuildDirChanged(inputStr);

            InsertSpace1x();
        }
        
        private void insertBuildFileExeNameHorizGroup()
        {
            string inputStr = base.InsertHorizLabeledTextField(
                _labelStr: "Build file name", 
                _tooltip: "Name for generated build.\n\n" +
                "Default: `Hathora-Unity-LinuxServer.x86_64`",
                _val: ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildExeName,
                _alignTextField: GuiAlign.SmallRight);
            
            bool isChanged = inputStr != ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildExeName;
            if (isChanged)
                onServerBuildExeNameChanged(inputStr);
            
            InsertSpace1x();
        }
        private void insertScriptingBackendHorizGroup()
        {
            int selectedIndex = ServerConfig.LinuxHathoraAutoBuildOpts.ScriptingBackendIndex;
            List<string> displayOptsStrArr = GetDisplayOptsStrArrFromEnum<HathoraAutoBuildOpts.ScriptingBackend>();
            int newSelectedIndex = base.InsertHorizLabeledPopupList(
                _labelStr: "Scripting backend", 
                _tooltip: "'Mono' - faster build times.\n Requires 'Linux Build Support (Mono)' module installed\n\n" +
                "'IL2CPP' - slower build, but better performance.\n Requires 'Linux Build Support (IL2CPP)' module installed\n\n" +
                "Default: `Mono`",
                _displayOptsStrArr: displayOptsStrArr.ToArray(),
                _selectedIndex: selectedIndex,
                _alignPopup: GuiAlign.SmallRight);
            
            bool isNewValidIndex = 
                newSelectedIndex != selectedIndex &&
                selectedIndex < displayOptsStrArr.Count;

            if (isNewValidIndex)
                onSelectedScriptingBackendPopupIndexChanged(newSelectedIndex);

            InsertSpace1x();
        }
        #endregion // UI Draw

        
        #region Event Logic
        private void onOpenDockerfileBtnClick()
        {
            HathoraServerPaths paths = new(ServerConfig);
            
            generateDockerfileIfNotExists(paths);
            HathoraDocker.OpenDockerfile(paths);
        }

        private void generateDockerfileIfNotExists(HathoraServerPaths _paths)
        {
            bool dockerfileExists = HathoraServerBuild.CheckIfDockerfileExists(_paths);
            
            if (!dockerfileExists)
                HathoraDocker.GenerateDockerFileStr(_paths);
        }
        
        private void onServerBuildDirChanged(string _inputStr)
        {
            ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildDirName = _inputStr;
            SaveConfigChange(
                nameof(ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildDirName), 
                _inputStr);
        }        
        
        private void onServerBuildExeNameChanged(string _inputStr)
        {
            ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildExeName = _inputStr;
            
            SaveConfigChange(
                nameof(ServerConfig.LinuxHathoraAutoBuildOpts.ServerBuildExeName), 
                _inputStr);
        }
        
        private void onSelectedScriptingBackendPopupIndexChanged(int _newSelectedIndex)
        {
            ServerConfig.LinuxHathoraAutoBuildOpts.ScriptingBackendIndex = _newSelectedIndex;
            SaveConfigChange(
                nameof(ServerConfig.LinuxHathoraAutoBuildOpts.ScriptingBackendIndex), 
                _newSelectedIndex.ToString());
        }
        
        private void onOverwriteDockerfileChanged(bool _inputBool)
        {
            ServerConfig.LinuxHathoraAutoBuildOpts.OverwriteDockerfile = _inputBool;
            
            SaveConfigChange(
                nameof(ServerConfig.LinuxHathoraAutoBuildOpts.OverwriteDockerfile), 
                _inputBool.ToString().ToLowerInvariant());
        }

        private void OnGenerateServerBuildBtnClick() =>
            _ = generateServerBuildAsync(); // !await

        private async Task<BuildReport> generateServerBuildAsync()
        {
            // Build headless Linux executable
            cancelBuildTokenSrc = new CancellationTokenSource(TimeSpan.FromMinutes(
                HathoraServerBuild.DEPLOY_TIMEOUT_MINS));
            
            BuildReport buildReport = null;

            try
            {
                // +Appends strb logs
                buildReport = await HathoraServerBuild.BuildHathoraLinuxServer(
                    ServerConfig,
                    SerializedConfig,
                    cancelBuildTokenSrc.Token);
            }
            catch (TaskCanceledException)
            {
                Debug.Log("Server build cancelled.");
                ServerConfig.LinuxHathoraAutoBuildOpts.LastBuildLogsStrb
                    .AppendLine()
                    .AppendLine("** BUILD CANCELLED BY USER **");
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"[HathoraConfigPostAuthBodyBuilderUI.generateServerBuildAsync] " +
                    $"Error: {e}");
                
                ServerConfig.LinuxHathoraAutoBuildOpts.LastBuildLogsStrb
                    .AppendLine()
                    .AppendLine("** BUILD ERROR BELOW **")
                    .AppendLine(e.Message);
                throw;
            }

            Assert.AreEqual(buildReport.summary.result, BuildResult.Succeeded,
                "Server build failed. Check console for details.");
            
            return buildReport;
        }
        #endregion // Event Logic
    }
}