Shader "AQUAS/Desktop/Back/Transparent Back"
{
	Properties
	{
		[NoScaleOffset][Header(Wave Options)]_NormalTexture("Normal Texture", 2D) = "bump" {}
		_NormalTiling("Normal Tiling", Range( 0.01 , 2)) = 1
		_NormalStrength("Normal Strength", Range( 0 , 2)) = 0
		_WaveSpeed("Wave Speed", Float) = 0
		_Refraction("Refraction", Range( 0 , 1)) = 0.1
		_DeepWaterColor("Deep Water Color", Color) = (0,0,0,0)
		[Header(Distance Options)]_MediumTilingDistance("Medium Tiling Distance", Float) = 0
		_FarTilingDistance("Far Tiling Distance", Float) = 0
		_DistanceFade("Distance Fade", Float) = 0
		[Header(Shoreline Waves)]_ShorelineFrequency("Shoreline Frequency", Float) = 0
		_ShorelineSpeed("Shoreline Speed", Range( 0 , 0.2)) = 0
		_ShorelineNormalStrength("Shoreline Normal Strength", Range( 0 , 1)) = 0
		_ShorelineBlend("Shoreline Blend", Range( 0 , 1)) = 0
		[NoScaleOffset]_ShorelineMask("Shoreline Mask", 2D) = "white" {}
		_RandomMask1("Random Mask", 2D) = "white" {}
		[NoScaleOffset][Header(Flowmap Options)]_FlowMap("FlowMap", 2D) = "white" {}
		_FlowSpeed("Flow Speed", Float) = 20
		[Toggle]_LinearColorSpace("Linear Color Space", Float) = 0
		[Header(Ripple Options)]_RippleStrength("Ripple Strength", Range( 0 , 1)) = 0.5
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
		[HideInInspector][Toggle]_ProjectGrid("Project Grid", Float) = 0
		[HideInInspector]_ObjectScale("Object Scale", Vector) = (0,0,0,0)
		[HideInInspector]_waterLevel("waterLevel", Float) = 0
		[HideInInspector]_RangeVector("Range Vector", Vector) = (0,0,0,0)
		_PhysicalNormalStrength("Physical Normal Strength", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
		[Header(Forward Rendering Options)]
		[ToggleOff] _GlossyReflections("Reflections", Float) = 1.0
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		LOD 200
		Cull Front
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.5
		#pragma shader_feature _GLOSSYREFLECTIONS_OFF
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma multi_compile __ LOD_FADE_CROSSFADE
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

		uniform float _ProjectGrid;
		uniform float4 _RangeVector;
		uniform float2 _ObjectScale;
		uniform float _waterLevel;
		uniform sampler2D _RandomMask1;
		uniform float4 _DeepWaterColor;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _Refraction;
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
		uniform float _PhysicalNormalStrength;


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
			float2 appendResult1435 = (float2(max( -_RangeVector.x , 0.0 ) , max( -_RangeVector.y , 0.0 )));
			float2 RangeX1436 = appendResult1435;
			float2 appendResult1434 = (float2(min( -_RangeVector.z , 0.0 ) , min( -_RangeVector.w , 0.0 )));
			float2 RangeZ1437 = appendResult1434;
			float2 ObjectScale1439 = _ObjectScale;
			float2 break1508 = ( (RangeX1436 + (v.texcoord.xy - float2( -0.5,-0.5 )) * (RangeZ1437 - RangeX1436) / (float2( 0.5,0.5 ) - float2( -0.5,-0.5 ))) * ObjectScale1439 );
			float3 appendResult1510 = (float3(break1508.x , 1.0 , break1508.y));
			float Displacement1441 = -10.0;
			float2 break1458 = ( (RangeX1436 + (v.texcoord.xy - float2( -0.5,-0.5 )) * (RangeZ1437 - RangeX1436) / (float2( 0.5,0.5 ) - float2( -0.5,-0.5 ))) * ObjectScale1439 );
			float3 appendResult1459 = (float3(break1458.x , 1.0 , break1458.y));
			float3 temp_output_1465_0 = ( ( appendResult1459 * Displacement1441 ) + ase_vertex3Pos );
			float3 objToWorld1466 = mul( unity_ObjectToWorld, float4( temp_output_1465_0, 1 ) ).xyz;
			float3 TargetPoint1469 = objToWorld1466;
			float3 objToWorld1467 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 1 ) ).xyz;
			float3 StartPointGlobal1468 = objToWorld1467;
			float3 temp_output_1473_0 = ( TargetPoint1469 - StartPointGlobal1468 );
			float3 x1477 = ( temp_output_1473_0 / length( temp_output_1473_0 ) );
			float3 _Vector11 = float3(0,1,0);
			float3 PlaneNormal1446 = _Vector11;
			float3 appendResult1443 = (float3(0.0 , _waterLevel , 0.0));
			float3 PlaneOrigin1444 = appendResult1443;
			float dotResult1483 = dot( PlaneNormal1446 , ( TargetPoint1469 - PlaneOrigin1444 ) );
			float distV21485 = dotResult1483;
			float dotResult1482 = dot( PlaneNormal1446 , x1477 );
			float cosPhi1484 = dotResult1482;
			float3 IntersectionPoint1493 = ( TargetPoint1469 - ( x1477 * ( distV21485 / cosPhi1484 ) ) );
			float3 Projection1514 = ( ( appendResult1510 * ( Displacement1441 * ( distance( IntersectionPoint1493 , StartPointGlobal1468 ) / distance( StartPointGlobal1468 , TargetPoint1469 ) ) ) ) + ase_vertex3Pos );
			float2 appendResult1_g615 = (float2(0.15 , 0.0));
			float2 temp_output_1531_0 = ( (IntersectionPoint1493).xz * 0.5 );
			float2 panner3_g615 = ( _Time.y * appendResult1_g615 + temp_output_1531_0);
			float2 temp_output_1535_0 = panner3_g615;
			float2 appendResult1_g619 = (float2(0.3 , 0.0));
			float cos1532 = cos( radians( 180.0 ) );
			float sin1532 = sin( radians( 180.0 ) );
			float2 rotator1532 = mul( temp_output_1531_0 - float2( 0.5,0.5 ) , float2x2( cos1532 , -sin1532 , sin1532 , cos1532 )) + float2( 0.5,0.5 );
			float2 panner3_g619 = ( _Time.y * appendResult1_g619 + rotator1532);
			float2 temp_output_1534_0 = panner3_g619;
			float4 _Vector17 = float4(0,2,-0.15,0.15);
			float4 temp_cast_2 = (_Vector17.x).xxxx;
			float4 temp_cast_3 = (_Vector17.y).xxxx;
			float4 temp_cast_4 = (_Vector17.z).xxxx;
			float4 temp_cast_5 = (_Vector17.w).xxxx;
			float4 lerpResult1556 = lerp( (temp_cast_4 + (( tex2Dlod( _RandomMask1, float4( temp_output_1535_0, 0, 0.0) ) + tex2Dlod( _RandomMask1, float4( temp_output_1534_0, 0, 0.0) ) ) - temp_cast_2) * (temp_cast_5 - temp_cast_4) / (temp_cast_3 - temp_cast_2)) , float4(0,0,0,0) , saturate( pow( ( distance( IntersectionPoint1493 , _WorldSpaceCameraPos ) / 1.0 ) , 0.5 ) ));
			float4 PhysicalWaves1557 = lerpResult1556;
			float3 worldToObjDir1447 = mul( unity_WorldToObject, float4( _Vector11, 0 ) ).xyz;
			float3 VertexNormal1448 = worldToObjDir1447;
			v.vertex.xyz = (( _ProjectGrid )?( ( float4( Projection1514 , 0.0 ) + ( PhysicalWaves1557 * float4( VertexNormal1448 , 0.0 ) ) ) ):( float4( ase_vertex3Pos , 0.0 ) )).xyz;
			float3 temp_output_1564_0 = VertexNormal1448;
			v.normal = temp_output_1564_0;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			o.Normal = float3(0,0,1);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float temp_output_209_0 = ( _Refraction * 0.2 );
			float waveSpeed675 = _WaveSpeed;
			float2 appendResult1_g311 = (float2(waveSpeed675 , 0.0));
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
			float4 flowMap1263 = (( _LinearColorSpace )?( pow( tex2DNode1258 , temp_cast_0 ) ):( tex2DNode1258 ));
			float FlowSpeed1256 = _FlowSpeed;
			float2 temp_output_1277_0 = ( (float2( -0.5,-0.5 ) + ((flowMap1263).rg - float2( 0,0 )) * (float2( 0.5,0.5 ) - float2( -0.5,-0.5 )) / (float2( 1,1 ) - float2( 0,0 ))) * FlowSpeed1256 );
			float mulTime1269 = _Time.y * 0.05;
			float temp_output_1279_0 = frac( mulTime1269 );
			float2 flowUV1Close1290 = ( temp_output_1277_0 * temp_output_1279_0 );
			float2 temp_output_1305_0 = ( ( BaseUV1197 * normalTiling618 ) + ( normalTiling618 * flowUV1Close1290 ) );
			float2 panner3_g311 = ( _Time.y * appendResult1_g311 + temp_output_1305_0);
			float2 appendResult1_g312 = (float2(waveSpeed675 , 0.0));
			float cos30 = cos( radians( 180.0 ) );
			float sin30 = sin( radians( 180.0 ) );
			float2 rotator30 = mul( temp_output_1305_0 - float2( 0.5,0.5 ) , float2x2( cos30 , -sin30 , sin30 , cos30 )) + float2( 0.5,0.5 );
			float2 panner3_g312 = ( _Time.y * appendResult1_g312 + rotator30);
			float normalStrength681 = _NormalStrength;
			float3 lerpResult67 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g311 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g312 ) ) ) , normalStrength681);
			float3 WavesCloseUV1207 = lerpResult67;
			float2 appendResult1_g315 = (float2(waveSpeed675 , 0.0));
			float2 flowUV2Close1289 = ( temp_output_1277_0 * frac( ( mulTime1269 + 0.5 ) ) );
			float2 temp_output_1324_0 = ( ( BaseUV1197 * normalTiling618 ) + ( normalTiling618 * flowUV2Close1289 ) );
			float2 panner3_g315 = ( _Time.y * appendResult1_g315 + temp_output_1324_0);
			float2 appendResult1_g316 = (float2(waveSpeed675 , 0.0));
			float cos1327 = cos( radians( 180.0 ) );
			float sin1327 = sin( radians( 180.0 ) );
			float2 rotator1327 = mul( temp_output_1324_0 - float2( 0.5,0.5 ) , float2x2( cos1327 , -sin1327 , sin1327 , cos1327 )) + float2( 0.5,0.5 );
			float2 panner3_g316 = ( _Time.y * appendResult1_g316 + rotator1327);
			float3 lerpResult1333 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g315 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g316 ) ) ) , normalStrength681);
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
			float2 appendResult1_g252 = (float2(0.1 , 0.0));
			float3 temp_output_21_0_g228 = ase_worldNormal;
			float temp_output_17_0_g228 = (temp_output_21_0_g228).y;
			float2 appendResult18_g228 = (float2(sign( temp_output_17_0_g228 ) , 1.0));
			float2 temp_output_1211_0 = ( ( appendResult18_g228 * (ase_worldPos).xz ) * float2( 0.05,0.05 ) );
			float2 panner3_g252 = ( _Time.y * appendResult1_g252 + temp_output_1211_0);
			float2 appendResult1_g251 = (float2(0.2 , 0.0));
			float cos1177 = cos( radians( 180.0 ) );
			float sin1177 = sin( radians( 180.0 ) );
			float2 rotator1177 = mul( temp_output_1211_0 - float2( 0.5,0.5 ) , float2x2( cos1177 , -sin1177 , sin1177 , cos1177 )) + float2( 0.5,0.5 );
			float2 panner3_g251 = ( _Time.y * appendResult1_g251 + rotator1177);
			float4 ShorelineNormal817 = ( float4( lerpResult824 , 0.0 ) * ( 1.0 - step( tex2DNode912.r , _ShorelineBlend ) ) * ( ( tex2D( _RandomMask1, panner3_g252 ) + tex2D( _RandomMask1, panner3_g251 ) ) / 2.0 ) );
			float4 NormalsClose1340 = ( float4( lerpResult1338 , 0.0 ) + ShorelineNormal817 );
			float temp_output_678_0 = ( waveSpeed675 / 10.0 );
			float2 appendResult1_g313 = (float2(temp_output_678_0 , 0.0));
			float temp_output_642_0 = ( normalTiling618 / 10.0 );
			float FlowSpeedMedium1300 = ( FlowSpeed1256 / 1.0 );
			float2 temp_output_1280_0 = ( (float2( -0.5,-0.5 ) + ((flowMap1263).rg - float2( 0,0 )) * (float2( 0.5,0.5 ) - float2( -0.5,-0.5 )) / (float2( 1,1 ) - float2( 0,0 ))) * FlowSpeedMedium1300 );
			float mulTime1270 = _Time.y * 0.05;
			float temp_output_1282_0 = frac( mulTime1270 );
			float2 flowUV1Medium1288 = ( temp_output_1280_0 * temp_output_1282_0 );
			float2 temp_output_1347_0 = ( ( BaseUV1197 * temp_output_642_0 ) + ( temp_output_642_0 * flowUV1Medium1288 ) );
			float2 panner3_g313 = ( _Time.y * appendResult1_g313 + temp_output_1347_0);
			float2 appendResult1_g314 = (float2(temp_output_678_0 , 0.0));
			float cos630 = cos( radians( 180.0 ) );
			float sin630 = sin( radians( 180.0 ) );
			float2 rotator630 = mul( temp_output_1347_0 - float2( 0.5,0.5 ) , float2x2( cos630 , -sin630 , sin630 , cos630 )) + float2( 0.5,0.5 );
			float2 panner3_g314 = ( _Time.y * appendResult1_g314 + rotator630);
			float mediumTilingDistance687 = _MediumTilingDistance;
			float tilingFade689 = _DistanceFade;
			float lerpResult693 = lerp( normalStrength681 , ( normalStrength681 / 20.0 ) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / mediumTilingDistance687 ) , tilingFade689 ) ));
			float normalStrengthMedium706 = lerpResult693;
			float3 lerpResult639 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g313 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g314 ) ) ) , normalStrengthMedium706);
			float3 WavesMediumUv1640 = lerpResult639;
			float temp_output_1362_0 = ( waveSpeed675 / 10.0 );
			float2 appendResult1_g310 = (float2(temp_output_1362_0 , 0.0));
			float temp_output_1358_0 = ( normalTiling618 / 10.0 );
			float2 flowUV2Medium1287 = ( temp_output_1280_0 * frac( ( mulTime1270 + 0.5 ) ) );
			float2 temp_output_1360_0 = ( ( BaseUV1197 * temp_output_1358_0 ) + ( temp_output_1358_0 * flowUV2Medium1287 ) );
			float2 panner3_g310 = ( _Time.y * appendResult1_g310 + temp_output_1360_0);
			float2 appendResult1_g317 = (float2(temp_output_1362_0 , 0.0));
			float cos1361 = cos( radians( 180.0 ) );
			float sin1361 = sin( radians( 180.0 ) );
			float2 rotator1361 = mul( temp_output_1360_0 - float2( 0.5,0.5 ) , float2x2( cos1361 , -sin1361 , sin1361 , cos1361 )) + float2( 0.5,0.5 );
			float2 panner3_g317 = ( _Time.y * appendResult1_g317 + rotator1361);
			float3 lerpResult1367 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g310 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g317 ) ) ) , normalStrengthMedium706);
			float3 WavesMediumUV21368 = lerpResult1367;
			float flowLerpMedium1297 = abs( ( ( 0.5 - temp_output_1282_0 ) / 0.5 ) );
			float3 lerpResult1369 = lerp( WavesMediumUv1640 , WavesMediumUV21368 , flowLerpMedium1297);
			float4 NormalsMedium1373 = ( float4( lerpResult1369 , 0.0 ) + ShorelineNormal817 );
			float4 lerpResult664 = lerp( NormalsClose1340 , NormalsMedium1373 , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / mediumTilingDistance687 ) , tilingFade689 ) ));
			float temp_output_680_0 = ( waveSpeed675 / 30.0 );
			float2 appendResult1_g620 = (float2(temp_output_680_0 , 0.0));
			float2 temp_output_1201_0 = ( BaseUV1197 * ( normalTiling618 / 1200.0 ) );
			float2 panner3_g620 = ( _Time.y * appendResult1_g620 + temp_output_1201_0);
			float2 appendResult1_g617 = (float2(temp_output_680_0 , 0.0));
			float cos646 = cos( radians( 180.0 ) );
			float sin646 = sin( radians( 180.0 ) );
			float2 rotator646 = mul( temp_output_1201_0 - float2( 0.5,0.5 ) , float2x2( cos646 , -sin646 , sin646 , cos646 )) + float2( 0.5,0.5 );
			float2 panner3_g617 = ( _Time.y * appendResult1_g617 + rotator646);
			float farTilingDistance688 = _FarTilingDistance;
			float lerpResult698 = lerp( normalStrengthMedium706 , ( lerpResult693 / 20.0 ) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / farTilingDistance688 ) , tilingFade689 ) ));
			float normalStrengthFar704 = lerpResult698;
			float3 lerpResult657 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g620 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g617 ) ) ) , normalStrengthFar704);
			float3 NormalsFar660 = lerpResult657;
			float4 lerpResult670 = lerp( lerpResult664 , float4( NormalsFar660 , 0.0 ) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / farTilingDistance688 ) , tilingFade689 ) ));
			float2 NormalSign1123 = appendResult18_g227;
			float2 WorldNormalXZ1122 = (temp_output_21_0_g227).xz;
			float WorldNormalY1121 = temp_output_17_0_g227;
			float3 appendResult4_g640 = (float3(( ( (lerpResult670.rgb).xy * NormalSign1123 ) + WorldNormalXZ1122 ) , WorldNormalY1121));
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentDir6_g640 = normalize( mul( ase_worldToTangent, (appendResult4_g640).xzy) );
			float3 break1421 = worldToTangentDir6_g640;
			float temp_output_32_0_g627 = ( -0.02 / ( _Scale0 / 30.0 ) );
			float temp_output_1_0_g621 = _Scale0;
			float Scale10_g621 = temp_output_1_0_g621;
			float2 break12_g621 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g621 ) ) * ( 1.0 / temp_output_1_0_g621 ) );
			float2 appendResult17_g621 = (float2(( ( _XOffset0 / Scale10_g621 ) + break12_g621.x ) , ( break12_g621.y + ( _ZOffset0 / Scale10_g621 ) )));
			float2 temp_output_1_0_g627 = appendResult17_g621;
			float4 tex2DNode8_g627 = tex2D( _RippleTex0, ( ( temp_output_32_0_g627 * float2( 0,1 ) ) + temp_output_1_0_g627 ) );
			float temp_output_17_0_g627 = ( ( tex2D( _RippleTex0, ( ( float2( 1,0 ) * temp_output_32_0_g627 ) + temp_output_1_0_g627 ) ).r - tex2DNode8_g627.r ) * 1.0 );
			float temp_output_18_0_g627 = ( 1.0 * ( tex2DNode8_g627.r - tex2D( _RippleTex0, temp_output_1_0_g627 ).r ) );
			float2 appendResult21_g627 = (float2(temp_output_17_0_g627 , temp_output_18_0_g627));
			float3 appendResult27_g627 = (float3(( -appendResult21_g627 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g627 , 1.0 ) ) - pow( temp_output_18_0_g627 , 1.0 ) ) )));
			float3 normalizeResult28_g627 = normalize( appendResult27_g627 );
			float2 appendResult37_g621 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g621 = ase_worldNormal;
			float3 appendResult4_g630 = (float3(( ( (normalizeResult28_g627).xy * appendResult37_g621 ) + (temp_output_30_0_g621).xz ) , (temp_output_30_0_g621).y));
			float3 worldToTangentDir6_g630 = normalize( mul( ase_worldToTangent, (appendResult4_g630).xzy) );
			float temp_output_32_0_g628 = ( -0.02 / ( _Scale1 / 30.0 ) );
			float temp_output_1_0_g618 = _Scale1;
			float Scale10_g618 = temp_output_1_0_g618;
			float2 break12_g618 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g618 ) ) * ( 1.0 / temp_output_1_0_g618 ) );
			float2 appendResult17_g618 = (float2(( ( _XOffset1 / Scale10_g618 ) + break12_g618.x ) , ( break12_g618.y + ( _ZOffset1 / Scale10_g618 ) )));
			float2 temp_output_1_0_g628 = appendResult17_g618;
			float4 tex2DNode8_g628 = tex2D( _RippleTex1, ( ( temp_output_32_0_g628 * float2( 0,1 ) ) + temp_output_1_0_g628 ) );
			float temp_output_17_0_g628 = ( ( tex2D( _RippleTex1, ( ( float2( 1,0 ) * temp_output_32_0_g628 ) + temp_output_1_0_g628 ) ).r - tex2DNode8_g628.r ) * 1.0 );
			float temp_output_18_0_g628 = ( 1.0 * ( tex2DNode8_g628.r - tex2D( _RippleTex1, temp_output_1_0_g628 ).r ) );
			float2 appendResult21_g628 = (float2(temp_output_17_0_g628 , temp_output_18_0_g628));
			float3 appendResult27_g628 = (float3(( -appendResult21_g628 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g628 , 1.0 ) ) - pow( temp_output_18_0_g628 , 1.0 ) ) )));
			float3 normalizeResult28_g628 = normalize( appendResult27_g628 );
			float2 appendResult37_g618 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g618 = ase_worldNormal;
			float3 appendResult4_g633 = (float3(( ( (normalizeResult28_g628).xy * appendResult37_g618 ) + (temp_output_30_0_g618).xz ) , (temp_output_30_0_g618).y));
			float3 worldToTangentDir6_g633 = normalize( mul( ase_worldToTangent, (appendResult4_g633).xzy) );
			float temp_output_32_0_g626 = ( -0.02 / ( _Scale2 / 30.0 ) );
			float temp_output_1_0_g622 = _Scale2;
			float Scale10_g622 = temp_output_1_0_g622;
			float2 break12_g622 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g622 ) ) * ( 1.0 / temp_output_1_0_g622 ) );
			float2 appendResult17_g622 = (float2(( ( _XOffset2 / Scale10_g622 ) + break12_g622.x ) , ( break12_g622.y + ( _ZOffset2 / Scale10_g622 ) )));
			float2 temp_output_1_0_g626 = appendResult17_g622;
			float4 tex2DNode8_g626 = tex2D( _RippleTex2, ( ( temp_output_32_0_g626 * float2( 0,1 ) ) + temp_output_1_0_g626 ) );
			float temp_output_17_0_g626 = ( ( tex2D( _RippleTex2, ( ( float2( 1,0 ) * temp_output_32_0_g626 ) + temp_output_1_0_g626 ) ).r - tex2DNode8_g626.r ) * 1.0 );
			float temp_output_18_0_g626 = ( 1.0 * ( tex2DNode8_g626.r - tex2D( _RippleTex2, temp_output_1_0_g626 ).r ) );
			float2 appendResult21_g626 = (float2(temp_output_17_0_g626 , temp_output_18_0_g626));
			float3 appendResult27_g626 = (float3(( -appendResult21_g626 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g626 , 1.0 ) ) - pow( temp_output_18_0_g626 , 1.0 ) ) )));
			float3 normalizeResult28_g626 = normalize( appendResult27_g626 );
			float2 appendResult37_g622 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g622 = ase_worldNormal;
			float3 appendResult4_g634 = (float3(( ( (normalizeResult28_g626).xy * appendResult37_g622 ) + (temp_output_30_0_g622).xz ) , (temp_output_30_0_g622).y));
			float3 worldToTangentDir6_g634 = normalize( mul( ase_worldToTangent, (appendResult4_g634).xzy) );
			float temp_output_32_0_g624 = ( -0.02 / ( _Scale3 / 30.0 ) );
			float temp_output_1_0_g616 = _Scale3;
			float Scale10_g616 = temp_output_1_0_g616;
			float2 break12_g616 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g616 ) ) * ( 1.0 / temp_output_1_0_g616 ) );
			float2 appendResult17_g616 = (float2(( ( _XOffset3 / Scale10_g616 ) + break12_g616.x ) , ( break12_g616.y + ( _ZOffset3 / Scale10_g616 ) )));
			float2 temp_output_1_0_g624 = appendResult17_g616;
			float4 tex2DNode8_g624 = tex2D( _RippleTex3, ( ( temp_output_32_0_g624 * float2( 0,1 ) ) + temp_output_1_0_g624 ) );
			float temp_output_17_0_g624 = ( ( tex2D( _RippleTex3, ( ( float2( 1,0 ) * temp_output_32_0_g624 ) + temp_output_1_0_g624 ) ).r - tex2DNode8_g624.r ) * 1.0 );
			float temp_output_18_0_g624 = ( 1.0 * ( tex2DNode8_g624.r - tex2D( _RippleTex3, temp_output_1_0_g624 ).r ) );
			float2 appendResult21_g624 = (float2(temp_output_17_0_g624 , temp_output_18_0_g624));
			float3 appendResult27_g624 = (float3(( -appendResult21_g624 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g624 , 1.0 ) ) - pow( temp_output_18_0_g624 , 1.0 ) ) )));
			float3 normalizeResult28_g624 = normalize( appendResult27_g624 );
			float2 appendResult37_g616 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g616 = ase_worldNormal;
			float3 appendResult4_g635 = (float3(( ( (normalizeResult28_g624).xy * appendResult37_g616 ) + (temp_output_30_0_g616).xz ) , (temp_output_30_0_g616).y));
			float3 worldToTangentDir6_g635 = normalize( mul( ase_worldToTangent, (appendResult4_g635).xzy) );
			float3 lerpResult1414 = lerp( ( worldToTangentDir6_g630 + worldToTangentDir6_g633 + worldToTangentDir6_g634 + worldToTangentDir6_g635 ) , float3(0,0,1) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / 100.0 ) , 0.5 ) ));
			float3 RippleNormals1415 = lerpResult1414;
			float3 lerpResult1419 = lerp( float3(0,0,1) , RippleNormals1415 , _RippleStrength);
			float3 break1420 = lerpResult1419;
			float3 _Vector16 = float3(0,0,1);
			float temp_output_32_0_g625 = ( -0.02 / ( 1.0 / 30.0 ) );
			float2 appendResult1_g615 = (float2(0.15 , 0.0));
			float2 appendResult1435 = (float2(max( -_RangeVector.x , 0.0 ) , max( -_RangeVector.y , 0.0 )));
			float2 RangeX1436 = appendResult1435;
			float2 appendResult1434 = (float2(min( -_RangeVector.z , 0.0 ) , min( -_RangeVector.w , 0.0 )));
			float2 RangeZ1437 = appendResult1434;
			float2 ObjectScale1439 = _ObjectScale;
			float2 break1458 = ( (RangeX1436 + (i.uv_texcoord - float2( -0.5,-0.5 )) * (RangeZ1437 - RangeX1436) / (float2( 0.5,0.5 ) - float2( -0.5,-0.5 ))) * ObjectScale1439 );
			float3 appendResult1459 = (float3(break1458.x , 1.0 , break1458.y));
			float Displacement1441 = -10.0;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 temp_output_1465_0 = ( ( appendResult1459 * Displacement1441 ) + ase_vertex3Pos );
			float3 objToWorld1466 = mul( unity_ObjectToWorld, float4( temp_output_1465_0, 1 ) ).xyz;
			float3 TargetPoint1469 = objToWorld1466;
			float3 objToWorld1467 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 1 ) ).xyz;
			float3 StartPointGlobal1468 = objToWorld1467;
			float3 temp_output_1473_0 = ( TargetPoint1469 - StartPointGlobal1468 );
			float3 x1477 = ( temp_output_1473_0 / length( temp_output_1473_0 ) );
			float3 _Vector11 = float3(0,1,0);
			float3 PlaneNormal1446 = _Vector11;
			float3 appendResult1443 = (float3(0.0 , _waterLevel , 0.0));
			float3 PlaneOrigin1444 = appendResult1443;
			float dotResult1483 = dot( PlaneNormal1446 , ( TargetPoint1469 - PlaneOrigin1444 ) );
			float distV21485 = dotResult1483;
			float dotResult1482 = dot( PlaneNormal1446 , x1477 );
			float cosPhi1484 = dotResult1482;
			float3 IntersectionPoint1493 = ( TargetPoint1469 - ( x1477 * ( distV21485 / cosPhi1484 ) ) );
			float2 temp_output_1531_0 = ( (IntersectionPoint1493).xz * 0.5 );
			float2 panner3_g615 = ( _Time.y * appendResult1_g615 + temp_output_1531_0);
			float2 temp_output_1535_0 = panner3_g615;
			float2 temp_output_1_0_g625 = temp_output_1535_0;
			float4 tex2DNode8_g625 = tex2D( _RandomMask1, ( ( temp_output_32_0_g625 * float2( 0,1 ) ) + temp_output_1_0_g625 ) );
			float temp_output_17_0_g625 = ( ( tex2D( _RandomMask1, ( ( float2( 1,0 ) * temp_output_32_0_g625 ) + temp_output_1_0_g625 ) ).r - tex2DNode8_g625.r ) * 1.0 );
			float temp_output_18_0_g625 = ( 1.0 * ( tex2DNode8_g625.r - tex2D( _RandomMask1, temp_output_1_0_g625 ).r ) );
			float2 appendResult21_g625 = (float2(temp_output_17_0_g625 , temp_output_18_0_g625));
			float3 appendResult27_g625 = (float3(( -appendResult21_g625 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g625 , 1.0 ) ) - pow( temp_output_18_0_g625 , 1.0 ) ) )));
			float3 normalizeResult28_g625 = normalize( appendResult27_g625 );
			float3 temp_output_21_0_g629 = ase_worldNormal;
			float temp_output_17_0_g629 = (temp_output_21_0_g629).y;
			float2 appendResult18_g629 = (float2(sign( temp_output_17_0_g629 ) , 1.0));
			float2 temp_output_1538_28 = appendResult18_g629;
			float2 temp_output_1538_26 = (temp_output_21_0_g629).xz;
			float temp_output_1538_27 = temp_output_17_0_g629;
			float3 appendResult4_g631 = (float3(( ( (normalizeResult28_g625).xy * temp_output_1538_28 ) + temp_output_1538_26 ) , temp_output_1538_27));
			float3 worldToTangentDir6_g631 = normalize( mul( ase_worldToTangent, (appendResult4_g631).xzy) );
			float temp_output_32_0_g623 = ( -0.02 / ( 1.0 / 30.0 ) );
			float2 appendResult1_g619 = (float2(0.3 , 0.0));
			float cos1532 = cos( radians( 180.0 ) );
			float sin1532 = sin( radians( 180.0 ) );
			float2 rotator1532 = mul( temp_output_1531_0 - float2( 0.5,0.5 ) , float2x2( cos1532 , -sin1532 , sin1532 , cos1532 )) + float2( 0.5,0.5 );
			float2 panner3_g619 = ( _Time.y * appendResult1_g619 + rotator1532);
			float2 temp_output_1534_0 = panner3_g619;
			float2 temp_output_1_0_g623 = temp_output_1534_0;
			float4 tex2DNode8_g623 = tex2D( _RandomMask1, ( ( temp_output_32_0_g623 * float2( 0,1 ) ) + temp_output_1_0_g623 ) );
			float temp_output_17_0_g623 = ( ( tex2D( _RandomMask1, ( ( float2( 1,0 ) * temp_output_32_0_g623 ) + temp_output_1_0_g623 ) ).r - tex2DNode8_g623.r ) * 1.0 );
			float temp_output_18_0_g623 = ( 1.0 * ( tex2DNode8_g623.r - tex2D( _RandomMask1, temp_output_1_0_g623 ).r ) );
			float2 appendResult21_g623 = (float2(temp_output_17_0_g623 , temp_output_18_0_g623));
			float3 appendResult27_g623 = (float3(( -appendResult21_g623 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g623 , 1.0 ) ) - pow( temp_output_18_0_g623 , 1.0 ) ) )));
			float3 normalizeResult28_g623 = normalize( appendResult27_g623 );
			float3 appendResult4_g632 = (float3(( ( (normalizeResult28_g623).xy * temp_output_1538_28 ) + temp_output_1538_26 ) , temp_output_1538_27));
			float3 worldToTangentDir6_g632 = normalize( mul( ase_worldToTangent, (appendResult4_g632).xzy) );
			float3 lerpResult1544 = lerp( ( worldToTangentDir6_g631 + worldToTangentDir6_g632 ) , _Vector16 , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / 10.0 ) , 0.5 ) ));
			float3 lerpResult1546 = lerp( _Vector16 , lerpResult1544 , _PhysicalNormalStrength);
			float3 PhysicalNormals1547 = lerpResult1546;
			float3 break1561 = PhysicalNormals1547;
			float3 appendResult1424 = (float3(( break1421.x + break1420.x + break1561.x ) , ( break1421.y + break1420.y + break1561.y ) , break1421.z));
			float3 resultingNormal674 = appendResult1424;
			float2 pseudoRefraction484 = ( (ase_grabScreenPosNorm).xy + ( temp_output_209_0 * (resultingNormal674).xy ) );
			float4 screenColor1381 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,pseudoRefraction484);
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
			float fresnelNdotV1379 = dot( mul(ase_tangentToWorldFast,resultingNormal674), ase_worldViewDir );
			float fresnelNode1379 = ( 0.0 + 0.05 * pow( 1.0 - fresnelNdotV1379, 10.0 ) );
			float4 lerpResult1378 = lerp( _DeepWaterColor , screenColor1381 , saturate( fresnelNode1379 ));
			o.Emission = lerpResult1378.rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5
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
				vertexDataFunc( v, customInputData );
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
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
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