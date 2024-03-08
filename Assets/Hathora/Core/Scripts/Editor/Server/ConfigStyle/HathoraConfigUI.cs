// Created by dylan@hathora.dev

using Hathora.Core.Scripts.Editor.Server.ConfigStyle.PostAuth;
using Hathora.Core.Scripts.Runtime.Server;
using UnityEditor;
using UnityEngine;

namespace Hathora.Core.Scripts.Editor.Server.ConfigStyle
{
    /// <summary>
    /// The main editor for HathoraServerConfig, including all the button clicks and extra UI.
    /// </summary>
    [CustomEditor(typeof(HathoraServerConfig), editorForChildClasses: true)]
    public class HathoraConfigUI : UnityEditor.Editor
    {
        #region Vars
        
        private HathoraConfigHeaderUI headerUI { get; set; }
        private HathoraConfigPreAuthBodyUI preAuthBodyUI { get; set; }
        private HathoraConfigPostAuthBodyUI postAuthBodyUI { get; set; }
        private HathoraConfigFooterUI footerUI { get; set; }
                
        private string previousDevAuthToken { get; set; }
        private HathoraServerConfig SelectedServerConfig { get; set; }
        private SerializedObject serializedConfig { get; set; }
        
        private bool IsAuthed => 
            SelectedServerConfig.HathoraCoreOpts.DevAuthOpts.HasAuthToken;

        private HathoraServerConfig getSelectedInstance() =>
            (HathoraServerConfig)target;
        #endregion // Vars

        
        #region Main
        public void OnEnable()
        {
            // Get instance - there may be multiple
            if (SelectedServerConfig == null)
                SelectedServerConfig = getSelectedInstance();
            
            // Save last-focused window so we can find it easy later via top Hathora/ menu
            HathoraServerConfigFinder.CacheSelectedConfig(SelectedServerConfig);

            // If !authed, check again for a physical token cache file
            if (SelectedServerConfig != null && !IsAuthed)
                HathoraConfigPreAuthBodyUI.CheckedTokenCache = false;
        }

        /// <summary>
        /// Essentially the editor version of Update().
        /// We'll mask over the entire ServerConfig with a styled UI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            checkForDirtyRefs();
            drawHeaderBodyFooter();
            // (!) Saved changes occur @ HathoraConfigUIBase.SaveConfigChange()
        }

        private void drawHeaderBodyFooter()
        {
            headerUI.Draw();
            
            if (IsAuthed)
                postAuthBodyUI.Draw();
            else
                preAuthBodyUI.Draw();
            
            footerUI.Draw();
        }

        private void checkForDirtyRefs()
        {
            bool lostRefs = headerUI == null 
                || preAuthBodyUI == null 
                || postAuthBodyUI == null
                || footerUI == null 
                || !ReferenceEquals(SelectedServerConfig, getSelectedInstance());
            
            if (lostRefs)
                initDrawUtils();
            
            serializedConfig.Update();
        }

        private void initDrawUtils()
        {
            SelectedServerConfig = getSelectedInstance();
            serializedConfig = new SerializedObject(SelectedServerConfig);

            // New instances of util draw classes
            headerUI = new HathoraConfigHeaderUI(SelectedServerConfig, serializedConfig);
            preAuthBodyUI = new HathoraConfigPreAuthBodyUI(SelectedServerConfig, serializedConfig);
            
            postAuthBodyUI = new HathoraConfigPostAuthBodyUI(
                SelectedServerConfig, 
                serializedConfig);
            
            footerUI = new HathoraConfigFooterUI(
                SelectedServerConfig, 
                serializedConfig,
                postAuthBodyUI.BodyBuildUI,
                postAuthBodyUI.BodyDeployUI
            );
            
            // Subscribe to repainting events // TODO: Deprecated?
            headerUI.RequestRepaint += Repaint;
            preAuthBodyUI.RequestRepaint += Repaint;
            postAuthBodyUI.RequestRepaint += Repaint;
            footerUI.RequestRepaint += Repaint;
        }
        #endregion // Main


        #region Core Buttons
        // private void insertSplitButtons(HathoraServerConfig _serverConfig, bool _isAuthed)
        // {
        //     EditorGUILayout.Space(5f);
        //
        //     if (!_isAuthed)
        //     {
        //
        //         return;
        //     }
        //
        //     // InsertHorizontalLine(1, Color.gray);
        //     EditorGUILayout.Space(10f);
        //
        //     EditorGUILayout.BeginVertical(GUI.skin.box);
        //     GUILayout.Label($"{HathoraEditorUtils.StartHathoraGreenColor}" +
        //         "Customize via `Linux Auto Build Opts`</color>", CenterAlignLabelStyle);
        //     insertBuildBtn(_serverConfig);
        //     EditorGUILayout.EndVertical();
        //
        //     EditorGUILayout.Space(10f);
        //
        //     EditorGUILayout.BeginVertical(GUI.skin.box);
        //     GUILayout.Label($"{HathoraEditorUtils.StartHathoraGreenColor}" +
        //         "Customize via `Hathora Deploy Opts`</color>", CenterAlignLabelStyle);
        //     insertHathoraDeployBtn(_serverConfig);
        //     EditorGUILayout.EndVertical();
        // }

        // private static async Task insertHathoraDeployBtn(HathoraServerConfig SelectedServerConfig)
        // {
        //     GUI.enabled = SelectedServerConfig.MeetsDeployBtnReqs();
        //
        //     if (GUILayout.Button("Deploy to Hathora", GeneralButtonStyle))
        //     {
        //         await HathoraServerDeploy.DeployToHathoraAsync(SelectedServerConfig);
        //         EditorGUILayout.Space(20f);
        //     }
        //     
        //     GUI.enabled = true;
        // }
        
        // private static void insertBuildBtn(HathoraServerConfig SelectedServerConfig)
        // {
        //     GUI.enabled = SelectedServerConfig.MeetsBuildBtnReqs();
        //     
        //     if (GUILayout.Button("Build Linux Server", GeneralButtonStyle))
        //     {
        //         HathoraServerBuild.BuildHathoraLinuxServer(SelectedServerConfig);
        //     }
        //     
        //     GUI.enabled = true;
        // }
        #endregion // Core Buttons
        
        
        #region Meta
        /// <summary>Change the icon of the ScriptableObject in the Project window.</summary>
        public override Texture2D RenderStaticPreview(
            string assetPath,
            Object[] subAssets,
            int width,
            int height)
        {
            Texture2D icon = Resources.Load<Texture2D>("Icons/HathoraSmall");
            return icon == null 
                ? base.RenderStaticPreview(assetPath, subAssets, width, height) // Default fallback
                : icon; // our custom icon
        }
        #endregion // Meta


        private void OnDisable()
        {
            if (headerUI != null)
                headerUI.RequestRepaint -= Repaint;
            
            if (preAuthBodyUI != null)
                preAuthBodyUI.RequestRepaint-= Repaint;
            
            if (postAuthBodyUI != null)
                postAuthBodyUI.RequestRepaint -= Repaint;
            
            if (footerUI != null)
                footerUI.RequestRepaint -= Repaint;
        }
    }
}