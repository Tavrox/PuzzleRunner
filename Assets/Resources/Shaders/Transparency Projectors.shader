Shader "Transparency Projectors" {	

Properties {
	_MainTex ("Cookie", 2D) = "" {TexGen ObjectLinear}
	_Color ("Offset", Color) = (0,0,0,0)
}
Category {
Tags {"Queue" = "Geometry+200"}
SubShader
{
	
	Pass 
	{
		ColorMask A
        Blend One One
        SetTexture [_MainTex] {
            Matrix[_Projector]
        }
        SetTexture [_MainTex] {
            constantColor [_Color]
            Combine previous * constant DOUBLE, previous * constant
        }  
		//SetTexture [_FalloffTex] { // add offset
//
     //           constantColor [_Tint]
//
         //       combine previous + constant
//
        //    }
	}   
}


}
}