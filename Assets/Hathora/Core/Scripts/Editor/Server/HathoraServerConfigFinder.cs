// Created by dylan@hathora.dev

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using Hathora.Core.Scripts.Runtime.Server;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hathora.Core.Scripts.Editor.Server
{
    public class HathoraServerConfigFinder : EditorWindow
    {
        private const string LAST_FOCUSED_SERVER_CONFIG_PATH = "HathoraServerConfigFinder.LastFocusedServerConfigPath";
        
        
        #region Menu Items
        [MenuItem("Hathora/Select Last-Used HathoraServerConfig _%#h", priority = -1000)] // Ctrl + Shift + H
        public static void SelectLastKnownServerConfig()
        {
            string assetPath = EditorPrefs.GetString(LAST_FOCUSED_SERVER_CONFIG_PATH);

            if (string.IsNullOrEmpty(assetPath))
            {
                List<HathoraServerConfig> configs = getAllHathoraServerConfigs();
                Assert.IsNotNull(configs?[0], "Expected at least 1 config; TODO: Create a new Config, if !exists");
                selectHathoraServerConfig(configs[0]); // Find the 1st one
            }

            // Find the last known
            HathoraServerConfig config = AssetDatabase.LoadAssetAtPath<HathoraServerConfig>(assetPath);
            selectHathoraServerConfig(config);
        }
        
        [MenuItem("Hathora/Create New HathoraServerConfig")]
        public static void CreateNewServerConfig()
        {
            HathoraServerConfig newConfig = ScriptableObject.CreateInstance<HathoraServerConfig>();
            
            // Ensure target dir exists
            const string dirPath = "Assets/";
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            
            // Set the asset path and asset name
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{dirPath}/{nameof(HathoraServerConfig)}.asset");

            // Create the asset in the designated path
            AssetDatabase.CreateAsset(newConfig, assetPathAndName);

            // Save and refresh the asset db
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            // Select the new config
            selectHathoraServerConfig(newConfig);
        }
        #endregion // Menu Items
        
   
        /// <returns>List of HathoraServerConfig</returns>
        private static List<HathoraServerConfig> getAllHathoraServerConfigs()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(HathoraServerConfig)}");
            
            List<HathoraServerConfig> serverConfigs = guids?.Select(guid => 
                AssetDatabase.LoadAssetAtPath<HathoraServerConfig>(AssetDatabase.GUIDToAssetPath(guid))
            ).ToList();

            return serverConfigs;
        }
        
        /// <summary>Find and select the Config to both show in the Inspector and Project windows</summary>
        /// <param name="_config"></param>
        private static void selectHathoraServerConfig(HathoraServerConfig _config)
        {
            EditorGUIUtility.PingObject(_config);
            Selection.activeObject = _config;
        }

        /// <summary>
        /// Call this from HathoraServerConfig to save the last focused
        /// Config to recall later via top Hathora/ menu.
        /// </summary>
        /// <param name="_activeObject"></param>
        public static void CacheSelectedConfig(HathoraServerConfig _activeObject)
        {
            string assetPath = AssetDatabase.GetAssetPath(_activeObject);
            EditorPrefs.SetString(LAST_FOCUSED_SERVER_CONFIG_PATH, assetPath);
        }
    }
}
