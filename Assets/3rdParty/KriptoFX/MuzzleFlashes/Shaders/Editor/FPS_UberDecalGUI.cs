using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;

public class FPS_UberDecalGUI : ShaderGUI
{
    static float TOLERANCE = 0.001f;
    public override void OnGUI(MaterialEditor m, MaterialProperty[] properties)
    {
        var _MainTex = ShaderGUI.FindProperty("_MainTex", properties);
        var _TintColor = ShaderGUI.FindProperty("_TintColor", properties);
        var _UseAlphaPow = ShaderGUI.FindProperty("_UseAlphaPow", properties);
        var _AlphaPow = ShaderGUI.FindProperty("_AlphaPow", properties);
        //var _UseLighting = ShaderGUI.FindProperty("_UseLighting", properties);
        //var _LightTranslucent = ShaderGUI.FindProperty("_LightTranslucent", properties);

        var _UseNoiseDistortion = ShaderGUI.FindProperty("_UseNoiseDistortion", properties);
        var _NoiseTex = ShaderGUI.FindProperty("_NoiseTex", properties);
        //var _DistortionSpeedScale = ShaderGUI.FindProperty("_DistortionSpeedScale", properties);
        var _DistortSpeed = ShaderGUI.FindProperty("_DistortSpeed", properties);
        var _DistortScale = ShaderGUI.FindProperty("_DistortScale", properties);
        //var _UseAlphaMask = ShaderGUI.FindProperty("_UseAlphaMask", properties);

        var _UseCutout = ShaderGUI.FindProperty("_UseCutout", properties);
        var _CutoutAlphaMul = ShaderGUI.FindProperty("_CutoutAlphaMul", properties);

        var _Cutout = ShaderGUI.FindProperty("_Cutout", properties);

        var _UseCutoutTex = ShaderGUI.FindProperty("_UseCutoutTex", properties);
        var _CutoutTex = ShaderGUI.FindProperty("_CutoutTex", properties);

        var _UseCutoutThreshold = ShaderGUI.FindProperty("_UseCutoutThreshold", properties);
        var _CutoutColor = ShaderGUI.FindProperty("_CutoutColor", properties);
       // var _CutoutRamp = ShaderGUI.FindProperty("_CutoutRamp", properties);
        //var _CutoutThreshold = ShaderGUI.FindProperty("_CutoutThreshold", properties);

        var _UseWorldSpaceUV = ShaderGUI.FindProperty("_UseWorldSpaceUV", properties); 
        var _UseFrameBlending = ShaderGUI.FindProperty("_UseFrameBlending", properties);

        var _BlendMode = ShaderGUI.FindProperty("_BlendMode", properties);
        
        var _SrcMode = ShaderGUI.FindProperty("_SrcMode", properties);
        var _DstMode = ShaderGUI.FindProperty("_DstMode", properties);
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        m.TextureProperty(_MainTex, _MainTex.displayName);
        m.ColorProperty(_TintColor, _TintColor.displayName);
        m.ShaderProperty(_UseAlphaPow, _UseAlphaPow.displayName);
        if (Mathf.Abs(_UseAlphaPow.floatValue - 1) < TOLERANCE)
        {
            m.ShaderProperty(_AlphaPow, _AlphaPow.displayName);
        }

        //m.ShaderProperty(_UseLighting, _UseLighting.displayName);
        //if (Mathf.Abs(_UseLighting.floatValue - 1) < TOLERANCE)
        //    m.ShaderProperty(_LightTranslucent, _LightTranslucent.displayName);


        m.ShaderProperty(_UseNoiseDistortion, _UseNoiseDistortion.displayName);
        if (Mathf.Abs(_UseNoiseDistortion.floatValue - 1) < TOLERANCE)
        {
            m.TextureProperty(_NoiseTex, _NoiseTex.displayName);
            m.ShaderProperty(_DistortSpeed, _DistortSpeed.displayName);
            m.ShaderProperty(_DistortScale, _DistortScale.displayName);
            //m.ShaderProperty(_UseAlphaMask, _UseAlphaMask.displayName);
        }

       
        m.ShaderProperty(_UseCutout, _UseCutout.displayName);
        if (Mathf.Abs(_UseCutout.floatValue - 1) < TOLERANCE)
        {
            m.ShaderProperty(_Cutout, _Cutout.displayName);
            m.ShaderProperty(_CutoutAlphaMul, _CutoutAlphaMul.name);

            m.ShaderProperty(_UseCutoutTex, _UseCutoutTex.displayName);
            if (Mathf.Abs(_UseCutoutTex.floatValue - 1) < TOLERANCE)
                m.TextureProperty(_CutoutTex, _CutoutTex.displayName);

            m.ShaderProperty(_UseCutoutThreshold, _UseCutoutThreshold.displayName);
            if (Mathf.Abs(_UseCutoutThreshold.floatValue - 1) < TOLERANCE)
            {
                m.ColorProperty(_CutoutColor, _CutoutColor.displayName);
               //m.TextureProperty(_CutoutRamp, _CutoutRamp.displayName);
               // m.ShaderProperty(_CutoutThreshold, _CutoutThreshold.displayName);
            }
        }

        m.ShaderProperty(_UseWorldSpaceUV, _UseWorldSpaceUV.displayName);
        m.ShaderProperty(_UseFrameBlending, _UseFrameBlending.displayName);

        m.ShaderProperty(_BlendMode, _BlendMode.displayName);
        
        if (Math.Abs(_BlendMode.floatValue) < TOLERANCE)
        {
            _SrcMode.floatValue = (int) UnityEngine.Rendering.BlendMode.SrcAlpha;
            _DstMode.floatValue = (int) UnityEngine.Rendering.BlendMode.One;
        }
        if (Math.Abs(_BlendMode.floatValue - 1) < TOLERANCE)
        {
            _SrcMode.floatValue = (int)UnityEngine.Rendering.BlendMode.SrcAlpha;
            _DstMode.floatValue = (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
        }
        if (Math.Abs(_BlendMode.floatValue - 2) < TOLERANCE)
        {
            _SrcMode.floatValue = (int)UnityEngine.Rendering.BlendMode.Zero;
            _DstMode.floatValue = (int)UnityEngine.Rendering.BlendMode.SrcColor;
        }
  
        m.RenderQueueField();

#if UNITY_5_6_OR_NEWER
        m.EnableInstancingField();
        Material material = (Material)m.target;
        material.enableInstancing = true;
#endif
    }
}
