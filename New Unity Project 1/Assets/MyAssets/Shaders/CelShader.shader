Shader "Quicksilver/CelShader"
{
	Properties
	{
		_Color ("_Color", Color) = (1,0,0,1)
	}

	SubShader
	{
		ZTest LEqual

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform float4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float ratio : VALUE;
				float4 vertex : POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				float3 norm = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				o.ratio  = length(norm.xy)/length(norm);
				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				float4 col = i.ratio < 0.9 ? _Color : float4(0,0,0,1);
				return col;
			}
			ENDCG
		}
	}
}
