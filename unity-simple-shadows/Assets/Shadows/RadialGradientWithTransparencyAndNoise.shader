// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//RadialGradientQuad.shader
Shader "Custom/RadialGradientWithTransparencyAndNoise" {
	Properties{
		_ColorA("Color A", Color) = (1, 1, 1, 1)
		_ColorB("Color B", Color) = (1, 1, 1, 0)
		_Slide("Slide", Range(0, 1)) = 0.5		
		_Noise("Noise", Range(0,1)) = 0.5
	}

	SubShader {
		Tags { 	"Queue" = "Transparent" // makes sprites render AFTER the opaque geometry in the scene
									// prevents quirk behavior when your custom shader overlaps with opaque geometry
				"IgnoreProjector" = "True" 
				"RenderType" = "Transparent" 
				"PreviewType" = "Plane" }
		LOD 100

		Pass {
		// Define Blend Mode to achieve transparency! == ALPHA BLENDING ==
		// take src color * src alpha + dst color * (1-srcalpha)
		Blend SrcAlpha OneMinusSrcAlpha
		// For addidtive blending 
		//Blend One One
		Cull Off // ZWrite Off ZTest Always

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		};

	struct v2f {
		float4 vertex : SV_POSITION;
		half2 texcoord : TEXCOORD0;
	};

	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.texcoord = v.texcoord;
		return o;
	}

	fixed4 _ColorA, _ColorB;
	float _Slide, _Noise;

	fixed4 frag(v2f i) : SV_Target
	{ 
		float t = length(i.texcoord - float2(0.5, 0.5)) * 1.41421356237; // 1.141... = sqrt(2)
		fixed4 rand = (_Noise * frac(sin(dot(i.texcoord, float3(12.9898, 78.233, 45.5432))) * 43758.5453));
		fixed4 o = lerp(_ColorA, _ColorB + rand, t + (_Slide - 0.5) * 2);
		// explicitly clip alpha values [Needed for Zed]
		if (o.a <= 0) o.a = 0;
		return o;
	}
		ENDCG
	}
	}
}