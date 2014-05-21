Shader "Transparent via Projector" {
	Properties {
	_Color ("Main Color", Color) = (1.0,1.0,1.0,1.0)
	}
SubShader
{
	Tags {"Queue" = "Geometry+300"}	
	Pass 
	{
		// The alpha is supplied by the projector.
		Blend OneMinusDstAlpha DstAlpha
		Color [_Color]
	}
}


}