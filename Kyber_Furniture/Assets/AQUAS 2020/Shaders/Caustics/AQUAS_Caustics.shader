Shader "AQUAS/Misc/Caustics"
{
	Properties
	{
		_CausticsScale("Caustics Scale", Float) = 2
		_WaterLevel("Water Level", Float) = 0
		_Texture("Texture", 2D) = "white" {}
		_Intensity("Intensity", Float) = 0
		_DistanceVisibility("Distance Visibility", Float) = 0
		_Fade("Fade", Float) = 0
		_DepthFade("Depth Fade", Float) = 0

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Overlay" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend One One
		Cull Back
		ColorMask RGBA
		ZWrite On
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
			#include "Lighting.cginc"
			#include "UnityShaderVariables.cginc"
			#include "AutoLight.cginc"
			#include "UnityStandardBRDF.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				half3 ase_normal : NORMAL;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
			};

			//This is a late directive
			
			uniform sampler2D _Texture;
			uniform float _CausticsScale;
			uniform float _WaterLevel;
			uniform float _DepthFade;
			uniform float _Intensity;
			uniform float _DistanceVisibility;
			uniform float _Fade;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord.xyz = ase_worldPos;
				half3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord1.xyz = ase_worldNormal;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord1.w = 0;
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
				float3 ase_worldPos = i.ase_texcoord.xyz;
				#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
				half4 ase_lightColor = 0;
				#else //aselc
				half4 ase_lightColor = _LightColor0;
				#endif //aselc
				half3 worldSpaceLightDir = Unity_SafeNormalize(UnityWorldSpaceLightDir(ase_worldPos));
				half3 ase_worldNormal = i.ase_texcoord1.xyz;
				half3 normalizedWorldNormal = normalize( ase_worldNormal );
				half dotResult62 = dot( worldSpaceLightDir , normalizedWorldNormal );
				half4 lerpResult63 = lerp( ( saturate( ( tex2D( _Texture, ( (ase_worldPos).xz * float2( 0.1,0.1 ) * _CausticsScale ) ) * half4( ase_lightColor.rgb , 0.0 ) ) ) * ( 1.0 - saturate( ( ( ( _WaterLevel + -1.0 ) * -1.0 ) + ase_worldPos.y ) ) ) * saturate( ( ase_worldPos.y + ( -1.0 * _DepthFade ) ) ) * _Intensity * saturate( dotResult62 ) ) , float4(0,0,0,1) , saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / _DistanceVisibility ) , _Fade ) ));
				
				
				finalColor = lerpResult63;
				return finalColor;
			}
			ENDCG
		}
	}
}