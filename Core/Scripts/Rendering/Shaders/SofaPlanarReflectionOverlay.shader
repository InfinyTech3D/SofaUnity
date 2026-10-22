// Overlay pass blending the planar reflection (rendered by SofaPlanarReflection) over a lit surface.
// Used as a second material on the floor renderer: the Lit material keeps lighting and shadows,
// this pass adds the mirrored scene with fresnel, roughness blur (mip level) and normal map distortion.
Shader "SofaUnity/PlanarReflectionOverlay"
{
    Properties
    {
        _Tint("Tint", Color) = (1, 1, 1, 1)
        _ReflectionStrength("Reflection Strength (facing)", Range(0, 1)) = 0.15
        _FresnelStrength("Reflection Strength (grazing)", Range(0, 1)) = 0.6
        _FresnelPower("Fresnel Power", Range(1, 8)) = 4
        _Blur("Blur (roughness)", Range(0, 6)) = 1.5
        _BumpMap("Distortion Normal Map", 2D) = "bump" {}
        _Distortion("Distortion", Range(0, 0.05)) = 0.008
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent-10" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True" }

        Pass
        {
            Name "PlanarReflection"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_SofaPlanarReflectionTex);
            SAMPLER(sampler_SofaPlanarReflectionTex);
            TEXTURE2D(_BumpMap);
            SAMPLER(sampler_BumpMap);
            // global: 1 when the reflection texture is valid for the current camera, 0 otherwise
            half _SofaPlanarReflectionWeight;

            CBUFFER_START(UnityPerMaterial)
                float4 _BumpMap_ST;
                half4 _Tint;
                half _ReflectionStrength;
                half _FresnelStrength;
                half _FresnelPower;
                half _Blur;
                half _Distortion;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 screenPos : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
                float2 uv : TEXCOORD3;
                half fogFactor : TEXCOORD4;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                output.screenPos = ComputeScreenPos(output.positionCS);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.uv = TRANSFORM_TEX(input.uv, _BumpMap);
                output.fogFactor = ComputeFogFactor(output.positionCS.z);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half3 bump = UnpackNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, input.uv));
                float2 uv = input.screenPos.xy / input.screenPos.w + bump.xy * _Distortion;

                half3 reflection = SAMPLE_TEXTURE2D_LOD(_SofaPlanarReflectionTex, sampler_SofaPlanarReflectionTex, uv, _Blur).rgb * _Tint.rgb;

                half3 viewDir = GetWorldSpaceNormalizeViewDir(input.positionWS);
                half NdotV = saturate(dot(normalize(input.normalWS), viewDir));
                half fresnel = pow(1.0h - NdotV, _FresnelPower);
                half alpha = lerp(_ReflectionStrength, _FresnelStrength, fresnel) * _Tint.a * _SofaPlanarReflectionWeight;

                reflection = MixFog(reflection, input.fogFactor);
                return half4(reflection, alpha);
            }
            ENDHLSL
        }
    }
}
