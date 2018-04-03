Shader "Quicksilver/GhostShader"
{
	Properties
	{
		//_Color ("_Color", Color) = (1,0,0,0.5)
	}

	SubShader
	{
		Tags {  "Queue" = "Transparent"
			"enderType" = "Transparent"
		}

		Pass
		{
			ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#pragma instancing_options procedural:setup
			
			#include "UnityCG.cginc"
			#include "ColorManipulation.cginc"

			struct vertData
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct fragData
			{
				float4 vertex : SV_POSITION;
				//float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
			UNITY_INSTANCING_BUFFER_END(Props)


			fragData vert (vertData v)
			{
				UNITY_SETUP_INSTANCE_ID(v);

				fragData o;
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.color = UNITY_ACCESS_INSTANCED(Props, _Color);
				return o;
			}

			half4 frag (fragData i) : COLOR
			{
				UNITY_SETUP_INSTANCE_ID(i);
				return UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
			}
			ENDCG
		}
	}
}
