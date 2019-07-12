Shader "Custom/Silhouette" {	Properties{
_Color("Color", Color) = (1,1,1,1)
_OutlineColor("Outline Color", Color) = (1,1,1,1)
	
_MainTex("Albedo (RGB)", 2D) = "white" {}
_Glossiness("Smoothness", Range(0,1)) = 0.5
_Metallic("Metallic", Range(0,1)) = 0.0

}
SubShader{

	Tags{ "RenderType" = "Opaque" }
	LOD 200

	//Render a pass if anything is infront 
	Pass
	{
		ZTest Greater
		ZWrite Off
		Cull Front
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		// make fog work
		#pragma multi_compile_fog

		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			//				float3 normal : NORMAL;
		};


		float4 _OutlineColor;

		v2f vert(appdata_full v)
		{
			v2f o;

			o.vertex = UnityObjectToClipPos(v.vertex);
			

			//Scroll the texture acording to world space
			UNITY_TRANSFER_FOG(o,o.vertex);

			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{


			return  _OutlineColor;

		}
		ENDCG

	}




	}
}
