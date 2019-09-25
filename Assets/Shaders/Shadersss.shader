Shader "Cg projector shader for adding light" {
   Properties {
      _exploredArea ("Explored Area Texture", 2D) = "black" {}
	  _visibleArea ("Visible Area Texture", 2D) = "black" {}
	  _fogColor ("Fog Color", Color) = (0,0,0,0)
   }
   SubShader {
	   Tags { "Queue"="Transparent+100" }

      Pass {      
         Blend SrcAlpha OneMinusSrcAlpha
            // add color of _ShadowTex to the color in the framebuffer 
         ZWrite Off // don't change depths
         Offset -1, -1 // avoid depth fighting (should be "Offset -1, -1")
			
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         // User-specified properties
         uniform sampler2D _exploredArea; 
		 uniform sampler2D _visibleArea; 
		 uniform float4  _fogColor; 
 
         // Projector-specific uniforms
         uniform float4x4 unity_Projector; // transformation matrix 
            // from object space to projector space 
 
          struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 posProj : TEXCOORD0;
               // position in projector space
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            output.posProj = mul(unity_Projector, input.vertex);
            output.pos = UnityObjectToClipPos(input.vertex);
            return output;
         }
 
 
         float4 frag(vertexOutput input) : COLOR
         {
			_fogColor.a = max(1-max(tex2D(_exploredArea,input.posProj).a*0.5, tex2D(_visibleArea,input.posProj).a),0);
            return _fogColor;
         }
 
         ENDCG
      }
   }  
   Fallback "Projector/Light"
}