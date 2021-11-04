// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ParallaxMapShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_LightColor("LightColor", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_ParallaxMap("_ParallaxMap (RGB)", 2D) = "white" {}
		_NormalMap("NormalMap", 2D) = "white" {}
		[PerRendererData] _MainLightPosition("MainLightPosition", Vector) = (0,0,0,0)
		[PerRendererData] _HeightScale("height scale", Float) = 0.01
	}
	SubShader{
	Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
	Pass{
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
		LOD 200

		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma vertex vs_main alpha
		#pragma fragment fs_main alpha
		#pragma multi_compile_instancing 

		//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

		//#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"

		struct VS_IN
		{
			float4 position : POSITION;

			float3 normal : NORMAL;
			float4 tangent : TANGENT;
			float2 uv : TEXCOORD0;

			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct VS_OUT
		{
			float4 position : POSITION;
			float2 uv : TEXCOORD0;
			float3 lightdir : TEXCOORD1;
			float3 viewdir : TEXCOORD2;

			float3 T : TEXCOORD3;
			float3 B : TEXCOORD4;
			float3 N : TEXCOORD5;

			UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
		};

		uniform float4 _Color;
		uniform float4 _LightColor;

		uniform float3 _MainLightPosition;

		uniform sampler _MainTex;
		uniform sampler _ParallaxMap;
		uniform sampler _NormalMap;

		uniform float _HeightScale;


		VS_OUT vs_main(VS_IN input)
		{
			VS_OUT output;

			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_INITIALIZE_OUTPUT(VS_OUT, output);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
			// calc output position directly
			output.position = UnityObjectToClipPos(input.position);

			// pass uv coord
			output.uv = input.uv;
			//output.color.a = input.color.a;

			// calc lightDir vector heading current vertex
			float4 worldPosition = mul(unity_ObjectToWorld, input.position);
			float3 lightDir = worldPosition.xyz - _MainLightPosition.xyz;
			output.lightdir = normalize(lightDir);

			// calc Normal, Binormal, Tangent vector in world space
			// cast 1st arg to 'float3x3' (type of input.normal is 'float3')
			float3 worldNormal = mul((float3x3)unity_ObjectToWorld, input.normal);
			float3 worldTangent = mul((float3x3)unity_ObjectToWorld, input.tangent);

			float3 binormal = cross(input.normal, input.tangent.xyz) * input.tangent.w;
			float3 worldBinormal = mul((float3x3)unity_ObjectToWorld, binormal);

			// and, set them
			output.N = normalize(worldNormal);
			output.T = normalize(worldTangent);
			output.B = normalize(worldBinormal);

			float3x3 TBN2 = transpose(float3x3(output.T, output.B, output.N));

			float3 worldPosition2 = mul(worldPosition,TBN2);
			float3 camPos = mul(_WorldSpaceCameraPos,TBN2);

			// calc viewDir vector 
			//float3 viewDir = normalize(worldPosition.xyz - _WorldSpaceCameraPos.xyz);
			float3 viewDir = normalize(worldPosition2.xyz - camPos.xyz);
			output.viewdir = viewDir;

			return output;
		}

		float2 parallax_mapping(float3x3 TBN, float2 tex_coord, float3 viewdir)
		{
			//viewdir = mul(TBN, viewdir);

			const float minLayers = 8;
			const float maxLayers = 32;
			float numLayers = lerp(maxLayers, minLayers, abs(dot(float3(0.0, 0.0, 1.0), viewdir)));
			// calculate the size of each layer
			float layerDepth = 1.0 / numLayers;
			// depth of current layer
			float currentLayerDepth = 0.0;
			int curLayer = 0;

			float2 P = viewdir.xy / viewdir.z * _HeightScale;
			float2 deltaTexCoords = P / numLayers;

			float2 currentTexCoords = tex_coord;
			float currentDepthMapValue = tex2D(_ParallaxMap, currentTexCoords).r;

			[loop]
			while (currentLayerDepth < currentDepthMapValue)
			{
				// shift texture coordinates along direction of P
				currentTexCoords -= deltaTexCoords;
				// get depthmap value at current texture coordinates
				currentDepthMapValue = tex2D(_ParallaxMap, currentTexCoords).r;
				// get depth of next layer
				currentLayerDepth += layerDepth;
				curLayer += 1;

				if (curLayer >= numLayers)
					break;
			}

			// get texture coordinates before collision (reverse operations)
			float2 prevTexCoords = currentTexCoords + deltaTexCoords;

			// get depth after and before collision for linear interpolation
			float afterDepth = currentDepthMapValue - currentLayerDepth;
			float beforeDepth = tex2D(_ParallaxMap, prevTexCoords).r - currentLayerDepth + layerDepth;

			// interpolation of texture coordinates
			float weight = afterDepth / (afterDepth - beforeDepth);
			float2 finalTexCoords = prevTexCoords * weight + currentTexCoords * (1.0 - weight);

			return finalTexCoords;
		}

		float4 fs_main(VS_OUT input) : COLOR
		{
			// 'TBN' transforms the world space into a tangent space
			// we need its inverse matrix
			// Tip : An inverse matrix of orthogonal matrix is its transpose matrix
			float3x3 TBN = float3x3(normalize(input.T), normalize(input.B), normalize(input.N));
			TBN = transpose(TBN);

			// twist here
			input.uv = parallax_mapping(TBN, input.uv, input.viewdir);
			if (input.uv.x > 1.0 || input.uv.y > 1.0 || input.uv.x < 0.0 || input.uv.y < 0.0)
				discard;

			// obtain a normal vector on tangent space
			float3 tangentNormal = tex2D(_NormalMap, input.uv).xyz;
			// and change range of values (0 ~ 1)
			tangentNormal = normalize(tangentNormal * 2 - 1);

			// finally we got a normal vector from the normal map
			float3 worldNormal = mul(TBN, tangentNormal);

			// Lambert here (cuz we're calculating Normal vector in this pixel shader)
			float4 albedo = tex2D(_MainTex, input.uv);
			//albedo *= UNITY_ACCESS_INSTANCED_PROP(PerInstance, _Color);
			float3 lightDir = worldNormal * -1;// normalize(input.lightdir);
			// calc diffuse, as we did in pixel shader
			float3 diffuse = saturate(dot(worldNormal, -lightDir));
			diffuse = albedo.rgb * diffuse;//_LightColor * 

			// Specular here
			float3 specular = 0;
			if (diffuse.x > 0) {
				float3 reflection = reflect(lightDir, worldNormal);
				float3 viewDir = normalize(input.viewdir);

				specular = saturate(dot(reflection, -viewDir));
				specular = pow(specular, 20.0f);
			}

			// make some ambient,
			float3 ambient = float3(0.1f, 0.1f, 0.1f) * 3 * albedo;

			// combine all of colors
			return float4(diffuse, albedo.a);// ambient + specular
		}
			ENDCG
		}
		}
			//FallBack "Diffuse"
}