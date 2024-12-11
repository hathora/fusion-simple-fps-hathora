// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Editor.Common;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using Hathora.Core.Scripts.Runtime.Server;
using HathoraCloud.Models.Shared;
using UnityEditor;
using UnityEngine;

namespace Hathora.Core.Scripts.Editor.Server.ConfigStyle.PostAuth
{
    public class HathoraConfigPostAuthBodyDeployUI : HathoraConfigUIBase
    {
        #region Vars
        private CancellationTokenSource cancelBuildTokenSrc;
        private bool isCancellingDeployment;
        
        // Foldouts
        private bool isDeploymentFoldout;
        
        /// <summary>For state persistence on which dropdown group was last clicked</summary>
        protected const string SERVER_DEPLOY_SETTINGS_FOLDOUT_STATE_KEY = "ServerDeploySettingsFoldoutState";
        #endregion // Vars


        #region Init
        public HathoraConfigPostAuthBodyDeployUI(
            HathoraServerConfig _serverConfig, 
            SerializedObject _serializedConfig)
            : base(_serverConfig, _serializedConfig)
        {
            initDrawUtils();
        }

        /// <summary>
        /// There are modulated parts of the post-auth body.
        /// </summary>
        private void initDrawUtils()
        {
            // _advancedDeployUI = new HathoraConfigPostAuthBodyDeployAdvUI(ServerConfig, SerializedConfig);
            HathoraServerDeploy.OnZipComplete += onDeployAppStatus_1ZipComplete;
            HathoraServerDeploy.OnBuildReqComplete += onDeployAppStatus_2BuildReqComplete;
            HathoraServerDeploy.OnUploadComplete += onDeployAppStatus_3UploadComplete;
        }
        #endregion // Init
        
        
        #region UI Draw
        public void Draw()
        {
            if (!IsAuthed)
                return; // You should be calling HathoraConfigPreAuthBodyUI.Draw()

            insertDeploymentSettingsFoldout();
        }

        private void insertDeploymentSettingsFoldout()
        {
            // Retrieve the saved foldout state from EditorPrefs
            isDeploymentFoldout = EditorPrefs.GetBool(
                SERVER_DEPLOY_SETTINGS_FOLDOUT_STATE_KEY, 
                defaultValue: false);
            
            // Create the foldout
            isDeploymentFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(
                isDeploymentFoldout, 
                "Hathora Deployment Configuration");
            
            // Save the new foldout state to EditorPrefs
            EditorPrefs.SetBool(
                SERVER_DEPLOY_SETTINGS_FOLDOUT_STATE_KEY, 
                isDeploymentFoldout);
            
            if (!isDeploymentFoldout)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }
    
            InsertSpace2x();

            insertDeploymentSettingsFoldoutComponents();

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void insertDeploymentSettingsFoldoutComponents()
        {
            insertPlanNameHorizPopupList();
            insertRoomsPerProcessHorizSliderGroup();
            insertContainerPortNumberHorizSliderGroup();
            insertTransportTypeHorizRadioBtnGroup();
            // _advancedDeployUI.Draw();

            bool deployBtnMeetsReqs = checkIsReadyToEnableToDeployBtn();
            insertDeployBtnHelpbox(deployBtnMeetsReqs);
            insertDeployAndOrCancelBtn(deployBtnMeetsReqs);
        }

        /// <summary>
        /// If deploying, we'll disable the btn and show a cancel btn.
        /// On cancel btn click, we'll disable until done ("Cancelling...") -> have a 2s cooldown.
        /// </summary>
        private void insertDeployAndOrCancelBtn(bool _deployBtnMeetsReqs)
        {
            // Deploy btn - active if !deploying
            insertDeployAppBtn(_deployBtnMeetsReqs);
            insertCancelOrCancellingBtn(); // Show cancel btn separately (below)
        }

        private void insertCancelOrCancellingBtn()
        {
            if (isCancellingDeployment)
                insertDeployAppCancellingDisabledBtn();
            else if (HathoraServerDeploy.IsDeploying && cancelBuildTokenSrc is { Token: { CanBeCanceled: true } })
                insertDeployAppCancelBtn();
        }

        private void insertDeployAppBtn(bool _deployBtnMeetsReqs)
        {
            string btnLabelStr = HathoraServerDeploy.IsDeploying 
                ? HathoraServerDeploy.GetDeployFriendlyStatus()
                : "Deploy Application";
            
            EditorGUI.BeginDisabledGroup(disabled: 
                HathoraServerDeploy.IsDeploying || 
                isCancellingDeployment || 
                !_deployBtnMeetsReqs);

            // USER INPUT >>
            bool clickedDeployBtn = InsertLeftGeneralBtn(btnLabelStr);
            
            EditorGUI.EndDisabledGroup();
            InsertSpace1x();
            
            if (clickedDeployBtn)
                _ = onClickedDeployAppBtnClick(); // !await
        } 
        
