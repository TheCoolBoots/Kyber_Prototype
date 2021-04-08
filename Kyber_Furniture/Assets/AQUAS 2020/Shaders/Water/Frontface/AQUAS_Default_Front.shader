Shader "AQUAS/Desktop/Front/Default"
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
		[HideInInspector]_PhysicalNormalStrength("Physical Normal Strength", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.5
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf StandardCustomLighting keepalpha noshadow vertex:vertexDataFunc 
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
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform sampler2D _NormalTexture;
		uniform float _WaveSpeed;
		uniform float _NormalTiling;
		uniform float _LinearColorSpace;
		uniform sampler2D _FlowMap;
		uniform float _FlowSpeed;
		uniform float _NormalStrength;
		uniform float _Refraction;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
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
		uniform float _RippleStrength;
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
			float2 appendResult1543 = (float2(max( -_RangeVector.x , 0.0 ) , max( -_RangeVector.y , 0.0 )));
			float2 RangeX1546 = appendResult1543;
			float2 appendResult1544 = (float2(min( -_RangeVector.z , 0.0 ) , min( -_RangeVector.w , 0.0 )));
			float2 RangeZ1545 = appendResult1544;
			float2 ObjectScale1549 = _ObjectScale;
			float2 break1604 = ( (RangeX1546 + (v.texcoord.xy - float2( -0.5,-0.5 )) * (RangeZ1545 - RangeX1546) / (float2( 0.5,0.5 ) - float2( -0.5,-0.5 ))) * ObjectScale1549 );
			float3 appendResult1606 = (float3(break1604.x , 1.0 , break1604.y));
			float Displacement1557 = -10.0;
			float2 break1556 = ( (RangeX1546 + (v.texcoord.xy - float2( -0.5,-0.5 )) * (RangeZ1545 - RangeX1546) / (float2( 0.5,0.5 ) - float2( -0.5,-0.5 ))) * ObjectScale1549 );
			float3 appendResult1559 = (float3(break1556.x , 1.0 , break1556.y));
			float3 objToWorld1565 = mul( unity_ObjectToWorld, float4( ( ( appendResult1559 * Displacement1557 ) + ase_vertex3Pos ), 1 ) ).xyz;
			float3 TargetPoint1567 = objToWorld1565;
			float3 objToWorld1564 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 1 ) ).xyz;
			float3 StartPointGlobal1566 = objToWorld1564;
			float3 temp_output_1570_0 = ( TargetPoint1567 - StartPointGlobal1566 );
			float3 x1575 = ( temp_output_1570_0 / length( temp_output_1570_0 ) );
			float3 _Vector11 = float3(0,1,0);
			float3 PlaneNormal1574 = _Vector11;
			float3 appendResult1621 = (float3(0.0 , _waterLevel , 0.0));
			float3 PlaneOrigin1613 = appendResult1621;
			float dotResult1579 = dot( PlaneNormal1574 , ( TargetPoint1567 - PlaneOrigin1613 ) );
			float distV21582 = dotResult1579;
			float dotResult1580 = dot( PlaneNormal1574 , x1575 );
			float cosPhi1581 = dotResult1580;
			float3 IntersectionPoint1592 = ( TargetPoint1567 - ( x1575 * ( distV21582 / cosPhi1581 ) ) );
			float3 Projection1611 = ( ( appendResult1606 * ( Displacement1557 * ( distance( IntersectionPoint1592 , StartPointGlobal1566 ) / distance( StartPointGlobal1566 , TargetPoint1567 ) ) ) ) + ase_vertex3Pos );
			float2 appendResult1_g632 = (float2(0.15 , 0.0));
			float2 temp_output_1640_0 = ( (IntersectionPoint1592).xz * 0.5 );
			float2 panner3_g632 = ( _Time.y * appendResult1_g632 + temp_output_1640_0);
			float2 temp_output_1643_0 = panner3_g632;
			float2 appendResult1_g628 = (float2(0.3 , 0.0));
			float cos1642 = cos( radians( 180.0 ) );
			float sin1642 = sin( radians( 180.0 ) );
			float2 rotator1642 = mul( temp_output_1640_0 - float2( 0.5,0.5 ) , float2x2( cos1642 , -sin1642 , sin1642 , cos1642 )) + float2( 0.5,0.5 );
			float2 panner3_g628 = ( _Time.y * appendResult1_g628 + rotator1642);
			float2 temp_output_1644_0 = panner3_g628;
			float4 _Vector12 = float4(0,2,-0.15,0.15);
			float4 temp_cast_2 = (_Vector12.x).xxxx;
			float4 temp_cast_3 = (_Vector12.y).xxxx;
			float4 temp_cast_4 = (_Vector12.z).xxxx;
			float4 temp_cast_5 = (_Vector12.w).xxxx;
			float4 lerpResult1667 = lerp( (temp_cast_4 + (( tex2Dlod( _RandomMask, float4( temp_output_1643_0, 0, 0.0) ) + tex2Dlod( _RandomMask, float4( temp_output_1644_0, 0, 0.0) ) ) - temp_cast_2) * (temp_cast_5 - temp_cast_4) / (temp_cast_3 - temp_cast_2)) , float4(0,0,0,0) , saturate( pow( ( distance( IntersectionPoint1592 , _WorldSpaceCameraPos ) / 1.0 ) , 0.5 ) ));
			float4 PhysicalWaves1647 = lerpResult1667;
			float3 worldToObjDir1609 = mul( unity_WorldToObject, float4( _Vector11, 0 ) ).xyz;
			float3 VertexNormal1612 = worldToObjDir1609;
			v.vertex.xyz = (( _ProjectGrid )?( ( float4( Projection1611 , 0.0 ) + ( PhysicalWaves1647 * float4( VertexNormal1612 , 0.0 ) ) ) ):( float4( ase_vertex3Pos , 0.0 ) )).xyz;
			float3 temp_output_1629_0 = VertexNormal1612;
			v.normal = temp_output_1629_0;
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
			float2 appendResult1_g587 = (float2(waveSpeed675 , 0.0));
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 temp_output_21_0_g9 = ase_worldNormal;
			float temp_output_17_0_g9 = (temp_output_21_0_g9).y;
			float2 appendResult18_g9 = (float2(sign( temp_output_17_0_g9 ) , 1.0));
			float3 ase_worldPos = i.worldPos;
			float2 BaseUV1197 = ( appendResult18_g9 * (ase_worldPos).xz );
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
			float2 panner3_g587 = ( _Time.y * appendResult1_g587 + temp_output_1305_0);
			float2 appendResult1_g592 = (float2(waveSpeed675 , 0.0));
			float cos30 = cos( radians( 180.0 ) );
			float sin30 = sin( radians( 180.0 ) );
			float2 rotator30 = mul( temp_output_1305_0 - float2( 0.5,0.5 ) , float2x2( cos30 , -sin30 , sin30 , cos30 )) + float2( 0.5,0.5 );
			float2 panner3_g592 = ( _Time.y * appendResult1_g592 + rotator30);
			float normalStrength681 = _NormalStrength;
			float3 lerpResult67 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g587 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g592 ) ) ) , normalStrength681);
			float3 WavesCloseUV1207 = lerpResult67;
			float temp_output_209_0 = ( _Refraction * 0.2 );
			float refractiveStrength496 = temp_output_209_0;
			float screenDepth514 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth514 = saturate( ( screenDepth514 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 0.1 ) );
			float2 temp_output_461_0 = ( (WavesCloseUV1207).xy * refractiveStrength496 * distanceDepth514 );
			float2 refraction511 = temp_output_461_0;
			float4 screenColor86_g659 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( (ase_screenPosNorm).xy + ( float2( 0.2,0 ) * refraction511 ) ));
			float3 LightWrapVector47_g659 = (( _LightWrapping * 0.5 )).xxx;
			float2 appendResult1_g590 = (float2(waveSpeed675 , 0.0));
			float2 flowUV2Close1289 = ( temp_output_1277_0 * frac( ( mulTime1269 + 0.5 ) ) );
			float2 temp_output_1324_0 = ( ( BaseUV1197 * normalTiling618 ) + ( normalTiling618 * flowUV2Close1289 ) );
			float2 panner3_g590 = ( _Time.y * appendResult1_g590 + temp_output_1324_0);
			float2 appendResult1_g591 = (float2(waveSpeed675 , 0.0));
			float cos1327 = cos( radians( 180.0 ) );
			float sin1327 = sin( radians( 180.0 ) );
			float2 rotator1327 = mul( temp_output_1324_0 - float2( 0.5,0.5 ) , float2x2( cos1327 , -sin1327 , sin1327 , cos1327 )) + float2( 0.5,0.5 );
			float2 panner3_g591 = ( _Time.y * appendResult1_g591 + rotator1327);
			float3 lerpResult1333 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g590 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g591 ) ) ) , normalStrength681);
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
			float2 appendResult1_g505 = (float2(0.2 , 0.0));
			float3 temp_output_21_0_g10 = ase_worldNormal;
			float temp_output_17_0_g10 = (temp_output_21_0_g10).y;
			float2 appendResult18_g10 = (float2(sign( temp_output_17_0_g10 ) , 1.0));
			float2 temp_output_1211_0 = ( ( appendResult18_g10 * (ase_worldPos).xz ) * float2( 0.05,0.05 ) );
			float cos1177 = cos( radians( 180.0 ) );
			float sin1177 = sin( radians( 180.0 ) );
			float2 rotator1177 = mul( temp_output_1211_0 - float2( 0.5,0.5 ) , float2x2( cos1177 , -sin1177 , sin1177 , cos1177 )) + float2( 0.5,0.5 );
			float2 panner3_g505 = ( _Time.y * appendResult1_g505 + rotator1177);
			float4 ShorelineNormal817 = ( float4( lerpResult824 , 0.0 ) * ( 1.0 - step( tex2DNode912.r , _ShorelineBlend ) ) * ( ( tex2D( _RandomMask, uv_RandomMask ) + tex2D( _RandomMask, panner3_g505 ) ) / 2.0 ) );
			float4 NormalsClose1340 = ( float4( lerpResult1338 , 0.0 ) + ShorelineNormal817 );
			float temp_output_678_0 = ( waveSpeed675 / 10.0 );
			float2 appendResult1_g588 = (float2(temp_output_678_0 , 0.0));
			float temp_output_642_0 = ( normalTiling618 / 10.0 );
			float FlowSpeedMedium1300 = ( FlowSpeed1256 / 1.0 );
			float2 temp_output_1280_0 = ( (float2( -0.5,-0.5 ) + ((flowMap1263).rg - float2( 0,0 )) * (float2( 0.5,0.5 ) - float2( -0.5,-0.5 )) / (float2( 1,1 ) - float2( 0,0 ))) * FlowSpeedMedium1300 );
			float mulTime1270 = _Time.y * 0.05;
			float temp_output_1282_0 = frac( mulTime1270 );
			float2 flowUV1Medium1288 = ( temp_output_1280_0 * temp_output_1282_0 );
			float2 temp_output_1347_0 = ( ( BaseUV1197 * temp_output_642_0 ) + ( temp_output_642_0 * flowUV1Medium1288 ) );
			float2 panner3_g588 = ( _Time.y * appendResult1_g588 + temp_output_1347_0);
			float2 appendResult1_g584 = (float2(temp_output_678_0 , 0.0));
			float cos630 = cos( radians( 180.0 ) );
			float sin630 = sin( radians( 180.0 ) );
			float2 rotator630 = mul( temp_output_1347_0 - float2( 0.5,0.5 ) , float2x2( cos630 , -sin630 , sin630 , cos630 )) + float2( 0.5,0.5 );
			float2 panner3_g584 = ( _Time.y * appendResult1_g584 + rotator630);
			float mediumTilingDistance687 = _MediumTilingDistance;
			float tilingFade689 = _DistanceFade;
			float lerpResult693 = lerp( normalStrength681 , ( normalStrength681 / 20.0 ) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / mediumTilingDistance687 ) , tilingFade689 ) ));
			float normalStrengthMedium706 = lerpResult693;
			float3 lerpResult639 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g588 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g584 ) ) ) , normalStrengthMedium706);
			float3 WavesMediumUv1640 = lerpResult639;
			float temp_output_1362_0 = ( waveSpeed675 / 10.0 );
			float2 appendResult1_g593 = (float2(temp_output_1362_0 , 0.0));
			float temp_output_1358_0 = ( normalTiling618 / 10.0 );
			float2 flowUV2Medium1287 = ( temp_output_1280_0 * frac( ( mulTime1270 + 0.5 ) ) );
			float2 temp_output_1360_0 = ( ( BaseUV1197 * temp_output_1358_0 ) + ( temp_output_1358_0 * flowUV2Medium1287 ) );
			float2 panner3_g593 = ( _Time.y * appendResult1_g593 + temp_output_1360_0);
			float2 appendResult1_g589 = (float2(temp_output_1362_0 , 0.0));
			float cos1361 = cos( radians( 180.0 ) );
			float sin1361 = sin( radians( 180.0 ) );
			float2 rotator1361 = mul( temp_output_1360_0 - float2( 0.5,0.5 ) , float2x2( cos1361 , -sin1361 , sin1361 , cos1361 )) + float2( 0.5,0.5 );
			float2 panner3_g589 = ( _Time.y * appendResult1_g589 + rotator1361);
			float3 lerpResult1367 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g593 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g589 ) ) ) , normalStrengthMedium706);
			float3 WavesMediumUV21368 = lerpResult1367;
			float flowLerpMedium1297 = abs( ( ( 0.5 - temp_output_1282_0 ) / 0.5 ) );
			float3 lerpResult1369 = lerp( WavesMediumUv1640 , WavesMediumUV21368 , flowLerpMedium1297);
			float4 NormalsMedium1373 = ( float4( lerpResult1369 , 0.0 ) + ShorelineNormal817 );
			float4 lerpResult664 = lerp( NormalsClose1340 , NormalsMedium1373 , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / mediumTilingDistance687 ) , tilingFade689 ) ));
			float temp_output_680_0 = ( waveSpeed675 / 30.0 );
			float2 appendResult1_g630 = (float2(temp_output_680_0 , 0.0));
			float2 temp_output_1201_0 = ( BaseUV1197 * ( normalTiling618 / 1200.0 ) );
			float2 panner3_g630 = ( _Time.y * appendResult1_g630 + temp_output_1201_0);
			float2 appendResult1_g625 = (float2(temp_output_680_0 , 0.0));
			float cos646 = cos( radians( 180.0 ) );
			float sin646 = sin( radians( 180.0 ) );
			float2 rotator646 = mul( temp_output_1201_0 - float2( 0.5,0.5 ) , float2x2( cos646 , -sin646 , sin646 , cos646 )) + float2( 0.5,0.5 );
			float2 panner3_g625 = ( _Time.y * appendResult1_g625 + rotator646);
			float farTilingDistance688 = _FarTilingDistance;
			float lerpResult698 = lerp( normalStrengthMedium706 , ( lerpResult693 / 20.0 ) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / farTilingDistance688 ) , tilingFade689 ) ));
			float normalStrengthFar704 = lerpResult698;
			float3 lerpResult657 = lerp( float3(0,0,1) , ( UnpackNormal( tex2D( _NormalTexture, panner3_g630 ) ) + UnpackNormal( tex2D( _NormalTexture, panner3_g625 ) ) ) , normalStrengthFar704);
			float3 NormalsFar660 = lerpResult657;
			float4 lerpResult670 = lerp( lerpResult664 , float4( NormalsFar660 , 0.0 ) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / farTilingDistance688 ) , tilingFade689 ) ));
			float2 NormalSign1123 = appendResult18_g9;
			float2 WorldNormalXZ1122 = (temp_output_21_0_g9).xz;
			float WorldNormalY1121 = temp_output_17_0_g9;
			float3 appendResult4_g652 = (float3(( ( (lerpResult670.rgb).xy * NormalSign1123 ) + WorldNormalXZ1122 ) , WorldNormalY1121));
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentDir6_g652 = normalize( mul( ase_worldToTangent, (appendResult4_g652).xzy) );
			float3 break1470 = worldToTangentDir6_g652;
			float temp_output_32_0_g640 = ( -0.02 / ( _Scale0 / 30.0 ) );
			float temp_output_1_0_g626 = _Scale0;
			float Scale10_g626 = temp_output_1_0_g626;
			float2 break12_g626 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g626 ) ) * ( 1.0 / temp_output_1_0_g626 ) );
			float2 appendResult17_g626 = (float2(( ( _XOffset0 / Scale10_g626 ) + break12_g626.x ) , ( break12_g626.y + ( _ZOffset0 / Scale10_g626 ) )));
			float2 temp_output_1_0_g640 = appendResult17_g626;
			float4 tex2DNode8_g640 = tex2D( _RippleTex0, ( ( temp_output_32_0_g640 * float2( 0,1 ) ) + temp_output_1_0_g640 ) );
			float temp_output_17_0_g640 = ( ( tex2D( _RippleTex0, ( ( float2( 1,0 ) * temp_output_32_0_g640 ) + temp_output_1_0_g640 ) ).r - tex2DNode8_g640.r ) * 1.0 );
			float temp_output_18_0_g640 = ( 1.0 * ( tex2DNode8_g640.r - tex2D( _RippleTex0, temp_output_1_0_g640 ).r ) );
			float2 appendResult21_g640 = (float2(temp_output_17_0_g640 , temp_output_18_0_g640));
			float3 appendResult27_g640 = (float3(( -appendResult21_g640 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g640 , 1.0 ) ) - pow( temp_output_18_0_g640 , 1.0 ) ) )));
			float3 normalizeResult28_g640 = normalize( appendResult27_g640 );
			float2 appendResult37_g626 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g626 = ase_worldNormal;
			float3 appendResult4_g646 = (float3(( ( (normalizeResult28_g640).xy * appendResult37_g626 ) + (temp_output_30_0_g626).xz ) , (temp_output_30_0_g626).y));
			float3 worldToTangentDir6_g646 = normalize( mul( ase_worldToTangent, (appendResult4_g646).xzy) );
			float temp_output_32_0_g641 = ( -0.02 / ( _Scale1 / 30.0 ) );
			float temp_output_1_0_g627 = _Scale1;
			float Scale10_g627 = temp_output_1_0_g627;
			float2 break12_g627 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g627 ) ) * ( 1.0 / temp_output_1_0_g627 ) );
			float2 appendResult17_g627 = (float2(( ( _XOffset1 / Scale10_g627 ) + break12_g627.x ) , ( break12_g627.y + ( _ZOffset1 / Scale10_g627 ) )));
			float2 temp_output_1_0_g641 = appendResult17_g627;
			float4 tex2DNode8_g641 = tex2D( _RippleTex1, ( ( temp_output_32_0_g641 * float2( 0,1 ) ) + temp_output_1_0_g641 ) );
			float temp_output_17_0_g641 = ( ( tex2D( _RippleTex1, ( ( float2( 1,0 ) * temp_output_32_0_g641 ) + temp_output_1_0_g641 ) ).r - tex2DNode8_g641.r ) * 1.0 );
			float temp_output_18_0_g641 = ( 1.0 * ( tex2DNode8_g641.r - tex2D( _RippleTex1, temp_output_1_0_g641 ).r ) );
			float2 appendResult21_g641 = (float2(temp_output_17_0_g641 , temp_output_18_0_g641));
			float3 appendResult27_g641 = (float3(( -appendResult21_g641 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g641 , 1.0 ) ) - pow( temp_output_18_0_g641 , 1.0 ) ) )));
			float3 normalizeResult28_g641 = normalize( appendResult27_g641 );
			float2 appendResult37_g627 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g627 = ase_worldNormal;
			float3 appendResult4_g642 = (float3(( ( (normalizeResult28_g641).xy * appendResult37_g627 ) + (temp_output_30_0_g627).xz ) , (temp_output_30_0_g627).y));
			float3 worldToTangentDir6_g642 = normalize( mul( ase_worldToTangent, (appendResult4_g642).xzy) );
			float temp_output_32_0_g639 = ( -0.02 / ( _Scale2 / 30.0 ) );
			float temp_output_1_0_g631 = _Scale2;
			float Scale10_g631 = temp_output_1_0_g631;
			float2 break12_g631 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g631 ) ) * ( 1.0 / temp_output_1_0_g631 ) );
			float2 appendResult17_g631 = (float2(( ( _XOffset2 / Scale10_g631 ) + break12_g631.x ) , ( break12_g631.y + ( _ZOffset2 / Scale10_g631 ) )));
			float2 temp_output_1_0_g639 = appendResult17_g631;
			float4 tex2DNode8_g639 = tex2D( _RippleTex2, ( ( temp_output_32_0_g639 * float2( 0,1 ) ) + temp_output_1_0_g639 ) );
			float temp_output_17_0_g639 = ( ( tex2D( _RippleTex2, ( ( float2( 1,0 ) * temp_output_32_0_g639 ) + temp_output_1_0_g639 ) ).r - tex2DNode8_g639.r ) * 1.0 );
			float temp_output_18_0_g639 = ( 1.0 * ( tex2DNode8_g639.r - tex2D( _RippleTex2, temp_output_1_0_g639 ).r ) );
			float2 appendResult21_g639 = (float2(temp_output_17_0_g639 , temp_output_18_0_g639));
			float3 appendResult27_g639 = (float3(( -appendResult21_g639 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g639 , 1.0 ) ) - pow( temp_output_18_0_g639 , 1.0 ) ) )));
			float3 normalizeResult28_g639 = normalize( appendResult27_g639 );
			float2 appendResult37_g631 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g631 = ase_worldNormal;
			float3 appendResult4_g643 = (float3(( ( (normalizeResult28_g639).xy * appendResult37_g631 ) + (temp_output_30_0_g631).xz ) , (temp_output_30_0_g631).y));
			float3 worldToTangentDir6_g643 = normalize( mul( ase_worldToTangent, (appendResult4_g643).xzy) );
			float temp_output_32_0_g638 = ( -0.02 / ( _Scale3 / 30.0 ) );
			float temp_output_1_0_g629 = _Scale3;
			float Scale10_g629 = temp_output_1_0_g629;
			float2 break12_g629 = ( ( (ase_worldPos).xz + ( float2( 0.5,0.5 ) * temp_output_1_0_g629 ) ) * ( 1.0 / temp_output_1_0_g629 ) );
			float2 appendResult17_g629 = (float2(( ( _XOffset3 / Scale10_g629 ) + break12_g629.x ) , ( break12_g629.y + ( _ZOffset3 / Scale10_g629 ) )));
			float2 temp_output_1_0_g638 = appendResult17_g629;
			float4 tex2DNode8_g638 = tex2D( _RippleTex3, ( ( temp_output_32_0_g638 * float2( 0,1 ) ) + temp_output_1_0_g638 ) );
			float temp_output_17_0_g638 = ( ( tex2D( _RippleTex3, ( ( float2( 1,0 ) * temp_output_32_0_g638 ) + temp_output_1_0_g638 ) ).r - tex2DNode8_g638.r ) * 1.0 );
			float temp_output_18_0_g638 = ( 1.0 * ( tex2DNode8_g638.r - tex2D( _RippleTex3, temp_output_1_0_g638 ).r ) );
			float2 appendResult21_g638 = (float2(temp_output_17_0_g638 , temp_output_18_0_g638));
			float3 appendResult27_g638 = (float3(( -appendResult21_g638 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g638 , 1.0 ) ) - pow( temp_output_18_0_g638 , 1.0 ) ) )));
			float3 normalizeResult28_g638 = normalize( appendResult27_g638 );
			float2 appendResult37_g629 = (float2(0.0 , 1.0));
			float3 temp_output_30_0_g629 = ase_worldNormal;
			float3 appendResult4_g644 = (float3(( ( (normalizeResult28_g638).xy * appendResult37_g629 ) + (temp_output_30_0_g629).xz ) , (temp_output_30_0_g629).y));
			float3 worldToTangentDir6_g644 = normalize( mul( ase_worldToTangent, (appendResult4_g644).xzy) );
			float3 lerpResult1528 = lerp( ( worldToTangentDir6_g646 + worldToTangentDir6_g642 + worldToTangentDir6_g643 + worldToTangentDir6_g644 ) , float3(0,0,1) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / 100.0 ) , 0.5 ) ));
			float3 RippleNormals1451 = lerpResult1528;
			float3 lerpResult1508 = lerp( float3(0,0,1) , RippleNormals1451 , _RippleStrength);
			float3 break1472 = lerpResult1508;
			float3 _Vector14 = float3(0,0,1);
			float temp_output_32_0_g637 = ( -0.02 / ( 1.0 / 30.0 ) );
			float2 appendResult1_g632 = (float2(0.15 , 0.0));
			float2 appendResult1543 = (float2(max( -_RangeVector.x , 0.0 ) , max( -_RangeVector.y , 0.0 )));
			float2 RangeX1546 = appendResult1543;
			float2 appendResult1544 = (float2(min( -_RangeVector.z , 0.0 ) , min( -_RangeVector.w , 0.0 )));
			float2 RangeZ1545 = appendResult1544;
			float2 ObjectScale1549 = _ObjectScale;
			float2 break1556 = ( (RangeX1546 + (i.uv_texcoord - float2( -0.5,-0.5 )) * (RangeZ1545 - RangeX1546) / (float2( 0.5,0.5 ) - float2( -0.5,-0.5 ))) * ObjectScale1549 );
			float3 appendResult1559 = (float3(break1556.x , 1.0 , break1556.y));
			float Displacement1557 = -10.0;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 objToWorld1565 = mul( unity_ObjectToWorld, float4( ( ( appendResult1559 * Displacement1557 ) + ase_vertex3Pos ), 1 ) ).xyz;
			float3 TargetPoint1567 = objToWorld1565;
			float3 objToWorld1564 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 1 ) ).xyz;
			float3 StartPointGlobal1566 = objToWorld1564;
			float3 temp_output_1570_0 = ( TargetPoint1567 - StartPointGlobal1566 );
			float3 x1575 = ( temp_output_1570_0 / length( temp_output_1570_0 ) );
			float3 _Vector11 = float3(0,1,0);
			float3 PlaneNormal1574 = _Vector11;
			float3 appendResult1621 = (float3(0.0 , _waterLevel , 0.0));
			float3 PlaneOrigin1613 = appendResult1621;
			float dotResult1579 = dot( PlaneNormal1574 , ( TargetPoint1567 - PlaneOrigin1613 ) );
			float distV21582 = dotResult1579;
			float dotResult1580 = dot( PlaneNormal1574 , x1575 );
			float cosPhi1581 = dotResult1580;
			float3 IntersectionPoint1592 = ( TargetPoint1567 - ( x1575 * ( distV21582 / cosPhi1581 ) ) );
			float2 temp_output_1640_0 = ( (IntersectionPoint1592).xz * 0.5 );
			float2 panner3_g632 = ( _Time.y * appendResult1_g632 + temp_output_1640_0);
			float2 temp_output_1643_0 = panner3_g632;
			float2 temp_output_1_0_g637 = temp_output_1643_0;
			float4 tex2DNode8_g637 = tex2D( _RandomMask, ( ( temp_output_32_0_g637 * float2( 0,1 ) ) + temp_output_1_0_g637 ) );
			float temp_output_17_0_g637 = ( ( tex2D( _RandomMask, ( ( float2( 1,0 ) * temp_output_32_0_g637 ) + temp_output_1_0_g637 ) ).r - tex2DNode8_g637.r ) * 1.0 );
			float temp_output_18_0_g637 = ( 1.0 * ( tex2DNode8_g637.r - tex2D( _RandomMask, temp_output_1_0_g637 ).r ) );
			float2 appendResult21_g637 = (float2(temp_output_17_0_g637 , temp_output_18_0_g637));
			float3 appendResult27_g637 = (float3(( -appendResult21_g637 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g637 , 1.0 ) ) - pow( temp_output_18_0_g637 , 1.0 ) ) )));
			float3 normalizeResult28_g637 = normalize( appendResult27_g637 );
			float3 temp_output_21_0_g635 = ase_worldNormal;
			float temp_output_17_0_g635 = (temp_output_21_0_g635).y;
			float2 appendResult18_g635 = (float2(sign( temp_output_17_0_g635 ) , 1.0));
			float2 temp_output_1687_28 = appendResult18_g635;
			float2 temp_output_1687_26 = (temp_output_21_0_g635).xz;
			float temp_output_1687_27 = temp_output_17_0_g635;
			float3 appendResult4_g647 = (float3(( ( (normalizeResult28_g637).xy * temp_output_1687_28 ) + temp_output_1687_26 ) , temp_output_1687_27));
			float3 worldToTangentDir6_g647 = normalize( mul( ase_worldToTangent, (appendResult4_g647).xzy) );
			float temp_output_32_0_g636 = ( -0.02 / ( 1.0 / 30.0 ) );
			float2 appendResult1_g628 = (float2(0.3 , 0.0));
			float cos1642 = cos( radians( 180.0 ) );
			float sin1642 = sin( radians( 180.0 ) );
			float2 rotator1642 = mul( temp_output_1640_0 - float2( 0.5,0.5 ) , float2x2( cos1642 , -sin1642 , sin1642 , cos1642 )) + float2( 0.5,0.5 );
			float2 panner3_g628 = ( _Time.y * appendResult1_g628 + rotator1642);
			float2 temp_output_1644_0 = panner3_g628;
			float2 temp_output_1_0_g636 = temp_output_1644_0;
			float4 tex2DNode8_g636 = tex2D( _RandomMask, ( ( temp_output_32_0_g636 * float2( 0,1 ) ) + temp_output_1_0_g636 ) );
			float temp_output_17_0_g636 = ( ( tex2D( _RandomMask, ( ( float2( 1,0 ) * temp_output_32_0_g636 ) + temp_output_1_0_g636 ) ).r - tex2DNode8_g636.r ) * 1.0 );
			float temp_output_18_0_g636 = ( 1.0 * ( tex2DNode8_g636.r - tex2D( _RandomMask, temp_output_1_0_g636 ).r ) );
			float2 appendResult21_g636 = (float2(temp_output_17_0_g636 , temp_output_18_0_g636));
			float3 appendResult27_g636 = (float3(( -appendResult21_g636 * float2( 3,3 ) ) , sqrt( ( ( 1.0 - pow( temp_output_17_0_g636 , 1.0 ) ) - pow( temp_output_18_0_g636 , 1.0 ) ) )));
			float3 normalizeResult28_g636 = normalize( appendResult27_g636 );
			float3 appendResult4_g645 = (float3(( ( (normalizeResult28_g636).xy * temp_output_1687_28 ) + temp_output_1687_26 ) , temp_output_1687_27));
			float3 worldToTangentDir6_g645 = normalize( mul( ase_worldToTangent, (appendResult4_g645).xzy) );
			float3 lerpResult1703 = lerp( ( worldToTangentDir6_g647 + worldToTangentDir6_g645 ) , _Vector14 , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / 10.0 ) , 0.5 ) ));
			float3 lerpResult1706 = lerp( _Vector14 , lerpResult1703 , _PhysicalNormalStrength);
			float3 PhysicalNormals1682 = lerpResult1706;
			float3 break1702 = PhysicalNormals1682;
			float3 appendResult1475 = (float3(( break1470.x + break1472.x + break1702.x ) , ( break1470.y + break1472.y + break1702.y ) , break1470.z));
			float3 resultingNormal674 = appendResult1475;
			float3 break736 = resultingNormal674;
			float3 appendResult735 = (float3(break736.x , break736.y , 1));
			float3 CurrentNormal23_g659 = normalize( (WorldNormalVector( i , appendResult735 )) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult20_g659 = dot( CurrentNormal23_g659 , ase_worldlightDir );
			float NDotL21_g659 = dotResult20_g659;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 AttenuationColor8_g659 = ( ase_lightColor.rgb * ase_lightAtten );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float2 pseudoRefraction484 = ( (ase_grabScreenPosNorm).xy + ( temp_output_209_0 * (resultingNormal674).xy ) );
			float4 screenColor146 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,pseudoRefraction484);
			float eyeDepth135 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float temp_output_141_0 = ( eyeDepth135 - i.eyeDepth );
			float3 appendResult258 = (float3(temp_output_141_0 , temp_output_141_0 , temp_output_141_0));
			float3 clampResult142 = clamp( (float3( 1,1,1 ) + (appendResult258 - float3(0,0,0)) * (float3( 0,0,0 ) - float3( 1,1,1 )) / (( _MainColor * ( 1.0 / _Density ) ).rgb - float3(0,0,0))) , float3( 0,0,0 ) , float3( 1,1,1 ) );
			float3 temp_cast_7 = (_Fade).xxx;
			float4 blendOpSrc147 = _DeepWaterColor;
			float4 blendOpDest147 = ( screenColor146 * float4( pow( clampResult142 , temp_cast_7 ) , 0.0 ) );
			float4 waterColor488 = ( saturate( ( blendOpSrc147 + blendOpDest147 ) ));
			float4 break1517 = lerpResult670;
			float3 appendResult1522 = (float3(( break1472.x + break1517.r + break1702.x ) , ( break1472.y + break1517.g + break1702.y ) , break1517.b));
			float3 ResultingNormalForDistortion1515 = appendResult1522;
			float4 realtimeReflection600 = tex2D( _ReflectionTex, ( (ase_screenPosNorm).xy + ( (ResultingNormalForDistortion1515).xy * _Distortion ) ) );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
			float fresnelNdotV1378 = dot( mul(ase_tangentToWorldFast,resultingNormal674), ase_worldViewDir );
			float fresnelNode1378 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV1378, 2.0 ) );
			float temp_output_1380_0 = saturate( fresnelNode1378 );
			float4 lerpResult1377 = lerp( waterColor488 , realtimeReflection600 , ( temp_output_1380_0 * _RealtimeReflectionIntensity ));
			float Distortion761 = _Distortion;
			float3 appendResult776 = (float3(( resultingNormal674 * Distortion761 ).x , 0.0 , 1.0));
			float3 indirectNormal727 = WorldNormalVector( i , appendResult776 );
			Unity_GlossyEnvironmentData g727 = UnityGlossyEnvironmentSetup( 1.0, data.worldViewDir, indirectNormal727, float3(0,0,0));
			float3 indirectSpecular727 = UnityGI_IndirectSpecular( data, 1.0, indirectNormal727, g727 );
			float fresnelNdotV755 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode755 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV755, 4.0 ) );
			float3 lerpResult754 = lerp( ( indirectSpecular727 * float3( 0.3,0.3,0.3 ) ) , indirectSpecular727 , fresnelNode755);
			float3 probeReflection766 = lerpResult754;
			float4 lerpResult1382 = lerp( (( _EnableRealtimeReflections )?( lerpResult1377 ):( waterColor488 )) , float4( probeReflection766 , 0.0 ) , ( temp_output_1380_0 * _ProbeReflectionIntensity ));
			float screenDepth313 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth313 = saturate( ( screenDepth313 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _FoamBlend ) );
			float2 appendResult1_g654 = (float2(_FoamSpeed , 0.0));
			float3 temp_output_21_0_g653 = ase_worldNormal;
			float temp_output_17_0_g653 = (temp_output_21_0_g653).y;
			float2 appendResult18_g653 = (float2(sign( temp_output_17_0_g653 ) , 1.0));
			float2 temp_output_1212_0 = ( ( appendResult18_g653 * (ase_worldPos).xz ) * _FoamTiling );
			float2 panner3_g654 = ( _Time.y * appendResult1_g654 + temp_output_1212_0);
			float2 appendResult1_g655 = (float2(_FoamSpeed , 0.0));
			float cos296 = cos( radians( 90.0 ) );
			float sin296 = sin( radians( 90.0 ) );
			float2 rotator296 = mul( temp_output_1212_0 - float2( 0.5,0.5 ) , float2x2( cos296 , -sin296 , sin296 , cos296 )) + float2( 0.5,0.5 );
			float2 panner3_g655 = ( _Time.y * appendResult1_g655 + rotator296);
			float3 desaturateInitialColor304 = ( tex2D( _FoamTexture, panner3_g654 ) - tex2D( _FoamTexture, panner3_g655 ) ).rgb;
			float desaturateDot304 = dot( desaturateInitialColor304, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar304 = lerp( desaturateInitialColor304, desaturateDot304.xxx, 1.0 );
			float3 temp_cast_11 = (_FoamContrast).xxx;
			float3 temp_cast_12 = (( 1.0 - _FoamContrast )).xxx;
			float2 _Vector3 = float2(0,1);
			float3 temp_cast_13 = (_Vector3.x).xxx;
			float3 temp_cast_14 = (_Vector3.y).xxx;
			float4 temp_output_319_0 = ( ( 1.0 - distanceDepth313 ) * ( float4( (temp_cast_13 + (desaturateVar304 - temp_cast_11) * (temp_cast_14 - temp_cast_13) / (temp_cast_12 - temp_cast_11)) , 0.0 ) * _FoamColor * _FoamIntensity * -1.0 ) );
			float3 temp_cast_16 = (_FoamContrast).xxx;
			float3 temp_cast_17 = (( 1.0 - _FoamContrast )).xxx;
			float3 temp_cast_18 = (_Vector3.x).xxx;
			float3 temp_cast_19 = (_Vector3.y).xxx;
			float4 foam406 = ( temp_output_319_0 * temp_output_319_0 );
			float4 foamyWater490 = ( (( _EnableProbeRelfections )?( lerpResult1382 ):( (( _EnableRealtimeReflections )?( lerpResult1377 ):( waterColor488 )) )) + ( foam406 * _FoamVisibility ) );
			float clampResult100_g659 = clamp( ase_worldlightDir.y , ( length( (UNITY_LIGHTMODEL_AMBIENT).rgb ) / 3.0 ) , 1.0 );
			float3 diffuseColor131_g659 = ( ( ( max( ( LightWrapVector47_g659 + ( ( 1.0 - LightWrapVector47_g659 ) * NDotL21_g659 ) ) , float3(0,0,0) ) * AttenuationColor8_g659 ) * foamyWater490.rgb ) * clampResult100_g659 );
			float3 normalizeResult77_g659 = normalize( ase_worldlightDir );
			float3 normalizeResult28_g659 = normalize( ( normalizeResult77_g659 + ase_worldViewDir ) );
			float3 HalfDirection29_g659 = normalizeResult28_g659;
			float dotResult32_g659 = dot( HalfDirection29_g659 , CurrentNormal23_g659 );
			float SpecularPower14_g659 = exp2( ( ( _Gloss * 10.0 ) + 1.0 ) );
			float screenDepth402 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth402 = saturate( abs( ( screenDepth402 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 0.2 ) ) );
			float4 specularity504 = ( ( distanceDepth402 * _Specular ) * _SpecularColor );
			float3 specularFinalColor42_g659 = ( AttenuationColor8_g659 * pow( max( dotResult32_g659 , 0.0 ) , SpecularPower14_g659 ) * specularity504.rgb );
			float3 diffuseSpecular132_g659 = ( diffuseColor131_g659 + specularFinalColor42_g659 );
			float screenDepth261 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth261 = saturate( ( screenDepth261 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthTransparency ) );
			float opacity508 = pow( distanceDepth261 , _TransparencyFade );
			float4 lerpResult87_g659 = lerp( screenColor86_g659 , float4( diffuseSpecular132_g659 , 0.0 ) , opacity508);
			c.rgb = ( lerpResult87_g659 * ase_lightAtten ).rgb;
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
}