#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

// #if defined(SHADERGRAPH_PREVIEW)
// #else
// #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
// #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
// #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
// #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
// #pragma multi_compile_fragment _ _SHADOWS_SOFT
// #pragma multi_compile _ SHADOWS_SHADOWMASK
// #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
// #pragma multi_compile _ LIGHTMAP_ON
// #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
// #endif

void MainLight_float(float3 WorldPos, out half3 Direction, out half3 Color, out half DistanceAtten, out half ShadowAtten)
{
#if defined(SHADERGRAPH_PREVIEW)
   Direction = half3(0.5, 0.5, 0);
   Color = 1;
   DistanceAtten = 1;
   ShadowAtten = 1;
#else
#if defined(SHADOWS_SCREEN)
   half4 clipPos = TransformWorldToHClip(WorldPos);
   half4 shadowCoord = ComputeScreenPos(clipPos);
#else
   half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
#endif
   Light mainLight = GetMainLight(shadowCoord);
   Direction = mainLight.direction;
   Color = mainLight.color;
   DistanceAtten = mainLight.distanceAttenuation;
   ShadowAtten = mainLight.shadowAttenuation;
#endif
}

void AdditionalLight_float(float3 WorldPos, int Index, out float3 Direction, 
    out float3 Color, out float DistanceAtten, out float ShadowAtten) 
{
    Direction = normalize(float3(0.5f,0.5f,0.25f));
    Color = float3(0.0f, 0.0f, 0.0f);
    DistanceAtten = 0.0f;
    ShadowAtten = 0.0f;

#ifndef SHADERGRAPH_PREVIEW
   int pixelLightCount = GetAdditionalLightsCount();
   if(Index < pixelLightCount)
    {
        Light light = GetAdditionalLight(Index, WorldPos);

        Direction = light.direction;
        Color = light.color;
        DistanceAtten = light.distanceAttenuation;
        ShadowAtten = light.shadowAttenuation;
    }
#endif
}

#endif