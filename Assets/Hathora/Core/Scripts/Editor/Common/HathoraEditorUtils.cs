// Created by dylan@hathora.dev

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Core.Scripts.Runtime.Common.Extensions;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Hathora.Core.Scripts.Editor.Common
{
    /// <summary>
    /// Editor script to add useful Editor utils for Hathora:
    /// - Color and hyperlink constants.
    /// - Hathora banner to an editor window.
    /// - Common style utils.
    /// - File utils, including ".hathora" dir helpers. 
    /// - Terminal Process cmd handling.
    /// - DateTime parsing.
    /// </summary>
    public static class HathoraEditorUtils
    {
        #region Links
        public const string HATHORA_HOME_URL = "https://hathora.dev";
        
        /// <summary>
        /// Generally appended with application/appId: "https://console.hathora.dev/application/{appId}"
        /// </summary>
        public const string HATHORA_CONSOLE_BASE_URL = "https://console.hathora.dev";
        
        /// <summary>
        /// Generally appended with the appId: "https://console.hathora.dev/application/{appId}"
        /// </summary>
        public const string HATHORA_CONSOLE_APP_BASE_URL = HATHORA_CONSOLE_BASE_URL + "/application"; 
        
        public const string HATHORA_DOCS_URL = "https://docs.hathora.dev";
        public const string HATHORA_DOCS_DEMO_PROJECTS_URL = "https://github.com/hathora";
        public const string HATHORA_DOCS_GETTING_STARTED_URL = "https://hathora.dev/docs/engines/unity/";
        public const string HATHORA_DOCS_UNITY_TUTORIAL_URL = "https://hathora.dev/docs/engines/unity/beginners-tutorial";
        public const string HATHORA_DISCORD_URL = "https://discord.gg/hathora";
        #endregion // Links
        
        
        #region Style/Color
        // HEX OPACITY GUIDE >>
        // ##########################################
        // 0%: 00 (fully transparent)
        // 20%: 33
        // 40%: 66
        // 60%: 99
        // 80%: CC
        // 100%: FF (fully opaque)
        // ##########################################
        
        public const string HATHORA_GREEN_COLOR_HEX = "#66B9A0";
        public const string HATHORA_VIOLET_COLOR_HEX = "#AF64EE";
        public const string HATHORA_DARK_INDIGO_COLOR_HEX = "#0E0E1B";
        public const string HATHORA_PINK_CANCEL_COLOR_HEX = "#ff4265";
        public const string HATHORA_GRAY_TRANSPARENT_COLOR_HEX = "#919191CC"; // 60% opacity
        public const string HATHORA_LINK_COLOR_HEX = "#02FE57";
        
        /// <summary>(!) Be sure to pair with an ending /color tag</summary>
        public static string StartGreenColor => 
            $"<color={HATHORA_GREEN_COLOR_HEX}>";
        
        public static GUIStyle GetRichFoldoutHeaderStyle() => new(EditorStyles.foldoutHeader) { richText = true };
        public static readonly RectOffset DefaultPadding = new(left: 1, right: 1, top: 0, bottom: 0);
        
        /// <summary>Useful when a button is inside a group - you don't want it to stretch 100%</summary>
        public static readonly RectOffset SideMarginsOnly = new(left: 20, right: 20, top: 0, bottom: 0);
        
        public static readonly RectOffset DefaultBtnPadding = new(left: 10, right: 10, top: 7, bottom: 7);
        public static readonly RectOffset NoPadding = new(left: 0, right: 0, top: 0, bottom: 0);
        public static readonly RectOffset DefaultMargin = new(left: 3, right: 3, top: 2, bottom: 2);
        public static readonly RectOffset NoMargin = new(left: 0, right: 0, top: 0, bottom: 0);
        
        public static GUIStyle GetRichButtonStyle(
            int _fontSize = 13, 
            bool _wordWrap = true,
            bool _sideMargins = false)
        {
            GUIStyle richBtnStyle = new(GUI.skin.button)
            {
                richText = true,
                fontSize = _fontSize,
                wordWrap = _wordWrap,
                // margin = DefaultMargin,
                padding = DefaultBtnPadding,
            };

            if (_sideMargins)
                richBtnStyle.margin = SideMarginsOnly;

            return richBtnStyle;
        }

        public static GUIStyle GetBigButtonStyle(
            int _fontSize = 13,
            bool _wordWrap = true,
            bool _sideMargins = false)
        {
            GUIStyle bigBtnStyle = new(GUI.skin.button)
            {
                richText = true,
                fontSize = _fontSize,
                wordWrap = _wordWrap,
                // margin = DefaultMargin,
                // padding = DefaultPadding,
                fixedHeight = 50,
            };

            if (_sideMargins)
                bigBtnStyle.margin = SideMarginsOnly;

            return bigBtnStyle;
        }
        
        public static GUIStyle GetRichLabelStyle(
            TextAnchor _align, 
            bool _wordWrap = true,
            int _fontSize = 13) => new(EditorStyles.label)
        {
            richText = true,
            alignment = _align,
            wordWrap = _wordWrap,
            fontSize = _fontSize,
            // margin = DefaultMargin,
            padding = DefaultPadding,
        };

        /// <summary>
        /// No padding on right
        /// </summary>
        /// <returns></returns>
        public static GUIStyle GetPreLinkLabelStyle()
        {
            GUIStyle style = new(EditorStyles.label)
            {
                margin = DefaultMargin,
                padding = DefaultMargin,
                wordWrap = true,
                richText = true,
            };
            
            style.padding.right = 0;

            return style;
        }

        /// <summary>
        /// There's no native label links, so we fake it.
        /// </summary>
        /// <returns></returns>
        public static GUIStyle GetRichLinkStyle(TextAnchor _align) => new(GUI.skin.label)
        {
            normal =
            {
                textColor = HexToColor(HATHORA_VIOLET_COLOR_HEX),
            },
            fontStyle = FontStyle.Bold,
            alignment = _align,
            richText = true,
            margin = DefaultMargin,
            padding = DefaultPadding,
        };
        
        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("#", "");
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a = 255;

            if (hex.Length == 8)
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            return new Color32(r, g, b, a);
        }
        #endregion // Style/Color
        

        /// <summary>
        /// Add 
        /// --
        /// Wrapped in a Rect. Aligns center. Dark bg on transparent img,
        /// showing an illusion of dynamic size. Shrinks to fit.
        /// </summary>
        /// <param name="_wrapperExtension">Extend the black banner down</param>
        /// <param name="_labelPadding">Push the content below down further</param>
        /// <param name="_includeVerticalGroup">
        /// (!) If not, include `GUILayout.EndVertical()` shortly after this call.
        /// You would set it to `false` if you wanted to include middleware in
        /// between to add labels/buttons INSIDE the banner.
        /// </param>
        public static void InsertBanner(
            float _wrapperExtension = 3, 
            float _labelPadding = 10f,
            bool _includeVerticalGroup = true)
        {
            Texture2D bannerTexture = Resources.Load<Texture2D>("HathoraConfigBanner");
            if (bannerTexture == null)
                return;

            float windowWidth = EditorGUIUtility.currentViewWidth;
            float bannerWidth = bannerTexture.width;
            float bannerHeight = bannerTexture.height;

            const float minBannerWidth = 200f; // Set the minimum banner width
            float maxBannerWidth = Mathf.Max(windowWidth * 0.5f, minBannerWidth);
            if (bannerWidth > maxBannerWidth)
            {
                float scale = maxBannerWidth / bannerWidth;
                bannerWidth = maxBannerWidth;
                bannerHeight *= scale;
            }

            // Calculate the banner's position and size
            const float bannerBgPaddingTop = 12f;
            const float bannerBgPaddingBottom = 5f;

            float bannerX = (windowWidth - bannerWidth) * 0.5f; // center
            float bannerY = bannerBgPaddingTop;

            // Create the padded rect with extended wrapper
            float adjustedBannerHeight = bannerHeight + bannerBgPaddingTop + bannerBgPaddingBottom + _wrapperExtension;
            Rect paddedRect = new(
                x: 0,
                y: 0,
                width: windowWidth,
                height: adjustedBannerHeight);

            // Draw a dark background for the entire horizontal area with padding
            Color indigoColor = HexToColor(HATHORA_DARK_INDIGO_COLOR_HEX);
            EditorGUI.DrawRect(paddedRect, indigoColor);

            // Draw the banner texture centered within the padded rect
            Rect bannerRect = new(bannerX, bannerY, bannerWidth, bannerHeight);
            GUI.DrawTexture(bannerRect, bannerTexture);

            GUILayout.BeginVertical(GUILayout.Height(paddedRect.height));
            GUILayout.Space(bannerHeight + _labelPadding);
            
            if (_includeVerticalGroup)
                GUILayout.EndVertical();
        }
        
        public static void InsertHathoraSloganLbl()
        {
            GUILayout.Label("Multiplayer Server Hosting", 
                GetRichLabelStyle(_align: TextAnchor.MiddleCenter));
        }

        /// <summary>Useful for 7z compression handling.</summary>
        /// <param name="_workingDirPath">Which starting dir to start issuing these cmds?</param>
        /// <param name="_cmdWithArgs"></param>
        /// <param name="_printLogs">This could get spammy</param>
        /// <param name="_cancelToken"></param>
        /// <returns></returns>
        public static async Task<(Process process, string resultLog)> ExecuteCrossPlatformShellCmdAsync(
            string _workingDirPath,
            string _cmdWithArgs, 
            bool _printLogs = true,
            CancellationToken _cancelToken = default)
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            string shell = isWindows ? "cmd.exe" : "/bin/bash";
            
            string escapedArgs = isWindows 
                ? $"/c {_cmdWithArgs}" 
                : $"-c \"{_cmdWithArgs}\"";
            
            Debug.Log($"[HathoraEditorUtils.ExecuteCrossPlatformShellCmdAsync] Details: " +
                $"\nshell: <color=yellow>{shell}</color>, " +
                $"\nworkingDir: <color=yellow>{_workingDirPath}</color>, " +
                $"\ncmd+args: <color=yellow>`{_cmdWithArgs}`</color>");

            Process process = new()
            { 
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = _workingDirPath,
                    FileName = shell,
                    Arguments = escapedArgs,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
            };

            process.Start();

            // Read the output and error asynchronously
            Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
            Task<string> errorTask = process.StandardError.ReadToEndAsync();

            // Wait for the process to finish and read the output and error
            await process.WaitForExitAsync(_cancelToken);

            // Combine output and error
            string outputLog = await outputTask;
            string errorLog = await errorTask;
            string resultLog = outputLog + errorLog;

            if (_printLogs)
            {
                Debug.Log("[HathoraEditorUtils.ExecuteCrossPlatformShellCmdAsync] " +
                    $"resultLog (including errs, if any): <color=yellow>{resultLog}</color>");
            }

            return (process, resultLog);
        }

        public static void ValidateCreateDotHathoraDir()
        {
            const string dotHathoraDirName = ".hathora";
            string projRoot = HathoraUtils.GetNormalizedPathToProjRoot();
            string pathToDotHathoraDir = $"{projRoot}/{dotHathoraDirName}";
            
            if (!Directory.Exists(pathToDotHathoraDir))
                Directory.CreateDirectory(pathToDotHathoraDir);
        }

        public static string GetFileFriendlyDateTime(DateTime _now) => 
            DateTime.Now.ToString("s")
                .Replace(":", "")
                .Replace("T", "_");

        public static void DeleteFileIfExists(string _pathToFile) 
        {
            try
            {
                if (File.Exists(_pathToFile))
                    File.Delete(_pathToFile);    
            }
            catch (Exception e) 
            {
                Debug.LogError($"[HathoraEditorUtils.DeleteFileIfExists] Error: {e}");
                throw;
            }
        }

        public static void FileMoveOverwrite(string _pathToSrc, string _pathToDest)
        {
            DeleteFileIfExists(_pathToDest);
            File.Move(_pathToSrc, _pathToDest);
        }
    }
}
