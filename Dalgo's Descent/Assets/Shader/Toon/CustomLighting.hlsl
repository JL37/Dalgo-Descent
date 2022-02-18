#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

#if defined(SHADERGRAPH_PREVIEW)
#else
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
#pragma multi_compile_fragment _ _SHADOWS_SOFT
#pragma multi_compile _ SHADOWS_SHADOWMASK
#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
#pragma multi_compile _ LIGHTMAP_ON
#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
#endif

void MainLight_float(float3 WorldPos, out half3 Direction, out half3 Color, 
   out half DistanceAtten, out half ShadowAtten)
{
#if defined(SHADERGRAPH_PREVIEW)
   Direction = half3(0.5f, 0.5f, 0.25f);
   Color = float3(1.0f,1.0f,1.0f);
   DistanceAtten = 1.0f;
   ShadowAtten = 1.0f;
#else
//#if defined(SHADOWS_SCREEN)
//    half4 clipPos = TransformWorldToHClip(WorldPos);
//    half4 shadowCoord = ComputeScreenPos(clipPos);
// #else
//    half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
// #endif

   //Tmp
   half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);

   Light mainLight = GetMainLight(shadowCoord);
   Direction = mainLight.direction;
   Color = mainLight.color;
   DistanceAtten = mainLight.distanceAttenuation;
   ShadowAtten = mainLight.shadowAttenuation;
#endif
}

float GetLightIntensity(float3 color) {
    return max(color.r, max(color.g, color.b));
}

void AdditionalLights_float(float3 WorldPos, float3 WorldNormal, float3 WorldView, float Smoothness,
   out float3 Diffuse, out float3 Specular) 
{
   float3 Direction = normalize(float3(0.5f,0.5f,0.25f));
   float3 Color = float3(0.0f, 0.0f, 0.0f);
   Diffuse = float3(0.0f, 0.0f, 0.0f);
   Specular = float3(0.0f, 0.0f, 0.0f);

#ifndef SHADERGRAPH_PREVIEW
   int pixelLightCount = GetAdditionalLightsCount();
   // if(Index < pixelLightCount)
   //  {
   //      Light light = GetAdditionalLight(Index, WorldPos);
   //      Direction = light.direction;
   //      Color = light.color;
   //      DistanceAtten = light.distanceAttenuation;
   //      ShadowAtten = light.shadowAttenuation;
   //  }
   
   //Smoothness = exp2(10 * Smoothness + 1);

   for(int i = 0; i < pixelLightCount; ++i)
   {
      Light light = GetAdditionalLight(i, WorldPos);

      Direction = light.direction;
      Color = light.color;

      //float NDotL = saturate(dot(WorldNormal, Direction));
      //Diffuse 
      //float atten = light.distanceAttenuation * light.shadowAttenuation * GetLightIntensity(light.color);
      //float thisDiffuse = NDotL * atten;

      half3 attenuatedLightColor = Color * (light.distanceAttenuation * light.shadowAttenuation);
      float thisDiffuse = LightingLambert(attenuatedLightColor, Direction, WorldNormal);

      //LightingSpecular(half3 lightColor, half3 lightDir, half3 normal, half3 viewDir, half4 specular, half smoothness)
      float thisSpecular = LightingSpecular(Color, Direction, WorldNormal, WorldView, 1, Smoothness);
      Diffuse += thisDiffuse;
      Specular += thisSpecular;
      Specular *= Diffuse; 
   }

#endif
}


#endif