Shader "Sprite/CircleClipLocalSpace"
{
	Properties
	{
		_MainTex("Base (RGB), Alpha (A)", 2D) = "white" {}
		_Edge("Edge", Float) = 2
		_Strength("Strength", Range(0,1)) = 0.5
	}

	SubShader
	{

		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			Lighting Off
			ZWrite Off

			Fog{ Color(0,0,0,0) }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma glsl
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Edge;
			float _Strength;

			struct v2f
			{
				half4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				half2 savedVertices : TEXCOORD1;
				fixed4 color : COLOR;
				half2 texUV : TEXCOORD2;
			};

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 savedVertices : TEXCOORD1;
				float4 color : COLOR;
			};

			inline float4 Overlay(float4 a, float4 b) {
				return lerp(1 - 2 * (1 - a) * (1 - b),    2 * a * b,    step(a, 0.5));
			}

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord - half2(0.5,0.5);
				o.color = v.color;
				o.texUV = v.savedVertices * _MainTex_ST.xy + _MainTex_ST.w;

				return o;
			}

			half4 frag(v2f IN) : COLOR
			{
				fixed4 col = Overlay(IN.color, tex2D(_MainTex, IN.texUV) * _Strength);
				fixed4 transparent = fixed4(col.xyz,0);
				float l = length(IN.texcoord);
				float thresholdWidth = length(float2(ddx(l),ddy(l))) * _Edge;

				float antialiasedCircle = saturate(((1.0 - (thresholdWidth * 0.25) - (l * 2)) / thresholdWidth) + 0.5);
				return lerp(transparent, col, antialiasedCircle);
			}
			ENDCG
		}
	}
}