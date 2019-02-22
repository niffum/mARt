Shader "Unlit/TextureBrightnessContrast"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MaskTex("MaskTexture", 2D) = "white" {}
		_ColorMask("ColorMask", Color) = (1, 1, 1, 1)
		_Brightness("Brightness", Range(0,1)) = 0.5
		_Contrast("Contrast", Range(0,1)) = 0.5

		_ShowMask("ShowMask", Float) = 0
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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _MaskTex;
			half4 _ColorMask;
			float4 _MainTex_ST;

			float _ShowMask;

			float _Contrast, _Brightness;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			inline float4 applyContrastAndBrightness(float4 startColor, float contrast, float brightness)
			{
				//https://forum.unity.com/threads/hue-saturation-brightness-contrast-shader.260649/

				
				brightness = brightness * 2 - 1;
				contrast = contrast * 2;
				
				//float _Saturation = hsbc.a * 2;

				float4 outputColor = startColor;
				outputColor.rgb = (outputColor.rgb - 0.5f) * (contrast)+0.5f;
				outputColor.rgb = outputColor.rgb + brightness;
				/*
				float3 intensity = dot(outputColor.rgb, float3(0.299, 0.587, 0.114));
				outputColor.rgb = lerp(intensity, outputColor.rgb, _Saturation);
				*/
				return outputColor;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 maskColor = tex2D(_MaskTex, i.uv);
				fixed4 col = float4(1, 1, 1, 1);
				// sample the texture
				if (maskColor.r > 0 && _ShowMask == 1)
				{
					col = applyContrastAndBrightness(tex2D(_MainTex, i.uv), _Contrast, _Brightness) * (_ColorMask);
				}
				else {
					col = applyContrastAndBrightness(tex2D(_MainTex, i.uv), _Contrast, _Brightness);
				}
				//fixed4 col = tex2D(_MainTex, i.uv);

				return col;
			}
			ENDCG
		}
	}
}
