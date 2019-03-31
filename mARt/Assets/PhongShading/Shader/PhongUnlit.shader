/*
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */
Shader "Unlit/PhongUnlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = v.normal;//normalize(UnityObjectToClipPos(v.normal));//normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
				return o;
			}

			float3 phong(float3 normal, float3 viewDir, float3 lightDir)
			{
				// Ambient
				float3 ambientColor = float3(0.5, 0.2, 0.2);
				float ambientIntensity = 0.5;
				float3 ambientComponent = ambientColor * ambientIntensity;

				// Diffuse
				float3 diffuseColor = float3(0.5, 1.0, 1.0);
				float diffuseIntensity = 1.0;

				float ndotl = dot(normal, lightDir);

				// When point is facing away from light
				if (ndotl <= 0.0)
				{
					return ambientComponent;
				}
				else
				{
					ndotl = max(ndotl, 0.0);
				}

				float3 diffuseComponent = diffuseColor * diffuseIntensity * ndotl;

				// Specular
				float3 specularColor = float3(1.0, 1.0, 1.0);
				float specularIntensity = 1.0;
				float shininessPower = 10.0;
				
				float3 reflectionDir = reflect(-lightDir, normal);

				float rdotv = max(dot(reflectionDir, viewDir), 0.0);
				
				float3 specularComponent = specularColor * specularIntensity * pow(rdotv, shininessPower);

				
				return ambientComponent + diffuseComponent + specularComponent;
				//return float4(abs(normal), 1.0);
			}	
			
			fixed4 frag (v2f i) : SV_Target
			{

				//float4 lightPos = mul(UNITY_MATRIX_V, _WorldSpaceLightPos0);
				//float4 lightPos = unity_LightPosition[0];
				float3 normal = normalize(i.normal);

				float3 lightDir = normalize((_WorldSpaceLightPos0 - i.vertex).xyz);
				//float4 camPos = mul(UNITY_MATRIX_V, _WorldSpaceCameraPos);
				float3 viewDir = normalize(_WorldSpaceCameraPos - i.vertex.xyz);

				return float4(phong(normal, lightDir, viewDir), 1.0);
			}
			ENDCG
		}
	}
}
