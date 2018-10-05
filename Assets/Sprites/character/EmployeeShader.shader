Shader "Sprites/character/EmployeeShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_HairGreyColor("Default Hair Color", Color) = (150, 150, 150, 1)
		_HairColor("Hair Color", Color) = (1, 1, 1, 1)
		_SkinGreyColor("Default Skin Color", Color) = (210, 210, 210, 1)
		_SkinColor("Skin Color", Color) = (1, 1, 1, 1)
		_ShirtGreyColor("Default Shirt Color", Color) = (113, 113, 113, 1)
		_ShirtColor("Shirt Color", Color) = (1, 1, 1, 1)
		_ShortsGreyColor("Default Shorts Color", Color) = (64, 64, 64, 1)
		_ShortsColor("Shorts Color", Color) = (1, 1, 1, 1)
		_ShoeGreyColor("Default Shoe Color", Color) = (45, 45, 45, 1)
		_ShoeColor("Shoe Color", Color) = (1, 1, 1, 1)
		_EyeGreyColor("Default Eye Color", Color) = (124, 124, 124, 1)
		_EyeColor("Eye Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }
		Cull Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			//Get the Global Variables from above into SubShader
			fixed4 _HairGreyColor;
			fixed4 _HairColor;
			fixed4 _SkinGreyColor;
			fixed4 _SkinColor;
			fixed4 _ShirtGreyColor;
			fixed4 _ShirtColor;
			fixed4 _ShortsGreyColor;
			fixed4 _ShortsColor;
			fixed4 _ShoeGreyColor;
			fixed4 _ShoeColor;
			fixed4 _EyeGreyColor;
			fixed4 _EyeColor;
			float4 _MainTex_TexelSize;
			

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb *= col.a;
				half4 oldHairColor = _HairGreyColor;
				half4 newHairColor = _HairColor;
				half4 oldSkinColor = _SkinGreyColor;
				half4 newSkinColor = _SkinColor;
				half4 oldShirtColor = _ShirtGreyColor;
				half4 newShirtColor = _ShirtColor;
				half4 oldShortsColor = _ShortsGreyColor;
				half4 newShortsColor = _ShortsColor;
				half4 oldShoeColor = _ShoeGreyColor;
				half4 newShoeColor = _ShoeColor;
				half4 oldEyeColor = _EyeGreyColor;
				half4 newEyeColor = _EyeColor;

				//Check if the Color, does match with a color, that needs to be modified

				if (col.r == oldHairColor.r && col.b == oldHairColor.b && col.g == oldHairColor.g && col.a == oldHairColor.a) {
					col.rgba = newHairColor;
				}
				else if (col.r == oldSkinColor.r && col.b == oldSkinColor.b && col.g == oldSkinColor.g && col.a == oldSkinColor.a) {
					col.rgba = newSkinColor;
				}
				else if (col.r == oldShirtColor.r && col.b == oldShirtColor.b && col.g == oldShirtColor.g && col.a == oldShirtColor.a) {
					col.rgba = newShirtColor;
				}
				else if (col.r == oldShortsColor.r && col.b == oldShortsColor.b && col.g == oldShortsColor.g && col.a == oldShortsColor.a) {
					col.rgba = newShortsColor;
				}
				else if (col.r == oldShoeColor.r && col.b == oldShoeColor.b && col.g == oldShoeColor.g && col.a == oldShoeColor.a) {
					col.rgba = newShoeColor;
				}
				else if (col.r == oldEyeColor.r && col.b == oldEyeColor.b && col.g == oldEyeColor.g && col.a == oldEyeColor.a) {
					col.rgba = newEyeColor;
				}
				//return the new Color
				return col;
			}
			ENDCG
		}
	}
}
