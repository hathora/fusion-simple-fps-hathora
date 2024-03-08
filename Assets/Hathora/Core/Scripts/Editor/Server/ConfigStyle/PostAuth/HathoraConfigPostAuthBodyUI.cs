// Created by dylan@hathora.dev

using Hathora.Core.Scripts.Runtime.Server;
using UnityEditor;

namespace Hathora.Core.Scripts.Editor.Server.ConfigStyle.PostAuth
{
    public class HathoraConfigPostAuthBodyUI : HathoraConfigUIBase
    {
        #region Vars
        private HathoraConfigPostAuthBodyHeaderUI bodyHeaderUI { get; set; }
        public HathoraConfigPostAuthBodyBuildUI BodyBuildUI { get; private set; }
        public HathoraConfigPostAuthBodyDeployUI BodyDeployUI { get; private set; }
        private HathoraConfigPostAuthBodyRoomUI bodyRoomUI { get; set; }
        #endregion // Vars


        #region Init
        public HathoraConfigPostAuthBodyUI(
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
            bodyHeaderUI = new HathoraConfigPostAuthBodyHeaderUI(ServerConfig, SerializedConfig);
            BodyBuildUI = new HathoraConfigPostAuthBodyBuildUI(ServerConfig, SerializedConfig);
            BodyDeployUI = new HathoraConfigPostAuthBodyDeployUI(ServerConfig, SerializedConfig);
            bodyRoomUI = new HathoraConfigPostAuthBodyRoomUI(ServerConfig, SerializedConfig);
        }
        #endregion // Init
        
        
        #region UI Draw
        public void Draw()
        {
            if (!IsAuthed)
                return; // You should be calling HathoraConfigPreAuthBodyUI.Draw()

            bodyHeaderUI.Draw();
            InsertSpace4x();
            insertBodyFoldoutComponents();
        }

        private void insertBodyFoldoutComponents()
        {
            BodyBuildUI.Draw();

            InsertSpace1x();
            BodyDeployUI.Draw();
            
            InsertSpace1x();
            bodyRoomUI.Draw();
        }
        #endregion // UI Draw
    }
}
