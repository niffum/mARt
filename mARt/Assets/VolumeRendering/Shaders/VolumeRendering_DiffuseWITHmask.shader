Shader "VolumeRendering/VolumeRendering"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_ColorMask ("ColorMask", Color) = (1, 1, 1, 1)
		_Volume ("Volume", 3D) = "" {}
		_VolumeMask ("VolumeMask", 3D) = "" {}
		_TranferColor ("TransferColor", 2D) = "" {}
		_Intensity ("Intensity", Range(0.0, 5.0)) = 1.2
		_IntensityMask ("IntensityMask", Range(1.0, 3.5)) = 0
		_Threshold ("Threshold", Range(0.0, 1.0)) = 0.95
		_SliceMin ("Slice min", Vector) = (0.0, 0.0, 0.0, -1.0)
		_SliceMax ("Slice max", Vector) = (1.0, 1.0, 1.0, -1.0)
		
		_ShowMask ("ShowMask", Float) = 0

		_Shininess("Shininess", Range(0.0, 20.0)) = 5.0

		_Gamma("Gamma", Range(0.0, 3.0)) = 0.5
	}

	CGINCLUDE

	ENDCG

	SubShader {
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		// ZTest Always

		Pass
		{
			CGPROGRAM

      #define ITERATIONS 100
			#include "./VolumeRendering_DiffuseWITHmask.cginc"
			#pragma vertex vert
			#pragma fragment frag
			

			ENDCG
		}
	}
}
