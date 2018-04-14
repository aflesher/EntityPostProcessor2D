Shader "WholeEffects2D/Displacement"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DisplaceTex ("Displacement Texture", 2D) = "white" {}
		_Magnitude ("Magnitude", Float) = 1.0
		_Speed ("Speed", Float) = 1.0
		_Magnify ("Magnify", Range(1.0, 20.0)) = 1.0
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
			sampler2D _DisplaceTex;
			float _Magnitude;
			float _Speed;
			float _Magnify;
			int _Paused;

			float2 Displace(in float2 uv, in sampler2D dTex, float magnify, float speed, float magnitude) {
				magnify = 1 / magnify;
				float2 distuv = float2((uv.x * magnify) + (_Time.x * speed), (uv.y * magnify) + (_Time.x * speed));
				return ((tex2D(dTex, distuv).xy * 2) - 1) * magnitude;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv + Displace(i.uv, _DisplaceTex, _Magnify, _Speed * (1 - _Paused), _Magnitude));
				return col;
			}
			ENDCG
		}
	}
}