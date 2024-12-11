// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Editor.Common;
using Hathora.Core.Scripts.Runtime.Common.Extensions;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using Hathora.Core.Scripts.Runtime.Server;
using Hathora.Core.Scripts.Runtime.Server.ApiWrapper;
using Hathora.Core.Scripts.Runtime.Server.Models;
using HathoraCloud;
using HathoraCloud.Models.Shared;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Security = HathoraCloud.Models.Shared.Security;

namespace Hathora.Core.Scripts.Editor.Server.ConfigStyle.PostAuth
{
    public class HathoraConfigPostAuthBodyRoomUI : HathoraConfigUIBase
    {
        #region Vars
        
        private HathoraConfigPostAuthBodyRoomLobbyUI roomLobbyUI { get; set; }
        public static CancellationTokenSource CreateRoomCancelTokenSrc { get; set; } // TODO
        private const int CREATE_ROOM_TIMEOUT_SECONDS = 30;
        private bool isCreatingRoomAwaitingActiveStatus { get; set; }
        
        // Region lists
        private readonly List<string> displayOptsStrList;
        private readonly List<int> originalIndices;
        
        // Foldouts
        private bool isRoomFoldout;
        
        /// <summary>For state persistence on which dropdown group was last clicked</summary>
        protected const string SERVER_ROOM_SETTINGS_FOLDOUT_STATE_KEY = "ServerRoomSettingsFoldoutState";
        #endregion // Vars


        #region Init
        public HathoraConfigPostAuthBodyRoomUI(
            HathoraServerConfig _serverConfig, 
            SerializedObject _serializedConfig)
            : base(_serverConfig, _serializedConfig)
        {
            
            
            initDrawUtils();
            
            // ----------------------
            // REGION LISTS >> We want to be able to sort them, yet still refer to original Enum index
            Dictionary<int, string> regionDisplayMap = 
                Enum.GetValues(typeof(Region))
                    .Cast<Region>()
                    .Select((Region region, int index) => new { region, index })
                    .ToDictionary(x => x.index, x => 
                        x.region.ToString().SplitPascalCase());

            try
            {
                displayOptsStrList = regionDisplayMap.Values.OrderBy(s => s).ToList();

                // (!) WORKAROUND: Note the `.Key+1`: Hathora SDK Enums starts at index 1; not 0: Care of indexes.
                originalIndices = displayOptsStrList.Select(
                        displayStr =>
                            regionDisplayMap.First(
                                    kvp =>
                                        kvp.Value == displayStr)
                                .Key+1)
                    .ToList();
                
                Assert.AreEqual(originalIndices[0], (int)(Region)originalIndices[0]);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to init Region lists: {e}");
            }
        }

        private void initDrawUtils()
        {
            roomLobbyUI = new HathoraConfigPostAuthBodyRoomLobbyUI(ServerConfig, SerializedConfig);
        }
        #endregion // Init
        
        
        #region UI Draw
        public void Draw()
        {
            if (!IsAuthed)
                return; // You should be calling HathoraConfigPreAuthBodyUI.Draw()

            insertCreateRoomFoldout();
        }

        private void insertCreateRoomFoldout()
        {
            // Retrieve the saved foldout state from EditorPrefs
            isRoomFoldout = EditorPrefs.GetBool(
                SERVER_ROOM_SETTINGS_FOLDOUT_STATE_KEY, 
                defaultValue: false);
            
            // Create the foldout
            isRoomFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(
                isRoomFoldout, 
                "Create Room");
            
            // Save the new foldout state to EditorPrefs
            EditorPrefs.SetBool(
                SERVER_ROOM_SETTINGS_FOLDOUT_STATE_KEY, 
                isRoomFoldout);
            
            if (!isRoomFoldout)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }
    
            EditorGUI.indentLevel++;
            InsertSpace2x();