        /// <summary>
        /// This cancel can take longer than usual, and requires a cooldown to prevent issues.
        /// Normally, we just cancel and allow a 2nd attempt instantly - but not in this case.
        /// </summary>
        private void insertDeployAppCancellingDisabledBtn()
        {
            string btnLabelStr = $"<color={HathoraEditorUtils.HATHORA_PINK_CANCEL_COLOR_HEX}>" +
                "<b>Cancelling...</b></color>";
            
            EditorGUI.BeginDisabledGroup(disabled: true);
            GUILayout.Button(btnLabelStr, GeneralButtonStyle); // (!) Not actual input
            EditorGUI.EndDisabledGroup();
            
            InsertSpace1x();
        }

        private void insertDeployAppCancelBtn()
        {
            string btnLabelStr = $"<color={HathoraEditorUtils.HATHORA_PINK_CANCEL_COLOR_HEX}>" +
                "<b>Cancel</b></color>";
            
            // USER INPUT >>
            bool clickedCancelBtn = GUILayout.Button(btnLabelStr, GeneralButtonStyle);
            
            if (clickedCancelBtn)
                _ = onDeployAppCancelBtnClick(); // !await
        }
        
        private void insertDeployBtnHelpbox(bool _enableDeployBtn)
        {
            if (_enableDeployBtn || HathoraServerDeploy.IsDeploying)
                insertDeployAppHelpbox();
            else
                insertDeployAppHelpboxErr();
        }

        private void insertRoomsPerProcessHorizSliderGroup()
        {
            int inputInt = base.InsertHorizLabeledConstrainedIntField(
                _labelStr: "Rooms per process",
                _tooltip: "For most Unity multiplayer games, this should be left as 1\n\n" +
                "For some lightweight servers, a single server instance (process) can handle multiple rooms/matches. If your server is built to support this, you can specify the number of rooms to fit on a process before spinning up a fresh instance.\n\n" +
                "Default: 1",
                _val: ServerConfig.HathoraDeployOpts.RoomsPerProcess,
                _minVal: 1,
                _maxVal: 10000,
                _alignPopup: GuiAlign.SmallRight);

            bool isChanged = inputInt != ServerConfig.HathoraDeployOpts.RoomsPerProcess;
            if (isChanged)
                onRoomsPerProcessNumChanged(inputInt);
            
            InsertSpace1x();
        }
        
        private void insertContainerPortNumberHorizSliderGroup()
        {
            int inputInt = base.InsertHorizLabeledConstrainedIntField(
                _labelStr: "Container port number",
                _tooltip: "This is the port your server code is listening on, Hathora will bind to this.\n" +
                "(NOTE: this will be different from the port players/clients connect to - see \"Create Room\")\n\n" +
                "Default: 7777 (<1024 is generally reserved by system)",
                _val: ServerConfig.HathoraDeployOpts.ContainerPortSerializable.Port,
                _minVal: 1024,
                _maxVal: 49151,
                _alignPopup: GuiAlign.SmallRight);

            bool isChanged = inputInt != ServerConfig.HathoraDeployOpts.ContainerPortSerializable.Port;
            if (isChanged && inputInt >= 1024)
            {
                int clampedInputInt = Math.Clamp(inputInt, 1024, ushort.MaxValue);
                onContainerPortNumberNumChanged(clampedInputInt);
            }
            
            InsertSpace1x();
        }
        
        private void insertTransportTypeHorizRadioBtnGroup()
        {
            int selectedIndex = ServerConfig.HathoraDeployOpts.TransportTypeIndex;
            
            // Get list of string names from PlanNameIndex Enum members. Set UPPER.
            List<string> displayOptsStrList = GetStrListOfEnumMemberKeys<TransportType>(EnumListOpts.AllCaps);
            int newSelectedIndex = base.InsertHorizLabeledPopupList(
                _labelStr: "Transport Type",
                _tooltip: 
                    "Default: `UDP` (Fastest; although less reliable) " +
                    "(!) For now, all transports override to UDP",
                _displayOptsStrArr: displayOptsStrList.ToArray(),
                _selectedIndex: selectedIndex,
                GuiAlign.SmallRight);

            bool isNewValidIndex = 
                newSelectedIndex != selectedIndex &&
                selectedIndex < displayOptsStrList.Count;

            if (isNewValidIndex)
                onSelectedTransportTypePopupIndexChanged(newSelectedIndex);
            
            InsertSpace2x();
        }

