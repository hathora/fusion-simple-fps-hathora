Shader "KriptoFX/FPS_Pack/Distortion" {
Properties {
		[HDR]_TintColor("Tint Color", Color) = (0,0,0,1)
		_BaseTex("Base (RGB) Gloss (A)", 2D) = "black" {}
		[HDR]_MainColor("Main Color", Color) = (1,1,1,1)
        _MainTex ("Normalmap & CutOut", 2D) = "black" {}
		_BumpAmt ("Distortion", Float) = 1
		_InvFade ("Soft Particles Factor", Float) = 0.5
}



	SubShader {

		Tags{ "Queue" = "Transparent-10" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			ZWrite Off

		Pass {

CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#pragma multi_compile_particles
#include "UnityCG.cginc"

struct appdata_t {
	float4 vertex : POSITION;
	float2 texcoord: TEXCOORD0;
	fixed4 color : COLOR;
};

struct v2f {
	float4 vertex : POSITION;
	float4 uvgrab : TEXCOORD0;
	float2 uvbump : TEXCOORD1;
	fixed4 color : COLOR;
	#ifdef SOFTPARTICLES_ON
		float4 projPos : TEXCOORD3;
	#endif

};

sampler2D _MainTex;
sampler2D _BaseTex;
half4 _TintColor;
half4 _MainColor;
float _BumpAmt;
sampler2D _CameraOpaqueTexture;



float4 _MainTex_ST;

	v2f vert (appdata_t v)
	{
		v2f o;

		o.vertex = UnityObjectToClipPos(v.vertex);

		#ifdef SOFTPARTICLES_ON
			o.projPos = ComputeScreenPos (o.vertex);
			COMPUTE_EYEDEPTH(o.projPos.z);
		#endif
		o.color = v.color;
		#if UNITY_UV_STARTS_AT_TOP
		float scale = -1.0;
		#else
		float scale = 1.0;
		#endif
		o.uvgrab = ComputeGrabScreenPos(o.vertex);

		o.uvbump = TRANSFORM_TEX( v.texcoord, _MainTex );

		return o;
	}

	sampler2D _CameraDepthTexture;
	float _InvFade;

	half4 frag( v2f i ) : COLOR
	{
		#ifdef SOFTPARTICLES_ON
			float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
			float partZ = i.projPos.z;
			float fade = saturate (_InvFade * (sceneZ-partZ));
			fade = _InvFade < 0.01 ? 1 : fade;
			i.color.a *= fade;
		#endif

		half3 bump = UnpackNormal(tex2D( _MainTex, i.uvbump ));
		half alphaBump = saturate((0.94 - pow(bump.z, 127)) * 5);
		i.uvgrab.xy = bump.rg * i.color.a * alphaBump * _BumpAmt + i.uvgrab.xy;

		half4 grab = tex2Dlod(_CameraOpaqueTexture, float4(i.uvgrab.xy / i.uvgrab.w, 0, 0)) * _MainColor;

		grab.a = saturate(grab.a * alphaBump);
		return grab;
	}
	ENDCG
		}
	}


}
