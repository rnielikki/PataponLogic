Shader "Unlit/BackgroundShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Cutoff("Cutoff", Float) = 0.5
		_MoveSensitivity("Move Sensitivity", Float) = 0.1
	}
		SubShader
		{
			Tags { "RenderType" = "TrnasparentCutout" }
			LOD 100
			AlphaToMask On
			Blend SrcAlpha OneMinusSrcAlpha

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
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _MainTex_TexelSize;
				float _Cutoff;
				float _MoveSensitivity;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.uv.x += _WorldSpaceCameraPos.x * _MoveSensitivity;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = tex2D(_MainTex, i.uv);
					return col;
				}
			ENDCG
			}
		}
}
