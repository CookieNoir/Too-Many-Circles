Shader "Unlit/Vertex Color (R) Alpha (B)"
{
	Properties
	{
		_Color1("Color (0)", color) = (1,1,1,1)
		_Color2("Color (R)", color) = (1,1,1,1)
	}
		SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend OneMinusSrcAlpha SrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			half4 _Color1;
			half4 _Color2;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				half4 col = lerp(_Color1, _Color2, i.color.r);
				col.a = i.color.b;
				return col;
			}
			ENDCG
		}
	}
}