            insertCreateRoomFoldoutComponents();

            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        
        private void insertCreateRoomFoldoutComponents()
        {
            insertRegionHorizPopupList();

            bool enableCreateRoomBtn = ServerConfig.MeetsCreateRoomBtnReqs();
            insertCreateRoomBtnHelpboxOnErr(enableCreateRoomBtn);
            insertCreateRoomOrCancelBtnWrapper(enableCreateRoomBtn);
            
            insertRoomInfoOrErrGroupWrapper();
            insertViewLogsMetricsLinkLbl();
        }

        private void insertCreateRoomOrCancelBtnWrapper(bool _enableCreateRoomBtn)
        {
            bool showCancelBtn = isCreatingRoomAwaitingActiveStatus && CreateRoomCancelTokenSrc.Token.CanBeCanceled; 
            if (showCancelBtn)
                insertCreateRoomCancelBtn(CreateRoomCancelTokenSrc);
            else
                insertCreateRoomBtn(_enableCreateRoomBtn);
        }

        private void insertRoomInfoOrErrGroupWrapper()
        {
            bool hasLastRoomInfo = ServerConfig.HathoraLobbyRoomOpts.HasLastCreatedRoomConnection;
            bool hasLastRoomErr = ServerConfig.HathoraLobbyRoomOpts.LastCreatedRoomConnection?.IsError ?? false;
            if (!hasLastRoomInfo && !hasLastRoomErr)
                return;
            
            base.BeginPaddedBox();
                
            if (hasLastRoomErr)
                insertLastCreatedRoomInfoErrGroup();
            else
                insertLastCreatedRoomInfoGroup();
                
            base.EndPaddedBox();
        }

        private void insertLastCreatedRoomInfoErrGroup()
        {
            insertRoomLastCreatedHeaderLbl();
            insertRoomLastCreatedErrLbl();
        }

        private void insertRoomLastCreatedErrLbl()
        {
            InsertLabel($"<color={HathoraEditorUtils.HATHORA_PINK_CANCEL_COLOR_HEX}><b>Error:</b> " +
                $"{ServerConfig.HathoraLobbyRoomOpts.LastCreatedRoomConnection.ErrReason}</color>");
            InsertSpace1x();        }

        private void insertLastCreatedRoomInfoGroup()
        {

            // GUI >>
            try
            {
                insertRoomLastCreatedHeaderLbl();
                insertRoomIdDateHorizGroup();
                insertRoomConnectionInfoBtnGroup();
                insertViewRoomInConsoleLinkLbl();   
            }
            catch (Exception e)
            {
                Debug.LogError($"[HathoraConfigPostAuthBodyRoomUI.insertLastCreatedRoomInfoGroup] " +
                    $"Error (skipping this group so UI can continue to load): {e}");
                throw;
            }
        }

        private void insertRoomConnectionInfoBtnGroup()
        {
            EditorGUILayout.BeginHorizontal();

            insertRoomConnectionInfoSelectableLbl();
            insertCopyRoomConnectionInfoBtn();
            
            EditorGUILayout.EndHorizontal();
        }

        private void insertRoomIdDateHorizGroup()
        {
            EditorGUILayout.BeginHorizontal();
            
            insertRoomIdLbl();
            insertRoomRegionLbl();
            insertRoomCreateDateLbl();
            
            EditorGUILayout.EndHorizontal();
            InsertSpace2x();
        }

        private void insertRoomCreateDateLbl()
        {
            DateTime? createdDateTime = ServerConfig.HathoraLobbyRoomOpts
                .LastCreatedRoomConnection?.Room?.CurrentAllocation?.ScheduledAt;

            string createdDateStr = HathoraUtils.GetFriendlyDateTimeShortStr(createdDateTime)
                ?? "{Unknown DateTime}";

            string labelStr = $"<b>Created:</b> {createdDateStr} (UTC)"; // Server logs, so UTC
            InsertLabel(labelStr, _fontSize: 10);
        }

