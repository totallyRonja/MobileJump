Shader "Sprites/RocketExhaust"{
	Properties{
		_Color ("Tint", Color) = (0, 0, 0, 1)
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}
		_Shape ("Shape", 2D) = "white" {}
		_Noise ("Noise", 2D) = "white" {}
		_Inner ("Inner Color", Color) = (1,1,1,1)
		_InnerToMiddle("First Cutoff", Range(0, 1)) = 0.333
		_Middle ("Middle Color", Color) = (1,1,1,1)
		_MiddleToOuter("Second Cutoff", Range(0, 1)) = 0.667
		_Outer ("Outer Color", Color) = (1,1,1,1)
		_Border("Visibility Cutoff", Range(0, 1)) = 1
	}

	SubShader{
		Tags{ 
			"RenderType"="Transparent" 
			"Queue"="Transparent"
		}

		Blend One OneMinusSrcAlpha

		ZWrite off
		Cull off

		Pass{

			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			//sampler2D _MainTex;
			//float4 _MainTex_ST;

			sampler2D _Shape;
			sampler2D _Noise;

			fixed4 _Color;
			fixed4 _Inner;
			fixed4 _Middle;
			fixed4 _Outer;

			float _InnerToMiddle;
			float _MiddleToOuter;
			float _Border;

			struct appdata{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			v2f vert(appdata v){
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET{
				float2 uv = i.uv;
				//make it wobble
				float2 uvNoise = tex2D(_Noise, i.uv*0.4 + float2(0.84, -0.54) * _Time.y).xy - 0.25;
				uv += uvNoise * 0.2;
				//and give it some downward spikeyness
				//its important that the second function doesnt use the output of the first (where easily avoidable)
				//as that would have a higher performance impact
				uvNoise = tex2D(_Noise, i.uv * float2(8, .2) + float2(-.1, 0.97) * _Time.y).xy - 0.25;
				uv += uvNoise * 0.1;

				//evaluate texture and apply falloff
				const float falloff = tex2D(_Shape, uv).a;
				if(falloff >= 1)
					return _Inner;
				if(falloff <= 0)
					return 0;
				float derivative = fwidth(falloff) * 0.5;
				fixed4 color = _Inner;
				float firstStep = smoothstep(_InnerToMiddle + derivative, _InnerToMiddle, falloff);
				color = lerp(color, _Middle, firstStep);
				float secondStep = smoothstep(_MiddleToOuter + derivative, _MiddleToOuter, falloff);
				color = lerp(color, _Outer, secondStep);
				float lastStep = smoothstep(_Border + derivative, _Border, falloff);
				color = lerp(color, 0, lastStep);
				return color;
			}

			ENDCG
		}
	}
}