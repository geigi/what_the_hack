// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/character/EmployeeLightingShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

        // required for UI.Mask
        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }
		Cull Off
		Blend One OneMinusSrcAlpha
        
        // required for UI.Mask
        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }
        ColorMask[_ColorMask]
        
		    CGPROGRAM
		    #pragma surface surf Lambert alpha:fade
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc" // for _LightColor0
            float4 _ClipRect;

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
                float4 worldPosition : TEXCOORD1; //we need to pass world pos to the fragment shader for clipping
                fixed4 diff : COLOR0; // diffuse lighting color
			};

            sampler2D _MainTex;

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
            
            fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 col = tex2D(_MainTex, uv);
                return col;
			}
              
            struct Input {
                float2 uv_MainTex;
            };
              
            void surf (Input IN, inout SurfaceOutput o) {      
                fixed4 c = SampleSpriteTexture (IN.uv_MainTex);
                o.Albedo = c.rgb * c.a; // vertex RGB
                o.Alpha = c.a;
            }
        ENDCG
    }
}
