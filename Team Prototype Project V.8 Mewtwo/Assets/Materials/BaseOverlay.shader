Shader "Custom/BaseOverlay" {
	Properties {
		_Color("Main Color", Color) = (1,0.5,0.5,1)
		_MainTex("Base Texture", 2D) = "white" {}
		_overlayTex("Overlay Texture", 2D) = "white" {}
	}
	SubShader {
		Pass{
			Material{
				Diffuse[_Color]
			}
			
			Lighting On
			SetTexture[_MainTex]{
				constantColor[_Color]
			}
			
			setTexture[_overlayTex]{
				constantColor[_Color]
			}
			

		}
	}
} 
