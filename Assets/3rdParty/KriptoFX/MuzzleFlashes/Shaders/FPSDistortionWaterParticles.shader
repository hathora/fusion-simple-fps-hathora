// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "KriptoFX/FPS_Pack/WaterParticles" {
Properties {
       [HDR] _TintColor ("Tint Color", Color) = (1,1,1,1)
		_MainTex ("Main Texture (R) CutOut (G)", 2D) = "white" {}
        _BumpMap ("Normalmap", 2D) = "bump" {}
		_BumpAmt ("Distortion", Float) = 10
}

Category {

	Tags { "Queue"="Transparent-10"  "IgnoreProjector"="True"  "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off
	ZWrite Off


	SubShader {

		Pass {


CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma multi_compile_particles
#include "UnityCG.cginc"

struct appdata_t {
	float4 vertex : POSITION;
	float4 texcoord: TEXCOORD0;
	fixed4 color : COLOR;
	float texcoordBlend : TEXCOORD1;
};

struct v2f {
	float4 vertex : POSITION;
	float4 uvgrab : TEXCOORD0;
	float4 uvbump : TEXCOORD1;
	fixed4 color : COLOR;

	#ifdef SOFTPARTICLES_ON
		float4 projPos : TEXCOORD4;
	#endif
		fixed blend : TEXCOORD6;

};

sampler2D _MainTex;
sampler2D _BumpMap;

float _BumpAmt;
float _ColorStrength;
sampler2D _CameraOpaqueTexture;
float4 _GrabTexture_TexelSize;
fixed4 _TintColor;
float4 _LightColor0;

float4 _BumpMap_ST;
float4 _MainTex_ST;


sampler2D RFX4_PointLightAttenuation;
half4 RFX4_AmbientColor;
float4 RFX4_LightPositions[40];
float4 RFX4_LightColors[40];
int RFX4_LightCount;

half3 ShadeCustomLights(float4 vertex, half3 normal, int lightCount)
{
	float3 worldPos = mul(unity_ObjectToWorld, vertex);
	float3 worldNormal = UnityObjectToWorldNormal(normal);

	float3 lightColor = RFX4_AmbientColor.xyz;
	for (int i = 0; i < lightCount; i++) {
		float3 lightDir = RFX4_LightPositions[i].xyz - worldPos.xyz * RFX4_LightColors[i].w;
		half normalizedDist = length(lightDir) / RFX4_LightPositions[i].w;
		fixed attenuation = tex2Dlod(RFX4_PointLightAttenuation, half4(normalizedDist.xx, 0, 0));
		attenuation = lerp(1, attenuation, RFX4_LightColors[i].w);
		float diff = max(0, dot(normalize(worldNormal), normalize(lightDir)));
		lightColor += RFX4_LightColors[i].rgb * (diff * attenuation);
	}
	return (lightColor);
}

v2f vert (appdata_t v)
{
	v2f o;

	o.vertex = UnityObjectToClipPos(v.vertex);

	#ifdef SOFTPARTICLES_ON
		o.projPos = ComputeScreenPos (o.vertex);
		COMPUTE_EYEDEPTH(o.projPos.z);
	#endif
	o.color = v.color;

	o.color.rgb *= saturate(ShadeCustomLights(v.vertex, float3(0, 1, 0), RFX4_LightCount));

	o.uvgrab = ComputeGrabScreenPos (o.vertex);
	o.uvbump.xy = TRANSFORM_TEX(v.texcoord.xy, _BumpMap);
	o.uvbump.zw = TRANSFORM_TEX(v.texcoord.zw, _BumpMap);
	o.blend = v.texcoordBlend;

	return o;
}

sampler2D _CameraDepthTexture;
float _InvFade;

half4 frag( v2f i ) : COLOR
{
	fixed4 bumpTex1 = tex2D(_BumpMap, i.uvbump.xy);
	fixed4 bumpTex2 = tex2D(_BumpMap, i.uvbump.zw);
	half3 bump = UnpackNormal(lerp(bumpTex1, bumpTex2, i.blend));
	half alphaBump = saturate((0.94 - pow(bump.z, 127)) * 5);

	if (alphaBump < 0.1) discard;


	fixed4 tex = tex2D(_MainTex, i.uvbump.xy);
	fixed4 tex2 = tex2D(_MainTex, i.uvbump.zw);
	tex = lerp(tex, tex2, i.blend);


	float2 offset = bump * _BumpAmt  * i.color.a * alphaBump;
	i.uvgrab.xy = offset  + i.uvgrab.xy;

	half4 grab = tex2Dlod(_CameraOpaqueTexture, float4(i.uvgrab.xy / i.uvgrab.w, 0, 0));


	//fixed4 cut = tex2D(_CutOut, i.uvcutout) * i.color;
	//fixed4 emission = col * i.color + tex.r * _ColorStrength * _TintColor * _LightColor0 * i.color * i.color.a;
	fixed4 emission = grab + tex.a * _TintColor * i.color * i.color.a;
    emission.a = _TintColor.a * alphaBump ;

	return saturate(emission);
}
ENDCG
		}
	}


}

}
