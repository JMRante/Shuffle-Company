Shader "Custom/StandardStageTexture" 
{
	Properties 
	{
		_Albedo ("Albedo", 2DArray) = "white" {}
		_Smoothness ("Smoothness", 2DArray) = "white" {}
		_Metallic ("Metallic", 2DArray) = "white" {}
		_Normal ("Normal", 2DArray) = "white" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.5 target, to use texture arrays
		#pragma target 3.5

		struct Input 
		{
			float2 uv_Albedo;
			float2 uv2_Albedo;
			float2 uv3_Albedo;
			float2 uv4_Albedo;
			float2 uv5_Albedo;
		};
		
		UNITY_DECLARE_TEX2DARRAY(_Albedo);
		UNITY_DECLARE_TEX2DARRAY(_Smoothness);
		UNITY_DECLARE_TEX2DARRAY(_Metallic);
		UNITY_DECLARE_TEX2DARRAY(_Normal);

		void vert (inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			// o.uv_AlbedoArray = v.texcoord1;
			// o.layer1AlbedoSmoothIndex = v.texcoord2;
			// o.layer1MetalNormalIndex = v.texcoord3;
			// o.layer2AlbedoSmoothIndex = v.texcoord4;
			// o.layer2MetalNormalIndex = v.texcoord5;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float2 uv = IN.uv_Albedo;
			float4 layer1Index = float4(IN.uv2_Albedo, IN.uv3_Albedo);
			// float4 layer2Index = float4(IN.layer2AlbedoSmoothIndex, IN.layer2MetalNormalIndex);

			fixed4 c1 = UNITY_SAMPLE_TEX2DARRAY(_Albedo, float3(uv, layer1Index.x));
			// fixed4 c2 = UNITY_SAMPLE_TEX2DARRAY(_AlbedoArray, float3(uv, layer2Index.x));
			// o.Albedo = (c2.rgb * c2.a) + (c1.rgb * (1.0 - c2.a));
			// o.Albedo = fixed3(0, 0, layer1Index.x);

			// half4 s1 = UNITY_SAMPLE_TEX2DARRAY(_Smoothness, float3(uv, layer1Index.y));
			// o.Smoothness = s1.r;

			// half4 m1 = UNITY_SAMPLE_TEX2DARRAY(_Metallic, float3(uv, layer1Index.z));
			// o.Metallic = m1.r;

			// float3 n1 = UnpackNormal(UNITY_SAMPLE_TEX2DARRAY(_Normal, float3(uv, layer1Index.w)));
			// float3 n2 = UnpackNormal(UNITY_SAMPLE_TEX2DARRAY(_Normal, float3(uv, layer2Index.w)));
			// o.Normal = normalize(float3(n1.xy + n2.xy, n1.z));
		}
		ENDCG
	}
	FallBack "Diffuse"
}
