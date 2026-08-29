Shader "FlashTools/SwfIncrMask" {
	Properties {
		[PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
		[PerRendererData] _AlphaTex ("Alpha Texture", 2D) = "white" {}
		[PerRendererData] _ExternalAlpha ("External Alpha", Float) = 0
		[HideInInspector] _StencilID ("Stencil ID", Float) = 0
		_AlphaClipThreshold ("Alpha Clip Threshold", Range(0,1)) = 0.5
	}

	SubShader {
		Tags { 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent" 
		}

		Cull Off
		ZWrite Off
		ZTest Always
		ColorMask 0
		Stencil {
			Ref [_StencilID]
			Comp Always
			Pass IncrSat
		}
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
			float _AlphaClipThreshold;
			v2f vert(appdata_t v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.color = v.color;
				return o;
			}
			fixed4 frag(v2f i) : SV_Target {
				fixed4 col = tex2D(_MainTex, i.texcoord);

				#if defined(ETC1_EXTERNAL_ALPHA)
				col.a = tex2D(_AlphaTex, i.texcoord).r;
				#endif

				clip(col.a - _AlphaClipThreshold);
				return fixed4(0,0,0,0);
			}
			ENDCG
		}
	}
}