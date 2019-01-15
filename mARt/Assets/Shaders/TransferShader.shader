Shader "Shader/TransferShader"
{
	Properties
	{
		_TransferColor ("TransferColor", 2D) = "" {}
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
			#include "./TransferShader.cginc"
			#pragma vertex vert
			#pragma fragment frag
			

			ENDCG
		}
	}
}
