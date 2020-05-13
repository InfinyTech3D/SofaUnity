//Shader from UCLA
Shader "SofaUnity/Wireframe/Double-Sided" 
{
	Properties 
	{
		_Color ("Line Color", Color) = (1, 0, 0, 1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_Thickness ("Thickness", Float) = 1
	}

	SubShader 
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		
		// First pass that renders the back faces of the model (cull front faces)
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha 
			ZWrite Off
			Cull Front
			LOD 200
			
			CGPROGRAM
				#pragma target 5.0
				#include "UnityCG.cginc"
				#include "UCLA GameLab Wireframe Functions.cginc"
				#pragma vertex vert
				#pragma fragment frag
				#pragma geometry geom

				// Vertex Shader
				UCLAGL_v2g vert(appdata_base v)
				{
					return UCLAGL_vert(v);
				}
				
				// Geometry Shader
				[maxvertexcount(3)]
				void geom(triangle UCLAGL_v2g p[3], inout TriangleStream<UCLAGL_g2f> triStream)
				{
					UCLAGL_geom( p, triStream);
				}
				
				// Fragment Shader
				float4 frag(UCLAGL_g2f input) : COLOR
				{	
					return UCLAGL_frag(input);
				}
			
			ENDCG
		}
		// Second pass to render the fronts of polygons.
		// Guarantees render order of back then front to avoid render artifacts
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha 
			ZWrite Off
			Cull Back
			LOD 200
			
			CGPROGRAM
				#pragma target 5.0
				#include "UnityCG.cginc"
				#include "UCLA GameLab Wireframe Functions.cginc"
				#pragma vertex vert
				#pragma fragment frag
				#pragma geometry geom

				// Vertex Shader
				UCLAGL_v2g vert(appdata_base v)
				{
					return UCLAGL_vert(v);
				}
				
				// Geometry Shader
				[maxvertexcount(3)]
				void geom(triangle UCLAGL_v2g p[3], inout TriangleStream<UCLAGL_g2f> triStream)
				{
					UCLAGL_geom( p, triStream);
				}
				
				// Fragment Shader
				float4 frag(UCLAGL_g2f input) : COLOR
				{	
					return UCLAGL_frag(input);
				}
			
			ENDCG
		}
	} 
}
