Shader "Quicksilver/Outline" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_OutlineColor ("Outline Color", Color) = (1,0,0,0)
		_Outline ("Outline width", Range (0, 1)) = 10
		_MainTex ("Base (RGB)", 2D) = "white" { }
	}
 
	CGINCLUDE
	#include "UnityCG.cginc"
	 
	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};
	 
	struct v2f {
		float4 pos : POSITION;
		float4 color : COLOR;
	};
	 
	uniform float _Outline;
	uniform float4 _OutlineColor;

	ENDCG

 
	SubShader {
		Tags { "Queue" = "Geometry" }
 
		Pass {
			Name "Outline"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZTest Less

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			v2f vert(appdata v) {
				// just make a copy of incoming vertex data but scaled according to normal direction (in projected screen space)
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				float3 norm  = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 offset = TransformViewToProjection(norm.xy);
			 
				o.pos.xy += offset * o.pos.z * _Outline;
				o.color = _OutlineColor;

				return o;
			}


			half4 frag(v2f i) :COLOR {
				return i.color;
			}
			ENDCG
		}

		Pass {
			Name "InnerWhiteOutline"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZTest Less
			ColorMask RGB // alpha not used
 
			Blend One Zero
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			v2f vert(appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				float3 norm  = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 offset = TransformViewToProjection(norm.xy);
				o.pos.xy += offset * o.pos.z * _Outline * 0.3;

				return o;
			}


			half4 frag() :COLOR {
				return float4(1,1,1,1);
			}
			ENDCG
		}

		Pass {
			Name "Base"
			ZTest LEqual
			Blend One Zero

			Material {
				Diffuse [_Color]
				Ambient [_Color]
			}
			Lighting On
			SetTexture [_MainTex] {
				ConstantColor [_Color]
				Combine texture * constant
			}
			SetTexture [_MainTex] {
				Combine previous * primary DOUBLE
			}
		}
	}
}