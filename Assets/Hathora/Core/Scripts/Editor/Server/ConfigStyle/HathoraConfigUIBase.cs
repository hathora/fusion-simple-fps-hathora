// Created by dylan@hathora.dev

using System;
using System.Collections.Generic;
using System.Linq;
using Hathora.Core.Scripts.Editor.Common;
using Hathora.Core.Scripts.Runtime.Common.Extensions;
using Hathora.Core.Scripts.Runtime.Server;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hathora.Core.Scripts.Editor.Server.ConfigStyle
{
    public abstract class HathoraConfigUIBase
    {
        #region Vars
        public enum AlignType
        {
            Center,
            Left,
            Right,
        }
        
        /// <summary>The selected ServerConfig instance</summary>
        protected HathoraServerConfig ServerConfig { get; }
        
        /// <summary>
        /// Used to ApplyModifiedProperties() - save ServerConfig with peristence.
        /// </summary>
        protected SerializedObject SerializedConfig { get; }
        
        /// <summary>Do we have an Auth token?</summary>
        protected bool IsAuthed => 
            ServerConfig.HathoraCoreOpts.DevAuthOpts.HasAuthToken;
        
        private const float DEFAULT_MAX_FIELD_WIDTH = 250F;
        
        protected GUIStyle CenterAlignLabelStyle { get; private set; }
        protected GUIStyle CenterAlignSmLabelStyle { get; private set; }
        protected GUIStyle CenterAlignLargerTxtLabelNoWrapStyle { get; private set; }
        protected GUIStyle LeftAlignLabelStyle { get; private set; }
        protected GUIStyle LeftAlignNoWrapLabelStyle { get; private set; }
        protected GUIStyle CenterLinkLabelStyle { get; private set; }
        protected GUIStyle RightAlignLabelStyle { get; private set; }
        protected GUIStyle PreLinkLabelStyle { get; private set; }
        protected GUIStyle GeneralButtonStyle { get; private set; }
        protected GUIStyle GeneralSideMarginsButtonStyle { get; private set; }
        protected GUIStyle BigButtonStyle { get; private set; }
        protected GUIStyle BigButtonSideMarginsStyle { get; private set; }
        protected GUIStyle BtnsFoldoutStyle { get; private set; }
        protected GUIStyle PaddedBoxStyle { get; private set; }

        public event Action RequestRepaint;

        public enum GuiAlign
        {
            Stretched,
            SmallLeft,
            SmallRight,
        }
        #endregion // Vars

        
        #region Init
        protected HathoraConfigUIBase(
            HathoraServerConfig _serverConfig, 
            SerializedObject _serializedConfig)
        {
            Assert.IsNotNull(_serverConfig, "ServerConfig cannot be null");
            Assert.IsNotNull(_serializedConfig, "SerializedConfig cannot be null");
            
            this.ServerConfig = _serverConfig;
            this.SerializedConfig = _serializedConfig;
            
            initStyles();
        }

        private void initStyles()
        {
            initButtonStyles();
            initBtnFoldoutStyles();
            initLabelStyles();
            initLayoutStyles();
        }

        private void initLayoutStyles()
        {
            PaddedBoxStyle = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(10, 10, 10, 10),
            };
        }

        /// <summary>
        /// Adds padding, rich text, and sets font size to 13.
        /// </summary>
        private void initButtonStyles()
        {
            this.GeneralButtonStyle ??= HathoraEditorUtils.GetRichButtonStyle();
            this.GeneralSideMarginsButtonStyle ??= HathoraEditorUtils.GetRichButtonStyle(_sideMargins: true);
            this.BigButtonStyle ??= HathoraEditorUtils.GetBigButtonStyle();
            this.BigButtonSideMarginsStyle ??= HathoraEditorUtils.GetBigButtonStyle(_sideMargins: true);
        }
 
        private void initBtnFoldoutStyles()
        {
            this.BtnsFoldoutStyle ??= HathoraEditorUtils.GetRichFoldoutHeaderStyle();
        }
        
        private void initLabelStyles()
        {
            this.LeftAlignLabelStyle ??= HathoraEditorUtils.GetRichLabelStyle(TextAnchor.MiddleLeft);
            this.LeftAlignNoWrapLabelStyle ??= HathoraEditorUtils.GetRichLabelStyle(TextAnchor.MiddleLeft, _wordWrap:false);
            this.CenterAlignLabelStyle ??= HathoraEditorUtils.GetRichLabelStyle(TextAnchor.MiddleCenter);
            this.RightAlignLabelStyle ??= HathoraEditorUtils.GetRichLabelStyle(TextAnchor.MiddleRight);
            this.CenterLinkLabelStyle ??= HathoraEditorUtils.GetRichLinkStyle(TextAnchor.MiddleCenter);
            this.PreLinkLabelStyle ??= HathoraEditorUtils.GetPreLinkLabelStyle();
            
            this.CenterAlignSmLabelStyle ??= HathoraEditorUtils.GetRichLabelStyle(
                TextAnchor.MiddleCenter,
                _fontSize: 9);
            
            this.CenterAlignLargerTxtLabelNoWrapStyle ??= HathoraEditorUtils.GetRichLabelStyle(
                TextAnchor.MiddleCenter,
                _wordWrap: false,
                _fontSize: 15);
        }
        #endregion // Init

        
        /// <summary>Are we logged in, already (is ServerConfig dev auth token set)?</summary>
        /// <returns></returns>
        protected bool CheckHasAuthToken() =>
            !string.IsNullOrEmpty(ServerConfig.HathoraCoreOpts.DevAuthOpts.HathoraDevToken);
        
        protected bool CheckHasSelectedApp() => 
            !string.IsNullOrEmpty(ServerConfig.HathoraCoreOpts.AppId);

        /// <summary>
        /// Add to this event to request a repaint from the main editor UI.
        /// Calling this will also unfocus any fields.
        /// </summary>
        protected void InvokeRequestRepaint()
        {
            RequestRepaint?.Invoke();
            unfocusFields();
        }

        /// <summary>
        /// Creates an invisible dummy ctrl - somewhat hacky.
        /// </summary>
        private void unfocusFields()
        {
            GUI.SetNextControlName("Dummy");
            GUI.TextField(new Rect(0, 0, 0, 0), "");
            GUI.FocusControl("Dummy");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_thickness"></param>
        /// <param name="_color">Default == Color.gray</param>
        /// <param name="_space"></param>
        protected void InsertHorizontalLine(
            float _thickness = 1.5f,
            Color _color = default, 
            int _space = 0)
        {
            if (_color == default)
                _color = Color.gray;
            
            Rect lineRect = EditorGUILayout.GetControlRect(hasLabel: false, _thickness);
            lineRect.height = _thickness;
            EditorGUI.DrawRect(lineRect, _color);
            
            if (_space > 0)
                EditorGUILayout.Space(_space);
        }

        /// <param name="_url">On click, open browser to this url; or set null for onClick only.</param>
        /// <param name="_centerAlign">Wrap in a horizontal layout with flex space</param>
        /// <param name="_label">Link label string</param>
        /// <param name="onClick">Get a callback on click. Set _url to null to skip link completely.</param>
        protected void InsertLinkLabel(
            string _label, 
            string _url, 
            bool _centerAlign,
            Action onClick = null)
        {
            if (_centerAlign)
                StartCenterHorizAlign();

            bool clickedLink = EditorGUILayout.LinkButton(
                _label,
                GUILayout.ExpandWidth(false));
            
            if (clickedLink)
            {
                bool isValidUrl = !string.IsNullOrEmpty(_url);
                if (isValidUrl)
                    Application.OpenURL(_url);
                
                onClick?.Invoke();
            }
            
            if (_centerAlign)
                EndCenterHorizAlign();
        }
        
        /// <summary>
        /// Handle the click event yourself
        /// </summary>
        /// <param name="_label"></param>
        /// <param name="_centerAlign">Wrap in a horizontal layout with flex space</param>
        /// <returns>clickedLabelLink</returns>
        protected bool InsertLinkLabelEvent(string _label, bool _centerAlign)
        {
            if (_centerAlign)
                StartCenterHorizAlign();

            // Use label rect to capture click events.
            Rect labelRect = GUILayoutUtility.GetRect(
                new GUIContent(_label), 
                CenterAlignLabelStyle);

            GUI.Label(labelRect, _label, CenterAlignLabelStyle);

            // Check if left mouse button is clicked within label rect
            bool clickedLabelLink = Event.current.type == EventType.MouseDown 
                && Event.current.button == 0 
                && labelRect.Contains(Event.current.mousePosition);

            if (_centerAlign)
                EndCenterHorizAlign();

            return clickedLabelLink;
        }
        
        protected static SerializedProperty FindNestedProperty(
            SerializedObject serializedObject, 
            params string[] propertyNames)
        {
            if (serializedObject == null || propertyNames == null || propertyNames.Length == 0)
            {
                Debug.LogError("SerializedObject or propertyNames is null or empty.");
                return null;
            }

            SerializedProperty currentProperty = serializedObject.FindProperty(propertyNames[0]);

            if (currentProperty == null)
            {
                Debug.LogError($"Could not find property '{propertyNames[0]}' in SerializedObject.");
                return null;
            }

            for (int i = 1; i < propertyNames.Length; i++)
            {
                if (currentProperty.isArray && i < propertyNames.Length - 1)
                {
                    int arrayIndex = int.Parse(propertyNames[i]);
                    currentProperty = currentProperty.GetArrayElementAtIndex(arrayIndex);
                    i++;
                }
                else
                {
                    currentProperty = currentProperty.FindPropertyRelative(propertyNames[i]);
                }

                if (currentProperty == null)
                {
                    Debug.LogError($"Could not find nested property '{propertyNames[i]}' in SerializedObject.");
                    return null;
                }
            }

            return currentProperty;
        }

        protected static void InsertTooltipIcon(string _tooltipStr)
        {
            Texture2D infoIcon = Resources.Load<Texture2D>("Icons/infoIcon");
            insertIconLabel(infoIcon, _tooltipStr);
        }

        /// <summary>
        /// Display a circular tooltip icon with hover str
        /// </summary>
        /// <param name="_infoIcon"></param>
        /// <param name="_tooltipStr"></param>
        /// <param name="_maxHeight"></param>
        private static void insertIconLabel(
            Texture _infoIcon, 
            string _tooltipStr,
            int _maxHeight = 16)
        {
            GUIContent iconContent = new(_infoIcon, _tooltipStr);
            GUILayout.Label(
                iconContent, 
                GUILayout.ExpandWidth(false), 
                GUILayout.MaxHeight(_maxHeight));
        }
        
        protected void InsertLeftSelectableLabel(
            string _contentStr,
            bool _vertCenter = false)
        {
            if (_vertCenter)
            {
                GUILayout.BeginVertical();
                InsertFlexSpace();    
            }

            Rect labelRect = GUILayoutUtility.GetRect(
                new GUIContent(_contentStr),
                LeftAlignLabelStyle,
                GUILayout.ExpandWidth(true));
        
            EditorGUI.SelectableLabel(labelRect, _contentStr, LeftAlignLabelStyle);
        
            if (_vertCenter)
            {
                InsertFlexSpace();
                GUILayout.EndVertical();    
            }

        }

        /// <summary>
        /// Add _tooltip str to include a _tooltip icon + hover text.
        /// </summary>
        /// <param name="_labelStr"></param>
        /// <param name="_tooltip"></param>
        /// <param name="_selectable">
        /// Want to select some text for copying?
        /// BUG: If you indent, there's sometimes a random indent
        /// </param>
        /// <param name="_wrap">Should the label text be wrapped? Good for short header labels</param>
        /// <param name="_vertCenter"></param>
        /// <param name="_fontSize">Default = 13</param>
        /// <param name="_horizAlign"></param>
        protected void InsertLabel(
            string _labelStr,
            string _tooltip = null,
            bool _selectable = false,
            bool _wrap = true,
            bool _vertCenter = false,
            int _fontSize = 13,
            AlignType _horizAlign = AlignType.Left)
        {
            GUIContent labelContent = new() { text = _labelStr };
            GUILayoutOption expandWidthOpt = GUILayout.ExpandWidth(false);

            if (_vertCenter)
            {
                GUILayout.BeginVertical();
                InsertFlexSpace();
            }

            GUIStyle alignGuiStyle = _horizAlign switch
            {
                // Create a GUI style initially from a template, then override as needed
                AlignType.Left => new GUIStyle(LeftAlignLabelStyle)
                {
                    fontSize = _fontSize,
                    wordWrap = _wrap,
                },
                AlignType.Center => new GUIStyle(CenterAlignLabelStyle)
                {
                    fontSize = _fontSize,
                    wordWrap = _wrap,
                },
                AlignType.Right => new GUIStyle(RightAlignLabelStyle)
                {
                    fontSize = _fontSize,
                    wordWrap = _wrap,
                },
                _ => null,
            };

            if (_selectable)
            {
                // BUG: If you indent, there's sometimes a random indent
                EditorGUILayout.SelectableLabel(
                    _labelStr,
                    alignGuiStyle,
                    expandWidthOpt);
            }
            else
            {
                GUILayout.Label(
                    labelContent,
                    alignGuiStyle,
                    expandWidthOpt);
            }

            if (!string.IsNullOrEmpty(_tooltip))
                InsertTooltipIcon(_tooltip);
            
            if (_vertCenter)
            {
                InsertFlexSpace();
                GUILayout.EndVertical();
            }
        }

        protected void InsertCenterLabel(string labelStr)
        {
            StartCenterHorizAlign();
            GUILayout.Label(labelStr, PreLinkLabelStyle);
            EndCenterHorizAlign();
        }

        /// <summary>
        /// Useful for smaller buttons you want centered for less emphasis.
        /// </summary>
        protected void StartCenterHorizAlign()
        {
            GUILayout.BeginHorizontal();
            InsertFlexSpace();
        }

        protected void EndCenterHorizAlign()
        {
            InsertFlexSpace();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// </summary>
        /// <param name="_btnLabelStr"></param>
        /// <param name="_btnStyle"></param>
        /// <param name="_percentWidthOfScreen"></param>
        /// <returns>OnClick bool</returns>
        protected bool InsertSmallCenteredBtn(
            string _btnLabelStr, 
            GUIStyle _btnStyle = null, 
            float _percentWidthOfScreen = 0.35f)
        {
            // This should be smaller than Login btn: Set to 35% of screen width
            GUILayoutOption regBtnWidth = GUILayout.Width(Screen.width * _percentWidthOfScreen);

            GUIStyle btnStyle = _btnStyle == null
                ? GeneralButtonStyle
                : _btnStyle;
            
            // USER INPUT >>
            bool clickedBtn = GUILayout.Button(_btnLabelStr, btnStyle, regBtnWidth);
            InsertSpace2x();

            return clickedBtn;
        }

        /// <returns>bool clicked</returns>
        protected bool InsertLeftGeneralBtn(string _content) =>
            GUILayout.Button(_content, GeneralButtonStyle);

        /// <summary>
        /// {label} {tooltip} {input}
        /// </summary>
        /// <param name="_labelStr"></param>
        /// <param name="_tooltip"></param>
        /// <param name="_val"></param>
        /// <param name="_alignTextField"></param>
        /// <param name="isTextArea"></param>
        /// <returns>inputStr</returns>
        protected string InsertHorizLabeledTextField(
            string _labelStr,
            string _tooltip,
            string _val,
            GuiAlign _alignTextField = GuiAlign.Stretched,
            bool isTextArea = false)
        {
            EditorGUILayout.BeginHorizontal();

            InsertLabel(_labelStr, _tooltip);
            
            if (_alignTextField == GuiAlign.SmallRight)
                InsertFlexSpace();

            float maxTxtFieldWidth = _alignTextField == GuiAlign.Stretched
                ? -1f
                : DEFAULT_MAX_FIELD_WIDTH;
            
            // USER INPUT >>
            string inputStr = isTextArea
                ? EditorGUILayout.TextArea(_val, getDefaultInputLayoutOpts(_maxWidth: maxTxtFieldWidth))
                : GUILayout.TextField(_val, getDefaultInputLayoutOpts(_maxWidth: maxTxtFieldWidth));
            
            if (_alignTextField == GuiAlign.SmallLeft)
                InsertFlexSpace();

            EditorGUILayout.EndHorizontal();
            return inputStr;
        }
        
        /// <summary>
        /// {label} {tooltip} {checkbox}
        /// </summary>
        /// <param name="_labelStr"></param>
        /// <param name="_tooltip"></param>
        /// <param name="_val"></param>
        /// <param name="_alignCheckbox"></param>
        /// <returns>isChecked</returns>
        protected bool InsertHorizLabeledCheckboxField(
            string _labelStr,
            string _tooltip,
            bool _val,
            GuiAlign _alignCheckbox = GuiAlign.Stretched)
        {
            EditorGUILayout.BeginHorizontal();

            InsertLabel(_labelStr, _tooltip);
    
            if (_alignCheckbox == GuiAlign.SmallRight)
                InsertFlexSpace();

            float maxToggleWidth = _alignCheckbox == GuiAlign.Stretched
                ? -1f
                : DEFAULT_MAX_FIELD_WIDTH;
    
            // USER INPUT >>
            bool isChecked = GUILayout.Toggle(
                _val, 
                text: "", 
                getDefaultInputLayoutOpts(_maxWidth: maxToggleWidth));
    
            if (_alignCheckbox == GuiAlign.SmallLeft)
                InsertFlexSpace();

            EditorGUILayout.EndHorizontal();
            return isChecked;
        }
        
        public enum EnumListOpts
        {
            AsIs,
            AllCaps,
            PascalWithSpaces,
        }

        /// <summary>
        /// Useful for Popup lists (dropdowns) for GUI selection.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        /// <param name="_opts"></param>
        protected static List<string> GetStrListOfEnumMemberKeys<TEnum>(EnumListOpts _opts) where TEnum : Enum
        {
            IEnumerable<string> enumerable = Enum
                .GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e =>
                {
                    return _opts switch
                    {
                        EnumListOpts.AllCaps => e.ToString().ToUpperInvariant(),
                        EnumListOpts.PascalWithSpaces => e.ToString().SplitPascalCase(),
                        _ => e.ToString(), // AsIs
                    };
                });

            return enumerable.ToList();
        }
        
        private static GUILayoutOption[] getDefaultInputLayoutOpts(
            float _maxWidth = DEFAULT_MAX_FIELD_WIDTH, 
            bool _expandWidth = false, 
            float _height = -1f)
        {
            List<GUILayoutOption> opts = new List<GUILayoutOption>();

            if (_expandWidth)
                opts.Add(GUILayout.ExpandWidth(true));

            if (_maxWidth > 0f)
                opts.Add(GUILayout.MaxWidth(_maxWidth));

            if (_height > 0f)
                opts.Add(GUILayout.Height(_height));

            return opts.ToArray();
        }

        /// <summary>
        /// {label} {tooltip} {popupList}
        /// </summary>
        /// <param name="_labelStr"></param>
        /// <param name="_tooltip"></param>
        /// <param name="_displayOptsStrArr"></param>
        /// <param name="_selectedIndex"></param>
        /// <param name="_alignPopup"></param>
        /// <returns>Returns selected index</returns>
        protected int InsertHorizLabeledPopupList(
            string _labelStr,
            string _tooltip,
            string[] _displayOptsStrArr,
            int _selectedIndex,
            GuiAlign _alignPopup = GuiAlign.Stretched)
        {
            EditorGUILayout.BeginHorizontal();

            InsertLabel(_labelStr, _tooltip);
            
            if (_alignPopup == GuiAlign.SmallRight)
                InsertFlexSpace();
            
            // USER INPUT >>
            int newSelectedIndex = EditorGUILayout.Popup(
                _selectedIndex, 
                _displayOptsStrArr,
                getDefaultInputLayoutOpts());
            
            if (_alignPopup == GuiAlign.SmallLeft)
                InsertFlexSpace();

            EditorGUILayout.EndHorizontal();
            return newSelectedIndex;
        }

        /// <summary>TODO: This is difficult to make look good</summary>
        // protected int InsertHorizLabeledRadioButtonGroup(
        //     string _labelStr,
        //     string _tooltip,
        //     string[] _displayOptsStrArr,
        //     int _selectedIndex,
        //     GuiAlign _alignPopup = GuiAlign.Stretched,
        //     int _buttonWidth = 100,
        //     int _bufferSpaceAfterTooltip = 30)
        // {
        //     EditorGUILayout.BeginHorizontal();
        //
        //     InsertLabel(_labelStr, _tooltip);
        //     GUILayout.Space(_bufferSpaceAfterTooltip);
        //
        //     if (_alignPopup == GuiAlign.SmallRight)
        //         InsertFlexSpace();
        //
        //     int newSelectedIndex = _selectedIndex;
        //
        //     for (int i = 0; i < _displayOptsStrArr.Length; i++)
        //     {
        //         bool wasSelected = _selectedIndex == i;
        //         bool nowSelected = GUILayout.Toggle(
        //             wasSelected,
        //             _displayOptsStrArr[i],
        //             GUILayout.Width(_buttonWidth));
        //
        //         // If the button state changed and it's now selected
        //         if (wasSelected != nowSelected && nowSelected)
        //             newSelectedIndex = i;
        //     }
        //
        //     if (_alignPopup == GuiAlign.SmallLeft)
        //         InsertFlexSpace();
        //
        //     EditorGUILayout.EndHorizontal();
        //
        //     return newSelectedIndex;
        // }

        /// <summary>
        /// The slider is not ideal for large val ranges.
        /// </summary>
        /// <param name="_labelStr"></param>
        /// <param name="_tooltip"></param>
        /// <param name="_val"></param>
        /// <param name="_minVal"></param>
        /// <param name="_maxVal"></param>
        /// <param name="_alignPopup"></param>
        /// <returns></returns>
        protected int InsertHorizLabeledIntSlider(
            string _labelStr,
            string _tooltip,
            int _val,
            int _minVal = 0,
            int _maxVal = int.MaxValue,
            GuiAlign _alignPopup = GuiAlign.Stretched)
        {
            EditorGUILayout.BeginHorizontal();
            
            InsertLabel(_labelStr, _tooltip);

            if (_alignPopup == GuiAlign.SmallRight)
                InsertFlexSpace();
             
            // USER INPUT >>
            int inputInt = EditorGUILayout.IntSlider(
                _val,
                _minVal,
                _maxVal,
                getDefaultInputLayoutOpts());

            if (_alignPopup == GuiAlign.SmallLeft)
                InsertFlexSpace();

            EditorGUILayout.EndHorizontal();
            return inputInt;
        }
        
        /// <summary>
        /// Beter than a slider for large int ranges, or in cases where
        /// you won't really change it often.
        /// </summary>
        /// <param name="_labelStr"></param>
        /// <param name="_tooltip"></param>
        /// <param name="_val"></param>
        /// <param name="_minVal"></param>
        /// <param name="_maxVal"></param>
        /// <param name="_alignPopup"></param>
        /// <returns></returns>
        protected int InsertHorizLabeledConstrainedIntField(
            string _labelStr,
            string _tooltip,
            int _val,
            int _minVal = 0,
            int _maxVal = int.MaxValue,
            GuiAlign _alignPopup = GuiAlign.Stretched)
        {
            EditorGUILayout.BeginHorizontal();
    
            InsertLabel(_labelStr, _tooltip);

            if (_alignPopup == GuiAlign.SmallRight)
                InsertFlexSpace();

            // USER INPUT >>
            int inputInt = EditorGUILayout.IntField(_val, getDefaultInputLayoutOpts());

            // Constraint the value
            inputInt = Mathf.Clamp(inputInt, _minVal, _maxVal);

            if (_alignPopup == GuiAlign.SmallLeft)
                InsertFlexSpace();

            EditorGUILayout.EndHorizontal();
            return inputInt;
        }
        
        protected void SaveConfigChange(
            string _logKeyName, 
            string _logKeyVal, 
            bool _skipLog = false)
         {
             if (!_skipLog)
             {
                 Debug.Log($"[HathoraConfigUIBase] Set new ServerConfig vals for: " +
                     $"`{_logKeyName}` to: `{_logKeyVal}`");    
             }

             SerializedConfig.ApplyModifiedProperties();
             EditorUtility.SetDirty(ServerConfig); // Mark the object as dirty
             AssetDatabase.SaveAssets(); // Save changes to the ScriptableObject asset
         }

        protected void InsertFlexSpace(int _count = 1)
        {
            for (int i = 0; i < _count; i++)
                GUILayout.FlexibleSpace();
        }

        protected void InsertSpace1x() =>
            EditorGUILayout.Space(5f);

        protected void InsertSpace2x() => 
            EditorGUILayout.Space(10f);
        
        protected void InsertSpace3x() => 
            EditorGUILayout.Space(20f);
        
        protected void InsertSpace4x() => 
            EditorGUILayout.Space(30f);

        /// <summary>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="_prettifyNames">
        /// If true, we'll SplitPascalCase() -
        /// (!) be sure to reference the enum by int and not string since the str vals won't match.
        /// </param>
        /// <returns></returns>
        protected static List<string> GetDisplayOptsStrArrFromEnum<TEnum>(bool _prettifyNames = false)
            where TEnum : Enum
        {
            IEnumerable<string> enumerable = Enum
                .GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(x => _prettifyNames
                    ? x.ToString().SplitPascalCase()
                    : x.ToString());

            return enumerable.ToList();
        }

        /// <summary>
        /// EditorGUI Indents don't seem to work in fields - this works around it.
        /// (!) MUST include a EndFieldIndent() call somewhere at the end. Care of early return statements.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected void BeginFieldIndent()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("", GUILayout.Width(10)); // Indent workaround for non-groups
        }

        protected void EndFieldIndent()
        {
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>(!) Don't forget to EndPaddedBox()</summary>
        protected void BeginPaddedBox()
        {
            // TODO: Find a way to indent children (indentLevel++ !works)
            EditorGUILayout.BeginVertical(
                PaddedBoxStyle, 
                GUILayout.ExpandWidth(true));
        }

        protected void EndPaddedBox()
        {
            // TODO: End indent to match BeginPaddedBox(), once indent is implemented
            EditorGUILayout.EndVertical();   
        }
    }
}
