
Shader "KriptoFX/FPS_Pack/Glass" {
Properties {
        [HDR]_TintColor ("Tint Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "black" {}
        _DuDvMap ("DuDv Map", 2D) = "black" {}
		_BumpAmt ("Distortion", Float) = 10
}

SubShader{

			Tags{ "Queue" = "Transparent-10" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			ZWrite On

			Pass{


CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

struct appdata_t {
	float4 vertex : POSITION;
	float2 texcoord: TEXCOORD0;
	float4 color : COLOR;
};

struct v2f {
	float4 vertex : POSITION;
	float4 uvgrab : TEXCOORD0;
	float2 uvbump : TEXCOORD1;
	float2 uvmain : TEXCOORD2;
	float4 color : COLOR;
};

sampler2D _MainTex;
sampler2D _DuDvMap;

float _BumpAmt;
float _ColorStrength;
sampler2D _CameraOpaqueTexture;


float4 _TintColor;
float4 _LightColor0;

float4 _DuDvMap_ST;
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


	o.color = v.color;
	o.color.rgb *= saturate(ShadeCustomLights(v.vertex, float3(0, 1, 0), RFX4_LightCount));

	o.uvgrab = ComputeGrabScreenPos(o.vertex);
	o.uvbump = TRANSFORM_TEX( v.texcoord, _DuDvMap );
	o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );

	return o;
}

half4 frag( v2f i ) : COLOR
{
	half3 bump = UnpackNormal(tex2D(_DuDvMap, i.uvbump));
	half alphaBump = saturate((0.94 - pow(bump.z, 127)) * 5);
	i.uvgrab.xy = bump.rg * i.color.a * alphaBump * _BumpAmt + i.uvgrab.xy;

	half4 grab = tex2Dlod(_CameraOpaqueTexture, float4(i.uvgrab.xy / i.uvgrab.w, 0, 0));
	fixed4 tex = tex2D(_MainTex, i.uvmain) * i.color;

	fixed4 res = grab + tex * _TintColor * i.color.a;
    res.a = saturate(res.a);
	return res;
}
ENDCG
		}
	}

	FallBack "Diffuse"



}

