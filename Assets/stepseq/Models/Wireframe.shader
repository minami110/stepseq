Shader "Hidden/Edanoue/Debug/Wireframe"
{
    // Created: 23-07-03
    // Author: Minami Tomonobu (Edanoue, Inc.)
    // Edge のみ含まれる Mesh のレンダリングに使用する Shader
    // Supported Features:
    // - VR (Multi-pass)
    
    Properties
    {
        [MainColor] _Color("Color", Color) = (0, 1, 0, 1)
        
        [Enum(Off, 0, On, 1)] _ZWrite("ZWrite", Float) = 1 // On
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4 // LEqual
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "UniversalMaterialType" = "Unlit"
            "RenderType"="Opaque"
        }
        LOD 100

        // Render State
        ZWrite [_ZWrite]
        ZTest [_ZTest]
        Cull [_Cull]

        Pass
        {
            Name "Unlit"

            HLSLPROGRAM
            
            // Pragmas
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma vertex vert
            #pragma fragment frag

            // Includes
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Properties
            CBUFFER_START(UnityPerMaterial)
            half4 _Color;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                // For VR
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                // Outputs
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                return output;
            }

            void frag(
                Varyings input, out half4 outColor : SV_Target0
            )
            {
                // Outputs
                outColor = _Color;
                outColor.a = 1.0f;
            }
            ENDHLSL
        }
    }

    Fallback "Hidden/Universal Render Pipeline/FallbackError"
}