// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Quicksilver/Disintegrate"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_YLimit ("Y Limit", Range(0,100)) = 100
	}

	CGINCLUDE
	#include "UnityCG.cginc"
	ENDCG

	SubShader
	{
		Tags { "Queue" = "Transparent"
			"RenderType" = "Transparent"
			"DisableBatching" = "True" //Need object space, so can't batch
		}

		Pass
		{
			Name "DisintegratingCel"
			ZWrite On
			ZTest LEqual

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			uniform float4 _Color;
			uniform float _YLimit;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				float4 worldVertex : POSITIONT;
				float ratio : VALUE;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldVertex = mul(unity_ObjectToWorld, v.vertex);

				float3 norm = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				o.ratio  = length(norm.xy)/length(norm);

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float4 col;
				col.rgb = i.ratio < 0.9 ? _Color.rgb : float3(0,0,0);
				col.w = i.worldVertex.y < _YLimit-_Time.y;
				return col;
			}
			ENDCG
		}


		Pass
		{
			Name "DisintegratingCel"
			ZWrite On
			ZTest LEqual
			Cull Off

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			uniform float4 _Color;
			uniform float _YLimit;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				float ratio : VALUE;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.ratio = 1;

				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				if (worldVertex.y >= _YLimit-_Time.y) {
					v.vertex.x -= _Time.y*1000;
					o.ratio = 1/_Time.z;
				}

				o.vertex = UnityObjectToClipPos(v.vertex);

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{	
				float4 col = _Color;
				col.w = i.ratio;
				return col;
			}
			ENDCG
		}
	}
}
