﻿Shader "Unlit/Smooth Line X"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", color) = (1,1,1,1)
		_AlphaMultiplier("Alpha Multiplier", float) = 1.0
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
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
					float2 uv2 : TEXCOORD1;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _Color;
				float _AlphaMultiplier;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					o.uv2 = TRANSFORM_TEX(v.uv, _MainTex);
					o.uv2.x = o.uv2.x - 2.0 * _Time.y;
					return o;
				}

				float cube(float x)
				{
					return x * x * x;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv2) * _Color;
					float gradientX = 1.0 - cube(max(0.0, abs(2.0*i.uv.x - 1.0) * 2.0 - 1.0));
					float gradientY = 1.0 - cube(max(0.0, abs(2.0*i.uv.y - 1.0) * 2.0 - 1.0));
					col.a *= gradientX * gradientY * _AlphaMultiplier;
					return col;
				}
				ENDCG
			}
		}
}
