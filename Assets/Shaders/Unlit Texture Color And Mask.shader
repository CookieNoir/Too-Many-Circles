Shader "Unlit/Texture Color And Mask"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_Color("Color", color) = (1,1,1,1)
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
					float2 uv2 : TEXCOORD1;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float2 uv2 : TEXCOORD1;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _Mask;
				float4 _Mask_ST;
				fixed4 _Color;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.uv2 = TRANSFORM_TEX(v.uv2, _Mask);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv) * _Color;
					col.a *= tex2D(_Mask, i.uv2).a;
					return col;
				}
				ENDCG
			}
		}
}