Shader "Sprites/character/EmployeeColorShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        _SwapTex("Color Data", 2D) = "transparent" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        
        // Outline
        _OutLineSpreadX ("Outline Spread", Range(0,0.03)) = 0.007
        _OutLineSpreadY ("Outline Spread", Range(0,0.03)) = 0.007
        [Toggle] _OutLineEnable("Enable Outline", Float) = 0

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
            Tags 
            { 
                "Queue" = "Transparent"
            }

            Cull Off
            ZWrite Off
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
                #pragma target 3.0
                #pragma surface surf Lambert alpha:fade vertex:vert
                #pragma multi_compile _ PIXELSNAP_ON
                #include "UnityCG.cginc"
                #include "UnityLightingCommon.cginc"

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 texcoord : TEXCOORD0;
                    float3 normal   : NORMAL;
                    float4 tangent  : TANGENT;
                };

                struct Input 
                {
                    float2 uv_MainTex;
                    float2 uv_BumpMap;
                    fixed4 color : COLOR;
                    float4 vertex   : SV_POSITION;
                    float2 texcoord  : TEXCOORD0;
                };

                fixed4 _Color;

                void vert(inout appdata_t IN, out Input OUT)
                {
                    UNITY_INITIALIZE_OUTPUT(Input, OUT);
                    OUT.vertex = UnityObjectToClipPos(IN.vertex);
                    OUT.texcoord = IN.texcoord;
                    OUT.color = IN.color * _Color;
    #ifdef PIXELSNAP_ON
                    OUT.vertex = UnityPixelSnap(OUT.vertex);
    #endif
                }

                sampler2D _MainTex;
                sampler2D _BumpMap;
                sampler2D _SwapTex;
                float _OutLineSpreadX;
                float _OutLineSpreadY;
                float _OutLineEnable;

                fixed4 SampleSpriteTexture(float2 uv)
                {
                    fixed4 color = tex2D(_MainTex, uv);
                    return color;
                }

                void surf (Input IN, inout SurfaceOutput o)
                {
                    fixed4 mainColor;
                    
                    if (_OutLineEnable > 0) {
                        fixed4 TempColor = tex2D(_MainTex, IN.texcoord+float2(_OutLineSpreadX,0.0)) + tex2D(_MainTex, IN.texcoord-float2(_OutLineSpreadX,0.0));
                        TempColor = TempColor + tex2D(_MainTex, IN.texcoord+float2(0.0,_OutLineSpreadY)) + tex2D(_MainTex, IN.texcoord-float2(0.0,_OutLineSpreadY));
                        if(TempColor.a == 1){
                            TempColor.a = 1;
                        }
                        fixed4 AlphaColor = (1,1,1,TempColor.a);
                        // This ensures that the outline is bright white
                        mainColor = AlphaColor * (3,3,3,3);
                        fixed4 addcolor = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
               
                        if(addcolor.a > 0.99){
                            mainColor = addcolor;
                        }
                    }
                    else {
                        mainColor = SampleSpriteTexture(IN.texcoord) * IN.color;
                    }
                    
                    fixed4 swapCol = tex2D(_SwapTex, float2(mainColor.r, 0));
                    fixed4 final = lerp(mainColor, swapCol, swapCol.a) * IN.color;
                    
                    o.Alpha = final.a;
                    o.Albedo = final.rgb * final.a;
                    o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
                }
            ENDCG
        }
}