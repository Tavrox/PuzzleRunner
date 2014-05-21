Shader "custom/MyBlendedAlphaColor" {
Properties {
	_MainTex ("Particle Texture", 2D) = "white" {}
	    _Color ("Main Color", Color) = (1,1,1,1)
}

Category {
	Tags { "Queue"="Geometry+1" } //"IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	SubShader {
		Pass {
		Material {
	            Diffuse [_Color]
	            Ambient [_Color]
	        }
			SetTexture [_MainTex] {
				combine texture * primary
			}
		}
	}
}
}
