Shader "Custom/BlueGalaxy_Interactive"
{
    Properties
    {
        _DeepBlue ("Deep Blue", Color) = (0.02, 0.05, 0.12, 1)
        _MidBlue ("Mid Blue", Color) = (0.08, 0.15, 0.3, 1)
        _ElectricBlue ("Electric Blue", Color) = (0.1, 0.4, 0.9, 1)
        _IceBlue ("Ice Blue", Color) = (0.4, 0.7, 1, 1)
        _StarColor ("Star Color", Color) = (0.8, 0.9, 1, 1)
        _Intensity ("Glow Intensity", Range(0,10)) = 3
        _Speed ("Galaxy Speed", Range(0,1)) = 0.4
        _Density ("Nebula Density", Range(0,3)) = 1.5
        _StarDensity ("Star Density", Range(0,8)) = 2
        _Pulse ("Interaction Pulse", Range(0,4)) = 0
        _SpiralStrength ("Spiral Strength", Range(0,1)) = 0.6
        _BlueSaturation ("Blue Saturation", Range(0,2)) = 1.3
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _DeepBlue;
                float4 _MidBlue;
                float4 _ElectricBlue;
                float4 _IceBlue;
                float4 _StarColor;
                float _Intensity;
                float _Speed;
                float _Density;
                float _StarDensity;
                float _Pulse;
                float _SpiralStrength;
                float _BlueSaturation;
            CBUFFER_END

            // Funciones de ruido
            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float a = rand(i);
                float b = rand(i + float2(1.0, 0.0));
                float c = rand(i + float2(0.0, 1.0));
                float d = rand(i + float2(1.0, 1.0));
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            float fbm(float2 p)
            {
                float value = 0.0;
                float amplitude = 0.5;
                float frequency = 1.0;
                
                for (int i = 0; i < 5; i++)
                {
                    value += amplitude * noise(p * frequency);
                    frequency *= 2.0;
                    amplitude *= 0.5;
                }
                return value;
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv = (IN.uv - 0.5) * 2.0;
                float t = _Time.y * _Speed;
                
                // Coordenadas polares
                float radius = length(uv);
                float angle = atan2(uv.y, uv.x);
                
                // Efectos espirales azules
                float spiral1 = sin(angle * 3.0 + radius * 8.0 - t * 2.0) * _SpiralStrength;
                float spiral2 = cos(angle * 2.5 + radius * 6.0 + t * 1.5) * _SpiralStrength * 0.7;
                float spiral = (spiral1 + spiral2) * 0.5;
                
                // Múltiples capas de nebulosa azul
                float2 nebulaUV1 = uv * _Density + float2(t * 0.2, -t * 0.15);
                float2 nebulaUV2 = uv * (_Density * 1.8) + float2(-t * 0.1, t * 0.25);
                float2 nebulaUV3 = uv * (_Density * 3.0) + float2(t * 0.3, t * 0.15);
                
                float nebula1 = fbm(nebulaUV1);
                float nebula2 = fbm(nebulaUV2);
                float nebula3 = fbm(nebulaUV3);
                
                float nebula = (nebula1 * 0.5 + nebula2 * 0.3 + nebula3 * 0.2);
                nebula *= (1.0 - radius * 0.6);
                nebula += spiral * 0.4;
                
                // Sistema de estrellas azules
                float2 starUV = uv * _StarDensity;
                float baseStars = pow(noise(starUV * 30.0), 15.0) * _Intensity;
                float twinklingStars = pow(noise(starUV * 25.0 + float2(t * 4.0, t * 3.0)), 18.0) * _Intensity * 1.2;
                
                // Estrellas azules eléctricas en los bordes
                float electricStars = pow(noise(starUV * 20.0 + float2(t * 5.0, -t * 4.0)), 12.0) * _Intensity * 2.0;
                electricStars *= pow(radius, 1.5); // Más en los bordes exteriores
                
                float stars = (baseStars + twinklingStars + electricStars) * pow(1.0 - radius, 0.8);
                
                // Núcleo galáctico azul brillante
                float core = pow(1.0 - saturate(radius * 1.2), 4.0) * _Intensity * 2.0;
                
                // Anillos azules concéntricos
                float rings = sin(radius * 12.0 - t * 3.0) * 0.3;
                rings *= pow(1.0 - abs(radius - 0.5), 2.0);
                
                // Efectos de interacción - puro azul eléctrico
                float pulseEffect = (1.0 + _Pulse * 2.0);
                float electricPulse = abs(sin(angle * 8.0 + t * 12.0 + radius * 10.0)) * 0.4 * _Pulse;
                electricPulse += abs(cos(angle * 6.0 - t * 8.0 + radius * 8.0)) * 0.3 * _Pulse;
                
                // Mezcla de colores - SOLO AZULES
                float3 deepNebula = _DeepBlue.rgb * nebula1;
                float3 midNebula = _MidBlue.rgb * nebula2;
                float3 spiralBlue = _ElectricBlue.rgb * spiral * 0.5;
                float3 starGlow = _StarColor.rgb * stars * 1.5;
                float3 coreGlow = _IceBlue.rgb * core;
                float3 ringGlow = _ElectricBlue.rgb * rings * 0.4;
                float3 pulseGlow = _ElectricBlue.rgb * electricPulse * pulseEffect;
                
                // Combinación final - enfatizar canales azules
                float3 finalColor = deepNebula + midNebula + spiralBlue + starGlow + coreGlow + ringGlow + pulseGlow;
                
                // Aumentar saturación de azul
                finalColor.b *= _BlueSaturation;
                finalColor = lerp(finalColor, float3(0, 0, finalColor.b * 1.2), 0.3);
                
                // Alpha - mantener transparencia
                float alpha = saturate(
                    nebula * 0.5 + 
                    stars * 0.8 + 
                    core * 0.7 + 
                    rings * 0.4 + 
                    electricPulse * 1.0
                );
                alpha *= pow(1.0 - saturate(radius - 0.4), 0.6);
                alpha = clamp(alpha, 0, 0.9);
                
                return float4(finalColor, alpha);
            }
            ENDHLSL
        }
    }
}