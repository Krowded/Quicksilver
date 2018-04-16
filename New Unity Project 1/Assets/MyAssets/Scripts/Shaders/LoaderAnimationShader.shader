Shader "Quicksilver/HolderShader"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_XLimit ("X Limit", Range(0,1)) = 0.1
		_YLimit ("Y Limit", Range(0,1)) = 0.1
	}

	SubShader
	{
		Tags {  "Queue" = "Transparent"
			"RenderType" = "Transparent"
			"DisableBatching" = "True" //Need object space, so can't batch
		}

		Pass
		{
			Name "Hologram"
			Cull Off
			ZWrite On
			ZTest LEqual

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#include "DigitalShader.cginc"
			#pragma vertex vertDigital
			#pragma fragment fragDigital

			ENDCG
		}
	}
}