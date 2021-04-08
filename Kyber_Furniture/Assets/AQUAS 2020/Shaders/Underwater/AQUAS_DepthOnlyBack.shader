Shader "Hidden/AQUAS/Utils/Depth Only Back"
{
	Properties
	{
		_ObjectScale("Object Scale", Vector) = (0,0,0,0)
		_waterLevel("waterLevel", Float) = 0
		_RangeVector("Range Vector", Vector) = (1,1,1,1)
		_RandomMask("Random Mask", 2D) = "white" {}
		[Toggle]_ProjectGrid("Project Grid", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Front
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPos;
		};

		uniform float _ProjectGrid;
		uniform float4 _RangeVector;
		uniform float2 _ObjectScale;
		uniform float _waterLevel;
		uniform sampler2D _RandomMask;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float2 appendResult16 = (float2(max( -_RangeVector.x , 0.0 ) , max( -_RangeVector.y , 0.0 )));
			float2 RangeX18 = appendResult16;
			float2 appendResult17 = (float2(min( -_RangeVector.z , 0.0 ) , min( -_RangeVector.w , 0.0 )));
			float2 RangeZ20 = appendResult17;
			float2 ObjectScale21 = _ObjectScale;
			float2 break86 = ( (RangeX18 + (v.texcoord.xy - float2( -0.5,-0.5 )) * (RangeZ20 - RangeX18) / (float2( 0.5,0.5 ) - float2( -0.5,-0.5 ))) * ObjectScale21 );
			float3 appendResult88 = (float3(break86.x , 1.0 , break86.y));
			float Displacement23 = -10.0;
			float2 break39 = ( (RangeX18 + (v.texcoord.xy - float2( -0.5,-0.5 )) * (RangeZ20 - RangeX18) / (float2( 0.5,0.5 ) - float2( -0.5,-0.5 ))) * ObjectScale21 );
			float3 appendResult40 = (float3(break39.x , 1.0 , break39.y));
			float3 temp_output_45_0 = ( ( appendResult40 * Displacement23 ) + ase_vertex3Pos );
			float3 objToWorld47 = mul( unity_ObjectToWorld, float4( temp_output_45_0, 1 ) ).xyz;
			float3 TargetPoint49 = objToWorld47;
			float3 objToWorld46 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 1 ) ).xyz;
			float3 StartPointGlobal48 = objToWorld46;
			float3 temp_output_52_0 = ( TargetPoint49 - StartPointGlobal48 );
			float3 x56 = ( temp_output_52_0 / length( temp_output_52_0 ) );
			float3 _Vector11 = float3(0,1,0);
			float3 PlaneNormal28 = _Vector11;
			float3 appendResult25 = (float3(0.0 , _waterLevel , 0.0));
			float3 PlaneOrigin27 = appendResult25;
			float dotResult61 = dot( PlaneNormal28 , ( TargetPoint49 - PlaneOrigin27 ) );
			float distV264 = dotResult61;
			float dotResult62 = dot( PlaneNormal28 , x56 );
			float cosPhi63 = dotResult62;
			float3 IntersectionPoint72 = ( TargetPoint49 - ( x56 * ( distV264 / cosPhi63 ) ) );
			float3 Projection92 = ( ( appendResult88 * ( Displacement23 * ( distance( IntersectionPoint72 , StartPointGlobal48 ) / distance( StartPointGlobal48 , TargetPoint49 ) ) ) ) + ase_vertex3Pos );
			float2 appendResult1_g575 = (float2(0.15 , 0.0));
			float2 temp_output_149_0 = ( (IntersectionPoint72).xz * 0.5 );
			float2 panner3_g575 = ( _Time.y * appendResult1_g575 + temp_output_149_0);
			float2 appendResult1_g576 = (float2(0.3 , 0.0));
			float cos150 = cos( radians( 180.0 ) );
			float sin150 = sin( radians( 180.0 ) );
			float2 rotator150 = mul( temp_output_149_0 - float2( 0.5,0.5 ) , float2x2( cos150 , -sin150 , sin150 , cos150 )) + float2( 0.5,0.5 );
			float2 panner3_g576 = ( _Time.y * appendResult1_g576 + rotator150);
			float4 _Vector12 = float4(0,2,-0.15,0.15);
			float4 temp_cast_2 = (_Vector12.x).xxxx;
			float4 temp_cast_3 = (_Vector12.y).xxxx;
			float4 temp_cast_4 = (_Vector12.z).xxxx;
			float4 temp_cast_5 = (_Vector12.w).xxxx;
			float4 lerpResult174 = lerp( (temp_cast_4 + (( tex2Dlod( _RandomMask, float4( panner3_g575, 0, 0.0) ) + tex2Dlod( _RandomMask, float4( panner3_g576, 0, 0.0) ) ) - temp_cast_2) * (temp_cast_5 - temp_cast_4) / (temp_cast_3 - temp_cast_2)) , float4(0,0,0,0) , saturate( pow( ( distance( IntersectionPoint72 , _WorldSpaceCameraPos ) / 1.0 ) , 0.5 ) ));
			float4 PhysicalWaves175 = lerpResult174;
			float3 worldToObjDir29 = mul( unity_WorldToObject, float4( _Vector11, 0 ) ).xyz;
			float3 VertexNormal30 = worldToObjDir29;
			v.vertex.xyz = ( _ProjectGrid )?( ( float4( Projection92 , 0.0 ) + ( PhysicalWaves175 * float4( VertexNormal30 , 0.0 ) ) ) ):( float4( ase_vertex3Pos , 0.0 ) ).xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float eyeDepth1 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float3 temp_cast_0 = (eyeDepth1).xxx;
			o.Emission = temp_cast_0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}