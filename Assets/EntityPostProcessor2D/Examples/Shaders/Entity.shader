Shader "EntityPostProcessor2D/Entity"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	CGINCLUDE
		#pragma multi_compile __ OUTLINE
		#pragma multi_compile __ DISSOLVE
		#pragma multi_compile __ COLORED

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float2 _MainTex_TexelSize;

		// outline
		fixed4 _Outline_OutlineColor;
		float _Outline_OutlineSize;

		// dissolve
		float _Dissolve_Progress;
		float _Dissolve_EdgeSize;
		fixed4 _Dissolve_EdgeColor;

		// color
		fixed4 _Color_Color;

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
		};

		v2f vert (appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			return o;
		}

		fixed4 frag (v2f i) : SV_Target
		{
			float2 uv = i.uv;
			fixed4 col = tex2D(_MainTex, uv);
			float alpha = 1;

			#if COLORED

			col.rgb *= _Color_Color.rgb;
			alpha = _Color_Color.a;
			col.a *= alpha;

			#endif

			#if OUTLINE

			float2 outlineVecs[16] = {
				float2(0, 1),
				float2(0.3826835, 0.9238796),
				float2(0.7071069, 0.7071068),
				float2(0.9238796, 0.3826834),
				float2(1, 0),
				float2(0.9238795, -0.3826835),
				float2(0.7071068, -0.7071068),
				float2(0.3826833, -0.9238796),
				float2(0, -1),
				float2(-0.3826835, -0.9238796),
				float2(-0.7071069, -0.7071067),
				float2(-0.9238797, -0.3826832),
				float2(-1, 0),
				float2(-0.9238795, 0.3826835),
				float2(-0.7071066, 0.707107),
				float2(-0.3826834, 0.9238796)
			};

			float outlineAlpha = 0;
			for (int d = 0; d < 16; d++) {
				float sampleAlpha = tex2D(_MainTex, uv + outlineVecs[d] * _MainTex_TexelSize * _Outline_OutlineSize).a * alpha;
				outlineAlpha = max(sampleAlpha, outlineAlpha);
			}
			outlineAlpha *= _Outline_OutlineColor.a;
			col.rgb += (_Outline_OutlineColor.rgb * max((outlineAlpha - col.a), 0));
			col.a = max(col.a, outlineAlpha);

			#endif

			#if DISSOLVE

			float dprogress = _Dissolve_Progress * .5f;
			float dy = abs(uv.y - .5f);
			col.a *= step(dprogress, dy);
			col.rgb = lerp(_Dissolve_EdgeColor.rgb, col.rgb, smoothstep(dprogress, dprogress + _Dissolve_EdgeSize, dy));

			#endif

			return col;
		}
	ENDCG

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
			ENDCG
		}
	}
}
