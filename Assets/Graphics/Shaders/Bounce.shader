Shader "Sprites/Bounce"{
	Properties{
        _Color ("Tint", Color) = (1,1,1,1)
		_Extents("Width", Range(0, 4)) = 0.5
		_Frequency("Frequency", Range(0, 50)) = 1
		_Amplitude("Amplitude", Range(0, 1)) = 0.2
        [PerRendererData][HideInInspector] _MainTex ("Sprite Texture", 2D) = "white" {} //unused but makes unity not complain
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
	}

	SubShader{
		Tags{ 
			"RenderType"="Transparent" 
			"Queue"="Transparent"
		}

		Blend SrcAlpha OneMinusSrcAlpha

		ZWrite off
		Cull off

		Pass{

			CGPROGRAM

			#include "UnityCG.cginc"
            #pragma multi_compile_instancing

			#pragma vertex vert
			#pragma fragment frag

			//sampler2D _MainTex;
			//float4 _MainTex_ST;

			fixed4 _Color;
			float _Extents;
			float _Frequency;
			float _Amplitude;

			#ifdef UNITY_INSTANCING_ENABLED
			    UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
			        UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
			        UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)
			        UNITY_DEFINE_INSTANCED_PROP(float, _BounceTime)
			    UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

			    #define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
			    #define _Flip           UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteFlipArray)
			    #define _BounceTime     UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _BounceTime)

			#endif // instancing

			CBUFFER_START(UnityPerDrawSprite)
			#ifndef UNITY_INSTANCING_ENABLED
			    fixed4 _RendererColor;
			    fixed2 _Flip;
				float _BounceTime;
			#endif
			    float _EnableExternalAlpha;
			CBUFFER_END

			struct appdata{
				float4 vertex : POSITION;
				//float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f{
				float4 vertex : SV_POSITION;
				//float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			v2f vert(appdata IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID (IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = IN.vertex;
				OUT.vertex.xy *= _Flip;
					
				float4 worldPos = mul(unity_ObjectToWorld, OUT.vertex);
				float3 origin = unity_ObjectToWorld._m03_m13_m23;
				float xDiff = abs(worldPos - origin);
				if(_BounceTime) {
					xDiff = clamp(xDiff * UNITY_PI, -UNITY_PI/2, UNITY_PI/2);
					float amount = cos(xDiff) * _Amplitude * (1 - _BounceTime);
					worldPos.y += amount * sin(_BounceTime * _Frequency);
				}
                OUT.vertex = UnityWorldToClipPos(worldPos);
                //OUT.texcoord = IN.texcoord;

                #ifdef UNITY_COLORSPACE_GAMMA
                fixed4 color = IN.color;
                #else
                fixed4 color = fixed4(GammaToLinearSpace(IN.color.rgb), IN.color.a);
                #endif

                OUT.color = color * _Color * _RendererColor;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

				//OUT.color = fixed4(xDiff.xxx, 1);

                return OUT;
            }

			fixed4 frag(v2f i) : SV_TARGET{
				return i.color;
			}

			ENDCG
		}
	}
}