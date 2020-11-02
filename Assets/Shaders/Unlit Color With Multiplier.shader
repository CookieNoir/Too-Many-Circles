Shader "Unlit/Color With Multiplier"
{
    Properties
    {
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
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

			fixed4 _Color;
			float _AlphaMultiplier;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;
				col.a *= _AlphaMultiplier;
                return col;
            }
            ENDCG
        }
    }
}
