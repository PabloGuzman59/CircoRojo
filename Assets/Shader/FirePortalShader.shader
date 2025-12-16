Shader "Custom/FirePortalShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        // Colores del fuego
        _CoreColor ("Core Color (Hottest)", Color) = (1.0, 1.0, 0.9, 1.0)
        _InnerColor ("Inner Flame Color", Color) = (1.0, 0.8, 0.2, 1.0)
        _MidColor ("Mid Flame Color", Color) = (1.0, 0.4, 0.0, 1.0)
        _OuterColor ("Outer Flame Color", Color) = (1.0, 0.15, 0.0, 1.0)
        _EdgeColor ("Edge Glow Color", Color) = (0.8, 0.1, 0.0, 1.0)
        
        // Control del fuego
        _FlameSpeed ("Flame Speed", Range(0.5, 5.0)) = 2.0
        _FlameHeight ("Flame Height", Range(0.0, 2.0)) = 1.0
        _FlameIntensity ("Flame Intensity", Range(0.5, 5.0)) = 2.0
        _FlameDistortion ("Flame Distortion", Range(0.0, 2.0)) = 1.0
        _NoiseScale ("Noise Scale", Range(1.0, 20.0)) = 8.0
        _Turbulence ("Turbulence", Range(0.0, 3.0)) = 1.5
        
        // Efectos visuales
        _GlowIntensity ("Glow Intensity", Range(0.0, 10.0)) = 5.0
        _EmberDensity ("Ember Density", Range(0.0, 50.0)) = 20.0
        _EmberSpeed ("Ember Speed", Range(0.5, 5.0)) = 2.0
        _InnerRadius ("Dark Core Radius", Range(0.0, 0.5)) = 0.1
        _RimPower ("Rim Power", Range(1.0, 10.0)) = 3.0
        _Brightness ("Overall Brightness", Range(0.5, 5.0)) = 2.5
        
        // Pulsación
        _PulseSpeed ("Pulse Speed", Range(0.0, 5.0)) = 1.5
        _PulseAmount ("Pulse Amount", Range(0.0, 1.0)) = 0.3
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
            float4 _CoreColor;
            float4 _InnerColor;
            float4 _MidColor;
            float4 _OuterColor;
            float4 _EdgeColor;
            float _FlameSpeed;
            float _FlameHeight;
            float _FlameIntensity;
            float _FlameDistortion;
            float _NoiseScale;
            float _Turbulence;
            float _GlowIntensity;
            float _EmberDensity;
            float _EmberSpeed;
            float _InnerRadius;
            float _RimPower;
            float _Brightness;
            float _PulseSpeed;
            float _PulseAmount;
            
            // Hash para ruido
            float hash(float2 p)
            {
                float3 p3 = frac(float3(p.xyx) * 0.13);
                p3 += dot(p3, p3.yzx + 3.333);
                return frac((p3.x + p3.y) * p3.z);
            }
            
            float hash13(float3 p3)
            {
                p3 = frac(p3 * 0.1031);
                p3 += dot(p3, p3.yzx + 19.19);
                return frac((p3.x + p3.y) * p3.z);
            }
            
            // Ruido de valor 2D mejorado
            float noise(float2 x)
            {
                float2 i = floor(x);
                float2 f = frac(x);
                
                // Interpolación suave
                f = f * f * f * (f * (f * 6.0 - 15.0) + 10.0);
                
                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));
                
                return lerp(lerp(a, b, f.x), lerp(c, d, f.x), f.y);
            }
            
            // FBM (Fractal Brownian Motion) para fuego
            float fbm(float2 p)
            {
                float value = 0.0;
                float amplitude = 0.5;
                float frequency = 1.0;
                
                for(int i = 0; i < 6; i++)
                {
                    value += amplitude * noise(p * frequency);
                    frequency *= 2.07;
                    amplitude *= 0.5;
                }
                
                return value;
            }
            
            // Ruido turbulento para llamas
            float turbulentNoise(float2 p)
            {
                return abs(fbm(p) - 0.5) * 2.0;
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
                float time = _Time.y * _FlameSpeed;
                
                // Coordenadas centradas
                float2 uv = i.uv - 0.5;
                float dist = length(uv);
                float angle = atan2(uv.y, uv.x);
                
                // Pulsación del fuego
                float pulse = sin(time * _PulseSpeed) * _PulseAmount + (1.0 - _PulseAmount);
                
                // Múltiples capas de ruido para simular llamas
                float2 flameUV = uv * _NoiseScale;
                
                // Capa 1: Movimiento principal de llamas
                float2 noiseUV1 = flameUV + float2(time * 0.3, -time * 0.8);
                float flame1 = fbm(noiseUV1) * _FlameHeight;
                
                // Capa 2: Turbulencia secundaria
                float2 noiseUV2 = flameUV * 1.7 + float2(-time * 0.4, -time * 1.2);
                float flame2 = turbulentNoise(noiseUV2) * _FlameHeight;
                
                // Capa 3: Detalles finos
                float2 noiseUV3 = flameUV * 3.5 + float2(time * 0.2, -time * 1.5);
                float flame3 = noise(noiseUV3) * _FlameHeight * 0.5;
                
                // Combinar capas de llamas
                float flamePattern = (flame1 + flame2 * 0.5 + flame3 * 0.3) * _Turbulence;
                
                // Distorsión radial del fuego
                float2 distortion = float2(
                    sin(angle * 4.0 + time * 2.0 + flamePattern * 5.0) * _FlameDistortion,
                    cos(angle * 6.0 - time * 1.5 + flamePattern * 4.0) * _FlameDistortion
                ) * (1.0 - dist * 0.5) * 0.1;
                
                float2 distortedUV = uv + distortion;
                float distortedDist = length(distortedUV);
                
                // Crear efecto de llamas que suben
                float flameShape = flamePattern - distortedDist * 2.0;
                flameShape = smoothstep(-0.5, 1.5, flameShape);
                
                // Gradiente de temperatura del fuego
                float tempGradient = 1.0 - smoothstep(0.0, 1.0, distortedDist);
                tempGradient *= flameShape;
                tempGradient *= pulse;
                
                // Calcular color basado en temperatura
                float4 fireColor;
                
                // Centro super caliente (blanco-amarillo)
                float coreAmount = smoothstep(0.3, 0.0, distortedDist) * flameShape;
                fireColor = _CoreColor * coreAmount;
                
                // Llama interna (amarillo-naranja)
                float innerAmount = smoothstep(0.6, 0.1, distortedDist) * flameShape * (1.0 - coreAmount);
                fireColor += _InnerColor * innerAmount;
                
                // Llama media (naranja)
                float midAmount = smoothstep(0.8, 0.3, distortedDist) * flameShape * (1.0 - innerAmount - coreAmount);
                fireColor += _MidColor * midAmount;
                
                // Llama exterior (rojo-naranja)
                float outerAmount = smoothstep(1.0, 0.4, distortedDist) * flameShape;
                fireColor += _OuterColor * outerAmount;
                
                // Agregar detalles de fuego con ruido
                fireColor.rgb += flamePattern * 0.3 * _InnerColor.rgb;
                
                // Borde brillante
                float edge = 1.0 - smoothstep(0.7, 1.0, distortedDist);
                edge *= pow(flameShape, 2.0);
                fireColor.rgb += edge * _EdgeColor.rgb * _GlowIntensity;
                
                // Efecto de "brasas" flotantes
                float embers = 0.0;
                for(int j = 0; j < 10; j++)
                {
                    float emberTime = time * _EmberSpeed + j * 30.0;
                    float2 emberPos = float2(
                        hash(float2(j * 10.0, floor(emberTime * 0.5))) * 2.0 - 1.0,
                        frac(emberTime * 0.3) * 2.0 - 1.0
                    );
                    
                    emberPos.x += sin(emberTime + j) * 0.3;
                    
                    float emberSize = 0.015 + hash(float2(j, 50)) * 0.025;
                    float d = length(uv - emberPos * 0.45);
                    float ember = exp(-d / emberSize) * 0.8;
                    
                    // Color de brasa basado en posición
                    float emberHeat = 1.0 - (emberPos.y + 1.0) * 0.5;
                    ember *= emberHeat;
                    
                    embers += ember;
                }
                
                fireColor.rgb += embers * _InnerColor.rgb * 1.5;
                
                // Chispas brillantes aleatorias
                float2 sparkUV = distortedUV * _EmberDensity + time * 3.0;
                float sparkNoise = hash(floor(sparkUV) + floor(time * 5.0));
                float sparks = step(0.98, sparkNoise) * (sin(time * 20.0 + sparkNoise * 100.0) * 0.5 + 0.5);
                fireColor.rgb += sparks * _CoreColor.rgb * 3.0;
                
                // Anillos de calor
                float heatRings = sin(distortedDist * 20.0 - time * 4.0 + flamePattern * 5.0) * 0.5 + 0.5;
                heatRings = pow(heatRings, 4.0) * flameShape;
                fireColor.rgb += heatRings * _InnerColor.rgb * 0.5;
                
                // Centro oscuro (núcleo del portal)
                float darkCore = smoothstep(_InnerRadius * 1.5, _InnerRadius * 0.3, distortedDist);
                fireColor.rgb *= darkCore * 0.2 + 0.8;
                
                // Rim lighting para profundidad
                float rim = 1.0 - saturate(dot(i.viewDir, i.worldNormal));
                rim = pow(rim, _RimPower);
                fireColor.rgb += rim * _EdgeColor.rgb * _GlowIntensity * 0.5;
                
                // Intensidad del fuego
                fireColor.rgb *= _FlameIntensity * pulse;
                
                // Brillo general
                fireColor.rgb *= _Brightness;
                
                // Alpha suave
                float alpha = smoothstep(1.2, 0.2, distortedDist);
                alpha *= flameShape;
                alpha *= (1.0 - smoothstep(_InnerRadius * 0.5, _InnerRadius * 1.2, distortedDist) * 0.6);
                alpha = saturate(alpha);
                
                fireColor.a = alpha;
                
                UNITY_APPLY_FOG(i.fogCoord, fireColor);
                return fireColor;
            }
            ENDCG
        }
    }
}