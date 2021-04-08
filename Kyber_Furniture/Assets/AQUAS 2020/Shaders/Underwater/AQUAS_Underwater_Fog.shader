Shader "Hidden/AQUAS/Underwater/Fog"
{
	Properties
	{
		_MainTex("Main Tex", 2D) = "white" {}
		_Fade("Fade", Range( 0.001 , 10)) = 1
		_FogColor("Fog Color", Color) = (0.3550641,0.6285198,0.745283,0)
		_Density("Density", Float) = 40
		_DepthMask("DepthMask", 2D) = "white" {}
		_DistortionLens("DistortionLens", 2D) = "white" {}
		_Distortion("Distortion", Range( 0 , 0.05)) = 0.05
		_DropletMask("Droplet Mask", 2D) = "white" {}
		_DropletNormals("Droplet Normals", 2D) = "white" {}
		_DropletCutout("DropletCutout", 2D) = "white" {}
		[Toggle]_EnableWetLens("Enable Wet Lens", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
			};

			uniform sampler2D _MainTex;
			uniform sampler2D _DropletCutout;
			uniform float4 _DropletCutout_ST;
			uniform sampler2D _DropletNormals;
			uniform float4 _DropletNormals_ST;
			uniform float _EnableWetLens;
			uniform sampler2D _DropletMask;
			uniform float4 _DropletMask_ST;
			uniform sampler2D _DistortionLens;
			uniform float4 _DistortionLens_ST;
			uniform float _Distortion;
			uniform sampler2D _DepthMask;
			uniform float4 _DepthMask_ST;
			uniform float4 _FogColor;
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float _Density;
			uniform float _Fade;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord1 = screenPos;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				float2 uv_DropletCutout = i.ase_texcoord.xy * _DropletCutout_ST.xy + _DropletCutout_ST.zw;
				float4 tex2DNode155 = tex2D( _DropletCutout, uv_DropletCutout );
				float2 uv_DropletNormals = i.ase_texcoord.xy * _DropletNormals_ST.xy + _DropletNormals_ST.zw;
				float2 uv_DropletMask = i.ase_texcoord.xy * _DropletMask_ST.xy + _DropletMask_ST.zw;
				float4 tex2DNode143 = tex2D( _DropletMask, uv_DropletMask );
				float2 lerpResult144 = lerp( float2( 0,0 ) , (( saturate( ( tex2DNode155.r + tex2DNode155.g + tex2DNode155.b ) ) * tex2D( _DropletNormals, uv_DropletNormals ) )).rg , ( _EnableWetLens )?( ( tex2DNode143.r * ( 1.0 - step( 0.95 , tex2DNode143.r ) ) ) ):( 0.0 ));
				float2 uv0153 = i.ase_texcoord.xy * float2( 1,1 ) + lerpResult144;
				float2 uv0127 = i.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv0_DistortionLens = i.ase_texcoord.xy * _DistortionLens_ST.xy + _DistortionLens_ST.zw;
				float cos95 = cos( 5.0 * _Time.y );
				float sin95 = sin( 5.0 * _Time.y );
				float2 rotator95 = mul( uv0_DistortionLens - float2( 0.5,0.5 ) , float2x2( cos95 , -sin95 , sin95 , cos95 )) + float2( 0.5,0.5 );
				float cos136 = cos( 1.5708 );
				float sin136 = sin( 1.5708 );
				float2 rotator136 = mul( uv0_DistortionLens - float2( 0.5,0.5 ) , float2x2( cos136 , -sin136 , sin136 , cos136 )) + float2( 0.5,0.5 );
				float cos139 = cos( 5.0 * _Time.y );
				float sin139 = sin( 5.0 * _Time.y );
				float2 rotator139 = mul( rotator136 - float2( 0.5,0.5 ) , float2x2( cos139 , -sin139 , sin139 , cos139 )) + float2( 0.5,0.5 );
				float temp_output_140_0 = ( tex2D( _DistortionLens, rotator95 ).r - tex2D( _DistortionLens, rotator139 ).r );
				float2 appendResult132 = (float2(temp_output_140_0 , temp_output_140_0));
				float4 screenPos = i.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 break116 = abs( (float2( -1,-1 ) + ((ase_screenPosNorm).xy - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 ))) );
				float2 uv_DepthMask = i.ase_texcoord.xy * _DepthMask_ST.xy + _DepthMask_ST.zw;
				float4 tex2DNode21 = tex2D( _DepthMask, uv_DepthMask );
				float temp_output_41_0 = ( tex2DNode21.g + tex2DNode21.b );
				float4 lerpResult142 = lerp( tex2D( _MainTex, uv0153 ) , tex2D( _MainTex, ( uv0127 + ( appendResult132 * ( 1.0 - pow( max( break116.x , break116.y ) , 3.0 ) ) * _Distortion ) ) ) , temp_output_41_0);
				float clampDepth12 = Linear01Depth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
				float4 lerpResult8 = lerp( lerpResult142 , _FogColor , ( temp_output_41_0 * saturate( pow( (0.0 + (clampDepth12 - 0.0) * (_Density - 0.0) / (1.0 - 0.0)) , _Fade ) ) ));
				
				
				finalColor = lerpResult8;
				return finalColor;
			}
			ENDCG
		}
	}
}