        private static string getPlanNameListWithExtraInfo(PlanName _planName)
        {
            switch (_planName)
            {
                default:
                case PlanName.Tiny:
                    return $"{nameof(PlanName.Tiny)} (Shared core, 1GB)";
                
                case PlanName.Small:
                    return $"{nameof(PlanName.Small)} (1 core, 2GB)";
                
                case PlanName.Medium:
                    return $"{nameof(PlanName.Medium)} (2 cores, 4GB)";
                
                case PlanName.Large:
                    return $"{nameof(PlanName.Large)} (4 cores, 8GB)"; 
            }
        }

        private void insertPlanNameHorizPopupList()
        {
            int selectedIndex = ServerConfig.HathoraDeployOpts.PlanNameIndex;
            
            // Get list of string names from PlanNameIndex Enum members - with extra info.
            // The index order is !modified.
            List<string> displayOptsStrArr = GetDisplayOptsStrArrFromEnum<PlanName>();
            int newSelectedIndex = base.InsertHorizLabeledPopupList(
                _labelStr: "Plan Size",
                _tooltip: "Determines amount of resources your server instances has access to\n\n" +
                "Tiny - Shared Core, 1GB Memory\n" +
                "Small - 1 Core, 2GB Memory\n" +
                "Medium - 2 Cores, 4GB Memory\n" +
                "Large - 4 Cores, 8GB Memory\n\n" +
                "Default: `Tiny`",
                _displayOptsStrArr: displayOptsStrArr.ToArray(),
                _selectedIndex: selectedIndex,
                GuiAlign.SmallRight);

            bool isNewValidIndex = 
                newSelectedIndex != selectedIndex &&
                selectedIndex < displayOptsStrArr.Count;

            if (isNewValidIndex)
                onSelectedPlanNamePopupIndexChanged(newSelectedIndex);
            
            string appUrl = "https://hathora.dev/docs/pricing-billing";
            InsertLinkLabel("See pricing details", appUrl, _centerAlign:false);
            
            InsertSpace2x();
        }
        
        private void insertDeployAppHelpbox()
        {
            InsertSpace2x();
            
            const MessageType helpMsgType = MessageType.Info;
            const string helpMsg = "This action will create a new deployment version of your application. " +
                "New rooms will be created with this version of your server, existing rooms will be unaffected.";

            // Post the help box *before* we disable the button so it's easier to see (if toggleable)
            EditorGUILayout.HelpBox(helpMsg, helpMsgType);
        }
        
        private void insertDeployAppHelpboxErr()
        {
            InsertSpace2x();

            StringBuilder helpboxLabelStrb = new("Missing required fields: ");
            if (!ServerConfig.HathoraCoreOpts.HasAppId)
                helpboxLabelStrb.Append("`AppId` ");
            
            if (ServerConfig.HathoraDeployOpts.PlanNameIndex < 0)
                helpboxLabelStrb.Append("`Plan Size` ");
            
            if (ServerConfig.HathoraDeployOpts.RoomsPerProcess < 0)
                helpboxLabelStrb.Append("`Rooms per Process` ");
            
            if (ServerConfig.HathoraDeployOpts.ContainerPortSerializable.Port < 0)
                helpboxLabelStrb.Append("`Container Port Number` ");
            
            if (ServerConfig.HathoraDeployOpts.TransportType < 0)
                helpboxLabelStrb.Append("`Transport Type`");

            // Post the help box *before* we disable the button so it's easier to see (if toggleable)
            EditorGUILayout.HelpBox(helpboxLabelStrb.ToString(), MessageType.Error);
        }
        #endregion // UI Draw

        
        #region Event Logic
        private void onSelectedPlanNamePopupIndexChanged(int _newSelectedIndex)
        {
            ServerConfig.HathoraDeployOpts.PlanNameIndex = _newSelectedIndex;
            SaveConfigChange(
                nameof(ServerConfig.HathoraDeployOpts.PlanNameIndex), 
                _newSelectedIndex.ToString());
        }
        
        private void onSelectedTransportTypePopupIndexChanged(int _newSelectedIndex)
        {
            ServerConfig.HathoraDeployOpts.TransportTypeIndex = _newSelectedIndex;
            SaveConfigChange(
                nameof(ServerConfig.HathoraDeployOpts.TransportType), 
                _newSelectedIndex.ToString());
        }

