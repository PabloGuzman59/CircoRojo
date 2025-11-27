Shader "Custom/TubularGalaxyElectric"
{
    Properties
    {
        _NebulaColor1 ("Nebula Color 1", Color) = (0.3, 0.1, 0.8, 1)
        _NebulaColor2 ("Nebula Color 2", Color) = (0.8, 0.2, 0.4, 1)
        _StarColor ("Star Color", Color) = (1, 1, 0.8, 1)
        _Intensity ("Glow Intensity", Range(0,10)) = 3
        _Speed ("Flow Speed", Range(0,2)) = 0.5
        _Density ("Nebula Density", Range(0,3)) = 1.5
        _StarDensity ("Star Density", Range(0,5)) = 2
        _Pulse ("Interaction Pulse", Range(0,3)) = 0
        _FlowStrength ("Flow Strength", Range(0,2)) = 0.8
        _TubeLength ("Tube Length Scale", Range(0.1,5)) = 1.0
        _TubeRadius ("Tube Radius Scale", Range(0.1,3)) = 1.0
        _RadialTiling ("Radial Tiling", Range(1,10)) = 3
        _LongitudinalTiling ("Longitudinal Tiling", Range(1,10)) = 2
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
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float2 uv : TEXCOORD2;
                float3 localPos : TEXCOORD3;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _NebulaColor1;
                float4 _NebulaColor2;
                float4 _StarColor;
                float _Intensity;
                float _Speed;
                float _Density;
                float _StarDensity;
                float _Pulse;
                float _FlowStrength;
                float _TubeLength;
                float _TubeRadius;
                float _RadialTiling;
                float _LongitudinalTiling;
            CBUFFER_END

            // Funciones de ruido para efectos galácticos
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
                
                for (int i = 0; i < 4; i++)
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
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.worldNormal = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv;
                OUT.localPos = IN.positionOS.xyz;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                // Coordenadas cilíndricas para el tubo
                float3 localPos = IN.localPos;
                
                // Calcular coordenadas longitudinales (a lo largo del tubo) y radiales
                float longitudinal = localPos.z * _TubeLength * _LongitudinalTiling;
                float radius = length(localPos.xy) * _TubeRadius;
                
                // Coordenadas angulares para el efecto envolvente
                float angle = atan2(localPos.y, localPos.x);
                float radialUV = angle * _RadialTiling;
                
                // UVs para el mapeo tubular
                float2 tubeUV = float2(longitudinal, radialUV);
                float t = _Time.y * _Speed;
                
                // Efecto de flujo longitudinal (como energía fluyendo por el tubo)
                float flow = sin(longitudinal * 2.0 - t * 3.0) * _FlowStrength;
                flow += sin(longitudinal * 4.0 + t * 2.0) * _FlowStrength * 0.5;
                
                // Nebula base adaptada al tubo
                float2 nebulaUV = tubeUV * _Density + float2(t * 0.3, -t * 0.1);
                float nebula1 = fbm(nebulaUV);
                float nebula2 = fbm(nebulaUV * 1.5 + float2(t * 0.4, 0.0));
                float nebula3 = fbm(nebulaUV * 2.5 + float2(0.0, t * 0.3));
                
                // Nebula con variación radial
                float nebula = (nebula1 * 0.5 + nebula2 * 0.3 + nebula3 * 0.2);
                nebula *= (1.0 - radius * 0.3); // Más intenso en el centro
                nebula += flow * 0.4;
                
                // Patrón espiral adaptado al tubo
                float spiral = sin(angle * 6.0 + longitudinal * 3.0 - t * 2.0) * _FlowStrength;
                nebula += spiral * 0.3;
                
                // Estrellas adaptadas a la superficie tubular
                float2 starUV = tubeUV * _StarDensity;
                float stars = pow(noise(starUV * 15.0), 6.0) * _Intensity;
                float movingStars = pow(noise(starUV * 12.0 + float2(t * 2.0, t * 1.5)), 8.0) * _Intensity * 1.5;
                
                // Efecto de núcleo/centro del tubo
                float core = pow(1.0 - saturate(radius * 2.0), 3.0) * _Intensity;
                
                // Efectos de pulso/interacción
                float pulseEffect = (1.0 + _Pulse * 2.0);
                float electricPulse = sin(angle * 8.0 + longitudinal * 5.0 + t * 8.0) * 0.4 * _Pulse;
                
                // Efecto de "anillos" de energía a lo largo del tubo
                float energyRings = sin(longitudinal * 8.0 + t * 4.0) * 0.3 * _Pulse;
                
                // Mezcla de colores
                float3 nebulaColor = lerp(_NebulaColor1.rgb, _NebulaColor2.rgb, nebula2);
                float3 starColor = _StarColor.rgb * (stars + movingStars) * pulseEffect;
                float3 coreColor = _StarColor.rgb * core * 1.5;
                float3 electricColor = float3(0.4, 0.9, 2.0) * (electricPulse + energyRings);
                
                // Color final
                float3 finalColor = nebulaColor + starColor + coreColor + electricColor;
                finalColor *= _Intensity * 0.4;
                
                // Alpha - control de transparencia
                float alpha = saturate(nebula * 0.6 + (stars + movingStars) * 0.3 + core * 0.5 + (electricPulse + energyRings) * 0.3);
                
                // Suavizado en los bordes del tubo (opcional)
                alpha *= (1.0 - smoothstep(0.7, 1.0, radius));
                
                return float4(finalColor, alpha);
            }
            ENDHLSL
        }
    }
}