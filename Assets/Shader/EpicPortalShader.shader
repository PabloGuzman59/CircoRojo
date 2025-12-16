Shader "Custom/EpicPortalShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _InnerColor ("Inner Color", Color) = (0.1, 0.0, 0.3, 1.0)
        _MidColor ("Mid Color", Color) = (0.5, 0.0, 1.0, 1.0)
        _OuterColor ("Outer Color", Color) = (0.3, 0.5, 1.0, 1.0)
        _EdgeColor ("Edge Glow Color", Color) = (0.8, 0.4, 1.0, 1.0)
        _ParticleColor ("Particle Color", Color) = (1.0, 0.8, 1.0, 1.0)
        
        _EnergySpeed ("Energy Speed", Range(0.5, 5.0)) = 2.0
        _DistortionIntensity ("Distortion Intensity", Range(0.0, 1.0)) = 0.3
        _VortexStrength ("Vortex Strength", Range(0.0, 10.0)) = 5.0
        _EdgeThickness ("Edge Thickness", Range(0.01, 0.3)) = 0.1
        _EdgeGlow ("Edge Glow", Range(0.0, 10.0)) = 5.0
        _ParticleDensity ("Particle Density", Range(1.0, 50.0)) = 20.0
        _ParticleSpeed ("Particle Speed", Range(0.5, 5.0)) = 2.0
        _InnerRadius ("Inner Radius", Range(0.0, 0.5)) = 0.15
        _NoiseScale ("Noise Scale", Range(1.0, 30.0)) = 10.0
        _Brightness ("Overall Brightness", Range(0.5, 3.0)) = 1.5
        _Turbulence ("Turbulence", Range(0.0, 2.0)) = 1.0
    }
    
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 100
        
        Blend SrcAlpha One
        ZWrite Off
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
                float3 worldNormal : TEXCOORD4;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _InnerColor;
            float4 _MidColor;
            float4 _OuterColor;
            float4 _EdgeColor;
            float4 _ParticleColor;
            float _EnergySpeed;
            float _DistortionIntensity;
            float _VortexStrength;
            float _EdgeThickness;
            float _EdgeGlow;
            float _ParticleDensity;
            float _ParticleSpeed;
            float _InnerRadius;
            float _NoiseScale;
            float _Brightness;
            float _Turbulence;
            
            // Hash para ruido
            float hash(float2 p)
            {
                float3 p3 = frac(float3(p.xyx) * 0.13);
                p3 += dot(p3, p3.yzx + 3.333);
                return frac((p3.x + p3.y) * p3.z);
            }
            
            // Ruido de valor 2D
            float noise(float2 x)
            {
                float2 i = floor(x);
                float2 f = frac(x);
                f = f * f * (3.0 - 2.0 * f);
                
                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));
                
                return lerp(lerp(a, b, f.x), lerp(c, d, f.x), f.y);
            }
            
            // FBM (Fractal Brownian Motion)
            float fbm(float2 p)
            {
                float value = 0.0;
                float amplitude = 0.5;
                float frequency = 1.0;
                
                for(int i = 0; i < 5; i++)
                {
                    value += amplitude * noise(p * frequency);
                    frequency *= 2.0;
                    amplitude *= 0.5;
                }
                
                return value;
            }
            
            // Rotación 2D
            float2 rotate(float2 p, float angle)
            {
                float c = cos(angle);
                float s = sin(angle);
                return float2(p.x * c - p.y * s, p.x * s + p.y * c);
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float time = _Time.y * _EnergySpeed;
                
                // Coordenadas centradas
                float2 uv = i.uv - 0.5;
                float dist = length(uv);
                float angle = atan2(uv.y, uv.x);
                
                // Efecto de vórtice
                float vortex = dist * _VortexStrength;
                float2 vortexUV = rotate(uv, vortex - time * 0.5);
                
                // Múltiples capas de ruido para energía turbulenta
                float2 noiseUV1 = vortexUV * _NoiseScale + time * 0.3;
                float2 noiseUV2 = vortexUV * _NoiseScale * 1.7 - time * 0.4;
                float2 noiseUV3 = vortexUV * _NoiseScale * 0.5 + time * 0.2;
                
                float noise1 = fbm(noiseUV1);
                float noise2 = fbm(noiseUV2);
                float noise3 = fbm(noiseUV3);
                
                // Combinar ruidos para turbulencia
                float turbulence = (noise1 + noise2 * 0.5 + noise3 * 0.3) * _Turbulence;
                
                // Distorsión dinámica
                float2 distortion = float2(
                    sin(angle * 3.0 + time + turbulence * 5.0),
                    cos(angle * 5.0 - time * 1.3 + turbulence * 5.0)
                ) * _DistortionIntensity * (1.0 - dist);
                
                float2 finalUV = uv + distortion;
                float finalDist = length(finalUV);
                
                // Anillos de energía animados
                float rings = 0.0;
                for(int j = 0; j < 3; j++)
                {
                    float ringDist = finalDist * 20.0 - time * 2.0 + j * 2.0;
                    rings += exp(-10.0 * pow(frac(ringDist) - 0.5, 2.0)) * 0.3;
                }
                
                // Rayos radiales
                float rays = sin(angle * 12.0 + time * 2.0 + turbulence * 3.0) * 0.5 + 0.5;
                rays = pow(rays, 3.0) * (1.0 - smoothstep(0.0, 0.5, finalDist));
                
                // Gradiente de color basado en distancia
                float colorGrad = smoothstep(_InnerRadius, 1.0, finalDist);
                float4 baseColor = lerp(_InnerColor, _MidColor, smoothstep(_InnerRadius, 0.5, finalDist));
                baseColor = lerp(baseColor, _OuterColor, smoothstep(0.5, 0.9, finalDist));
                
                // Agregar energía de ruido al color
                baseColor.rgb += turbulence * 0.3 * _MidColor.rgb;
                baseColor.rgb += rings * _EdgeColor.rgb;
                baseColor.rgb += rays * 0.5;
                
                // Borde brillante con detección de edge
                float edgeDist = abs(finalDist - 0.5);
                float edge = exp(-edgeDist / _EdgeThickness) * _EdgeGlow;
                edge *= (noise1 * 0.5 + 0.5);
                baseColor.rgb += edge * _EdgeColor.rgb;
                
                // Sistema de partículas
                float particles = 0.0;
                for(int k = 0; k < 8; k++)
                {
                    float particleTime = time * _ParticleSpeed + k * 50.0;
                    float2 particlePos = float2(
                        hash(float2(k, particleTime * 0.1)) * 2.0 - 1.0,
                        hash(float2(k + 10.0, particleTime * 0.1)) * 2.0 - 1.0
                    );
                    
                    // Hacer que las partículas sigan el vórtice
                    float particleAngle = atan2(particlePos.y, particlePos.x);
                    float particleDist = length(particlePos);
                    particlePos = rotate(particlePos, particleDist * 3.0 - particleTime * 0.3);
                    
                    float particleSize = 0.02 + hash(float2(k, 100)) * 0.03;
                    float d = length(uv - particlePos * 0.4);
                    particles += exp(-d / particleSize) * 0.5;
                }
                
                baseColor.rgb += particles * _ParticleColor.rgb;
                
                // Efecto de "chispas" rápidas
                float sparks = 0.0;
                float2 sparkUV = finalUV * _ParticleDensity + time * 5.0;
                float sparkNoise = hash(floor(sparkUV));
                sparks = step(0.97, sparkNoise) * (sin(time * 10.0 + sparkNoise * 100.0) * 0.5 + 0.5);
                baseColor.rgb += sparks * _ParticleColor.rgb * 2.0;
                
                // Centro oscuro (el "agujero" del portal)
                float innerHole = smoothstep(_InnerRadius, _InnerRadius * 0.5, finalDist);
                baseColor.rgb *= innerHole * 0.3 + 0.7;
                
                // Brillo general
                baseColor.rgb *= _Brightness;
                
                // Alpha con degradado suave
                float alpha = smoothstep(1.0, 0.2, finalDist);
                alpha *= (1.0 - smoothstep(_InnerRadius * 0.3, _InnerRadius, finalDist) * 0.7);
                alpha *= (noise1 * 0.3 + 0.7);
                alpha = saturate(alpha);
                
                baseColor.a = alpha;
                
                UNITY_APPLY_FOG(i.fogCoord, baseColor);
                return baseColor;
            }
            ENDCG
        }
    }
}