        private void insertRoomRegionLbl()
        {
            InsertFlexSpace();
            
            string region = ServerConfig.HathoraLobbyRoomOpts.LastCreatedRoomConnection?.GetFriendlyRegionStr();
            InsertLabel($"<b>Region:</b> {region}", _fontSize: 10);

            InsertFlexSpace();
        }

        private void insertRoomConnectionInfoSelectableLbl()
        {
            string connInfoStr = ServerConfig.HathoraLobbyRoomOpts
                .LastCreatedRoomConnection?.GetConnInfoStr();
            
            InsertLabel(
                $"<b>Connection Info:</b> {connInfoStr}", 
                _fontSize: 10,
                _vertCenter: true,
                _selectable: false); // BUG: If true, there's a random indent
        }

        private void insertRoomIdLbl()
        {
            string roomId = ServerConfig.HathoraLobbyRoomOpts.LastCreatedRoomConnection?.Room?.RoomId;
            InsertLabel($"<b>Room ID:</b> {roomId}", _fontSize: 10);
            InsertSpace1x();
        }

        private void insertRoomLastCreatedHeaderLbl()
        {
            InsertLabel("<color=white><b>Last Created Room:</b></color>");
            InsertSpace1x();
        }

        private void insertCopyRoomConnectionInfoBtn()
        {
            // USER INPUT >>
            bool clickedRoomCopyConnInfoBtn = InsertLeftGeneralBtn("Copy Connection Info");
            if (clickedRoomCopyConnInfoBtn)
                onCopyRoomConnectionInfoBtnClick();
        }

        private void insertViewRoomInConsoleLinkLbl()
        {
            string appId = ServerConfig.HathoraCoreOpts?.AppId ?? "APP_ID_MISSING"; 
            string consoleAppUrl = $"{HathoraEditorUtils.HATHORA_CONSOLE_APP_BASE_URL}/{appId}";
            string processId = ServerConfig.HathoraLobbyRoomOpts.LastCreatedRoomConnection.Room?
                    .CurrentAllocation?.ProcessId ?? "PROCESS_ID_MISSING"; 
            string appUrl = $"{consoleAppUrl}/process/{processId}";
            
            InsertLinkLabel("View room in Hathora Console", appUrl, _centerAlign:false);
        }

        private void insertCreateRoomCancelBtn(CancellationTokenSource _cancelTokenSrc)
        {
            string btnLabelStr = $"<color={HathoraEditorUtils.HATHORA_PINK_CANCEL_COLOR_HEX}>" +
                "<b>Cancel</b> (Creating Room...)</color>";

            // USER INPUT >>
            bool clickedCancelBtn = GUILayout.Button(btnLabelStr, GeneralButtonStyle);
            if (clickedCancelBtn)
                onCreateRoomCancelBtnClick(_cancelTokenSrc);
        }

        /// <summary>
        /// Generally used for helpboxes to explain why a button is disabled.
        /// </summary>
        /// <param name="_serverConfig"></param>
        /// <param name="_includeMissingReqsFieldsPrefixStr">Useful if you had a combo of this </param>
        /// <returns></returns>
        public static StringBuilder GetCreateRoomMissingReqsStrb(
            HathoraServerConfig _serverConfig,
            bool _includeMissingReqsFieldsPrefixStr = true)
        {
            StringBuilder helpboxLabelStrb = new(_includeMissingReqsFieldsPrefixStr 
                ? "Missing required fields: " 
                : ""
            );
            
            if (!_serverConfig.HathoraCoreOpts.HasAppId)
                helpboxLabelStrb.Append("`AppId` ");

            return helpboxLabelStrb;
        }
        
        private void insertCreateRoomBtnHelpboxOnErr(bool _enable)
        {
            if (_enable)
                return;
            
            // Explain why the button is disabled
            StringBuilder helpboxLabelStrb = GetCreateRoomMissingReqsStrb(ServerConfig);
            
            // Post the help box *before* we disable the button so it's easier to see (if toggleable)
            EditorGUILayout.HelpBox(helpboxLabelStrb.ToString(), MessageType.Error);
        }

