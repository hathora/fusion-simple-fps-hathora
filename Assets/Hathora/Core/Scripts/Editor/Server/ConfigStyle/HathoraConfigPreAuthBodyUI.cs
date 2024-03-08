// Created by dylan@hathora.dev

using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Editor.Common;
using Hathora.Core.Scripts.Editor.Server.Auth0;
using Hathora.Core.Scripts.Runtime.Server;
using UnityEditor;
using UnityEngine;

#pragma warning disable 4014
namespace Hathora.Core.Scripts.Editor.Server.ConfigStyle
{
    public class HathoraConfigPreAuthBodyUI : HathoraConfigUIBase
    {
        // Declare an event to trigger a repaint
        private static bool devAuthLoginButtonInteractable = true;

        /// <summary>Checking a file for a cached token repeatedly would cause lag</summary>
        public static bool CheckedTokenCache { get; set; }
        
        private string cachedToken;
        private bool CheckHasCachedToken() => 
            !string.IsNullOrEmpty(cachedToken);
        
        
        public HathoraConfigPreAuthBodyUI(
            HathoraServerConfig _serverConfig, 
            SerializedObject _serializedConfig) 
            : base(_serverConfig, _serializedConfig)
        {
        }
        
        
        #region UI Draw
        public void Draw()
        {
            if (IsAuthed)
                return; // You should be calling HathoraConfigPostAuthBodyUI.Draw()

            EditorGUILayout.BeginVertical(GUI.skin.box);
            
            insertAuthHeaderLbl();
            insertRegAuthBtns();

            EditorGUILayout.EndVertical();
            InsertSpace2x();
            
            InvokeRequestRepaint();
        }

        private void insertAuthHeaderLbl()
        {
            InsertSpace3x();   
            const string labelStr = "Create an account or log in to Hathora Cloud's Console to get started";
            GUILayout.Label(labelStr, 
                CenterAlignLabelStyle);
            
            InsertSpace2x();        
        }
        
        /// <summary>
        /// These essentially do the same thing
        /// </summary>
        private void insertRegAuthBtns()
        {
            bool showCancelBtn = !HathoraServerAuth.IsAuthComplete && !devAuthLoginButtonInteractable;
            
            // !await these
            if (showCancelBtn)
            {
                insertAuthCancelBtn(HathoraServerAuth.AuthCancelTokenSrc);
                return;
            }
            
            insertDevAuthLoginBtn(); // !await
            InsertMoreActionsLbl();
            insertRegisterOrTokenCacheLogin();
        }

        private void insertRegisterOrTokenCacheLogin()
        {
            tokenCheckSetCache();
            
            if (CheckHasCachedToken())
                insertTokenCacheBtn();
            else
                insertRegisterLinkLbl();
        }

        private void InsertMoreActionsLbl()
        {
            InsertCenterLabel("<b>- or -</b>");
            InsertSpace1x();
        }

        /// <summary>
        /// If we didn't check it before and exists, insert the button
        /// </summary>
        private void tokenCheckSetCache()
        {
            if (CheckedTokenCache)
                return;
            
            cachedToken = Auth0Login.CheckForExistingCachedTokenAsync();
            CheckedTokenCache = true;
        }

        private void insertTokenCacheBtn()
        {
            InsertCenterLabel("Existing token cache found");
        
            // USER INPUT >> Calls back onClick
            InsertLinkLabel(
                "Log in with token",
                _url: null,
                _centerAlign: true,
                onClick: onLoginWithTokenCacheBtnClick);  

            InsertSpace3x();
        }

        private void insertRegisterLinkLbl()
        {
            InsertCenterLabel("Don't have an account yet?");
        
            // USER INPUT >> Calls back onClick
            InsertLinkLabel(
                "Register Here",
                _url: null,
                _centerAlign: true,
                onClick: onRegisterBtnClick);  

            InsertSpace3x();
        }

        private async Task insertDevAuthLoginBtn()
        {
            // USER INPUT >>
            bool clickedLoginBtn = GUILayout.Button("Log in to Hathora Cloud", BigButtonSideMarginsStyle);
            if (clickedLoginBtn)
                await onLoginBtnClickAsync();
            
            InsertSpace2x();
        }
        
        private void insertAuthCancelBtn(CancellationTokenSource _cancelTokenSrc) 
        {
            string btnLabelStr = $"<color={HathoraEditorUtils.HATHORA_PINK_CANCEL_COLOR_HEX}>" +
                "<b>Cancel</b> (Logging in via browser...)</color>";
            
            // USER INPUT >>
            bool clickedAuthCancelBtn = GUILayout.Button(btnLabelStr, GeneralSideMarginsButtonStyle);
            if (clickedAuthCancelBtn)
                onAuthCancelBtnClick(_cancelTokenSrc);
            
            InsertSpace2x();
            InvokeRequestRepaint();
        }
        
        private void onLoginWithTokenCacheBtnClick() =>
            onInsertTokenCacheBtnClick(cachedToken);
        
        private void onInsertTokenCacheBtnClick(string _cachedAuthToken)
        {
            HathoraServerAuth.SetAuthToken(ServerConfig, _cachedAuthToken);
            onLoginSuccess();
        }
        #endregion // UI Draw

        
        #region Logic Events
        private void onRegisterBtnClick() => 
            onLoginBtnClickAsync(); // !await
        
        private async Task<bool> onLoginBtnClickAsync()
        {
            Debug.Log("[HathoraConfigPreAuthBodyUI] onLoginBtnClickAsync");
            devAuthLoginButtonInteractable = false;

            HathoraServerAuth.AuthCompleteSrc = new TaskCompletionSource<bool>();
            bool isSuccess = await HathoraServerAuth.DevAuthLogin(ServerConfig);
            if (!isSuccess)
                onLoginFail();
            else
                onLoginSuccess();
            
            devAuthLoginButtonInteractable = true;
            return isSuccess;
        }

        private void onLoginSuccess()
        {
            Debug.Log("[HathoraConfigPreAuthBodyUI] onLoginSuccess");
             
            // Set a flag to refresh apps automatically the next time we Draw post-auth body header
            if (!HathoraServerAuth.IsAuthComplete)
                HathoraServerAuth.AuthCompleteSrc?.SetResult(true); // isSuccess
            
            ServerConfig.HathoraCoreOpts.DevAuthOpts.RecentlyAuthed = true;
        }

        private void onLoginFail()
        {
            Debug.Log("[HathoraConfigPreAuthBodyUI] onLoginFail");
            
            if (!HathoraServerAuth.IsAuthComplete)
                HathoraServerAuth.AuthCompleteSrc?.SetResult(false); // !isSuccess
        }

        private void onAuthCancelBtnClick(CancellationTokenSource _cancelTokenSrc)
        {
            _cancelTokenSrc?.Cancel();
            devAuthLoginButtonInteractable = true;
        }
        #endregion // Logic Events


        #region Utils
        /// <summary>The auth button should be disabled while authenticating.</summary>
        private bool checkHasActiveAuthPolling() =>
            devAuthLoginButtonInteractable && !CheckHasAuthToken();
        #endregion // Utils
    }
}
#pragma warning restore 4014