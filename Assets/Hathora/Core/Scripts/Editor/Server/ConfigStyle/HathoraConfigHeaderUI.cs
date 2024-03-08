// Created by dylan@hathora.dev

using Hathora.Core.Scripts.Editor.Common;
using Hathora.Core.Scripts.Runtime.Server;
using UnityEditor;
using UnityEngine;

namespace Hathora.Core.Scripts.Editor.Server.ConfigStyle
{
    public class HathoraConfigHeaderUI : HathoraConfigUIBase
    {
        public HathoraConfigHeaderUI(
            HathoraServerConfig _serverConfig, 
            SerializedObject _serializedConfig) 
            : base(_serverConfig, _serializedConfig)
        {
        }

        public void Draw()
        {
            HathoraEditorUtils.InsertBanner(
                _includeVerticalGroup: false,
                _wrapperExtension: 60f); // Place banner @ top
            HathoraEditorUtils.InsertHathoraSloganLbl();

            InsertSpace1x();
            insertHeaderBtns();
            GUILayout.EndVertical();
            
            insertEditingTemplateWarningMemo();
            InsertSpace2x();
        }
        
        private void insertHeaderBtns()
        {
            StartCenterHorizAlign();
            
            GUILayoutOption[] buttonOptions =
            {
                GUILayout.MinWidth(100), 
                GUILayout.MinHeight(24), 
                GUILayout.ExpandWidth(true),
            };
            

            if (GUILayout.Button("Console", GeneralButtonStyle, buttonOptions))
                Application.OpenURL(HathoraEditorUtils.HATHORA_CONSOLE_BASE_URL);

            InsertSpace1x();

            if (GUILayout.Button("Tutorial", GeneralButtonStyle, buttonOptions))
                Application.OpenURL(HathoraEditorUtils.HATHORA_DOCS_UNITY_TUTORIAL_URL);

            InsertSpace1x();

            if (GUILayout.Button("Docs", GeneralButtonStyle, buttonOptions))
                Application.OpenURL(HathoraEditorUtils.HATHORA_DOCS_GETTING_STARTED_URL);

            InsertSpace1x();

            if (GUILayout.Button("Discord", GeneralButtonStyle, buttonOptions))
                Application.OpenURL(HathoraEditorUtils.HATHORA_DISCORD_URL);

            EndCenterHorizAlign();
        }
        
        /// <summary>
        /// Only insert memo if using the template file
        /// </summary>
        private void insertEditingTemplateWarningMemo()
        {
            string configTemplateName = $"{nameof(HathoraServerConfig)}.template";
            bool isDefaultName = ServerConfig.name == configTemplateName;

            if (isDefaultName)
                insertEditingTemplateWarnHelpbox();
            // else            
            //     insertGitIgnoreThisWarnHelpbox(); // TODO: Do we want this?
        }

        private void insertGitIgnoreThisWarnHelpbox()
        {
            string helpText = "(.gitignore this file)";
            GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.LabelField(helpText, centeredStyle);
            
            InsertSpace2x();
        }

        private void insertEditingTemplateWarnHelpbox()
        {
            EditorGUILayout.HelpBox("You are editing a template!\n" +
                "1. Duplicate this (CTRL+D)\n" +
                "2. Add dupe to .gitignore >> treat as an .env file", 
                MessageType.Warning);
            
            InsertSpace2x();
        }
    }
}
