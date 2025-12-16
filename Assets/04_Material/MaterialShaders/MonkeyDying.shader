Shader "Custom/BurnShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _BurnTex ("Burn Texture", 2D) = "white" {}
        _BurnColor ("Burn Color", Color) = (1, 0.5, 0, 1)
        _BurnProgress ("Burn Progress", Range(0, 1)) = 0
        _BurnWidth ("Burn Width", Range(0, 0.2)) = 0.05
        _NoiseScale ("Noise Scale", Float) = 1
        _EdgeGlow ("Edge Glow", Float) = 2
        _AshColor ("Ash Color", Color) = (0.2, 0.2, 0.2, 1)
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
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
                float2 burnUV : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float3 normal : TEXCOORD3;
                UNITY_FOG_COORDS(4)
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BurnTex;
            float4 _BurnTex_ST;
            float4 _BurnColor;
            float _BurnProgress;
            float _BurnWidth;
            float _NoiseScale;
            float _EdgeGlow;
            float4 _AshColor;
            
            // Función de ruido simple
            float noise(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.burnUV = TRANSFORM_TEX(v.uv, _BurnTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Textura principal
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Ruido para efecto orgánico
                float noiseVal = noise(i.burnUV * _NoiseScale + _Time.y * 0.5);
                float noiseVal2 = noise(i.burnUV * _NoiseScale * 2 - _Time.y * 0.3);
                
                // Progreso de quema con ruido
                float burnProgress = _BurnProgress + noiseVal * 0.1 + noiseVal2 * 0.05;
                
                // Textura de patrón de quemado
                float burnPattern = tex2D(_BurnTex, i.burnUV).r;
                
                // Línea de quema principal
                float burnLine = burnPattern - burnProgress;
                
                // Zona quemada (ya desaparecida)
                if (burnLine < -_BurnWidth)
                {
                    discard;
                }
                
                // Zona de borde en llamas
                if (burnLine < 0)
                {
                    float edgeFactor = 1 - saturate(abs(burnLine) / _BurnWidth);
                    float glow = edgeFactor * _EdgeGlow;
                    
                    // Color del borde quemándose
                    col.rgb = lerp(col.rgb, _BurnColor.rgb, edgeFactor);
                    col.rgb += _BurnColor.rgb * glow;
                    col.a *= edgeFactor;
                }
                // Zona carbonizada (próxima a quemarse)
                else if (burnLine < _BurnWidth * 2)
                {
                    float ashFactor = saturate(burnLine / (_BurnWidth * 2));
                    col.rgb = lerp(_AshColor.rgb, col.rgb, ashFactor);
                    col.rgb *= 0.7; // Oscurecer
                }
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}