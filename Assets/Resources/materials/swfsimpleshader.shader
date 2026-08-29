Shader "FlashTools/SwfSimple" {
	Properties {
		[PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
		[PerRendererData] _AlphaTex ("Alpha Texture", 2D) = "white" {}
		[PerRendererData] _ExternalAlpha ("External Alpha", Float) = 0
		[PerRendererData] _Tint ("Tint", Vector) = (1,1,1,1)
		[Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("BlendOp", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("SrcBlend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("DstBlend", Float) = 10
	}

	SubShader {
		Tags { 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent" 
		}

		Cull Off
		ZWrite Off
		BlendOp [_BlendOp]
		Blend [_SrcBlend] [_DstBlend]

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float4 _MainTex_ST;
			fixed4 _Tint;

			v2f vert(appdata_t v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.color = v.color * _Tint;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
				#if defined(ETC1_EXTERNAL_ALPHA)
				col.a *= tex2D(_AlphaTex, i.texcoord).r;
				#endif
				return col;
			}
			ENDCG
		}
	}
}