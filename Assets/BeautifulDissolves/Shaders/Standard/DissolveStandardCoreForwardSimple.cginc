#ifndef DISSOLVE_STANDARD_CORE_FORWARD_SIMPLE_INCLUDED
#define DISSOLVE_STANDARD_CORE_FORWARD_SIMPLE_INCLUDED

#include "UnityStandardCoreForwardSimple.cginc"
#include "DissolveCoreFunctions.cginc"

half4 fragForwardBaseSimpleInternalDissolve (VertexOutputBaseSimple i)
{
    UNITY_APPLY_DITHER_CROSSFADE(i.pos.xy);

    FragmentCommonData s = FragmentSetupSimple(i);

	#if SHADER_TARGET >= 30
		s.diffColor = Dissolve(s.diffColor, i.tex.xy, s.oneMinusReflectivity);
	#endif

    UnityLight mainLight = MainLightSimple(i, s);

    #if !defined(LIGHTMAP_ON) && defined(_NORMALMAP)
    half ndotl = saturate(dot(s.tangentSpaceNormal, i.tangentSpaceLightDir));
    #else
    half ndotl = saturate(dot(s.normalWorld, mainLight.dir));
    #endif

    //we can't have worldpos here (not enough interpolator on SM 2.0) so no shadow fade in that case.
    half shadowMaskAttenuation = UnitySampleBakedOcclusion(i.ambientOrLightmapUV, 0);
    half realtimeShadowAttenuation = SHADOW_ATTENUATION(i);
    half atten = UnityMixRealtimeAndBakedShadows(realtimeShadowAttenuation, shadowMaskAttenuation, 0);

    half occlusion = Occlusion(i.tex.xy);
    half rl = dot(REFLECTVEC_FOR_SPECULAR(i, s), LightDirForSpecular(i, mainLight));

    UnityGI gi = FragmentGI (s, occlusion, i.ambientOrLightmapUV, atten, mainLight);
    half3 attenuatedLightColor = gi.light.color * ndotl;

    half3 c = BRDF3_Indirect(s.diffColor, s.specColor, gi.indirect, PerVertexGrazingTerm(i, s), PerVertexFresnelTerm(i));
    c += BRDF3DirectSimple(s.diffColor, s.specColor, s.smoothness, rl) * attenuatedLightColor;

	half3 emis = Emission(i.tex.xy);
	#if SHADER_TARGET >= 30
		emis += DissolveEmission(i.tex.xy);
	#endif

	c += emis;

    UNITY_APPLY_FOG(i.fogCoord, c);

    return OutputForward (half4(c, 1), s.alpha);
}

half4 fragForwardBaseSimpleDissolve (VertexOutputBaseSimple i) : SV_Target	// backward compatibility (this used to be the fragment entry function)
{
	return fragForwardBaseSimpleInternalDissolve(i);
}

half4 fragForwardAddSimpleInternalDissolve (VertexOutputForwardAddSimple i)
{
    UNITY_APPLY_DITHER_CROSSFADE(i.pos.xy);

    FragmentCommonData s = FragmentSetupSimpleAdd(i);

    #if SHADER_TARGET >= 30
		s.diffColor = Dissolve(s.diffColor, i.tex.xy, s.oneMinusReflectivity);
	#endif

    half3 c = BRDF3DirectSimple(s.diffColor, s.specColor, s.smoothness, dot(REFLECTVEC_FOR_SPECULAR(i, s), i.lightDir));

    #if SPECULAR_HIGHLIGHTS // else diffColor has premultiplied light color
        c *= _LightColor0.rgb;
    #endif

    c *= UNITY_SHADOW_ATTENUATION(i, s.posWorld) * saturate(dot(LightSpaceNormal(i, s), i.lightDir));

    UNITY_APPLY_FOG_COLOR(i.fogCoord, c.rgb, half4(0,0,0,0)); // fog towards black in additive pass
    return OutputForward (half4(c, 1), s.alpha);
}


#endif // DISSOLVE_STANDARD_CORE_FORWARD_SIMPLE_INCLUDED
