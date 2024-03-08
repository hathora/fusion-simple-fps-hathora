// Created by dylan@hathora.dev

using System.Text;
using Hathora.Core.Scripts.Editor.Common;
using Hathora.Core.Scripts.Editor.Server.ConfigStyle.PostAuth;
using Hathora.Core.Scripts.Runtime.Server;
using UnityEditor;
using UnityEngine;

namespace Hathora.Core.Scripts.Editor.Server.ConfigStyle
{
    public class HathoraConfigFooterUI : HathoraConfigUIBase
    {
        /// <summary>
        /// For the combo build+deploy btn, we want to use the same deploy func
        /// </summary>
        private readonly HathoraConfigPostAuthBodyDeployUI postAuthBodyDeployUI;
        
        /// <summary>
        /// For the combo build+deploy btn, we want to use the same build func
        /// </summary>
        private readonly HathoraConfigPostAuthBodyBuildUI postAuthBodyBuildUI;
        
        // Scrollable logs
        /// <summary>Useful for debugging/styling without having to build each time</summary>
        private const bool MOCK_BUILD_LOGS = false;
        private Vector2 buildLogsScrollPos = Vector2.zero;
        private Vector2 deployLogsScrollPos = Vector2.zero;
        private bool isBuildLogsFoldoutHeaderOpen = true;
        private bool isDeployLogsFoldoutHeaderOpen = true;
        

        public HathoraConfigFooterUI(
            HathoraServerConfig _serverConfig, 
            SerializedObject _serializedConfig,
            HathoraConfigPostAuthBodyBuildUI _postAuthBodyBuildUI,
            HathoraConfigPostAuthBodyDeployUI _postAuthBodyDeployUI) 
            : base(_serverConfig, _serializedConfig)
        {
            this.postAuthBodyBuildUI = _postAuthBodyBuildUI;
            this.postAuthBodyDeployUI = _postAuthBodyDeployUI;
        }

        public void Draw()
        {
            if (IsAuthed)
            {
                InsertSpace1x();
                insertPostAuthFooter();
            }
            else
            {
                InsertSpace4x();
                insertPreAuthFooter();
            }
        }

        private void insertPostAuthFooter()
        {
            insertScrollableLogs();
        }

        private void insertScrollableLogs()
        {
            bool hasLastbuildLogsStrb = ServerConfig.LinuxHathoraAutoBuildOpts.HasLastBuildLogsStrb;
            bool hasLastDeployLogsStrb = ServerConfig.HathoraDeployOpts.HasLastDeployLogsStrb;
            if (!hasLastbuildLogsStrb && !hasLastDeployLogsStrb && !MOCK_BUILD_LOGS)
                return;

            InsertHorizontalLine();
            InsertSpace1x();

            if (hasLastbuildLogsStrb || MOCK_BUILD_LOGS)
                insertBuildLogsFoldoutHeader();

            if (hasLastDeployLogsStrb || MOCK_BUILD_LOGS)
                insertDeployLogsFoldoutHeader();
        }

        private void insertDeployLogsFoldoutHeader()
        {
            if (!ServerConfig.HathoraDeployOpts.HasLastDeployLogsStrb)
            {
                if (!MOCK_BUILD_LOGS)
                    return;
                
                // createFakeLogs(ServerConfig.HathoraDeployOpts.LastDeployLogsStrb);
            }

            isDeployLogsFoldoutHeaderOpen = EditorGUILayout.BeginFoldoutHeaderGroup(
                isDeployLogsFoldoutHeaderOpen, 
                @"""Deploy Application"" logs");
            
            // USER INPUT >>
            if (!isDeployLogsFoldoutHeaderOpen)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }

            // Content within the foldout >>
            insertDeployLogsScrollLbl();
            
            EditorGUILayout.EndFoldoutHeaderGroup();
            InsertSpace1x();
        }

        private void insertBuildLogsFoldoutHeader()
        {
            if (!ServerConfig.LinuxHathoraAutoBuildOpts.HasLastBuildLogsStrb)
            {
                if (!MOCK_BUILD_LOGS)
                    return;
                
                // createFakeLogs(ServerConfig.LinuxHathoraAutoBuildOpts.LastBuildLogsStrb);
            }

            isBuildLogsFoldoutHeaderOpen = EditorGUILayout.BeginFoldoutHeaderGroup(
                isBuildLogsFoldoutHeaderOpen, 
                @"""Generate Server Build"" logs");
            
            // USER INPUT >>
            if (!isBuildLogsFoldoutHeaderOpen)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }

            // Content within the foldout >>
            insertBuildLogsScrollLbl();
            
            EditorGUILayout.EndFoldoutHeaderGroup();
            InsertSpace1x();
        }
        