        private void insertViewLogsMetricsLinkLbl()
        {
            InsertSpace2x();
            
            InsertCenterLabel("View logs and metrics for your active rooms and processes below:");

            string consoleAppUrl = HathoraEditorUtils.HATHORA_CONSOLE_APP_BASE_URL +
                $"/{ServerConfig.HathoraCoreOpts.AppId}";
            
            InsertLinkLabel(
                "Hathora Console",
                consoleAppUrl,
                _centerAlign: true);
            
            InsertSpace1x();
        }

        private void insertCreateRoomBtn(bool _enable)
        {
            string btnLabelStr = isCreatingRoomAwaitingActiveStatus 
                ? "Creating Room..." 
                : "Create Room";

            EditorGUI.BeginDisabledGroup(disabled: !_enable);
            
            // USER INPUT >>
            bool clickedCreateRoomLobbyBtn = InsertLeftGeneralBtn(btnLabelStr);

            EditorGUI.EndDisabledGroup();
            InsertSpace1x();

            if (!clickedCreateRoomLobbyBtn)
                return;

            _ = onCreateRoomBtnClick(); // !await

            EditorGUI.EndDisabledGroup();
        }

        private void insertRegionHorizPopupList()
        {
            int selectedIndex = ServerConfig.HathoraLobbyRoomOpts.HathoraRegionIndex;

            // Get list of string names from Region Enum members - with prettified names. Index order !modified.
            List<string> displayOptsStrArr = GetDisplayOptsStrArrFromEnum<Region>(_prettifyNames: true);
            
            // Prettify those opts
            
            int newSelectedIndex = base.InsertHorizLabeledPopupList(
                _labelStr: "Region",
                _tooltip: "Default: `Seattle`",
                _displayOptsStrArr: displayOptsStrArr.ToArray(),
                _selectedIndex: selectedIndex,
                GuiAlign.SmallRight);
            
            bool isNewValidIndex =
                newSelectedIndex != selectedIndex &&
                selectedIndex < displayOptsStrArr.Count;
            
            if (isNewValidIndex)
                onSelectedRegionPopupIndexChanged(newSelectedIndex);

            InsertSpace2x();
        }
        #endregion // UI Draw
        
        
        #region Event Logic
        private void onSelectedRegionPopupIndexChanged(int _newSelectedIndex)
        {
            // Sorted list (order, names and index) will be different from the original list
            ServerConfig.HathoraLobbyRoomOpts.HathoraRegionIndex = _newSelectedIndex;
            
            SaveConfigChange(
                nameof(ServerConfig.HathoraLobbyRoomOpts.HathoraRegionIndex), 
                _newSelectedIndex.ToString());
        }
        
        /// <summary>
        /// On cancel, we'll set !isCreatingRoomAwaitingActiveStatus so we can try again.
        /// </summary>
        private async Task onCreateRoomBtnClick()
        {
            isCreatingRoomAwaitingActiveStatus = true;
            resetLastCreatedRoom(); // Both UI + ServerConfig

            Region lastRegion = ServerConfig.HathoraLobbyRoomOpts.HathoraRegion;
            createNewCreateRoomCancelToken();
            
            Security security = new()
            {
                HathoraDevToken = ServerConfig.HathoraCoreOpts.DevAuthOpts.HathoraDevToken,
            };
            
            HathoraServerRoomApiWrapper serverRoomApiWrapper = new(
                new HathoraCloudSDK(security, null, ServerConfig.HathoraCoreOpts.AppId),
                ServerConfig);

            (Room room, ConnectionInfoV2 connInfo) roomConnInfoTuple;

            try
            {
                
                roomConnInfoTuple = await serverRoomApiWrapper.ServerCreateRoomAwaitActiveAsync(
                    _region: ServerConfig.HathoraLobbyRoomOpts.HathoraRegion, 
                    _cancelToken: CreateRoomCancelTokenSrc.Token);
            }
            // catch (TaskCanceledException e)
            // {
            // }
            catch (Exception e)
            {
                // Could be a TaskCanceledException
                onCreateRoomDone();
                onCreateRoomFail(_reason: e.Message);
                return;
            }
            
            // Parse to helper class containing extra parsing
            // We can also save to ServerConfig as this type
            HathoraCachedRoomConnection roomConnInfo = new(
                lastRegion,
                roomConnInfoTuple.room, 
                roomConnInfoTuple.connInfo);
            
            onCreateRoomDone(roomConnInfo); // Asserts
            onCreateRoomSuccess(roomConnInfo);
        }

