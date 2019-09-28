Shader "minimapShader"
{
    Properties
    {
      _exploredTexture ("Explored Area Texture", 2D) = "black" {}
	  _visibleTexture ("Visible Area Texture", 2D) = "black" {}
    }
    SubShader
    {
	   Tags { "Queue"="Transparent+1" }

        Pass
        {
			ZWrite Off 
			
            CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag 


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _exploredTexture;
			sampler2D _visibleTexture;


            v2f vert (appdata v)
            {
				v2f o; // создаем возвращаемую структуру 
				o.vertex = UnityObjectToClipPos(v.vertex); // переводим координатыиз пространства модели в проекционное 
				o.uv = v.uv; // просто передаем uv координаты
				return o; 
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 col=tex2D(_exploredTexture, i.uv)*fixed4(0.5,0.5,0.5,1);
                return col;
            }
            ENDCG
        }
    }
}
