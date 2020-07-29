
///
/// From: HoloToolkit / Spatial Mapping / Shaders / Occlusion.shader
/// Basic occlusion shader that can be used with spatial mapping meshes.
/// No pixels will be rendered at the object's location.
///

Shader "Custom/ClipInWorldSpace"
{
	Properties
	{
	}
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"Queue" = "Geometry-1"
		}

		Pass
		{
			ColorMask 0 // Color will not be rendered.
			Offset 50, 100

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// We only target the HoloLens (and the Unity editor), so take advantage of shader model 5.
			//#pragma target 5.0
			//#pragma only_renderers d3d11

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				// push the screen space position to just less than the far plane 
				// https://forum.unity.com/threads/subshader-with-zwrite-off-visible-in-scene-view-but-not-in-game-preview.269379/ 
				o.pos.z = 0.9999;
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				return float4(1,1,1,1);
			}
			ENDCG
		}
	}
}