        private void onCreateRoomFail(string _reason)
        {
            ServerConfig.HathoraLobbyRoomOpts.LastCreatedRoomConnection = new HathoraCachedRoomConnection
            {
                IsError = true,
                ErrReason = _reason,
            };
        }

        /// <summary>
        /// This being null should trigger the UI to auto-hide the info box
        /// </summary>
        private void resetLastCreatedRoom() =>
            ServerConfig.HathoraLobbyRoomOpts.LastCreatedRoomConnection = null;

        private void onCreateRoomSuccess(HathoraCachedRoomConnection _roomConnInfo)
        {
            Debug.Log("[HathoraConfigPostAuthBodyRoomUI] onCreateRoomSuccess");

            // Save to this session ONLY - restarting Unity will reset this
            ServerConfig.HathoraLobbyRoomOpts.LastCreatedRoomConnection = _roomConnInfo;
            
            //// (!) While Rooms last only 5m, don't actually persist this
            // SaveConfigChange(
            //     nameof(ServerConfig.HathoraLobbyRoomOpts.LastCreatedRoomConnection), 
            //     $"RoomId={_room?.RoomId} | ProcessId={_room?.CurrentAllocation.ProcessId}");
        }

        private void onCreateRoomCancelBtnClick(CancellationTokenSource _cancelTokenSrc)
        {
            Debug.Log("[HathoraConfigPostAuthBodyRoomUI] onCreateRoomCancelBtnClick");
            _cancelTokenSrc?.Cancel();
            onCreateRoomDone();
        }

        private void onCreateRoomDone(HathoraCachedRoomConnection _roomConnInfo = null)
        {
            Debug.Log("[HathoraConfigPostAuthBodyRoomUI.onCreateRoomDone] Done || Cancelled)");

            isCreatingRoomAwaitingActiveStatus = false;

            if (_roomConnInfo == null)
                return;
            
            // Potential success >> Validate
            Assert.IsNotNull(_roomConnInfo.Room?.RoomId, "!RoomId");
            
            Assert.AreEqual(_roomConnInfo.ConnectionInfoV2?.Status,
                RoomReadyStatus.Active, 
                "Status !Active");
        }
        
        
        private void onCopyRoomConnectionInfoBtnClick()
        {
            string connectionInfoStr = ServerConfig.HathoraLobbyRoomOpts.LastCreatedRoomConnection?.GetConnInfoStr();
            GUIUtility.systemCopyBuffer = connectionInfoStr; // Copy to clipboard
            
            Debug.Log($"Copied connection info to clipboard: `{connectionInfoStr}`");
        }
        #endregion // Event Logic

        
        #region Utils
        /// <summary>Cancel old, create new</summary>
        private static void createNewCreateRoomCancelToken()
        {
            // Cancel an old op 1st
            if (CreateRoomCancelTokenSrc != null && CreateRoomCancelTokenSrc.Token.CanBeCanceled)
                CreateRoomCancelTokenSrc.Cancel();
 
            CreateRoomCancelTokenSrc = new CancellationTokenSource(
                TimeSpan.FromSeconds(CREATE_ROOM_TIMEOUT_SECONDS));
        }
        #endregion // Utils
    }
}