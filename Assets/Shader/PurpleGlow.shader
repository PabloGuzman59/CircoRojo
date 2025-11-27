Shader "Custom/PurpleGlow"
{
    Properties
    {
        _EmissionStrength("Emission Strength", Range(0, 10)) = 6
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings { float4 positionHCS : SV_POSITION; };

            float _EmissionStrength;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            float4 frag (Varyings IN) : SV_Target
            {
                float3 purple = float3(0.6, 0, 1) * _EmissionStrength;
                return float4(purple, 1);
            }
            ENDHLSL
        }
    }
}
