/*
 * Source: https://github.com/mattatz/unity-volume-rendering
 * Modified by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */

Shader "VolumeRendering/VolumeRendering"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Volume ("Volume", 3D) = "" {}
		_Intensity ("Intensity", Range(0.0, 5.0)) = 1.2
		_Threshold ("Threshold", Range(0.0, 1.0)) = 0.95
		_SliceMin ("Slice min", Vector) = (0.0, 0.0, 0.0, -1.0)
		_SliceMax ("Slice max", Vector) = (1.0, 1.0, 1.0, -1.0)
		
		// Added by Viola Jertschat-----------
		_ShowMask ("ShowMask", Float) = 0
		_ColorMask("ColorMask", Color) = (1, 1, 1, 1)
		_IntensityMask("IntensityMask", Range(1.0, 3.5)) = 0
		_VolumeMask("VolumeMask", 3D) = "" {}
		_TranferColor("TransferColor", 2D) = "" {}

		// -----------------------------------
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
			#include "./VolumeRendering_Diffuse.cginc"
			#pragma vertex vert
			#pragma fragment frag
			

			ENDCG
		}
	}
}
