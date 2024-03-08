Shader "KriptoFX/FPS_Pack/Leafs" {
	Properties{
		[HDR]_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Particle Texture", 2D) = "white" {}
		_InvFade("Soft Particles Factor", Range(0.01,5)) = 1.0
	}

	
		

		SubShader{
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			ZWrite On
		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#pragma multi_compile_fog

#include "UnityCG.cginc"


		sampler2D RFX4_PointLightAttenuation;
		half4 RFX4_AmbientColor;
		float4 RFX4_LightPositions[40];
		float4 RFX4_LightColors[40];
		int RFX4_LightCount;


		sampler2D _MainTex;
	fixed4 _TintColor;

	struct appdata_t {
		float4 vertex : POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
		half3 normal:NORMAL;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;

		UNITY_FOG_COORDS(3)

		UNITY_VERTEX_OUTPUT_STEREO
	};

	float4 _MainTex_ST;


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

	v2f vert(appdata_t v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.vertex = UnityObjectToClipPos(v.vertex);

		o.color = v.color * _TintColor;
		o.color.rgb *= max(ShadeCustomLights(v.vertex, v.normal, RFX4_LightCount), ShadeCustomLights(v.vertex, -v.normal, RFX4_LightCount));

		o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	sampler2D_float _CameraDepthTexture;
	float _InvFade;

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.texcoord);
		col *= 2.0f * i.color;
		UNITY_APPLY_FOG(i.fogCoord, col);
		col.a = saturate(col.a);
		return col;
	}
		ENDCG
	}

			Pass
	{
		Tags{ "Queue" = "Transparent" "LightMode" = "ShadowCaster" }

		CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_shadowcaster
#pragma fragmentoption ARB_precision_hint_fastest

#include "UnityCG.cginc"

	sampler2D _MainTex;
	float4 _MainTex_ST;

	struct appdata
	{
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		half3 normal : NORMAL;
	};


	struct v2f
	{
		float2 texcoord : TEXCOORD3;
		V2F_SHADOW_CASTER;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
		return o;
	}

	float4 frag(v2f i) : COLOR
	{
		fixed col = tex2D(_MainTex, i.texcoord).a * 2;
		if (col < 0.01) discard;
		SHADOW_CASTER_FRAGMENT(i)
	}

		ENDCG
	}


	}
		
}
