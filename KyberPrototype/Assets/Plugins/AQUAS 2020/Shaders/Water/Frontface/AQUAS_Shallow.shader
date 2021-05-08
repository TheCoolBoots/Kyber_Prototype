Shader "AQUAS/Desktop/Front/Shallow"
{
	Properties
	{
		[NoScaleOffset][Header(Wave Options)]_NormalTexture("Normal Texture", 2D) = "bump" {}
		_NormalTiling("Normal Tiling", Range( 0.01 , 2)) = 1
		_NormalStrength("Normal Strength", Range( 0 , 2)) = 0
		_WaveSpeed("Wave Speed", Float) = 0
		_Refraction("Refraction", Range( 0 , 1)) = 0.1
		[Header(Color Options)]_DirtColor("Dirt Color", Color) = (0,0,0,0)
		_DirtDensity("Dirt Density", Range( 0 , 1)) = 0
		[Header(Lighting Options)]_Specular("Specular", Float) = 0
		_SpecularColor("Specular Color", Color) = (0,0,0,0)
		_Gloss("Gloss", Float) = 0
		_LightWrapping("Light Wrapping", Range( 0 , 2)) = 0
		[Header(Reflection Options)][Toggle]_EnableRealtimeReflections("Enable Realtime Reflections", Float) = 1
		_RealtimeReflectionIntensity("Realtime Reflection Intensity", Range( 0 , 1)) = 0
		[Toggle]_EnableProbeRelfections("Enable Probe Relfections", Float) = 1
		_ProbeReflectionIntensity("Probe Reflection Intensity", Range( 0 , 1)) = 0
		_Distortion("Distortion", Range( 0 , 1)) = 0
		[HideInInspector]_ReflectionTex("Reflection Tex", 2D) = "white" {}
		[Header(Distance Options)]_MediumTilingDistance("Medium Tiling Distance", Float) = 0
		_FarTilingDistance("Far Tiling Distance", Float) = 0
		_DistanceFade("Distance Fade", Float) = 0
		[Header(Shoreline Waves)]_ShorelineFrequency("Shoreline Frequency", Float) = 0
		_ShorelineSpeed("Shoreline Speed", Range( 0 , 0.2)) = 0
		_ShorelineNormalStrength("Shoreline Normal Strength", Range( 0 , 1)) = 0
		_ShorelineBlend("Shoreline Blend", Range( 0 , 1)) = 0
		[NoScaleOffset]_ShorelineMask("Shoreline Mask", 2D) = "white" {}
		[HideInInspector][NoScaleOffset]_RandomMask("Random Mask", 2D) = "white" {}
		[NoScaleOffset][Header(Flowmap Options)]_FlowMap("FlowMap", 2D) = "white" {}
		_FlowSpeed("Flow Speed", Float) = 20
		[Toggle]_LinearColorSpace("Linear Color Space", Float) = 0
		[Header(Ripple Options)]_RippleStrength("Ripple Strength", Range( 0 , 1)) = 0.5
		_RippleTex0("RippleTex0", 2D) = "white" {}
		_Scale0("Scale0", Float) = 0
		[HideInInspector]_XOffset0("XOffset0", Float) = 0
		[HideInInspector]_ZOffset0("ZOffset0", Float) = 0
		[HideInInspector]_RippleTex1("RippleTex1", 2D) = "white" {}
		[HideInInspector]_Scale1("Scale1", Float) = 0
		[HideInInspector]_XOffset1("XOffset1", Float) = 0
		[HideInInspector]_ZOffset1("ZOffset1", Float) = 0
		[HideInInspector]_RippleTex2("RippleTex2", 2D) = "white" {}
		[HideInInspector]_Scale2("Scale2", Float) = 0
		[HideInInspector]_XOffset2("XOffset2", Float) = 0
		[HideInInspector]_ZOffset2("ZOffset2", Float) = 0
		[HideInInspector]_RippleTex3("RippleTex3", 2D) = "white" {}
		[HideInInspector]_Scale3("Scale3", Float) = 0
		[HideInInspector]_XOffset3("XOffset3", Float) = 0
		[HideInInspector]_ZOffset3("ZOffset3", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float4 screenPos;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _Sampler1231394;
		uniform float _LightWrapping;
		uniform sampler2D _NormalTexture;
		uniform float _WaveSpeed;
		uniform float _NormalTiling;
		uniform float _LinearColorSpace;
		uniform sampler2D _FlowMap;
		uniform float _FlowSpeed;
		uniform float _NormalStrength;
		uniform float _ShorelineSpeed;
		uniform sampler2D _ShorelineMask;
		uniform float _ShorelineFrequency;
		uniform float _ShorelineNormalStrength;
		uniform float _ShorelineBlend;
		uniform sampler2D _RandomMask;
		uniform float _MediumTilingDistance;
		uniform float _DistanceFade;
		uniform float _FarTilingDistance;
		uniform sampler2D _RippleTex0;
		uniform float _Scale0;
		uniform float _XOffset0;
		uniform float _ZOffset0;
		uniform sampler2D _RippleTex1;
		uniform float _Scale1;
		uniform float _XOffset1;
		uniform float _ZOffset1;
		uniform sampler2D _RippleTex2;
		uniform float _Scale2;
		uniform float _XOffset2;
		uniform float _ZOffset2;
		uniform sampler2D _RippleTex3;
		uniform float _Scale3;
		uniform float _XOffset3;
		uniform float _ZOffset3;
		uniform float _RippleStrength;
		uniform float _EnableProbeRelfections;
		uniform float _EnableRealtimeReflections;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _Refraction;
		uniform float4 _DirtColor;
		uniform float _DirtDensity;
		uniform sampler2D _ReflectionTex;
		uniform float _Distortion;
		uniform float _RealtimeReflectionIntensity;
		uniform float _ProbeReflectionIntensity;
		uniform float _Gloss;
		uniform float _Specular;
		uniform float4 _SpecularColor;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float3 LightWrapVector47_g456 = (( _LightWrapping * 0.5 )).xxx;
			float waveSpeed675 = _WaveSpeed;
			float2 appendResult1_g293 = (float2(waveSpeed675 , 0.0));
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 temp_output_21_0_g227 = ase_worldNormal;
			float temp_output_17_0_g227 = (temp_output_21_0_g227).y;
			float2 appendResult18_g227 = (float2(sign( temp_output_17_0_g227 ) , 1.0));
			float3 ase_worldPos = i.worldPos;
			float2 BaseUV1197 = ( appendResult18_g227 * (ase_worldPos).xz );
			float normalTiling618 = _NormalTiling;
			float2 uv_FlowMap1258 = i.uv_texcoord;
			float4 tex2DNode1258 = tex2D( _FlowMap, uv_FlowMap1258 );
			float4 temp_cast_0 = (( 1.0 / 2.2 )).xxxx;
			float4 flowMap1263 = ( _LinearColorSpace )?( pow( tex2DNode1258 , temp_cast_0 ) ):( tex2DNode1258 );
			float FlowSpeed1256 = _FlowSpeed;
			float2 temp_output_1277_0 = ( (float2( -0.5,-0.5 ) + ((flowMap1263).rg - float2( 0,0 )) * (float2( 0.5,0.5 ) - float2( -0.5,-0.5 )) / (float2( 1,1 ) - float2( 0,0 ))) * FlowSpeed1256 );
			float mulTime1269 = _Time.y * 0.05;
			float temp_output_1279_0 = frac( mulTime1269 );
			float2 flowUV1Close1290 = ( temp_output_1277_0 * temp_output_1279_0 );
			float2 temp_output_1305_0 = ( ( BaseUV1197 * normalTiling618 ) + ( normalTiling618 * flowUV1Close1290 ) );
			float2 panner3_g293 = ( _Time.y * appendResult1_g293 + temp_output_1305_0);
			float2 appendResult1_g296 = (float2(waveSpeed675 , 0.0));
			float cos30 = cos( radians( 180.0 ) );
			float sin30 = sin( radians( 180.0 ) );
			float2 rotator30 = mul( temp_output_1305_0 - float2( 0.5,0.5 ) , float2x2( cos30 , -sin30 , sin30 , cos30 )) + float2( 0.5,0.5 );
			float2 panner3_g296 = ( _Time.y * appendResult1_g296 + rotator30);
			float normalStrength681 = _NormalStrength;
			float3 lerpResult67 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g293 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g296 ) ) ) , normalStrength681);
			float3 WavesCloseUV1207 = lerpResult67;
			float2 appendResult1_g295 = (float2(waveSpeed675 , 0.0));
			float2 flowUV2Close1289 = ( temp_output_1277_0 * frac( ( mulTime1269 + 0.5 ) ) );
			float2 temp_output_1324_0 = ( ( BaseUV1197 * normalTiling618 ) + ( normalTiling618 * flowUV2Close1289 ) );
			float2 panner3_g295 = ( _Time.y * appendResult1_g295 + temp_output_1324_0);
			float2 appendResult1_g299 = (float2(waveSpeed675 , 0.0));
			float cos1327 = cos( radians( 180.0 ) );
			float sin1327 = sin( radians( 180.0 ) );
			float2 rotator1327 = mul( temp_output_1324_0 - float2( 0.5,0.5 ) , float2x2( cos1327 , -sin1327 , sin1327 , cos1327 )) + float2( 0.5,0.5 );
			float2 panner3_g299 = ( _Time.y * appendResult1_g299 + rotator1327);
			float3 lerpResult1333 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g295 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g299 ) ) ) , normalStrength681);
			float3 WavesCloseUV21337 = lerpResult1333;
			float flowLerpClose1298 = abs( ( ( 0.5 - temp_output_1279_0 ) / 0.5 ) );
			float3 lerpResult1338 = lerp( WavesCloseUV1207 , WavesCloseUV21337 , flowLerpClose1298);
			float mulTime785 = _Time.y * _ShorelineSpeed;
			float2 appendResult914 = (float2(i.uv_texcoord.x , ( 1.0 - i.uv_texcoord.y )));
			float cos930 = cos( radians( 90.0 ) );
			float sin930 = sin( radians( 90.0 ) );
			float2 rotator930 = mul( appendResult914 - float2( 0.5,0.5 ) , float2x2( cos930 , -sin930 , sin930 , cos930 )) + float2( 0.5,0.5 );
			float4 tex2DNode912 = tex2D( _ShorelineMask, rotator930 );
			float temp_output_823_0 = ( 1.0 - tex2DNode912.r );
			float temp_output_788_0 = ( ( mulTime785 + temp_output_823_0 ) * _ShorelineFrequency * ( 2.0 * UNITY_PI ) );
			float temp_output_810_0 = ( 1.0 - temp_output_823_0 );
			float lerpResult815 = lerp( cos( temp_output_788_0 ) , 0.0 , temp_output_810_0);
			float lerpResult793 = lerp( sin( temp_output_788_0 ) , 0.0 , temp_output_810_0);
			float3 appendResult816 = (float3(lerpResult815 , lerpResult815 , lerpResult793));
			float3 lerpResult824 = lerp( float3(0,0,1) , appendResult816 , _ShorelineNormalStrength);
			float2 appendResult1_g244 = (float2(0.1 , 0.0));
			float3 temp_output_21_0_g228 = ase_worldNormal;
			float temp_output_17_0_g228 = (temp_output_21_0_g228).y;
			float2 appendResult18_g228 = (float2(sign( temp_output_17_0_g228 ) , 1.0));
			float2 temp_output_1211_0 = ( ( appendResult18_g228 * (ase_worldPos).xz ) * float2( 0.05,0.05 ) );
			float2 panner3_g244 = ( _Time.y * appendResult1_g244 + temp_output_1211_0);
			float2 appendResult1_g245 = (float2(0.2 , 0.0));
			float cos1177 = cos( radians( 180.0 ) );
			float sin1177 = sin( radians( 180.0 ) );
			float2 rotator1177 = mul( temp_output_1211_0 - float2( 0.5,0.5 ) , float2x2( cos1177 , -sin1177 , sin1177 , cos1177 )) + float2( 0.5,0.5 );
			float2 panner3_g245 = ( _Time.y * appendResult1_g245 + rotator1177);
			float4 ShorelineNormal817 = ( float4( lerpResult824 , 0.0 ) * ( 1.0 - step( tex2DNode912.r , _ShorelineBlend ) ) * ( ( tex2D( _RandomMask, panner3_g244 ) + tex2D( _RandomMask, panner3_g245 ) ) / 2.0 ) );
			float4 NormalsClose1340 = ( float4( lerpResult1338 , 0.0 ) + ShorelineNormal817 );
			float temp_output_678_0 = ( waveSpeed675 / 10.0 );
			float2 appendResult1_g292 = (float2(temp_output_678_0 , 0.0));
			float temp_output_642_0 = ( normalTiling618 / 10.0 );
			float FlowSpeedMedium1300 = ( FlowSpeed1256 / 1.0 );
			float2 temp_output_1280_0 = ( (float2( -0.5,-0.5 ) + ((flowMap1263).rg - float2( 0,0 )) * (float2( 0.5,0.5 ) - float2( -0.5,-0.5 )) / (float2( 1,1 ) - float2( 0,0 ))) * FlowSpeedMedium1300 );
			float mulTime1270 = _Time.y * 0.05;
			float temp_output_1282_0 = frac( mulTime1270 );
			float2 flowUV1Medium1288 = ( temp_output_1280_0 * temp_output_1282_0 );
			float2 temp_output_1347_0 = ( ( BaseUV1197 * temp_output_642_0 ) + ( temp_output_642_0 * flowUV1Medium1288 ) );
			float2 panner3_g292 = ( _Time.y * appendResult1_g292 + temp_output_1347_0);
			float2 appendResult1_g297 = (float2(temp_output_678_0 , 0.0));
			float cos630 = cos( radians( 180.0 ) );
			float sin630 = sin( radians( 180.0 ) );
			float2 rotator630 = mul( temp_output_1347_0 - float2( 0.5,0.5 ) , float2x2( cos630 , -sin630 , sin630 , cos630 )) + float2( 0.5,0.5 );
			float2 panner3_g297 = ( _Time.y * appendResult1_g297 + rotator630);
			float mediumTilingDistance687 = _MediumTilingDistance;
			float tilingFade689 = _DistanceFade;
			float lerpResult693 = lerp( normalStrength681 , ( normalStrength681 / 20.0 ) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / mediumTilingDistance687 ) , tilingFade689 ) ));
			float normalStrengthMedium706 = lerpResult693;
			float3 lerpResult639 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g292 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g297 ) ) ) , normalStrengthMedium706);
			float3 WavesMediumUv1640 = lerpResult639;
			float temp_output_1362_0 = ( waveSpeed675 / 10.0 );
			float2 appendResult1_g298 = (float2(temp_output_1362_0 , 0.0));
			float temp_output_1358_0 = ( normalTiling618 / 10.0 );
			float2 flowUV2Medium1287 = ( temp_output_1280_0 * frac( ( mulTime1270 + 0.5 ) ) );
			float2 temp_output_1360_0 = ( ( BaseUV1197 * temp_output_1358_0 ) + ( temp_output_1358_0 * flowUV2Medium1287 ) );
			float2 panner3_g298 = ( _Time.y * appendResult1_g298 + temp_output_1360_0);
			float2 appendResult1_g294 = (float2(temp_output_1362_0 , 0.0));
			float cos1361 = cos( radians( 180.0 ) );
			float sin1361 = sin( radians( 180.0 ) );
			float2 rotator1361 = mul( temp_output_1360_0 - float2( 0.5,0.5 ) , float2x2( cos1361 , -sin1361 , sin1361 , cos1361 )) + float2( 0.5,0.5 );
			float2 panner3_g294 = ( _Time.y * appendResult1_g294 + rotator1361);
			float3 lerpResult1367 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g298 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g294 ) ) ) , normalStrengthMedium706);
			float3 WavesMediumUV21368 = lerpResult1367;
			float flowLerpMedium1297 = abs( ( ( 0.5 - temp_output_1282_0 ) / 0.5 ) );
			float3 lerpResult1369 = lerp( WavesMediumUv1640 , WavesMediumUV21368 , flowLerpMedium1297);
			float4 NormalsMedium1373 = ( float4( lerpResult1369 , 0.0 ) + ShorelineNormal817 );
			float4 lerpResult664 = lerp( NormalsClose1340 , NormalsMedium1373 , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / mediumTilingDistance687 ) , tilingFade689 ) ));
			float temp_output_680_0 = ( waveSpeed675 / 30.0 );
			float2 appendResult1_g302 = (float2(temp_output_680_0 , 0.0));
			float2 temp_output_1201_0 = ( BaseUV1197 * ( normalTiling618 / 1200.0 ) );
			float2 panner3_g302 = ( _Time.y * appendResult1_g302 + temp_output_1201_0);
			float2 appendResult1_g301 = (float2(temp_output_680_0 , 0.0));
			float cos646 = cos( radians( 180.0 ) );
			float sin646 = sin( radians( 180.0 ) );
			float2 rotator646 = mul( temp_output_1201_0 - float2( 0.5,0.5 ) , float2x2( cos646 , -sin646 , sin646 , cos646 )) + float2( 0.5,0.5 );
			float2 panner3_g301 = ( _Time.y * appendResult1_g301 + rotator646);
			float farTilingDistance688 = _FarTilingDistance;
			float lerpResult698 = lerp( normalStrengthMedium706 , ( lerpResult693 / 20.0 ) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / farTilingDistance688 ) , tilingFade689 ) ));
			float normalStrengthFar704 = lerpResult698;
			float3 lerpResult657 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g302 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g301 ) ) ) , normalStrengthFar704);
			float3 NormalsFar660 = lerpResult657;
			float4 lerpResult670 = lerp( lerpResult664 , float4( NormalsFar660 , 0.0 ) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / farTilingDistance688 ) , tilingFade689 ) ));
			float2 NormalSign1123 = appendResult18_g227;
			float2 WorldNormalXZ1122 = (temp_output_21_0_g227).xz;
			float WorldNormalY1121 = temp_output_17_0_g227;
			float3 appendResult4_g455 = (float3(( ( (lerpResult670.rgb).xy * NormalSign1123 ) + WorldNormalXZ1122 ) , WorldNormalY1121));
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentDir6_g455 = normalize( mul( ase_worldToTangent, (appendResult4_g455).xzy) );
			float3 break1434 = worldToTangentDir6_g455;
			float temp_output_32_0_g4 = ( -0.02 / ( _Scale0 / 30.0 ) );
			float temp_output_1_0_g440 = _Scale0;
			float Scale10_g440 = temp_output_1_0_g440;
			float2 break12_g440 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g440 ) ) * ( 1.0 / temp_output_1_0_g440 ) );
			float2 appendResult17_g440 = (float2(( ( _XOffset0 / Scale10_g440 ) + break12_g440.x ) , ( break12_g440.y + ( _ZOffset0 / Scale10_g440 ) )));
			float2 temp_output_1_0_g4 = appendResult17_g440;
			float4 tex2DNode8_g4 = tex2D( _RippleTex0, ( ( temp_output_32_0_g4 * float2( 0,1 ) ) + temp_output_1_0_g4 ) );
			float temp_output_17_0_g4 = ( ( tex2D( _RippleTex0, ( ( float2( 1,0 ) * temp_output_32_0_g4 ) + temp_output_1_0_g4 ) ).r - tex2DNode8_g4.r ) * 1.0 );
			float temp_output_18_0_g4 = ( 1.0 * ( tex2DNode8_g4.r - tex2D( _RippleTex0, temp_output_1_0_g4 ).r ) );
			float2 appendResult21_g4 = (float2(temp_output_17_0_g4 , temp_output_18_0_g4));
			float3 appendResult27_g4 = (float3(( -appendResult21_g4 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g4 , 1.0 ) ) - pow( temp_output_18_0_g4 , 1.0 ) ) )));
			float3 normalizeResult28_g4 = normalize( appendResult27_g4 );
			float2 appendResult37_g440 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g440 = ase_worldNormal;
			float3 appendResult4_g450 = (float3(( ( (normalizeResult28_g4).xy * appendResult37_g440 ) + (temp_output_30_0_g440).xz ) , (temp_output_30_0_g440).y));
			float3 worldToTangentDir6_g450 = normalize( mul( ase_worldToTangent, (appendResult4_g450).xzy) );
			float temp_output_32_0_g1 = ( -0.02 / ( _Scale1 / 30.0 ) );
			float temp_output_1_0_g438 = _Scale1;
			float Scale10_g438 = temp_output_1_0_g438;
			float2 break12_g438 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g438 ) ) * ( 1.0 / temp_output_1_0_g438 ) );
			float2 appendResult17_g438 = (float2(( ( _XOffset1 / Scale10_g438 ) + break12_g438.x ) , ( break12_g438.y + ( _ZOffset1 / Scale10_g438 ) )));
			float2 temp_output_1_0_g1 = appendResult17_g438;
			float4 tex2DNode8_g1 = tex2D( _RippleTex1, ( ( temp_output_32_0_g1 * float2( 0,1 ) ) + temp_output_1_0_g1 ) );
			float temp_output_17_0_g1 = ( ( tex2D( _RippleTex1, ( ( float2( 1,0 ) * temp_output_32_0_g1 ) + temp_output_1_0_g1 ) ).r - tex2DNode8_g1.r ) * 1.0 );
			float temp_output_18_0_g1 = ( 1.0 * ( tex2DNode8_g1.r - tex2D( _RippleTex1, temp_output_1_0_g1 ).r ) );
			float2 appendResult21_g1 = (float2(temp_output_17_0_g1 , temp_output_18_0_g1));
			float3 appendResult27_g1 = (float3(( -appendResult21_g1 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g1 , 1.0 ) ) - pow( temp_output_18_0_g1 , 1.0 ) ) )));
			float3 normalizeResult28_g1 = normalize( appendResult27_g1 );
			float2 appendResult37_g438 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g438 = ase_worldNormal;
			float3 appendResult4_g449 = (float3(( ( (normalizeResult28_g1).xy * appendResult37_g438 ) + (temp_output_30_0_g438).xz ) , (temp_output_30_0_g438).y));
			float3 worldToTangentDir6_g449 = normalize( mul( ase_worldToTangent, (appendResult4_g449).xzy) );
			float temp_output_32_0_g2 = ( -0.02 / ( _Scale2 / 30.0 ) );
			float temp_output_1_0_g441 = _Scale2;
			float Scale10_g441 = temp_output_1_0_g441;
			float2 break12_g441 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g441 ) ) * ( 1.0 / temp_output_1_0_g441 ) );
			float2 appendResult17_g441 = (float2(( ( _XOffset2 / Scale10_g441 ) + break12_g441.x ) , ( break12_g441.y + ( _ZOffset2 / Scale10_g441 ) )));
			float2 temp_output_1_0_g2 = appendResult17_g441;
			float4 tex2DNode8_g2 = tex2D( _RippleTex2, ( ( temp_output_32_0_g2 * float2( 0,1 ) ) + temp_output_1_0_g2 ) );
			float temp_output_17_0_g2 = ( ( tex2D( _RippleTex2, ( ( float2( 1,0 ) * temp_output_32_0_g2 ) + temp_output_1_0_g2 ) ).r - tex2DNode8_g2.r ) * 1.0 );
			float temp_output_18_0_g2 = ( 1.0 * ( tex2DNode8_g2.r - tex2D( _RippleTex2, temp_output_1_0_g2 ).r ) );
			float2 appendResult21_g2 = (float2(temp_output_17_0_g2 , temp_output_18_0_g2));
			float3 appendResult27_g2 = (float3(( -appendResult21_g2 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g2 , 1.0 ) ) - pow( temp_output_18_0_g2 , 1.0 ) ) )));
			float3 normalizeResult28_g2 = normalize( appendResult27_g2 );
			float2 appendResult37_g441 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g441 = ase_worldNormal;
			float3 appendResult4_g448 = (float3(( ( (normalizeResult28_g2).xy * appendResult37_g441 ) + (temp_output_30_0_g441).xz ) , (temp_output_30_0_g441).y));
			float3 worldToTangentDir6_g448 = normalize( mul( ase_worldToTangent, (appendResult4_g448).xzy) );
			float temp_output_32_0_g3 = ( -0.02 / ( _Scale3 / 30.0 ) );
			float temp_output_1_0_g439 = _Scale3;
			float Scale10_g439 = temp_output_1_0_g439;
			float2 break12_g439 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g439 ) ) * ( 1.0 / temp_output_1_0_g439 ) );
			float2 appendResult17_g439 = (float2(( ( _XOffset3 / Scale10_g439 ) + break12_g439.x ) , ( break12_g439.y + ( _ZOffset3 / Scale10_g439 ) )));
			float2 temp_output_1_0_g3 = appendResult17_g439;
			float4 tex2DNode8_g3 = tex2D( _RippleTex3, ( ( temp_output_32_0_g3 * float2( 0,1 ) ) + temp_output_1_0_g3 ) );
			float temp_output_17_0_g3 = ( ( tex2D( _RippleTex3, ( ( float2( 1,0 ) * temp_output_32_0_g3 ) + temp_output_1_0_g3 ) ).r - tex2DNode8_g3.r ) * 1.0 );
			float temp_output_18_0_g3 = ( 1.0 * ( tex2DNode8_g3.r - tex2D( _RippleTex3, temp_output_1_0_g3 ).r ) );
			float2 appendResult21_g3 = (float2(temp_output_17_0_g3 , temp_output_18_0_g3));
			float3 appendResult27_g3 = (float3(( -appendResult21_g3 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g3 , 1.0 ) ) - pow( temp_output_18_0_g3 , 1.0 ) ) )));
			float3 normalizeResult28_g3 = normalize( appendResult27_g3 );
			float2 appendResult37_g439 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g439 = ase_worldNormal;
			float3 appendResult4_g451 = (float3(( ( (normalizeResult28_g3).xy * appendResult37_g439 ) + (temp_output_30_0_g439).xz ) , (temp_output_30_0_g439).y));
			float3 worldToTangentDir6_g451 = normalize( mul( ase_worldToTangent, (appendResult4_g451).xzy) );
			float3 lerpResult1427 = lerp( ( worldToTangentDir6_g450 + worldToTangentDir6_g449 + worldToTangentDir6_g448 + worldToTangentDir6_g451 ) , float3(0,0,1) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / 100.0 ) , 0.5 ) ));
			float3 RippleNormals1428 = lerpResult1427;
			float3 lerpResult1432 = lerp( float3(0,0,1) , RippleNormals1428 , _RippleStrength);
			float3 break1433 = lerpResult1432;
			float3 appendResult1437 = (float3(( break1434.x + break1433.x ) , ( break1434.y + break1433.y ) , break1434.z));
			float3 resultingNormal674 = appendResult1437;
			float3 break736 = resultingNormal674;
			float3 appendResult735 = (float3(break736.x , break736.y , 1));
			float3 CurrentNormal23_g456 = normalize( (WorldNormalVector( i , appendResult735 )) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult20_g456 = dot( CurrentNormal23_g456 , ase_worldlightDir );
			float NDotL21_g456 = dotResult20_g456;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 lerpResult164_g456 = lerp( float4( ( ase_lightColor.rgb * ase_lightAtten ) , 0.0 ) , UNITY_LIGHTMODEL_AMBIENT , ( 1.0 - ase_lightAtten ));
			float4 AttenuationColor8_g456 = lerpResult164_g456;
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float temp_output_209_0 = ( _Refraction * 0.2 );
			float2 pseudoRefraction484 = ( (ase_grabScreenPosNorm).xy + ( temp_output_209_0 * (resultingNormal674).xy ) );
			float4 screenColor1377 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,pseudoRefraction484);
			float4 lerpResult1447 = lerp( screenColor1377 , _DirtColor , _DirtDensity);
			float4 break1439 = lerpResult670;
			float3 appendResult1443 = (float3(( break1433.x + break1439.r ) , ( break1433.y + break1439.g ) , break1439.b));
			float3 ResultingNormalForDistortion1444 = appendResult1443;
			float4 realtimeReflection600 = tex2D( _ReflectionTex, ( (ase_screenPosNorm).xy + ( (ResultingNormalForDistortion1444).xy * _Distortion ) ) );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
			float fresnelNdotV723 = dot( mul(ase_tangentToWorldFast,resultingNormal674), ase_worldViewDir );
			float fresnelNode723 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV723, 2.0 ) );
			float temp_output_1387_0 = saturate( fresnelNode723 );
			float4 lerpResult1385 = lerp( lerpResult1447 , realtimeReflection600 , ( temp_output_1387_0 * _RealtimeReflectionIntensity ));
			float Distortion761 = _Distortion;
			float3 break775 = ( resultingNormal674 * Distortion761 );
			float3 appendResult776 = (float3(break775.x , break775.y , 1.0));
			float3 indirectNormal727 = WorldNormalVector( i , appendResult776 );
			Unity_GlossyEnvironmentData g727 = UnityGlossyEnvironmentSetup( 1.0, data.worldViewDir, indirectNormal727, float3(0,0,0));
			float3 indirectSpecular727 = UnityGI_IndirectSpecular( data, 1.0, indirectNormal727, g727 );
			float fresnelNdotV755 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode755 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV755, 4.0 ) );
			float3 lerpResult754 = lerp( ( indirectSpecular727 * float3( 0.3,0.3,0.3 ) ) , indirectSpecular727 , fresnelNode755);
			float3 probeReflection766 = lerpResult754;
			float4 lerpResult1391 = lerp( ( _EnableRealtimeReflections )?( lerpResult1385 ):( lerpResult1447 ) , float4( probeReflection766 , 0.0 ) , ( temp_output_1387_0 * _ProbeReflectionIntensity ));
			float4 foamyWater490 = ( _EnableProbeRelfections )?( lerpResult1391 ):( ( _EnableRealtimeReflections )?( lerpResult1385 ):( lerpResult1447 ) );
			float clampResult100_g456 = clamp( ase_worldlightDir.y , ( length( (UNITY_LIGHTMODEL_AMBIENT).rgb ) / 3.0 ) , 1.0 );
			float4 DiffuseColor70_g456 = ( ( ( float4( max( ( LightWrapVector47_g456 + ( ( 1.0 - LightWrapVector47_g456 ) * NDotL21_g456 ) ) , float3(0,0,0) ) , 0.0 ) * AttenuationColor8_g456 ) * float4( foamyWater490.rgb , 0.0 ) ) * clampResult100_g456 );
			float3 normalizeResult77_g456 = normalize( ase_worldlightDir );
			float3 normalizeResult28_g456 = normalize( ( normalizeResult77_g456 + ase_worldViewDir ) );
			float3 HalfDirection29_g456 = normalizeResult28_g456;
			float dotResult32_g456 = dot( HalfDirection29_g456 , CurrentNormal23_g456 );
			float SpecularPower14_g456 = exp2( ( ( _Gloss * 10.0 ) + 1.0 ) );
			float4 specularity504 = ( _Specular * _SpecularColor );
			float4 specularFinalColor42_g456 = ( AttenuationColor8_g456 * pow( max( dotResult32_g456 , 0.0 ) , SpecularPower14_g456 ) * float4( specularity504.rgb , 0.0 ) );
			float4 DiffuseSpecular83_g456 = ( DiffuseColor70_g456 + specularFinalColor42_g456 );
			float4 lerpResult87_g456 = lerp( tex2D( _Sampler1231394, ( (ase_screenPosNorm).xy + ( float2( 0.2,0 ) * float2( 0,0 ) ) ) ) , DiffuseSpecular83_g456 , 1.0);
			c.rgb = ( lerpResult87_g456 * ase_lightAtten ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}