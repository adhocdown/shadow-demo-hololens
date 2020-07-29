// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//RadialGradientQuad.shader
Shader "Custom/RadialGradientWithTransparencyAndNoiseAndClippedShadows" {
	Properties{
		_ColorA("Color A", Color) = (1, 1, 1, 1)
		_ColorB("Color B", Color) = (1, 1, 1, 0)
		_ShadowColor("Color Shadow", Color) = (0,0,0,1)
		_Slide("Slide", Range(0, 1)) = 0.5		
		_Noise("Noise", Range(0,1)) = 0.5
	}

	CGINCLUDE
	#include "UnityCG.cginc"
	#include "AutoLight.cginc"
	
	struct v2f_shadow
	{
		float4 pos : SV_POSITION;
		LIGHTING_COORDS(0, 1)
		half2 texcoord : TEXCOORD1;
	};

	half4 _ShadowColor;

	v2f_shadow vert_shadow(appdata_full v)
	{
		v2f_shadow o;
		o.pos = UnityObjectToClipPos(v.vertex);
		TRANSFER_VERTEX_TO_FRAGMENT(o);
		o.pos = UnityObjectToClipPos(v.vertex); 
		o.texcoord = v.texcoord; 
		return o;
	}

	// Radial vv 
	fixed4 _ColorA, _ColorB;
	float _Slide, _Noise;

	half4 frag_shadow(v2f_shadow IN) : SV_Target
	{
		// Shadow
		half atten = LIGHT_ATTENUATION(IN);
		half4 shadow = half4(_ShadowColor.rgb, lerp(_ShadowColor.a, 0, atten));

		// Radial 
		float t = length(IN.texcoord - float2(0.5, 0.5)) * 1.41421356237; // 1.41... = sqrt(2)
		fixed4 rand = (_Noise * frac(sin(dot(IN.texcoord, float3(12.9898, 78.233, 45.5432))) * 43758.5453));
		fixed4 o = lerp(_ColorA, _ColorB + rand, t + (_Slide - 0.5) * 2);

		// explicitly clip alpha values [Needed for Zed] 
		if (o.a <= 0) o.a = 0;
		if (shadow.a >= 0.001) o.a = 0; // cut that shadow shit out ;) 
		return o;

	}
	ENDCG 

	SubShader 
	{
		Tags { 	"Queue" = "Transparent" // makes sprites render AFTER the opaque geometry in the scene
										// prevents quirk behavior when your custom shader overlaps with opaque geometry
				"IgnoreProjector" = "True" 
				"RenderType" = "Transparent" 
				"PreviewType" = "Plane" 
			 }	

		// Forward base pass 			
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert_shadow
			#pragma fragment frag_shadow
			#pragma multi_compile_fwdbase
			ENDCG
		}

			// Forward add pass 
			Pass
		{
			Tags{ "LightMode" = "ForwardAdd" }
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert_shadow
			#pragma fragment frag_shadow
			#pragma multi_compile_fwdadd_fullshadows
			ENDCG
		}
	} // eof SubShader
} // eofShader