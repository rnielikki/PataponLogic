Shader "Unlit/MinigameBg"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                //v.vertex.x += sin(_Time.y * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount;
                /*
                col *= clamp(
                    (sin(i.vertex.x /(12 + _SinTime.w * 0.5) + cos(i.vertex.y / (12 + _SinTime.w * 0.5)) + _SinTime.w * 8) - cos(i.vertex.y /10 + _CosTime.w* 8))
                * sin(i.vertex.x/50) + cos(i.vertex.y / 50) + 0.5
                , 0.3, 0.6);
                */
                /*
                float val = sin(i.vertex.x /(20+_SinTime.w * 0.6)) + cos(i.vertex.y /(15-_SinTime.w * 0.8));
                col += clamp(val, 0, 0.2);
                col -= clamp(val, 0.3, 0.5);
                col += clamp(val, 0.9, 1);
                col.rgb = (col.rgb - 0.5) * (_CosTime.x * 0.1 + 0.5);
                */
                float val = sin(i.vertex.x /(20+_SinTime.w * 0.6)) + cos(i.vertex.y /(15-_SinTime.w * 0.8));
                col += clamp(sin(i.vertex.y * clamp(val, 0.2, 0.8)/(250 + _SinTime.w * 0.6)
                + _SinTime.w + sin(i.vertex.x/(20 + _SinTime.w * 0.5)) + _SinTime.w), 0, 0.1);
                col += _CosTime.w * i.vertex.y * 0.0005;
                //col += res;
                return col;
            }
            ENDCG
        }
    }
}
