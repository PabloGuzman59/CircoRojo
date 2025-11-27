Shader "Custom/RedGlow"
{
    Properties
    {
        _Color("Color", Color) = (1, 0, 0, 1)
        _EmissionStrength("Emission Strength", Range(0, 10)) = 5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            float4 _Color;
            float  _EmissionStrength;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float3 finalColor = _Color.rgb * _EmissionStrength;
                return float4(finalColor, 1);
            }
            ENDHLSL
        }
    }
}
