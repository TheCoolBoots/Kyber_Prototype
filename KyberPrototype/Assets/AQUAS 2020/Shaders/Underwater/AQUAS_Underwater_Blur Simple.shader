Shader "Hidden/AQUAS/Underwater/Blur Simple"
{
	Properties
	{
		_MainTex("Main Tex", 2D) = "white" {}
		_BlurSize("Blur Size", Float) = 0
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
			
			CGPROGRAM

#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
		//only defining to not throw compilation error over Unity 5.5
		#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

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
			};

			uniform sampler2D _MainTex;
			uniform float _BlurSize;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				float3 vertexValue =  float3(0,0,0) ;
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
				float2 appendResult56 = (float2(_BlurSize , _BlurSize));
				float2 uv08 = i.ase_texcoord.xy * float2( 1,1 ) + appendResult56;
				float2 uv09 = i.ase_texcoord.xy * float2( 1,1 ) + -appendResult56;
				float2 appendResult17 = (float2(-_BlurSize , _BlurSize));
				float2 uv010 = i.ase_texcoord.xy * float2( 1,1 ) + appendResult17;
				float2 appendResult20 = (float2(_BlurSize , -_BlurSize));
				float2 uv011 = i.ase_texcoord.xy * float2( 1,1 ) + appendResult20;
				
				
				finalColor = ( ( tex2D( _MainTex, uv08 ) + tex2D( _MainTex, uv09 ) + tex2D( _MainTex, uv010 ) + tex2D( _MainTex, uv011 ) ) / 4.0 );
				return finalColor;
			}
			ENDCG
		}
	}
}