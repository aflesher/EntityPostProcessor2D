Shader "EntityPostProcessor2D/Dissolve"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_EdgeColor ("Color", Color) = (1,1,1,1)
		_EdgeSize ("Edge Size", Float) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

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
			
			sampler2D _MainTex;
			fixed4 _EdgeColor;
			float _Progress;
			float _EdgeSize;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float progress = _Progress * .5f;
				float y = abs(i.uv.y - .5f);
				col.a *= step(progress, y);
				col.rgb = lerp(_EdgeColor.rgb, col.rgb, smoothstep(progress, progress + _EdgeSize, y));

				return col;
			}
			ENDCG
		}
	}
}