        private void insertDeployLogsScrollLbl()
        {
            // If we have both logs, we want 1/2 the size
            float height = ServerConfig.LinuxHathoraAutoBuildOpts.HasLastBuildLogsStrb
                ? 150f // Also has build logs 
                : 300f;
            
            deployLogsScrollPos = GUILayout.BeginScrollView(
                deployLogsScrollPos,
                GUILayout.ExpandWidth(true),
                GUILayout.Height(height));
            
            // Content within the scroller >>
            base.BeginPaddedBox();
            InsertLabel(ServerConfig.HathoraDeployOpts.LastDeployLogsStrb.ToString());
            base.EndPaddedBox();
            
            GUILayout.EndScrollView();
            InsertSpace1x();
        }

        private void insertBuildLogsScrollLbl()
        {
            // If we have both logs, we want 1/2 the size
            float height = ServerConfig.HathoraDeployOpts.HasLastDeployLogsStrb
                ? 150f // Also has build logs 
                : 300f;
            
            buildLogsScrollPos = GUILayout.BeginScrollView(
                buildLogsScrollPos,
                GUILayout.ExpandWidth(true),
                GUILayout.Height(height));
            
            // Content within the scroller >>
            base.BeginPaddedBox();
            
            if (ServerConfig.LinuxHathoraAutoBuildOpts.LastBuildLogsStrb?.Length > 0)
            {
                InsertLabel(ServerConfig.LinuxHathoraAutoBuildOpts.LastBuildLogsStrb.ToString());
            }
            else
            {
                InsertLabel(ServerConfig.LinuxHathoraAutoBuildOpts.LastBuildLogsStr);
            }
            base.EndPaddedBox();
            
            GUILayout.EndScrollView();
            InsertSpace1x();
        }

        private void createFakeLogs(StringBuilder _strb)
        {
            // Foo 1 ~ 50
            _strb.Clear()
                .AppendLine("Foo 1").AppendLine("Foo 2").AppendLine("Foo 3")
                .AppendLine("Foo 4").AppendLine("Foo 5").AppendLine("Foo 6")
                .AppendLine("Foo 7").AppendLine("Foo 8").AppendLine("Foo 9")
                .AppendLine("Foo 10").AppendLine("Foo 11").AppendLine("Foo 12")
                .AppendLine("Foo 13").AppendLine("Foo 14").AppendLine("Foo 15")
                .AppendLine("Foo 16").AppendLine("Foo 17").AppendLine("Foo 18")
                .AppendLine("Foo 19").AppendLine("Foo 20").AppendLine("Foo 21")
                .AppendLine("Foo 22").AppendLine("Foo 23").AppendLine("Foo 24")
                .AppendLine("Foo 25").AppendLine("Foo 26").AppendLine("Foo 27")
                .AppendLine("Foo 28").AppendLine("Foo 29").AppendLine("Foo 30")
                .AppendLine("Foo 31").AppendLine("Foo 32").AppendLine("Foo 33")
                .AppendLine("Foo 34").AppendLine("Foo 35").AppendLine("Foo 36")
                .AppendLine("Foo 37").AppendLine("Foo 38").AppendLine("Foo 39")
                .AppendLine("Foo 40").AppendLine("Foo 41").AppendLine("Foo 42")
                .AppendLine("Foo 43").AppendLine("Foo 44").AppendLine("Foo 45")
                .AppendLine("Foo 46").AppendLine("Foo 47").AppendLine("Foo 48")
                .AppendLine("Foo 49").AppendLine("Foo 50");
        }

        private void insertPreAuthFooter()
        {
            InsertHorizontalLine(1.5f, Color.gray, _space: 20);
            InsertCenterLabel("Learn more about Hathora Cloud");
            InsertLinkLabel("Documentation", HathoraEditorUtils.HATHORA_DOCS_URL, _centerAlign: true);
            InsertLinkLabel("Demo Projects", HathoraEditorUtils.HATHORA_DOCS_DEMO_PROJECTS_URL, _centerAlign: true);
        }
    }
}