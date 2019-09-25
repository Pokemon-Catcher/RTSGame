// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/testShader"
{
  Properties // отображаемые в инспекторе свойства
   { 
    _text("Texture",2D) = "white" {} 
   }
   SubShader 
   {
     Pass 
      {
        CGPROGRAM
			struct vertexInput // входящие данные
			{
				float4 vertex:POSITION; // позиция вершины в пространстве модели
				float2 uv:TEXCOORD0; // uv координаты данной вершины 
			};
			struct vertexOutput // выходящие данные
			{
				float4 position:SV_POSITION; // позиция 
				float2 uv:TEXCOORD0; // uv координаты
			};
			#pragma vertex vert 
			#pragma fragment frag 
			
			uniform sampler2D _text; // внешняя текстура 
			
			vertexOutput vert(vertexInput v) 
			{
				vertexOutput o; // создаем возвращаемую структуру 
				float4 pos = (UnityObjectToClipPos(v.vertex));
				o.position = pos*float4(1,1,1,1); // переводим координатыиз пространства модели в проекционное 
				o.uv = v.uv; // просто передаем uv координаты
				return o; 
			}
			

			fixed4 frag(vertexOutput v):SV_Target 
			{
				fixed4 col = tex2D(_text,v.uv)*fixed4(1,1,sin(distance(v.position,_WorldSpaceCameraPos)*0.1),0);
				return col;
			} 
        // тут будет CG код
        ENDCG
      }
   }
  Fallback "Diffuse" 
}
