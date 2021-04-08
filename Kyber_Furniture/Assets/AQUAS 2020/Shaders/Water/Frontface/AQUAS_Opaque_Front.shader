Shader "Hidden/AQUAS/Desktop/Front/Opaque"
{
	Properties
	{
		[NoScaleOffset][Header(Wave Options)]_NormalTexture("Normal Texture", 2D) = "bump" {}
		_NormalTiling("Normal Tiling", Range( 0.01 , 2)) = 1
		_NormalStrength("Normal Strength", Range( 0 , 2)) = 0
		_WaveSpeed("Wave Speed", Float) = 0
		[Header(Color Options)]_MainColor("Main Color", Color) = (0,0.4867925,0.6792453,0)
		_DeepWaterColor("Deep Water Color", Color) = (0.5,0.2712264,0.2712264,0)
		_Density("Density", Range( 0 , 1)) = 1
		_Fade("Fade", Float) = 0
		[Header(Transparency Options)]_DepthTransparency("Depth Transparency", Float) = 0
		_TransparencyFade("Transparency Fade", Float) = 0
		_Refraction("Refraction", Range( 0 , 1)) = 0.1
		[Header(Lighting Options)]_Specular("Specular", Float) = 0
		_SpecularColor("Specular Color", Color) = (0,0,0,0)
		_Gloss("Gloss", Float) = 0
		_LightWrapping("Light Wrapping", Range( 0 , 2)) = 0
		[NoScaleOffset][Header(Foam Options)]_FoamTexture("Foam Texture", 2D) = "white" {}
		_FoamTiling("Foam Tiling", Range( 0 , 2)) = 0
		_FoamVisibility("Foam Visibility", Range( 0 , 1)) = 0
		_FoamBlend("Foam Blend", Float) = 0
		_FoamColor("Foam Color", Color) = (0.8773585,0,0,0)
		_FoamContrast("Foam Contrast", Range( 0 , 0.5)) = 0
		_FoamIntensity("Foam Intensity", Float) = 0.21
		_FoamSpeed("Foam Speed", Float) = 0.1
		[Header(Reflection Options)][Toggle]_EnableRealtimeReflections("Enable Realtime Reflections", Float) = 1
		_RealtimeReflectionIntensity("Realtime Reflection Intensity", Range( 0 , 1)) = 0
		[Toggle]_EnableProbeRelfections("Enable Probe Relfections", Float) = 0
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
		[HideInInspector]_RandomMask("Random Mask", 2D) = "white" {}
		[NoScaleOffset][Header(Flowmap Options)]_FlowMap("FlowMap", 2D) = "white" {}
		_FlowSpeed("Flow Speed", Float) = 20
		[Toggle]_LinearColorSpace("Linear Color Space", Float) = 0
		[Header(Ripple Options)]_RippleStrength1("Ripple Strength", Range( 0 , 1)) = 0.5
		[HideInInspector]_RippleTex0("RippleTex0", 2D) = "white" {}
		[HideInInspector]_Scale0("Scale0", Float) = 0
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
		[HideInInspector][Toggle]_ProjectGrid("Project Grid", Float) = 1
		[HideInInspector]_ObjectScale("Object Scale", Vector) = (0,0,0,0)
		[HideInInspector]_waterLevel("waterLevel", Float) = 0
		[HideInInspector]_RangeVector("Range Vector", Vector) = (0,0,0,0)
		[HideInInspector]_PhysicalNormalStrength("Physical Normal Strength", Range( 0 , 1)) = 0
		[HideInInspector][NoScaleOffset]_DeTex("DeTex", 2D) = "white" {}
		[HideInInspector][NoScaleOffset]_ColorTex("ColorTex", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustomLighting keepalpha vertex:vertexDataFunc  addshadow
		struct Input
		{
			float4 screenPos;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
			float2 uv_texcoord;
			float eyeDepth;
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

		uniform float _ProjectGrid;
		uniform float4 _RangeVector;
		uniform float2 _ObjectScale;
		uniform float _waterLevel;
		uniform sampler2D _RandomMask;
		uniform sampler2D _ColorTex;
		uniform sampler2D _NormalTexture;
		uniform float _WaveSpeed;
		uniform float _NormalTiling;
		uniform float _LinearColorSpace;
		uniform sampler2D _FlowMap;
		uniform float _FlowSpeed;
		uniform float _NormalStrength;
		uniform float _Refraction;
		uniform sampler2D _DeTex;
		uniform float _LightWrapping;
		uniform float _ShorelineSpeed;
		uniform sampler2D _ShorelineMask;
		uniform float _ShorelineFrequency;
		uniform float _ShorelineNormalStrength;
		uniform float _ShorelineBlend;
		uniform float4 _RandomMask_ST;
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
		uniform float _RippleStrength1;
		uniform float _PhysicalNormalStrength;
		uniform float _EnableProbeRelfections;
		uniform float _EnableRealtimeReflections;
		uniform float4 _DeepWaterColor;
		uniform float4 _MainColor;
		uniform float _Density;
		uniform float _Fade;
		uniform sampler2D _ReflectionTex;
		uniform float _Distortion;
		uniform float _RealtimeReflectionIntensity;
		uniform float _ProbeReflectionIntensity;
		uniform float _FoamBlend;
		uniform sampler2D _FoamTexture;
		uniform float _FoamSpeed;
		uniform float _FoamTiling;
		uniform float _FoamContrast;
		uniform float4 _FoamColor;
		uniform float _FoamIntensity;
		uniform float _FoamVisibility;
		uniform float _Gloss;
		uniform float _Specular;
		uniform float4 _SpecularColor;
		uniform float _DepthTransparency;
		uniform float _TransparencyFade;


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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float2 appendResult1598 = (float2(max( -_RangeVector.x , 0.0 ) , max( -_RangeVector.y , 0.0 )));
			float2 RangeX1599 = appendResult1598;
			float2 appendResult1597 = (float2(min( -_RangeVector.z , 0.0 ) , min( -_RangeVector.w , 0.0 )));
			float2 RangeZ1600 = appendResult1597;
			float2 ObjectScale1602 = _ObjectScale;
			float2 break1680 = ( (RangeX1599 + (v.texcoord.xy - float2( -0.5,-0.5 )) * (RangeZ1600 - RangeX1599) / (float2( 0.5,0.5 ) - float2( -0.5,-0.5 ))) * ObjectScale1602 );
			float3 appendResult1683 = (float3(break1680.x , 1.0 , break1680.y));
			float Displacement1604 = -10.0;
			float2 break1621 = ( (RangeX1599 + (v.texcoord.xy - float2( -0.5,-0.5 )) * (RangeZ1600 - RangeX1599) / (float2( 0.5,0.5 ) - float2( -0.5,-0.5 ))) * ObjectScale1602 );
			float3 appendResult1623 = (float3(break1621.x , 1.0 , break1621.y));
			float3 temp_output_1628_0 = ( ( appendResult1623 * Displacement1604 ) + ase_vertex3Pos );
			float3 objToWorld1630 = mul( unity_ObjectToWorld, float4( temp_output_1628_0, 1 ) ).xyz;
			float3 TargetPoint1631 = objToWorld1630;
			float3 objToWorld1629 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 1 ) ).xyz;
			float3 StartPointGlobal1632 = objToWorld1629;
			float3 temp_output_1646_0 = ( TargetPoint1631 - StartPointGlobal1632 );
			float3 x1649 = ( temp_output_1646_0 / length( temp_output_1646_0 ) );
			float3 _Vector12 = float3(0,1,0);
			float3 PlaneNormal1609 = _Vector12;
			float3 appendResult1606 = (float3(0.0 , _waterLevel , 0.0));
			float3 PlaneOrigin1608 = appendResult1606;
			float dotResult1656 = dot( PlaneNormal1609 , ( TargetPoint1631 - PlaneOrigin1608 ) );
			float distV21658 = dotResult1656;
			float dotResult1655 = dot( PlaneNormal1609 , x1649 );
			float cosPhi1657 = dotResult1655;
			float3 IntersectionPoint1666 = ( TargetPoint1631 - ( x1649 * ( distV21658 / cosPhi1657 ) ) );
			float3 Projection1687 = ( ( appendResult1683 * ( Displacement1604 * ( distance( IntersectionPoint1666 , StartPointGlobal1632 ) / distance( StartPointGlobal1632 , TargetPoint1631 ) ) ) ) + ase_vertex3Pos );
			float2 appendResult1_g587 = (float2(0.15 , 0.0));
			float2 temp_output_1696_0 = ( (IntersectionPoint1666).xz * 0.5 );
			float2 panner3_g587 = ( _Time.y * appendResult1_g587 + temp_output_1696_0);
			float2 temp_output_1700_0 = panner3_g587;
			float2 appendResult1_g589 = (float2(0.3 , 0.0));
			float cos1697 = cos( radians( 180.0 ) );
			float sin1697 = sin( radians( 180.0 ) );
			float2 rotator1697 = mul( temp_output_1696_0 - float2( 0.5,0.5 ) , float2x2( cos1697 , -sin1697 , sin1697 , cos1697 )) + float2( 0.5,0.5 );
			float2 panner3_g589 = ( _Time.y * appendResult1_g589 + rotator1697);
			float2 temp_output_1699_0 = panner3_g589;
			float4 _Vector13 = float4(0,2,-0.15,0.15);
			float4 temp_cast_2 = (_Vector13.x).xxxx;
			float4 temp_cast_3 = (_Vector13.y).xxxx;
			float4 temp_cast_4 = (_Vector13.z).xxxx;
			float4 temp_cast_5 = (_Vector13.w).xxxx;
			float4 lerpResult1721 = lerp( (temp_cast_4 + (( tex2Dlod( _RandomMask, float4( temp_output_1700_0, 0, 0.0) ) + tex2Dlod( _RandomMask, float4( temp_output_1699_0, 0, 0.0) ) ) - temp_cast_2) * (temp_cast_5 - temp_cast_4) / (temp_cast_3 - temp_cast_2)) , float4(0,0,0,0) , saturate( pow( ( distance( IntersectionPoint1666 , _WorldSpaceCameraPos ) / 1.0 ) , 0.5 ) ));
			float4 PhysicalWaves1722 = lerpResult1721;
			float3 worldToObjDir1610 = mul( unity_WorldToObject, float4( _Vector12, 0 ) ).xyz;
			float3 VertexNormal1611 = worldToObjDir1610;
			v.vertex.xyz = ( _ProjectGrid )?( ( float4( Projection1687 , 0.0 ) + ( PhysicalWaves1722 * float4( VertexNormal1611 , 0.0 ) ) ) ):( float4( ase_vertex3Pos , 0.0 ) ).xyz;
			float3 temp_output_1726_0 = VertexNormal1611;
			v.normal = temp_output_1726_0;
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
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
			float waveSpeed675 = _WaveSpeed;
			float2 appendResult1_g345 = (float2(waveSpeed675 , 0.0));
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
			float2 panner3_g345 = ( _Time.y * appendResult1_g345 + temp_output_1305_0);
			float2 appendResult1_g347 = (float2(waveSpeed675 , 0.0));
			float cos30 = cos( radians( 180.0 ) );
			float sin30 = sin( radians( 180.0 ) );
			float2 rotator30 = mul( temp_output_1305_0 - float2( 0.5,0.5 ) , float2x2( cos30 , -sin30 , sin30 , cos30 )) + float2( 0.5,0.5 );
			float2 panner3_g347 = ( _Time.y * appendResult1_g347 + rotator30);
			float normalStrength681 = _NormalStrength;
			float3 lerpResult67 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g345 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g347 ) ) ) , normalStrength681);
			float3 WavesCloseUV1207 = lerpResult67;
			float temp_output_209_0 = ( _Refraction * 0.2 );
			float refractiveStrength496 = temp_output_209_0;
			float SceneDepth1392 = tex2D( _DeTex, (ase_screenPosNorm).xy ).r;
			float temp_output_7_0_g621 = 0.1;
			float2 temp_output_461_0 = ( (WavesCloseUV1207).xy * refractiveStrength496 * saturate( ( ( SceneDepth1392 - i.eyeDepth ) / temp_output_7_0_g621 ) ) );
			float2 refraction511 = temp_output_461_0;
			float3 LightWrapVector47_g623 = (( _LightWrapping * 0.5 )).xxx;
			float2 appendResult1_g340 = (float2(waveSpeed675 , 0.0));
			float2 flowUV2Close1289 = ( temp_output_1277_0 * frac( ( mulTime1269 + 0.5 ) ) );
			float2 temp_output_1324_0 = ( ( BaseUV1197 * normalTiling618 ) + ( normalTiling618 * flowUV2Close1289 ) );
			float2 panner3_g340 = ( _Time.y * appendResult1_g340 + temp_output_1324_0);
			float2 appendResult1_g342 = (float2(waveSpeed675 , 0.0));
			float cos1327 = cos( radians( 180.0 ) );
			float sin1327 = sin( radians( 180.0 ) );
			float2 rotator1327 = mul( temp_output_1324_0 - float2( 0.5,0.5 ) , float2x2( cos1327 , -sin1327 , sin1327 , cos1327 )) + float2( 0.5,0.5 );
			float2 panner3_g342 = ( _Time.y * appendResult1_g342 + rotator1327);
			float3 lerpResult1333 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g340 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g342 ) ) ) , normalStrength681);
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
			float2 uv_RandomMask = i.uv_texcoord * _RandomMask_ST.xy + _RandomMask_ST.zw;
			float2 appendResult1_g260 = (float2(0.2 , 0.0));
			float3 temp_output_21_0_g228 = ase_worldNormal;
			float temp_output_17_0_g228 = (temp_output_21_0_g228).y;
			float2 appendResult18_g228 = (float2(sign( temp_output_17_0_g228 ) , 1.0));
			float2 temp_output_1211_0 = ( ( appendResult18_g228 * (ase_worldPos).xz ) * float2( 0.05,0.05 ) );
			float cos1177 = cos( radians( 180.0 ) );
			float sin1177 = sin( radians( 180.0 ) );
			float2 rotator1177 = mul( temp_output_1211_0 - float2( 0.5,0.5 ) , float2x2( cos1177 , -sin1177 , sin1177 , cos1177 )) + float2( 0.5,0.5 );
			float2 panner3_g260 = ( _Time.y * appendResult1_g260 + rotator1177);
			float4 ShorelineNormal817 = ( float4( lerpResult824 , 0.0 ) * ( 1.0 - step( tex2DNode912.r , _ShorelineBlend ) ) * ( ( tex2D( _RandomMask, uv_RandomMask ) + tex2D( _RandomMask, panner3_g260 ) ) / 2.0 ) );
			float4 NormalsClose1340 = ( float4( lerpResult1338 , 0.0 ) + ShorelineNormal817 );
			float temp_output_678_0 = ( waveSpeed675 / 10.0 );
			float2 appendResult1_g343 = (float2(temp_output_678_0 , 0.0));
			float temp_output_642_0 = ( normalTiling618 / 10.0 );
			float FlowSpeedMedium1300 = ( FlowSpeed1256 / 1.0 );
			float2 temp_output_1280_0 = ( (float2( -0.5,-0.5 ) + ((flowMap1263).rg - float2( 0,0 )) * (float2( 0.5,0.5 ) - float2( -0.5,-0.5 )) / (float2( 1,1 ) - float2( 0,0 ))) * FlowSpeedMedium1300 );
			float mulTime1270 = _Time.y * 0.05;
			float temp_output_1282_0 = frac( mulTime1270 );
			float2 flowUV1Medium1288 = ( temp_output_1280_0 * temp_output_1282_0 );
			float2 temp_output_1347_0 = ( ( BaseUV1197 * temp_output_642_0 ) + ( temp_output_642_0 * flowUV1Medium1288 ) );
			float2 panner3_g343 = ( _Time.y * appendResult1_g343 + temp_output_1347_0);
			float2 appendResult1_g346 = (float2(temp_output_678_0 , 0.0));
			float cos630 = cos( radians( 180.0 ) );
			float sin630 = sin( radians( 180.0 ) );
			float2 rotator630 = mul( temp_output_1347_0 - float2( 0.5,0.5 ) , float2x2( cos630 , -sin630 , sin630 , cos630 )) + float2( 0.5,0.5 );
			float2 panner3_g346 = ( _Time.y * appendResult1_g346 + rotator630);
			float mediumTilingDistance687 = _MediumTilingDistance;
			float tilingFade689 = _DistanceFade;
			float lerpResult693 = lerp( normalStrength681 , ( normalStrength681 / 20.0 ) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / mediumTilingDistance687 ) , tilingFade689 ) ));
			float normalStrengthMedium706 = lerpResult693;
			float3 lerpResult639 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g343 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g346 ) ) ) , normalStrengthMedium706);
			float3 WavesMediumUv1640 = lerpResult639;
			float temp_output_1362_0 = ( waveSpeed675 / 10.0 );
			float2 appendResult1_g344 = (float2(temp_output_1362_0 , 0.0));
			float temp_output_1358_0 = ( normalTiling618 / 10.0 );
			float2 flowUV2Medium1287 = ( temp_output_1280_0 * frac( ( mulTime1270 + 0.5 ) ) );
			float2 temp_output_1360_0 = ( ( BaseUV1197 * temp_output_1358_0 ) + ( temp_output_1358_0 * flowUV2Medium1287 ) );
			float2 panner3_g344 = ( _Time.y * appendResult1_g344 + temp_output_1360_0);
			float2 appendResult1_g341 = (float2(temp_output_1362_0 , 0.0));
			float cos1361 = cos( radians( 180.0 ) );
			float sin1361 = sin( radians( 180.0 ) );
			float2 rotator1361 = mul( temp_output_1360_0 - float2( 0.5,0.5 ) , float2x2( cos1361 , -sin1361 , sin1361 , cos1361 )) + float2( 0.5,0.5 );
			float2 panner3_g341 = ( _Time.y * appendResult1_g341 + rotator1361);
			float3 lerpResult1367 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g344 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g341 ) ) ) , normalStrengthMedium706);
			float3 WavesMediumUV21368 = lerpResult1367;
			float flowLerpMedium1297 = abs( ( ( 0.5 - temp_output_1282_0 ) / 0.5 ) );
			float3 lerpResult1369 = lerp( WavesMediumUv1640 , WavesMediumUV21368 , flowLerpMedium1297);
			float4 NormalsMedium1373 = ( float4( lerpResult1369 , 0.0 ) + ShorelineNormal817 );
			float4 lerpResult664 = lerp( NormalsClose1340 , NormalsMedium1373 , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / mediumTilingDistance687 ) , tilingFade689 ) ));
			float temp_output_680_0 = ( waveSpeed675 / 30.0 );
			float2 appendResult1_g594 = (float2(temp_output_680_0 , 0.0));
			float2 temp_output_1201_0 = ( BaseUV1197 * ( normalTiling618 / 1200.0 ) );
			float2 panner3_g594 = ( _Time.y * appendResult1_g594 + temp_output_1201_0);
			float2 appendResult1_g588 = (float2(temp_output_680_0 , 0.0));
			float cos646 = cos( radians( 180.0 ) );
			float sin646 = sin( radians( 180.0 ) );
			float2 rotator646 = mul( temp_output_1201_0 - float2( 0.5,0.5 ) , float2x2( cos646 , -sin646 , sin646 , cos646 )) + float2( 0.5,0.5 );
			float2 panner3_g588 = ( _Time.y * appendResult1_g588 + rotator646);
			float farTilingDistance688 = _FarTilingDistance;
			float lerpResult698 = lerp( normalStrengthMedium706 , ( lerpResult693 / 20.0 ) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / farTilingDistance688 ) , tilingFade689 ) ));
			float normalStrengthFar704 = lerpResult698;
			float3 lerpResult657 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g594 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g588 ) ) ) , normalStrengthFar704);
			float3 NormalsFar660 = lerpResult657;
			float4 lerpResult670 = lerp( lerpResult664 , float4( NormalsFar660 , 0.0 ) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / farTilingDistance688 ) , tilingFade689 ) ));
			float2 NormalSign1123 = appendResult18_g227;
			float2 WorldNormalXZ1122 = (temp_output_21_0_g227).xz;
			float WorldNormalY1121 = temp_output_17_0_g227;
			float3 appendResult4_g613 = (float3(( ( (lerpResult670.rgb).xy * NormalSign1123 ) + WorldNormalXZ1122 ) , WorldNormalY1121));
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentDir6_g613 = normalize( mul( ase_worldToTangent, (appendResult4_g613).xzy) );
			float3 break1571 = worldToTangentDir6_g613;
			float temp_output_32_0_g601 = ( -0.02 / ( _Scale0 / 30.0 ) );
			float temp_output_1_0_g593 = _Scale0;
			float Scale10_g593 = temp_output_1_0_g593;
			float2 break12_g593 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g593 ) ) * ( 1.0 / temp_output_1_0_g593 ) );
			float2 appendResult17_g593 = (float2(( ( _XOffset0 / Scale10_g593 ) + break12_g593.x ) , ( break12_g593.y + ( _ZOffset0 / Scale10_g593 ) )));
			float2 temp_output_1_0_g601 = appendResult17_g593;
			float4 tex2DNode8_g601 = tex2D( _RippleTex0, ( ( temp_output_32_0_g601 * float2( 0,1 ) ) + temp_output_1_0_g601 ) );
			float temp_output_17_0_g601 = ( ( tex2D( _RippleTex0, ( ( float2( 1,0 ) * temp_output_32_0_g601 ) + temp_output_1_0_g601 ) ).r - tex2DNode8_g601.r ) * 1.0 );
			float temp_output_18_0_g601 = ( 1.0 * ( tex2DNode8_g601.r - tex2D( _RippleTex0, temp_output_1_0_g601 ).r ) );
			float2 appendResult21_g601 = (float2(temp_output_17_0_g601 , temp_output_18_0_g601));
			float3 appendResult27_g601 = (float3(( -appendResult21_g601 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g601 , 1.0 ) ) - pow( temp_output_18_0_g601 , 1.0 ) ) )));
			float3 normalizeResult28_g601 = normalize( appendResult27_g601 );
			float2 appendResult37_g593 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g593 = ase_worldNormal;
			float3 appendResult4_g602 = (float3(( ( (normalizeResult28_g601).xy * appendResult37_g593 ) + (temp_output_30_0_g593).xz ) , (temp_output_30_0_g593).y));
			float3 worldToTangentDir6_g602 = normalize( mul( ase_worldToTangent, (appendResult4_g602).xzy) );
			float temp_output_32_0_g597 = ( -0.02 / ( _Scale1 / 30.0 ) );
			float temp_output_1_0_g591 = _Scale1;
			float Scale10_g591 = temp_output_1_0_g591;
			float2 break12_g591 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g591 ) ) * ( 1.0 / temp_output_1_0_g591 ) );
			float2 appendResult17_g591 = (float2(( ( _XOffset1 / Scale10_g591 ) + break12_g591.x ) , ( break12_g591.y + ( _ZOffset1 / Scale10_g591 ) )));
			float2 temp_output_1_0_g597 = appendResult17_g591;
			float4 tex2DNode8_g597 = tex2D( _RippleTex1, ( ( temp_output_32_0_g597 * float2( 0,1 ) ) + temp_output_1_0_g597 ) );
			float temp_output_17_0_g597 = ( ( tex2D( _RippleTex1, ( ( float2( 1,0 ) * temp_output_32_0_g597 ) + temp_output_1_0_g597 ) ).r - tex2DNode8_g597.r ) * 1.0 );
			float temp_output_18_0_g597 = ( 1.0 * ( tex2DNode8_g597.r - tex2D( _RippleTex1, temp_output_1_0_g597 ).r ) );
			float2 appendResult21_g597 = (float2(temp_output_17_0_g597 , temp_output_18_0_g597));
			float3 appendResult27_g597 = (float3(( -appendResult21_g597 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g597 , 1.0 ) ) - pow( temp_output_18_0_g597 , 1.0 ) ) )));
			float3 normalizeResult28_g597 = normalize( appendResult27_g597 );
			float2 appendResult37_g591 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g591 = ase_worldNormal;
			float3 appendResult4_g607 = (float3(( ( (normalizeResult28_g597).xy * appendResult37_g591 ) + (temp_output_30_0_g591).xz ) , (temp_output_30_0_g591).y));
			float3 worldToTangentDir6_g607 = normalize( mul( ase_worldToTangent, (appendResult4_g607).xzy) );
			float temp_output_32_0_g600 = ( -0.02 / ( _Scale2 / 30.0 ) );
			float temp_output_1_0_g592 = _Scale2;
			float Scale10_g592 = temp_output_1_0_g592;
			float2 break12_g592 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g592 ) ) * ( 1.0 / temp_output_1_0_g592 ) );
			float2 appendResult17_g592 = (float2(( ( _XOffset2 / Scale10_g592 ) + break12_g592.x ) , ( break12_g592.y + ( _ZOffset2 / Scale10_g592 ) )));
			float2 temp_output_1_0_g600 = appendResult17_g592;
			float4 tex2DNode8_g600 = tex2D( _RippleTex2, ( ( temp_output_32_0_g600 * float2( 0,1 ) ) + temp_output_1_0_g600 ) );
			float temp_output_17_0_g600 = ( ( tex2D( _RippleTex2, ( ( float2( 1,0 ) * temp_output_32_0_g600 ) + temp_output_1_0_g600 ) ).r - tex2DNode8_g600.r ) * 1.0 );
			float temp_output_18_0_g600 = ( 1.0 * ( tex2DNode8_g600.r - tex2D( _RippleTex2, temp_output_1_0_g600 ).r ) );
			float2 appendResult21_g600 = (float2(temp_output_17_0_g600 , temp_output_18_0_g600));
			float3 appendResult27_g600 = (float3(( -appendResult21_g600 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g600 , 1.0 ) ) - pow( temp_output_18_0_g600 , 1.0 ) ) )));
			float3 normalizeResult28_g600 = normalize( appendResult27_g600 );
			float2 appendResult37_g592 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g592 = ase_worldNormal;
			float3 appendResult4_g608 = (float3(( ( (normalizeResult28_g600).xy * appendResult37_g592 ) + (temp_output_30_0_g592).xz ) , (temp_output_30_0_g592).y));
			float3 worldToTangentDir6_g608 = normalize( mul( ase_worldToTangent, (appendResult4_g608).xzy) );
			float temp_output_32_0_g598 = ( -0.02 / ( _Scale3 / 30.0 ) );
			float temp_output_1_0_g590 = _Scale3;
			float Scale10_g590 = temp_output_1_0_g590;
			float2 break12_g590 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g590 ) ) * ( 1.0 / temp_output_1_0_g590 ) );
			float2 appendResult17_g590 = (float2(( ( _XOffset3 / Scale10_g590 ) + break12_g590.x ) , ( break12_g590.y + ( _ZOffset3 / Scale10_g590 ) )));
			float2 temp_output_1_0_g598 = appendResult17_g590;
			float4 tex2DNode8_g598 = tex2D( _RippleTex3, ( ( temp_output_32_0_g598 * float2( 0,1 ) ) + temp_output_1_0_g598 ) );
			float temp_output_17_0_g598 = ( ( tex2D( _RippleTex3, ( ( float2( 1,0 ) * temp_output_32_0_g598 ) + temp_output_1_0_g598 ) ).r - tex2DNode8_g598.r ) * 1.0 );
			float temp_output_18_0_g598 = ( 1.0 * ( tex2DNode8_g598.r - tex2D( _RippleTex3, temp_output_1_0_g598 ).r ) );
			float2 appendResult21_g598 = (float2(temp_output_17_0_g598 , temp_output_18_0_g598));
			float3 appendResult27_g598 = (float3(( -appendResult21_g598 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g598 , 1.0 ) ) - pow( temp_output_18_0_g598 , 1.0 ) ) )));
			float3 normalizeResult28_g598 = normalize( appendResult27_g598 );
			float2 appendResult37_g590 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g590 = ase_worldNormal;
			float3 appendResult4_g606 = (float3(( ( (normalizeResult28_g598).xy * appendResult37_g590 ) + (temp_output_30_0_g590).xz ) , (temp_output_30_0_g590).y));
			float3 worldToTangentDir6_g606 = normalize( mul( ase_worldToTangent, (appendResult4_g606).xzy) );
			float3 lerpResult1565 = lerp( ( worldToTangentDir6_g602 + worldToTangentDir6_g607 + worldToTangentDir6_g608 + worldToTangentDir6_g606 ) , float3(0,0,1) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / 100.0 ) , 0.5 ) ));
			float3 RippleNormals1566 = lerpResult1565;
			float3 lerpResult1570 = lerp( float3(0,0,1) , RippleNormals1566 , _RippleStrength1);
			float3 break1572 = lerpResult1570;
			float3 _Vector15 = float3(0,0,1);
			float temp_output_32_0_g595 = ( -0.02 / ( 1.0 / 30.0 ) );
			float2 appendResult1_g587 = (float2(0.15 , 0.0));
			float2 appendResult1598 = (float2(max( -_RangeVector.x , 0.0 ) , max( -_RangeVector.y , 0.0 )));
			float2 RangeX1599 = appendResult1598;
			float2 appendResult1597 = (float2(min( -_RangeVector.z , 0.0 ) , min( -_RangeVector.w , 0.0 )));
			float2 RangeZ1600 = appendResult1597;
			float2 ObjectScale1602 = _ObjectScale;
			float2 break1621 = ( (RangeX1599 + (i.uv_texcoord - float2( -0.5,-0.5 )) * (RangeZ1600 - RangeX1599) / (float2( 0.5,0.5 ) - float2( -0.5,-0.5 ))) * ObjectScale1602 );
			float3 appendResult1623 = (float3(break1621.x , 1.0 , break1621.y));
			float Displacement1604 = -10.0;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 temp_output_1628_0 = ( ( appendResult1623 * Displacement1604 ) + ase_vertex3Pos );
			float3 objToWorld1630 = mul( unity_ObjectToWorld, float4( temp_output_1628_0, 1 ) ).xyz;
			float3 TargetPoint1631 = objToWorld1630;
			float3 objToWorld1629 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 1 ) ).xyz;
			float3 StartPointGlobal1632 = objToWorld1629;
			float3 temp_output_1646_0 = ( TargetPoint1631 - StartPointGlobal1632 );
			float3 x1649 = ( temp_output_1646_0 / length( temp_output_1646_0 ) );
			float3 _Vector12 = float3(0,1,0);
			float3 PlaneNormal1609 = _Vector12;
			float3 appendResult1606 = (float3(0.0 , _waterLevel , 0.0));
			float3 PlaneOrigin1608 = appendResult1606;
			float dotResult1656 = dot( PlaneNormal1609 , ( TargetPoint1631 - PlaneOrigin1608 ) );
			float distV21658 = dotResult1656;
			float dotResult1655 = dot( PlaneNormal1609 , x1649 );
			float cosPhi1657 = dotResult1655;
			float3 IntersectionPoint1666 = ( TargetPoint1631 - ( x1649 * ( distV21658 / cosPhi1657 ) ) );
			float2 temp_output_1696_0 = ( (IntersectionPoint1666).xz * 0.5 );
			float2 panner3_g587 = ( _Time.y * appendResult1_g587 + temp_output_1696_0);
			float2 temp_output_1700_0 = panner3_g587;
			float2 temp_output_1_0_g595 = temp_output_1700_0;
			float4 tex2DNode8_g595 = tex2D( _RandomMask, ( ( temp_output_32_0_g595 * float2( 0,1 ) ) + temp_output_1_0_g595 ) );
			float temp_output_17_0_g595 = ( ( tex2D( _RandomMask, ( ( float2( 1,0 ) * temp_output_32_0_g595 ) + temp_output_1_0_g595 ) ).r - tex2DNode8_g595.r ) * 1.0 );
			float temp_output_18_0_g595 = ( 1.0 * ( tex2DNode8_g595.r - tex2D( _RandomMask, temp_output_1_0_g595 ).r ) );
			float2 appendResult21_g595 = (float2(temp_output_17_0_g595 , temp_output_18_0_g595));
			float3 appendResult27_g595 = (float3(( -appendResult21_g595 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g595 , 1.0 ) ) - pow( temp_output_18_0_g595 , 1.0 ) ) )));
			float3 normalizeResult28_g595 = normalize( appendResult27_g595 );
			float3 temp_output_21_0_g596 = ase_worldNormal;
			float temp_output_17_0_g596 = (temp_output_21_0_g596).y;
			float2 appendResult18_g596 = (float2(sign( temp_output_17_0_g596 ) , 1.0));
			float2 temp_output_1701_28 = appendResult18_g596;
			float2 temp_output_1701_26 = (temp_output_21_0_g596).xz;
			float temp_output_1701_27 = temp_output_17_0_g596;
			float3 appendResult4_g605 = (float3(( ( (normalizeResult28_g595).xy * temp_output_1701_28 ) + temp_output_1701_26 ) , temp_output_1701_27));
			float3 worldToTangentDir6_g605 = normalize( mul( ase_worldToTangent, (appendResult4_g605).xzy) );
			float temp_output_32_0_g599 = ( -0.02 / ( 1.0 / 30.0 ) );
			float2 appendResult1_g589 = (float2(0.3 , 0.0));
			float cos1697 = cos( radians( 180.0 ) );
			float sin1697 = sin( radians( 180.0 ) );
			float2 rotator1697 = mul( temp_output_1696_0 - float2( 0.5,0.5 ) , float2x2( cos1697 , -sin1697 , sin1697 , cos1697 )) + float2( 0.5,0.5 );
			float2 panner3_g589 = ( _Time.y * appendResult1_g589 + rotator1697);
			float2 temp_output_1699_0 = panner3_g589;
			float2 temp_output_1_0_g599 = temp_output_1699_0;
			float4 tex2DNode8_g599 = tex2D( _RandomMask, ( ( temp_output_32_0_g599 * float2( 0,1 ) ) + temp_output_1_0_g599 ) );
			float temp_output_17_0_g599 = ( ( tex2D( _RandomMask, ( ( float2( 1,0 ) * temp_output_32_0_g599 ) + temp_output_1_0_g599 ) ).r - tex2DNode8_g599.r ) * 1.0 );
			float temp_output_18_0_g599 = ( 1.0 * ( tex2DNode8_g599.r - tex2D( _RandomMask, temp_output_1_0_g599 ).r ) );
			float2 appendResult21_g599 = (float2(temp_output_17_0_g599 , temp_output_18_0_g599));
			float3 appendResult27_g599 = (float3(( -appendResult21_g599 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g599 , 1.0 ) ) - pow( temp_output_18_0_g599 , 1.0 ) ) )));
			float3 normalizeResult28_g599 = normalize( appendResult27_g599 );
			float3 appendResult4_g604 = (float3(( ( (normalizeResult28_g599).xy * temp_output_1701_28 ) + temp_output_1701_26 ) , temp_output_1701_27));
			float3 worldToTangentDir6_g604 = normalize( mul( ase_worldToTangent, (appendResult4_g604).xzy) );
			float3 lerpResult1709 = lerp( ( worldToTangentDir6_g605 + worldToTangentDir6_g604 ) , _Vector15 , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / 10.0 ) , 0.5 ) ));
			float3 lerpResult1711 = lerp( _Vector15 , lerpResult1709 , _PhysicalNormalStrength);
			float3 PhysicalNormals1712 = lerpResult1711;
			float3 break1733 = PhysicalNormals1712;
			float3 appendResult1575 = (float3(( break1571.x + break1572.x + break1733.x ) , ( break1571.y + break1572.y + break1733.y ) , break1571.z));
			float3 resultingNormal674 = appendResult1575;
			float3 break736 = resultingNormal674;
			float3 appendResult735 = (float3(break736.x , break736.y , 1));
			float3 CurrentNormal23_g623 = normalize( (WorldNormalVector( i , appendResult735 )) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult20_g623 = dot( CurrentNormal23_g623 , ase_worldlightDir );
			float NDotL21_g623 = dotResult20_g623;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 lerpResult164_g623 = lerp( float4( ( ase_lightColor.rgb * ase_lightAtten ) , 0.0 ) , UNITY_LIGHTMODEL_AMBIENT , ( 1.0 - ase_lightAtten ));
			float4 AttenuationColor8_g623 = lerpResult164_g623;
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float2 pseudoRefraction484 = ( (ase_grabScreenPosNorm).xy + ( temp_output_209_0 * (resultingNormal674).xy ) );
			float temp_output_141_0 = ( SceneDepth1392 - i.eyeDepth );
			float3 appendResult258 = (float3(temp_output_141_0 , temp_output_141_0 , temp_output_141_0));
			float3 clampResult142 = clamp( (float3( 1,1,1 ) + (appendResult258 - float3(0,0,0)) * (float3( 0,0,0 ) - float3( 1,1,1 )) / (( _MainColor * ( 1.0 / _Density ) ).rgb - float3(0,0,0))) , float3( 0,0,0 ) , float3( 1,1,1 ) );
			float3 temp_cast_9 = (_Fade).xxx;
			float4 blendOpSrc147 = _DeepWaterColor;
			float4 blendOpDest147 = ( tex2D( _ColorTex, pseudoRefraction484 ) * float4( pow( clampResult142 , temp_cast_9 ) , 0.0 ) );
			float4 waterColor488 = ( saturate( ( blendOpSrc147 + blendOpDest147 ) ));
			float4 break1578 = lerpResult670;
			float3 appendResult1582 = (float3(( break1572.x + break1578.r + break1733.x ) , ( break1572.y + break1578.g + break1733.y ) , break1578.b));
			float3 ResultingNormalForDistortion1584 = appendResult1582;
			float4 realtimeReflection600 = tex2D( _ReflectionTex, ( (ase_screenPosNorm).xy + ( (ResultingNormalForDistortion1584).xy * _Distortion ) ) );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
			float fresnelNdotV1378 = dot( mul(ase_tangentToWorldFast,resultingNormal674), ase_worldViewDir );
			float fresnelNode1378 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV1378, 2.0 ) );
			float temp_output_1380_0 = saturate( fresnelNode1378 );
			float4 lerpResult1377 = lerp( waterColor488 , realtimeReflection600 , ( temp_output_1380_0 * _RealtimeReflectionIntensity ));
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
			float4 lerpResult1382 = lerp( ( _EnableRealtimeReflections )?( lerpResult1377 ):( waterColor488 ) , float4( probeReflection766 , 0.0 ) , ( temp_output_1380_0 * _ProbeReflectionIntensity ));
			float temp_output_7_0_g617 = _FoamBlend;
			float2 appendResult1_g616 = (float2(_FoamSpeed , 0.0));
			float3 temp_output_21_0_g614 = ase_worldNormal;
			float temp_output_17_0_g614 = (temp_output_21_0_g614).y;
			float2 appendResult18_g614 = (float2(sign( temp_output_17_0_g614 ) , 1.0));
			float2 temp_output_1212_0 = ( ( appendResult18_g614 * (ase_worldPos).xz ) * _FoamTiling );
			float2 panner3_g616 = ( _Time.y * appendResult1_g616 + temp_output_1212_0);
			float2 appendResult1_g615 = (float2(_FoamSpeed , 0.0));
			float cos296 = cos( radians( 90.0 ) );
			float sin296 = sin( radians( 90.0 ) );
			float2 rotator296 = mul( temp_output_1212_0 - float2( 0.5,0.5 ) , float2x2( cos296 , -sin296 , sin296 , cos296 )) + float2( 0.5,0.5 );
			float2 panner3_g615 = ( _Time.y * appendResult1_g615 + rotator296);
			float3 desaturateInitialColor304 = ( tex2D( _FoamTexture, panner3_g616 ) - tex2D( _FoamTexture, panner3_g615 ) ).rgb;
			float desaturateDot304 = dot( desaturateInitialColor304, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar304 = lerp( desaturateInitialColor304, desaturateDot304.xxx, 1.0 );
			float3 temp_cast_13 = (_FoamContrast).xxx;
			float3 temp_cast_14 = (( 1.0 - _FoamContrast )).xxx;
			float2 _Vector3 = float2(0,1);
			float3 temp_cast_15 = (_Vector3.x).xxx;
			float3 temp_cast_16 = (_Vector3.y).xxx;
			float4 temp_output_319_0 = ( ( 1.0 - saturate( ( ( SceneDepth1392 - i.eyeDepth ) / temp_output_7_0_g617 ) ) ) * ( float4( (temp_cast_15 + (desaturateVar304 - temp_cast_13) * (temp_cast_16 - temp_cast_15) / (temp_cast_14 - temp_cast_13)) , 0.0 ) * _FoamColor * _FoamIntensity * -1.0 ) );
			float3 temp_cast_18 = (_FoamContrast).xxx;
			float3 temp_cast_19 = (( 1.0 - _FoamContrast )).xxx;
			float3 temp_cast_20 = (_Vector3.x).xxx;
			float3 temp_cast_21 = (_Vector3.y).xxx;
			float4 foam406 = ( temp_output_319_0 * temp_output_319_0 );
			float4 foamyWater490 = ( ( _EnableProbeRelfections )?( lerpResult1382 ):( ( _EnableRealtimeReflections )?( lerpResult1377 ):( waterColor488 ) ) + ( foam406 * _FoamVisibility ) );
			float clampResult100_g623 = clamp( ase_worldlightDir.y , ( length( (UNITY_LIGHTMODEL_AMBIENT).rgb ) / 3.0 ) , 1.0 );
			float4 DiffuseColor70_g623 = ( ( ( float4( max( ( LightWrapVector47_g623 + ( ( 1.0 - LightWrapVector47_g623 ) * NDotL21_g623 ) ) , float3(0,0,0) ) , 0.0 ) * AttenuationColor8_g623 ) * float4( foamyWater490.rgb , 0.0 ) ) * clampResult100_g623 );
			float3 normalizeResult77_g623 = normalize( ase_worldlightDir );
			float3 normalizeResult28_g623 = normalize( ( normalizeResult77_g623 + ase_worldViewDir ) );
			float3 HalfDirection29_g623 = normalizeResult28_g623;
			float dotResult32_g623 = dot( HalfDirection29_g623 , CurrentNormal23_g623 );
			float SpecularPower14_g623 = exp2( ( ( _Gloss * 10.0 ) + 1.0 ) );
			float temp_output_7_0_g619 = 0.0;
			float4 specularity504 = ( ( saturate( ( ( SceneDepth1392 - i.eyeDepth ) / temp_output_7_0_g619 ) ) * _Specular ) * _SpecularColor );
			float4 specularFinalColor42_g623 = ( AttenuationColor8_g623 * pow( max( dotResult32_g623 , 0.0 ) , SpecularPower14_g623 ) * float4( specularity504.rgb , 0.0 ) );
			float4 DiffuseSpecular83_g623 = ( DiffuseColor70_g623 + specularFinalColor42_g623 );
			float temp_output_7_0_g620 = _DepthTransparency;
			float opacity508 = pow( saturate( ( ( SceneDepth1392 - i.eyeDepth ) / temp_output_7_0_g620 ) ) , _TransparencyFade );
			float4 lerpResult87_g623 = lerp( tex2D( _ColorTex, ( (ase_screenPosNorm).xy + ( float2( 0.2,0 ) * refraction511 ) ) ) , DiffuseSpecular83_g623 , opacity508);
			c.rgb = ( lerpResult87_g623 * ase_lightAtten ).rgb;
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
	}
	Fallback "Diffuse"
}