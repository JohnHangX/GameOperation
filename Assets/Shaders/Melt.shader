Shader "Unlit/Melt"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseMap("NoiseTex",2D) = "white" {}
		_StartColor("StartColor", Color) = (0,0,0,0)
		_EndColor("EndColor", Color) = (0,0,0,0)
		_MeltThreshold("MeltThreshold", Range(0,1)) = 0
		_Erode("Erode", Range(0.0, 1.0)) = 0.98
		_ErodeThreshold("ErodeThreshold" ,Range(0.0, 1.0)) = 0.71
	}
	SubShader
	{
		CGINCLUDE
			#include "Lighting.cginc"
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _NoiseMap;

			fixed4 _StartColor;
			fixed4 _EndColor;
			float _MeltThreshold;
			float _Erode;
			float _ErodeThreshold;

			struct a2v
			{
				float4 vertex: POSITION;
				float3 normal:NORMAL;
				float4 texcoord:TEXCOORD0;
				
			};

			struct v2f
			{
				float4 pos:SV_POSITION;
				float3 worldNormal:TEXCOORD0;
				float3 worldPos:TEXCOORD1;
				float2 uv:TEXCOORD2;
				SHADOW_COORDS(3)

			};

			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				TRANSFER_SHADOW(o);
				return o;
			};

			fixed4 frag(v2f i):SV_Target
			{
				fixed3 melt = tex2D(_NoiseMap, i.uv).rgb;

				clip(melt.r - _MeltThreshold);

				fixed3 albedo = tex2D(_MainTex, i.uv).rgb;

				fixed3 worldNormal = normalize(i.worldNormal);

				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos);

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

				fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, -worldLightDir));

				fixed3 lightColor =  diffuse *atten + ambient;

				float result = _MeltThreshold / melt.r;

				if(result > _Erode)
				{
					if(result > _ErodeThreshold)
					{
						return _EndColor;
					}
					return _StartColor;
				}
				return fixed4(lightColor, 1);
			}

		ENDCG

		Pass
		{
			Tags{"RendType" = "Opaque"}
			Cull off
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			ENDCG
		}
		
	}
	FallBack Off
}
