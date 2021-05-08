Shader "Hidden/AQUAS/Underwater/Blur"
{
	Properties
	{
		_MainTex("Main Tex", 2D) = "white" {}
		_BlurSize("Blur Size", Float) = 0
		_DepthMask("Depth Mask", 2D) = "white" {}
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
			
			CGPROGRAM
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
			uniform float4 _MainTex_ST;
			uniform float _BlurSize;
			uniform sampler2D _DepthMask;
			uniform float4 _DepthMask_ST;
			
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
				fixed4 finalColor;
				float2 uv_MainTex = i.ase_texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 appendResult56 = (float2(_BlurSize , _BlurSize));
				float2 uv8 = i.ase_texcoord.xy * float2( 1,1 ) + appendResult56;
				float2 uv9 = i.ase_texcoord.xy * float2( 1,1 ) + -appendResult56;
				float2 appendResult17 = (float2(-_BlurSize , _BlurSize));
				float2 uv10 = i.ase_texcoord.xy * float2( 1,1 ) + appendResult17;
				float2 appendResult20 = (float2(_BlurSize , -_BlurSize));
				float2 uv11 = i.ase_texcoord.xy * float2( 1,1 ) + appendResult20;
				float2 uv_DepthMask = i.ase_texcoord.xy * _DepthMask_ST.xy + _DepthMask_ST.zw;
				float4 tex2DNode31 = tex2D( _DepthMask, uv_DepthMask );
				float4 lerpResult32 = lerp( tex2D( _MainTex, uv_MainTex ) , ( ( tex2D( _MainTex, uv8 ) + tex2D( _MainTex, uv9 ) + tex2D( _MainTex, uv10 ) + tex2D( _MainTex, uv11 ) ) / 4.0 ) , ( tex2DNode31.g + tex2DNode31.b ));
				
				
				finalColor = lerpResult32;
				return finalColor;
			}
			ENDCG
		}
	}
}