        private void onRoomsPerProcessNumChanged(int _inputInt)
        {
            ServerConfig.HathoraDeployOpts.RoomsPerProcess = _inputInt;
            SaveConfigChange(
                nameof(ServerConfig.HathoraDeployOpts.RoomsPerProcess), 
                _inputInt.ToString());
        }
        
        private void onContainerPortNumberNumChanged(int _inputInt)
        {
            ServerConfig.HathoraDeployOpts.ContainerPortSerializable.Port = _inputInt;
            SaveConfigChange(
                nameof(ServerConfig.HathoraDeployOpts.ContainerPortSerializable.Port), 
                _inputInt.ToString());
        }
        
        private async Task onDeployAppCancelBtnClick()
        {
            isCancellingDeployment = true;
            cancelBuildTokenSrc.Cancel();
            
            // 1s cooldown: Using this btn immediately after causes issues
            await Task.Delay(TimeSpan.FromSeconds(1));
            isCancellingDeployment = false;
        }

        private async Task onClickedDeployAppBtnClick() => 
            await DeployApp();

        /// <summary>
        /// Optionally sub to events:
        /// - OnZipComplete
        /// - OnBuildReqComplete
        /// - OnUploadComplete
        /// </summary>
        /// <returns></returns>
        private async Task DeployApp()
        {
            // Before we begin, close this group so we can more-easily see the logs
            isDeploymentFoldout = false;
            
            cancelBuildTokenSrc = new CancellationTokenSource(TimeSpan.FromMinutes(
                HathoraServerDeploy.DEPLOY_TIMEOUT_MINS));

            // Catch errs so we can reset the UI on fail
            try
            {
                DeploymentV3 deployment = await HathoraServerDeploy.DeployToHathoraAsync(
                    ServerConfig,
                    cancelBuildTokenSrc.Token);

                bool isSuccess = !string.IsNullOrEmpty(deployment?.DeploymentId);
                if (isSuccess)
                    onDeployAppSuccess(deployment);
                else
                    onDeployAppFail();
            }
            catch (Exception e)
            {
                Debug.LogError($"[HathoraConfigPostAuthBodyDeployUI.DeployApp] Error: {e}");
                onDeployAppFail();

                throw;
            }
            finally
            {
                InvokeRequestRepaint();
            }
        }

        private void onDeployAppFail()
        {
        }

        /// <summary>Step 1 of 4</summary>
        private void onDeployAppStatus_1ZipComplete()
        {
            Debug.Log("[HathoraConfigPostAuthBodyDeployUI] <color=yellow>" +
                "onDeployAppStatus_1ZipComplete</color>");
        }
        
        /// <summary>Step 2 of 4</summary>
        private void onDeployAppStatus_2BuildReqComplete(CreatedBuildV3WithMultipartUrls _build)
        {
            Debug.Log("[HathoraConfigPostAuthBodyDeployUI] <color=yellow>" +
                "onDeployAppStatus_2BuildReqComplete</color>");
            // TODO
        }
        
        /// <summary>Step 3 of 4</summary>
        private void onDeployAppStatus_3UploadComplete()
        {
            Debug.Log("[HathoraConfigPostAuthBodyDeployUI] <color=yellow>" +
                "onDeployAppStatus_3UploadComplete</color>");
            // TODO
        }

        /// <summary>
        /// Cache last successful Deployment for the session
        /// </summary>
        /// <param name="_deployment"></param>
        private void onDeployAppSuccess(DeploymentV3 _deployment) =>
            ServerConfig.HathoraDeployOpts.LastDeployment = _deployment;
        #endregion // Event Logic
        
        
        #region Utils
        /// <returns>isReadyToEnableToDeployBtn</returns>
        private bool checkIsReadyToEnableToDeployBtn() =>
            !HathoraServerDeploy.IsDeploying &&
            ServerConfig.HathoraDeployOpts.RoomsPerProcess > 0 &&
            ServerConfig.HathoraDeployOpts.ContainerPortSerializable.Port >= 1024;

        #endregion //Utils

        
        ~HathoraConfigPostAuthBodyDeployUI()
        {
            HathoraServerDeploy.OnZipComplete -= onDeployAppStatus_1ZipComplete;
            HathoraServerDeploy.OnBuildReqComplete -= onDeployAppStatus_2BuildReqComplete;
            HathoraServerDeploy.OnUploadComplete -= onDeployAppStatus_3UploadComplete;
        }
    }
}
