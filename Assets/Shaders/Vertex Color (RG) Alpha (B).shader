Shader "Unlit/Vertex Color (RG) Alpha (B)"
{
    Properties
    {
		_Color1("Color (0)", color) = (1,1,1,1)
		_Color2("Color (R)", color) = (1,1,1,1)
		_Color3("Color (G)", color) = (1,1,1,1)
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

			fixed4 _Color1;
			fixed4 _Color2;
			fixed4 _Color3;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 col = lerp(_Color1, lerp(_Color2, _Color3, i.color.g), i.color.r);
				col.a = i.color.b;
				return col;
            }
            ENDCG
        }
    }
}
