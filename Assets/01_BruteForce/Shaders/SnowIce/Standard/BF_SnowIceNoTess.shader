// MADE BY MATTHIEU HOULLIER
// Copyright 2022 BRUTE FORCE, all rights reserved.
// You are authorized to use this work if you have purchased the asset.
// Mail me at bruteforcegamesstudio@gmail.com if you have any questions or improvements you need.
Shader "BruteForce/Standard/SnowIceNoTessellation" {

	Properties{

		[Header(IIIIIIII          Snow Textures          IIIIIIII)]
		[Space]
		_MainTex("Snow Albedo", 2D) = "white" {}
		_MainTexMult("Snow Albedo Multiplier", Range(0,2)) = 0.11
		[MainColor][HDR]_Color("Snow Tint", Color) = (0.77,0.86,0.91,1)
		_OverallScale("Overall Scale", Float) = 1
		[Space]
		[NoScaleOffset]_BumpMap("Snow Bumpmap", 2D) = "white" {}
		_NormalMultiplier("Snow Bumpmap Multiplier", Range(0,2)) = 0.4
		_SnowNormalScale("Snow Bumpmap Scale", Range(0,2)) = 1

		[Space(20)]
		[NoScaleOffset]_SpecGlossMap("Snow Specular", 2D) = "black" {}
		[NoScaleOffset]_LittleSpec("Snow Little Spec", 2D) = "black" {}
		_LittleSpecForce("Little Spec Multiplier", Float) = 0.5

		[NoScaleOffset]_SnowHeight("Snow Displacement Texture", 2D) = "white" {}
		_HeightScale("Displacement Scale", Float) = 0.33
		_DisplacementStrength("Displacement Strength", Float) = 0.3
		_DisplacementOffset("Displacement Offset", Float) = 0.1
		_DisplacementColorMult("Displacement Color Multiplier", Float) = 0.95
		_DisplacementShadowMult("Displacement Shadow Multiplier",  Range(0,2)) = 0.56
		_UpVector("Up Vector", Float) = 1
		_NormalVector("Normal Vector", Float) = 0

		[Space(20)]
		[NoScaleOffset]_SnowTransition("Snow Transition", 2D) = "black" {}
		_TransitionScale("Transition Scale", Float) = 0.73
		_TransitionPower("Transition Power", Float) = 0.22
		_TransitionColor("Transition Color (additive only)", Color) = (1,1,1,1)


		[Space(10)]
		[Header(IIIIIIII          Snow Values          IIIIIIII)]
		[Space(10)]
		_MountColor("Mount Color", color) = (0.12,0.12,0.121,1)
		_BotColor("Dig Color", color) = (0.71,0.87,0.91,0)
		_NormalRTDepth("Normal Effect Depth", Range(0,3)) = 0.12
		_NormalRTStrength("Normal Effect Strength", Range(0,4)) = 2.2
		_AddSnowStrength("Mount Snow Strength", Range(0,3)) = 0.52
		_RemoveSnowStrength("Dig Snow Strength", Range(0,3)) = 0.5
		_SnowScale("Snow Scale", float) = 1

		[Space(10)]
		[Header(IIIIIIII          Ice Textures          IIIIIIII)]
		[Space(10)]
		_IceTex("Ice Albedo", 2D) = "white" {}
		[HDR]_IceTint("Ice Texture Tint", Color) = (0.14,0.35,0.49,1)
		[Space]
		[NoScaleOffset]_ParallaxLayers("Parallax Ice Texture", 2D) = "black" {}
		_ParallaxStrength("Parallax Fade Strength", vector) = (-1.04, 0.125, -0.13, 0.1)
		_OffsetScale("Parallax Offset Scale", float) = 0.14
		[NoScaleOffset]_NormalTex("Ice Normal Texture", 2D) = "black" {}
		_NormalScale("Ice Normal Scale", Range(0,1)) = 0.766
		[NoScaleOffset]_Roughness("Ice Roughness Texture", 2D) = "black" {}
		[NoScaleOffset]_UnderWaterFish("Under Water Fish", 2D) = "black" {}
		_FishScale("Fish Scale", float) = 1
		_FishSpeed("Fish Speed", float) = 1
		_FishIntensity("Fish Intensity", float) = 1
		_FishAngle("Fish Angle", float) = 0
		_FishMaskIntensity("Fish Mask Intensity", Range(0,1)) = 0.25


		[Space(10)]
		[Header(IIIIIIII          Ice Values          IIIIIIII)]
		[Space(10)]
		[HDR]_ParalaxColor("Parallax Color", Color) = (0,0,0,1)
		_ParalaxColorScale("Parallax Color Scale", Range(-2,2)) = -0.88
		_IceTrail("Ice Trail Color", Color) = (0.40,0.1,0.01,1)
		_TransparencyValue("Ice Transparency", Range(0,1)) = 1
		_IceScale("Ice Scale", float) = 1

		[Space(10)]
		[Header(IIIIIIII          Custom Fog          IIIIIIII)]
		[Space(10)]
		[NoScaleOffset]_FogTex("Fog Texture", 2D) = "black" {}
		[NoScaleOffset]_FlowTex("Flow Texture", 2D) = "black" {}
		_FlowMultiplier("Flow Multiplier", Range(0,1)) = 0.3
		_FogIntensity("Fog Intensity", Range(0,1)) = 0.3
		_FogScale("Fog Scale", float) = 1
		_FogDirection("Fog Direction", vector) = (1, 0.3, 2, 0)

		[Space(10)]
		[Header(IIIIIIII          Lighting          IIIIIIII)]
		[Space(10)]
		_ProjectedShadowColor("Projected Shadow Color",Color) = (0.17 ,0.56 ,0.1,1)
		_SpecColor("Specular Color", Color) = (1,1,1,1)
		_SpecForce("Specular Force", Float) = 3
		_RoughnessStrength("Ice Roughness Strength", Float) = 1.75
		_ShininessIce("Ice Shininess", Float) = 10
		_ShininessSnow("Snow Shininess", Float) = 25
		[Space]
		_LightOffset("Light Offset", Range(0, 1)) = 0.2
		_LightHardness("Light Hardness", Range(0, 1)) = 0.686
		_ReflectionOpacity("Reflection Opacity", Float) = 0.25
		_ReflectionStrength("Rim Ice Strength", Range(0, 2)) = 0.07
		_ReflectionColor("Rim Ice Color", Color) = (0,0.1,0.15,0.8)
		_RimColor("Rim Snow Color", Color) = (0.03,0.03,0.03,0)
		_LightIntensity("Additional Lights Intensity", Range(0.00, 4)) = 1

		[Header(Procedural Tiling)]
		[Space]
		[Toggle(USE_PR)] _UsePR("Use Procedural Tiling (Reduce performance)", Float) = 0
		_ProceduralDistance("Tile start distance", Float) = 5.5
		_ProceduralStrength("Tile Smoothness", Float) = 1.5
		[Space]

		[Space(10)]
		[Header(IIIIIIII          Pragmas          IIIIIIII)]
		[Space(10)]
		[Toggle(IS_ICE)] _ISICE("Is Only Ice", Float) = 0
		[Toggle(IS_SNOW)] _ISSNOW("Is Only Snow", Float) = 0
		[Toggle(IS_UNLIT)] _ISUNLIT("Is Unlit", Float) = 0
		[Toggle(IS_ADD)] _ISADD("Is Additive Snow", Float) = 0
		[HideInInspector][Toggle(USE_INTER)] _USEINTER("Use Intersection", Float) = 0
		[Toggle(USE_AL)] _UseAmbientLight("Use Ambient Light", Float) = 1
		[Toggle(USE_LS)] _UseSunReflection("Use Sun Reflection", Float) = 1
		[Toggle(USE_RT)] _USERT("Use RT", Float) = 1
		[Toggle(IS_T)] _IST("Is Terrain", Float) = 0
		[Toggle(USE_VR)] _UseVR("Use For VR", Float) = 0
		[Toggle(USE_WC)] _USEWC("Use World Displacement", Float) = 1
		[Toggle(USE_WT)] _USEWT("Use World Coordinates", Float) = 0
		[Toggle(USE_FOG)] _USEFOG("Use Custom Fog", Float) = 1
		[Toggle(USE_LOW)] _USELOW("Use Low End", Float) = 0

			// TERRAIN PROPERTIES //
			[HideInInspector] _Control0("Control0 (RGBA)", 2D) = "white" {}
			[HideInInspector] _Control1("Control1 (RGBA)", 2D) = "white" {}
			// Textures
			[HideInInspector] _Splat0("Layer 0 (R)", 2D) = "white" {}
			[HideInInspector] _Splat1("Layer 1 (G)", 2D) = "white" {}
			[HideInInspector] _Splat2("Layer 2 (B)", 2D) = "white" {}
			[HideInInspector] _Splat3("Layer 3 (A)", 2D) = "white" {}
			[HideInInspector] _Splat4("Layer 4 (R)", 2D) = "white" {}
			[HideInInspector] _Splat5("Layer 5 (G)", 2D) = "white" {}
			[HideInInspector] _Splat6("Layer 6 (B)", 2D) = "white" {}
			[HideInInspector] _Splat7("Layer 7 (A)", 2D) = "white" {}

			// Normal Maps
			[HideInInspector] _Normal0("Normal 0 (R)", 2D) = "bump" {}
			[HideInInspector] _Normal1("Normal 1 (G)", 2D) = "bump" {}
			[HideInInspector] _Normal2("Normal 2 (B)", 2D) = "bump" {}
			[HideInInspector] _Normal3("Normal 3 (A)", 2D) = "bump" {}
			[HideInInspector] _Normal4("Normal 4 (R)", 2D) = "bump" {}
			[HideInInspector] _Normal5("Normal 5 (G)", 2D) = "bump" {}
			[HideInInspector] _Normal6("Normal 6 (B)", 2D) = "bump" {}
			[HideInInspector] _Normal7("Normal 7 (A)", 2D) = "bump" {}

			// Normal Scales
			[HideInInspector] _NormalScale0("Normal Scale 0 ", Float) = 1
			[HideInInspector] _NormalScale1("Normal Scale 1 ", Float) = 1
			[HideInInspector] _NormalScale2("Normal Scale 2 ", Float) = 1
			[HideInInspector] _NormalScale3("Normal Scale 3 ", Float) = 1
			[HideInInspector] _NormalScale4("Normal Scale 4 ", Float) = 1
			[HideInInspector] _NormalScale5("Normal Scale 5 ", Float) = 1
			[HideInInspector] _NormalScale6("Normal Scale 6 ", Float) = 1
			[HideInInspector] _NormalScale7("Normal Scale 7 ", Float) = 1

				// Mask Maps
				[HideInInspector] _Mask0("Mask 0 (R)", 2D) = "bump" {}
				[HideInInspector] _Mask1("Mask 1 (G)", 2D) = "bump" {}
				[HideInInspector] _Mask2("Mask 2 (B)", 2D) = "bump" {}
				[HideInInspector] _Mask3("Mask 3 (A)", 2D) = "bump" {}
				[HideInInspector] _Mask4("Mask 4 (R)", 2D) = "bump" {}
				[HideInInspector] _Mask5("Mask 5 (G)", 2D) = "bump" {}
				[HideInInspector] _Mask6("Mask 6 (B)", 2D) = "bump" {}
				[HideInInspector] _Mask7("Mask 7 (A)", 2D) = "bump" {}

				// specs color
				[HideInInspector] _Specular0("Specular 0 (R)", Color) = (1,1,1,1)
				[HideInInspector] _Specular1("Specular 1 (G)", Color) = (1,1,1,1)
				[HideInInspector] _Specular2("Specular 2 (B)", Color) = (1,1,1,1)
				[HideInInspector] _Specular3("Specular 3 (A)", Color) = (1,1,1,1)
				[HideInInspector] _Specular4("Specular 4 (R)", Color) = (1,1,1,1)
				[HideInInspector] _Specular5("Specular 5 (G)", Color) = (1,1,1,1)
				[HideInInspector] _Specular6("Specular 6 (B)", Color) = (1,1,1,1)
				[HideInInspector] _Specular7("Specular 7 (A)", Color) = (1,1,1,1)

					// Metallic
					[HideInInspector] _Metallic0("Metallic0", Float) = 0
					[HideInInspector] _Metallic1("Metallic1", Float) = 0
					[HideInInspector] _Metallic2("Metallic2", Float) = 0
					[HideInInspector] _Metallic3("Metallic3", Float) = 0
					[HideInInspector] _Metallic4("Metallic4", Float) = 0
					[HideInInspector] _Metallic5("Metallic5", Float) = 0
					[HideInInspector] _Metallic6("Metallic6", Float) = 0
					[HideInInspector] _Metallic7("Metallic7", Float) = 0

					[HideInInspector] _Splat0_ST("Size0", Vector) = (1,1,0)
					[HideInInspector] _Splat1_ST("Size1", Vector) = (1,1,0)
					[HideInInspector] _Splat2_ST("Size2", Vector) = (1,1,0)
					[HideInInspector] _Splat3_ST("Size3", Vector) = (1,1,0)
					[HideInInspector] _Splat4_STn("Size4", Vector) = (1,1,0)
					[HideInInspector] _Splat5_STn("Size5", Vector) = (1,1,0)
					[HideInInspector] _Splat6_STn("Size6", Vector) = (1,1,0)
					[HideInInspector] _Splat7_STn("Size7", Vector) = (1,1,0)

					[HideInInspector] _TerrainScale("Terrain Scale", Vector) = (1, 1 ,0)
					// TERRAIN PROPERTIES //
	}

		CGINCLUDE
					// Reused functions //

#pragma shader_feature IS_T
#pragma shader_feature IS_ICE
#pragma shader_feature IS_SNOW
#pragma shader_feature USE_VR
#pragma shader_feature USE_COMPLEX_T

#include "UnityCG.cginc"

#ifdef IS_T

				// TERRAIN DATA //
					sampler2D _Control0;
#ifdef USE_COMPLEX_T
				sampler2D _Control1;
#endif
				half4 _Specular0, _Specular1, _Specular2, _Specular3;
				float4 _Splat0_ST, _Splat1_ST, _Splat2_ST, _Splat3_ST;
				half _Metallic0, _Metallic1, _Metallic2, _Metallic3;
				half _NormalScale0, _NormalScale1, _NormalScale2, _NormalScale3;
				Texture2D _Splat0, _Splat1, _Splat2, _Splat3;
				Texture2D _Normal0, _Normal1, _Normal2, _Normal3;
				Texture2D _Mask0, _Mask1, _Mask2, _Mask3;

#ifdef USE_COMPLEX_T
				half4 _Specular4, _Specular5, _Specular6, _Specular7;
				float4 _Splat4_STn, _Splat5_STn, _Splat6_STn, _Splat7_STn;
				half _Metallic4, _Metallic5, _Metallic6, _Metallic7;
				half _NormalScale4, _NormalScale5, _NormalScale6, _NormalScale7;
				Texture2D _Splat4, _Splat5, _Splat6, _Splat7;
				Texture2D _Normal4, _Normal5, _Normal6, _Normal7;
				Texture2D _Mask4, _Mask5, _Mask6, _Mask7;
#endif

				float3 _TerrainScale;
				// TERRAIN DATA //
#endif
				SamplerState my_linear_repeat_sampler;
				SamplerState my_bilinear_repeat_sampler;
				SamplerState my_trilinear_repeat_sampler;
				SamplerState my_linear_clamp_sampler;

				float4 RotateAroundZInDegrees(float4 vertex, float degrees)
				{
					float alpha = degrees * UNITY_PI / 180.0;
					float sina, cosa;
					sincos(alpha, sina, cosa);
					float2x2 m = float2x2(cosa, -sina, sina, cosa);
					return float4(mul(m, vertex.zy), vertex.xw).zyxw;
				}
				float4 RotateAroundXInDegrees(float4 vertex, float degrees)
				{
					float alpha = degrees * UNITY_PI / 180.0;
					float sina, cosa;
					sincos(alpha, sina, cosa);
					float2x2 m = float2x2(cosa, -sina, sina, cosa);
					return float4(mul(m, vertex.xy), vertex.zw).xyzw;
				}

				float4 multQuat(float4 q1, float4 q2) {
					return float4(
						q1.w * q2.x + q1.x * q2.w + q1.z * q2.y - q1.y * q2.z,
						q1.w * q2.y + q1.y * q2.w + q1.x * q2.z - q1.z * q2.x,
						q1.w * q2.z + q1.z * q2.w + q1.y * q2.x - q1.x * q2.y,
						q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z
						);
				}

				float3 rotateVector(float4 quat, float3 vec) {
					// https://twistedpairdevelopment.wordpress.com/2013/02/11/rotating-a-vector-by-a-quaternion-in-glsl/
					float4 qv = multQuat(quat, float4(vec, 0.0));
					return multQuat(qv, float4(-quat.x, -quat.y, -quat.z, quat.w)).xyz;
				}

				ENDCG

					SubShader{

						Pass {
							Tags {
								"LightMode" = "ForwardBase"
							}
								Blend SrcAlpha OneMinusSrcAlpha

							CGPROGRAM
					// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
					#pragma exclude_renderers gles

					#pragma target 3.0


					#pragma multi_compile _ LOD_FADE_CROSSFADE

					#pragma multi_compile_fwdbase
					#pragma multi_compile_fog
					#pragma multi_compile _ LIGHTMAP_ON

					#pragma vertex vert
					#pragma fragment frag

					#define FORWARD_BASE_PASS
					#pragma shader_feature USE_AL
					#pragma shader_feature USE_RT
					#pragma shader_feature IS_ADD
					#pragma shader_feature USE_INTER
					#pragma shader_feature USE_WC
					#pragma shader_feature USE_WT
					#pragma shader_feature USE_LS
					#pragma shader_feature USE_LOW
					#pragma shader_feature USE_PR
					#pragma shader_feature USE_FOG
					#pragma shader_feature IS_UNLIT


					#include "UnityPBSLighting.cginc"
					#include "AutoLight.cginc"

			struct VertexData //appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;

			#ifdef SHADOWS_SCREEN
				SHADOW_COORDS(1)
#endif
							UNITY_FOG_COORDS(2)
#ifdef USE_VR
							UNITY_VERTEX_INPUT_INSTANCE_ID
#endif
#ifdef IS_ADD
#ifdef USE_INTER
							float2 uv3 : TEXCOORD3;
							float2 uv4 : TEXCOORD4;
							float2 uv6 : TEXCOORD6;
						float2 uv7 : TEXCOORD7;
#endif
#endif
#ifdef LIGHTMAP_ON
						float2 lmap : TEXCOORD5;
#endif
					};

					struct InterpolatorsVertex
					{
						float4 vertex : SV_POSITION;
						float3 normal : TEXCOORD1;
						float4 tangent : TANGENT;
						float4 uv : TEXCOORD0;
						float4 color : COLOR;
						float3 worldPos : TEXCOORD2;
						float3 viewDir: POSITION1;
						float3 normalDir: TEXCOORD3;

					#ifdef SHADOWS_SCREEN
						SHADOW_COORDS(4)
					#endif
							UNITY_FOG_COORDS(5)
#ifdef USE_VR
							UNITY_VERTEX_OUTPUT_STEREO
#endif

#ifdef LIGHTMAP_ON
					float2 lmap : TEXCOORD6;
#endif

					float3 lightTS : TEXCOORD7; // light Direction in tangent space
					};

					sampler2D  _DetailTex;
					float4 _MainTex_ST, _DetailTex_ST;
					float4 _IceTex_ST;

					sampler2D _NormalMap;

					float _BumpScale, _DetailBumpScale;

					half4 _Color;

					// Render Texture Effects //
					uniform sampler2D _GlobalEffectRT;
					uniform float3 _Position;
					uniform float _OrthographicCamSize;
					uniform sampler2D _GlobalEffectRTAdditional;
					uniform float3 _PositionAdd;
					uniform float _OrthographicCamSizeAdditional;

					sampler2D _MainTex;
					sampler2D _SpecGlossMap;
					sampler2D _BumpMap;
					Texture2D _LittleSpec;

					Texture2D _UnderWaterFish;

					half4 _MountColor;
					half4 _BotColor;

					float _SpecForce, _LittleSpecForce, _UpVector, _NormalVector, _IceScale, _SnowScale, _TransparencyValue;
					float _NormalRTDepth, _NormalRTStrength, _AddSnowStrength, _RemoveSnowStrength, _DisplacementStrength, _NormalMultiplier;

					//ICE Variables
					sampler2D _IceTex;
					Texture2D _NormalTex;
					sampler2D _ParallaxLayers;
					Texture2D _Roughness;
					sampler2D _SnowHeight;
					Texture2D _SnowTransition;
					float _TransitionScale;
					float _TransitionPower;
					float _HeightScale;
					float _LightOffset;
					float _LightHardness;
					float _LightIntensity;
					float _DisplacementColorMult, _ReflectionOpacity, _DisplacementShadowMult;
					float _FogIntensity, _FogScale, _FlowMultiplier;

					Texture2D _FogTex;
					Texture2D _FlowTex;

					half _OffsetScale;
					half _OverallScale;
					half _RoughnessStrength;
					half4 _ParallaxStrength;

					half _NormalScale, _ParalaxColorScale, _DisplacementOffset, _SnowNormalScale, _MainTexMult;
					half4 _IceTint, _ParalaxColor;
					half4 _IceTrail;

					float _ShininessIce, _ShininessSnow;
					float _ReflectionStrength;
					float _HasRT;
					float _FishScale, _FishSpeed, _FishIntensity, _FishAngle, _FishMaskIntensity;
					float4 _ProjectedShadowColor, _TransitionColor, _ReflectionColor, _RimColor;
					float3 _FogDirection;
					float _ProceduralDistance, _ProceduralStrength;


					float3 calcNormal(float2 texcoord, sampler2D globalEffect)
					{
						const float3 off = float3(-0.0005 * _NormalRTDepth, 0, 0.0005 * _NormalRTDepth); // texture resolution to sample exact texels
						const float2 size = float2(0.002, 0.0); // size of a single texel in relation to world units

					#ifdef USE_LOW

						float sS = tex2Dlod(globalEffect, float4(texcoord.xy + 1 * off.xy * 10, 0, 0)).y;
						float s01 = sS * 0.245945946 * _NormalRTDepth;
						float s21 = sS * 0.216216216 * _NormalRTDepth;
						float s10 = sS * 0.540540541 * _NormalRTDepth;
						float s12 = sS * 0.162162162 * _NormalRTDepth;

						float gG = tex2Dlod(globalEffect, float4(texcoord.xy + 1 * off.xy, 0, 0)).z;
						float g01 = gG * 1.945945946 * _NormalRTDepth;
						float g21 = gG * 1.216216216 * _NormalRTDepth;
						float g10 = gG * 0.540540541 * _NormalRTDepth;
						float g12 = gG * 0.162162162 * _NormalRTDepth;

						float3 va = normalize(float3(size.xy, 0)) + normalize(float3(size.xy, g21 - g01));
						float3 vb = normalize(float3(size.yx, 0)) + normalize(float3(size.xy, g12 - g10));

						float3 vc = normalize(float3(size.xy, 0)) + normalize(float3(size.xy, s21 - s01));
						float3 vd = normalize(float3(size.yx, 0)) + normalize(float3(size.xy, s12 - s10));

						float3 calculatedNormal = normalize(cross(va, vb));
						calculatedNormal.y = normalize(cross(vc, vd)).x;
						return calculatedNormal;

					#else

						float s01 = tex2Dlod(globalEffect, float4(texcoord.xy + 4 * off.xy * 10, 0, 0)).y * 0.245945946 * _NormalRTDepth;
						float s21 = tex2Dlod(globalEffect, float4(texcoord.xy + 3 * off.zy * 10, 0, 0)).y * 0.216216216 * _NormalRTDepth;
						float s10 = tex2Dlod(globalEffect, float4(texcoord.xy + 2 * off.yx * 10, 0, 0)).y * 0.540540541 * _NormalRTDepth;
						float s12 = tex2Dlod(globalEffect, float4(texcoord.xy + 1 * off.yz * 10, 0, 0)).y * 0.162162162 * _NormalRTDepth;

						float g01 = tex2Dlod(globalEffect, float4(texcoord.xy + 4 * off.xy, 0, 0)).z * 1.945945946 * _NormalRTDepth;
						float g21 = tex2Dlod(globalEffect, float4(texcoord.xy + 3 * off.zy, 0, 0)).z * 1.216216216 * _NormalRTDepth;
						float g10 = tex2Dlod(globalEffect, float4(texcoord.xy + 2 * off.yx, 0, 0)).z * 0.540540541 * _NormalRTDepth;
						float g12 = tex2Dlod(globalEffect, float4(texcoord.xy + 1 * off.yz, 0, 0)).z * 0.162162162 * _NormalRTDepth;

						float3 va = normalize(float3(size.xy, 0)) + normalize(float3(size.xy, g21 - g01));
						float3 vb = normalize(float3(size.yx, 0)) + normalize(float3(size.xy, g12 - g10));

						float3 vc = normalize(float3(size.xy, 0)) + normalize(float3(size.xy, s21 - s01));
						float3 vd = normalize(float3(size.yx, 0)) + normalize(float3(size.xy, s12 - s10));

						float3 calculatedNormal = normalize(cross(va, vb));
						calculatedNormal.y = normalize(cross(vc, vd)).x;
						return calculatedNormal;
					#endif
					}

					float4 blendMultiply(float4 baseTex, float4 blendTex, float opacity)
					{
						float4 baseBlend = baseTex * blendTex;
						float4 ret = lerp(baseTex, baseBlend, opacity);
						return ret;
					}

					float2 hash2D2D(float2 s)
					{
						//magic numbers
						return frac(sin(s) * 4.5453);
					}

					//stochastic sampling
					float4 tex2DStochastic(sampler2D tex, float2 UV)
					{
						float4x3 BW_vx;
						float2 skewUV = mul(float2x2 (1.0, 0.0, -0.57735027, 1.15470054), UV * 3.464);

						//vertex IDs and barycentric coords
						float2 vxID = float2 (floor(skewUV));
						float3 barry = float3 (frac(skewUV), 0);
						barry.z = 1.0 - barry.x - barry.y;

						BW_vx = ((barry.z > 0) ?
							float4x3(float3(vxID, 0), float3(vxID + float2(0, 1), 0), float3(vxID + float2(1, 0), 0), barry.zyx) :
							float4x3(float3(vxID + float2 (1, 1), 0), float3(vxID + float2 (1, 0), 0), float3(vxID + float2 (0, 1), 0), float3(-barry.z, 1.0 - barry.y, 1.0 - barry.x)));

						//calculate derivatives to avoid triangular grid artifacts
						float2 dx = ddx(UV);
						float2 dy = ddy(UV);

						float4 stochasticTex = mul(tex2D(tex, UV + hash2D2D(BW_vx[0].xy), dx, dy), BW_vx[3].x) +
							mul(tex2D(tex, UV + hash2D2D(BW_vx[1].xy), dx, dy), BW_vx[3].y) +
							mul(tex2D(tex, UV + hash2D2D(BW_vx[2].xy), dx, dy), BW_vx[3].z);
						return stochasticTex;
					}


					InterpolatorsVertex vert(VertexData v) {
						InterpolatorsVertex i;

#ifdef USE_VR
						UNITY_SETUP_INSTANCE_ID(v);
						UNITY_INITIALIZE_OUTPUT(InterpolatorsVertex, i);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(i);
#endif

#ifdef IS_T
						_HeightScale = _HeightScale * 0.066; // correct the heightScale to what it was before update 1.3
#endif

						float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
						float3 originalPos = worldPos;

						//RT Cam effects
						float2 uv = worldPos.xz - _Position.xz;
						uv = uv / (_OrthographicCamSize * 2);
						uv += 0.5;

						float2 uvAdd = worldPos.xz - _PositionAdd.xz;
						uvAdd = uvAdd / (_OrthographicCamSizeAdditional * 2);
						uvAdd += 0.5;

						float3 rippleMain = 0;
						float3 rippleMainAdditional = 0;

						float ripples = 0;
						float ripples2 = 0;
						float ripples3 = 0;

						float uvRTValue = 0;
								_OverallScale = _OverallScale * 1;

#ifdef LIGHTMAP_ON
						i.lmap = v.lmap.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif

					#ifdef USE_RT
						if (_HasRT == 1)
						{
							// .b(lue) = Snow Dig / .r(ed) = Snow To Ice / .g(reen) = Snow Mount
							rippleMain = tex2Dlod(_GlobalEffectRT, float4(uv, 0, 0));
							rippleMainAdditional = tex2Dlod(_GlobalEffectRTAdditional, float4(uvAdd, 0, 0));
						}

					#ifdef IS_ICE
					#else
						float2 uvGradient = smoothstep(0, 5, length(max(abs(_Position.xz - worldPos.xz) - _OrthographicCamSize + 5, 0.0)));
						uvRTValue = saturate(uvGradient.x);

						if (v.color.b > 0.95 && v.color.g < 0.05)
						{
						}
						else
						{
							ripples = lerp(rippleMain.x, rippleMainAdditional.x, uvRTValue);
							ripples2 = lerp(rippleMain.y, rippleMainAdditional.y, uvRTValue);
							ripples3 = lerp(rippleMain.z, rippleMainAdditional.z, uvRTValue);
						}
					#endif

					#endif

						float slopeValue = 0;
					#ifdef IS_T
						half4 splat_control = tex2Dlod(_Control0, float4(v.uv, 0,0));

						#ifdef USE_COMPLEX_T
						half4 splat_control1 = tex2Dlod(_Control1, float4(v.uv, 0, 0));
						#endif

#ifdef USE_COMPLEX_T
						float iceValue = saturate(1 - splat_control.r * _Metallic0 - splat_control.g * _Metallic1 - splat_control.b * _Metallic2 - splat_control.a * _Metallic3
							- splat_control1.r * _Metallic4 - splat_control1.g * _Metallic5 - splat_control1.b * _Metallic6 - splat_control1.a * _Metallic7
							- ripples);
#else
						float iceValue = saturate(1 - splat_control.r * _Metallic0 - splat_control.g * _Metallic1 - splat_control.b * _Metallic2 - splat_control.a * _Metallic3
							- ripples);
#endif

						float snowHeightNew = _Mask0.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r;
						snowHeightNew = lerp(snowHeightNew, _Mask1.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control.g * (1 - _Metallic1)));
						snowHeightNew = lerp(snowHeightNew, _Mask2.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control.b * (1 - _Metallic2)));
						snowHeightNew = lerp(snowHeightNew, _Mask3.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control.a * (1 - _Metallic3)));
#ifdef USE_COMPLEX_T
						snowHeightNew = lerp(snowHeightNew, _Mask4.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control1.r * (1 - _Metallic4)));
						snowHeightNew = lerp(snowHeightNew, _Mask5.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control1.g * (1 - _Metallic5)));
						snowHeightNew = lerp(snowHeightNew, _Mask6.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control1.b * (1 - _Metallic6)));
						snowHeightNew = lerp(snowHeightNew, _Mask7.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control1.a * (1 - _Metallic7)));
#endif
						float snowHeight = snowHeightNew;
#else
						float iceValue = 0;
#ifdef USE_INTER
#ifdef IS_ADD			
						// custom intersection and slope value //
						float4 midPoint = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0));

						float4 quaternion = float4(v.uv6.x, -v.uv6.y, -v.uv7.x, -v.uv7.y);
						float3 offsetPoint = worldPos.xyz - midPoint;

						float3 rotatedVert = rotateVector(quaternion, -offsetPoint);
						float manualLerp = 0;
						manualLerp = v.uv4.x;

						rotatedVert = RotateAroundZInDegrees(float4(rotatedVert, 0), lerp(6,-6, (manualLerp)));
						rotatedVert = RotateAroundXInDegrees(float4(rotatedVert, 0), lerp(-55,55, (manualLerp))) + midPoint;

						slopeValue = ((v.color.a) - (rotatedVert.y - 0.5));

						if (slopeValue > 0.0)
						{
							v.color.g = saturate(v.color.g + saturate(slopeValue * 3));
							v.color.b = saturate(v.color.b + saturate(slopeValue * 3));
						}
#endif
#endif
						if (v.color.b > 0.6 && v.color.g < 0.4)
						{
							iceValue = saturate(1 - v.color.b);
						}
						else
						{
							iceValue = saturate((v.color.g + v.color.b) / 2 - ripples);
						}

					#ifdef USE_WC
						float snowHeight = tex2Dlod(_SnowHeight, float4(originalPos.xz, 0, 0) * _HeightScale * 0.1 * _SnowScale * _OverallScale).r;
					#else
						float2 snowUV = float2(v.uv.x * _MainTex_ST.x + _MainTex_ST.z, v.uv.y * _MainTex_ST.y + _MainTex_ST.w);
						float snowHeight = tex2Dlod(_SnowHeight, float4(snowUV, 0, 0) * _HeightScale * _SnowScale * _OverallScale).r;
					#endif

					#endif

						i.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject));
					#ifdef IS_ICE
					#else

					#ifdef USE_RT
						if (_HasRT == 1)
						{
							if (v.color.b > 0.95 && v.color.g < 0.05)
							{
								i.normal = normalize(v.normal);
							}
							else
							{
								i.normal = normalize(lerp(v.normal, calcNormal(uv, _GlobalEffectRT).rbb, iceValue));
							}
						}
						else
						{
							i.normal = normalize(v.normal);
						}
					#else
						i.normal = normalize(v.normal);
					#endif

					#ifdef IS_ADD
						float3 newNormal = normalize(i.normalDir);
						worldPos += ((float4(0, -_RemoveSnowStrength, 0, 0) * _UpVector - newNormal * _RemoveSnowStrength * _NormalVector) * ripples3 + (float4(0, _AddSnowStrength * snowHeight, 0, 0) * _UpVector + newNormal * _AddSnowStrength * snowHeight * _NormalVector) * ripples2 * saturate(1 - ripples3)) * saturate(iceValue * 3);
						worldPos += (float4(0, _DisplacementOffset, 0, 0) * _UpVector + newNormal * _DisplacementOffset * _NormalVector) * saturate(iceValue * 2.5);
						worldPos += (float4(0, 2 * _DisplacementStrength * snowHeight , 0, 0) * _UpVector) + (newNormal * 2 * _DisplacementStrength * snowHeight * _NormalVector * clamp(slopeValue * 20, 1, 2)) * saturate(saturate(iceValue * 2.5));

						worldPos = lerp(worldPos, mul(unity_ObjectToWorld, v.vertex), saturate(v.color.g - saturate(v.color.r + v.color.b)));

						v.vertex.xyz = lerp(mul(unity_WorldToObject, float4(originalPos, 1)).xyz, mul(unity_WorldToObject, float4(worldPos, 1)).xyz, iceValue);
					#else
						float3 newNormal = normalize(i.normalDir);
						worldPos += ((float4(0, -_RemoveSnowStrength, 0, 0) * _UpVector - newNormal * _RemoveSnowStrength * _NormalVector) * ripples3 + (float4(0, _AddSnowStrength * snowHeight, 0, 0) * _UpVector + newNormal * _AddSnowStrength * snowHeight * _NormalVector) * ripples2 * saturate(1 - ripples3)) * saturate(iceValue * 3);
						worldPos += (float4(0, _DisplacementOffset, 0, 0) * _UpVector + newNormal * _DisplacementOffset * _NormalVector) * saturate(iceValue * 2.5);
						worldPos += (float4(0, 2 * _DisplacementStrength * snowHeight, 0, 0) * _UpVector) + (newNormal * 2 * _DisplacementStrength * snowHeight * _NormalVector) * saturate(saturate(iceValue * 2.5));

						worldPos = lerp(worldPos, mul(unity_ObjectToWorld, v.vertex), saturate(v.color.g - saturate(v.color.r + v.color.b)));

						v.vertex.xyz = lerp(mul(unity_WorldToObject, float4(originalPos, 1)).xyz, mul(unity_WorldToObject, float4(worldPos, 1)).xyz, iceValue);

					#endif
					#endif

					#ifdef USE_RT
						if (_HasRT == 1)
						{
							v.color = lerp(v.color, saturate(float4(1, 0, 0, 1)), ripples);
						}
					#endif
						i.vertex = UnityObjectToClipPos(v.vertex);

						float4 objCam = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1.0));
						float3 viewDir = v.vertex.xyz - objCam.xyz;

					#ifdef IS_T
						float4 tangent = float4 (1.0, 0.0, 0.0, -1.0);
						tangent.xyz = tangent.xyz - v.normal * dot(v.normal, tangent.xyz); // Orthogonalize tangent to normal.

						float tangentSign = tangent.w * unity_WorldTransformParams.w;
						float3 bitangent = cross(v.normal.xyz, tangent.xyz) * tangentSign;

						i.viewDir = float3(
							dot(viewDir, tangent.xyz),
							dot(viewDir, bitangent.xyz),
							dot(viewDir, v.normal.xyz)
							);

						i.worldPos.xyz = mul(unity_ObjectToWorld, v.vertex);
						i.tangent = tangent;

					#else
						float tangentSign = v.tangent.w * unity_WorldTransformParams.w;
						float3 bitangent = cross(v.normal.xyz, v.tangent.xyz) * tangentSign;

						i.viewDir = float3(
							dot(viewDir, v.tangent.xyz),
							dot(viewDir, bitangent.xyz),
							dot(viewDir, v.normal.xyz)
							);

						i.worldPos.xyz = mul(unity_ObjectToWorld, v.vertex);
						i.tangent = v.tangent;
					#endif

						i.color = v.color;

					#ifdef IS_T
						i.uv.xy = v.uv;
					#else
						i.uv.xy = v.uv * _OverallScale;
					#endif
						i.uv.zw = TRANSFORM_TEX(v.uv, _DetailTex);

					#ifdef SHADOWS_SCREEN
						i._ShadowCoord = ComputeScreenPos(i.vertex);
					#endif

#ifdef IS_ADD
#ifdef USE_INTER
						// NORMAL BASED ON HIT NORMAL //
						 i.normalDir = lerp(i.normalDir, normalize(float3(v.uv3.x, v.uv3.y, v.uv4.y)), saturate(slopeValue));
						 i.normal = normalize(float3(v.uv3.x, v.uv3.y, v.uv4.y));
#endif
#endif					

						 // Basic lighting from lightsource pos //
						 half3 localCoords = half3(0,0,0);

						 float3 worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
						 float3 tangentB = normalize(UnityObjectToWorldDir(v.tangent.xyz));
						 float3 bitangentB = normalize(cross(tangentB, worldNormal) * v.tangent.w);

						 //normal transpose matrix
						 float3x3 local2WorldTranspose = float3x3
							 (
								 tangentB,
								 bitangentB,
								 worldNormal
								 );

						 //Calculate normal direction
						 i.lightTS = normalize(mul(localCoords, local2WorldTranspose));

#ifdef SHADER_API_MOBILE
						 TRANSFER_VERTEX_TO_FRAGMENT(i);
#endif
						UNITY_TRANSFER_FOG(i,i.vertex);
						return i;
					}


					float4 frag(InterpolatorsVertex i) : SV_Target
					{
#ifdef USE_VR
								UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i)
#endif
								// Linear to Gamma //
half gamma = 0.454545;

							float4 bakedColorTex = 0;
							float shadowmap = 0;
#ifdef IS_T
							_HeightScale = _HeightScale * 0.066; // correct the heightScale to what it was before update 1.3
#endif
#ifdef LIGHTMAP_ON
#ifdef LIGHTMAP_SHADOW_MIXING
							bakedColorTex = saturate(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap) * saturate(SHADOW_ATTENUATION(i) + 0.5));
#else
							bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap);
#endif
							shadowmap = DecodeLightmap(bakedColorTex);
#else

							shadowmap = SHADOW_ATTENUATION(i);
#endif

#ifdef IS_UNLIT
	half4 lightColor = half4(1,1,1,1);
#else

#ifdef LIGHTMAP_ON
	half4 lightColor = bakedColorTex;
#else
	half4 lightColor = _LightColor0;
	#endif
#endif


#if !UNITY_COLORSPACE_GAMMA
	//_Color = pow(_Color, gamma);
	_TransitionColor = pow(_TransitionColor, gamma);
	_MountColor = pow(_MountColor, gamma);
	_BotColor = pow(_BotColor, gamma);
	_IceTint = pow(_IceTint, 0.55) * 1.5;
	_ParalaxColor = pow(_ParalaxColor, gamma);
	_IceTrail = pow(_IceTrail, gamma);
	//_ProjectedShadowColor = pow(_ProjectedShadowColor, gamma);
	_SpecColor = pow(_SpecColor, gamma);
	_ReflectionColor = pow(_ReflectionColor, gamma);
	_RimColor = pow(_RimColor, gamma);
	lightColor = pow(lightColor, gamma);
	_LittleSpecForce = pow(_LittleSpecForce, gamma) * 1.5;

	#ifdef IS_T
						_Specular0 = pow(_Specular0,gamma);
						_Specular1 = pow(_Specular1,gamma);
						_Specular2 = pow(_Specular2,gamma);
						_Specular3 = pow(_Specular3,gamma);
	#ifdef USE_COMPLEX_T
						_Specular4 = pow(_Specular4,gamma);
						_Specular5 = pow(_Specular5,gamma);
						_Specular6 = pow(_Specular6,gamma);
						_Specular7 = pow(_Specular7,gamma);
	#endif
	#endif

	#endif

									float uvRTValue = 0;
									float2 uv = i.worldPos.xz - _Position.xz;
									uv = uv / (_OrthographicCamSize * 2);
									uv += 0.5;

									float2 uvAdd = i.worldPos.xz - _PositionAdd.xz;
									uvAdd = uvAdd / (_OrthographicCamSizeAdditional * 2);
									uvAdd += 0.5;

									float3 rippleMain = 0;
									float3 rippleMainAdditional = 0;
									float3 calculatedNormal = 0;
									float3 calculatedNormalAdd = 0;

									float ripples = 0;
									float ripples2 = 0;
									float ripples3 = 0;
									float iceValue = 1;


									_OverallScale = _OverallScale * 1;


	#ifdef USE_PR
						float dist = clamp(lerp(0, 1, (distance(_WorldSpaceCameraPos, i.worldPos) - _ProceduralDistance) / max(_ProceduralStrength,0.05)), 0, 1);
	#endif

								#ifdef USE_WT
									float2 snowUV = float2(i.worldPos.x + _MainTex_ST.z, i.worldPos.z + _MainTex_ST.w) * _HeightScale * 0.1 * _OverallScale;
									float2 iceUV = float2(i.worldPos.x + _IceTex_ST.z, i.worldPos.z + _IceTex_ST.w) * _HeightScale * 0.1 * _OverallScale * 3;
								#else
									float2 snowUV = float2(i.uv.x * _MainTex_ST.x + _MainTex_ST.z, i.uv.y * _MainTex_ST.y + _MainTex_ST.w);
									float2 iceUV = float2(i.uv.x * _IceTex_ST.x + _IceTex_ST.z, i.uv.y * _IceTex_ST.y + _IceTex_ST.w) * 3;
								#endif


						#ifdef IS_ICE
									float snowHeight = 0;
									float snowHeightReal = 0;
						#else
									float snowHeight = saturate(_SnowTransition.Sample(my_linear_repeat_sampler, snowUV * _TransitionScale * _SnowScale).r);

						#ifdef USE_WC

						#ifdef USE_PR
									float snowHeightReal = lerp(tex2D(_SnowHeight, float2(i.worldPos.x, i.worldPos.z) * _HeightScale * 0.1 * _SnowScale * _OverallScale).r,tex2DStochastic(_SnowHeight, float2(i.worldPos.x, i.worldPos.z) * _HeightScale * 0.1 * _SnowScale * _OverallScale).r,dist);
						#else
									float snowHeightReal = tex2D(_SnowHeight, float2(i.worldPos.x, i.worldPos.z) * _HeightScale * 0.1 * _SnowScale * _OverallScale).r;
						#endif

						#else

						#ifdef USE_PR
									float snowHeightReal = lerp(tex2D(_SnowHeight, snowUV.xy * _HeightScale * _SnowScale * _OverallScale).r,tex2DStochastic(_SnowHeight, snowUV.xy * _HeightScale * _SnowScale * _OverallScale).r,dist);
						#else
									float snowHeightReal = tex2D(_SnowHeight, snowUV.xy * _HeightScale * _SnowScale * _OverallScale).r;
						#endif
						#endif


						#endif

						#ifdef IS_ICE
						#else
						#ifdef USE_RT

									if (_HasRT == 1)
									{
										rippleMain = tex2D(_GlobalEffectRT, uv);
										rippleMainAdditional = tex2D(_GlobalEffectRTAdditional, uvAdd);

										float2 uvGradient = smoothstep(0, 5, length(max(abs(_Position.xz - i.worldPos.xz) - _OrthographicCamSize + 5, 0.0)));
										uvRTValue = saturate(uvGradient.x);
										ripples = lerp(rippleMain.x, rippleMainAdditional.x, uvRTValue);
									}
						#endif
						#endif



						#ifdef IS_T
									half4 splat_control = tex2D(_Control0, i.uv);

									#ifdef USE_COMPLEX_T
										half4 splat_control1 = tex2D(_Control1, i.uv);
									#endif


									splat_control.r = lerp(splat_control.r, 0, saturate(ripples));
									splat_control.g = lerp(splat_control.g, 1, saturate(ripples));

									#ifdef USE_COMPLEX_T
										splat_control1.r = lerp(splat_control1.r, 0, saturate(ripples));
										splat_control1.g = lerp(splat_control1.g, 1, saturate(ripples));
									#endif

	#ifdef USE_COMPLEX_T
									float splatOverall = saturate(1 - splat_control.r * _Metallic0 - splat_control.g * _Metallic1 - splat_control.b * _Metallic2 - splat_control.a * _Metallic3
									- splat_control1.r * _Metallic4 - splat_control1.g * _Metallic5 - splat_control1.b * _Metallic6 - splat_control1.a * _Metallic7);
	#else								
									float splatOverall = saturate(1 - splat_control.r * _Metallic0 - splat_control.g * _Metallic1 - splat_control.b * _Metallic2 - splat_control.a * _Metallic3);
	#endif
	#ifdef IS_SNOW
									iceValue = 1;
	#else
									iceValue = pow(splatOverall, 1 + clamp(abs((snowHeight - 0.5) * 20) * -_TransitionPower * splatOverall, -1, 1));
	#endif
									i.uv = i.uv * _OverallScale;

									float3 SnowSplat0 = _Splat0.Sample(my_linear_repeat_sampler, i.uv * (_TerrainScale.xz / _Splat0_ST.xy));
									SnowSplat0 = lerp(pow(SnowSplat0, 0.4) * _Specular0 * 2, lerp(1, saturate(pow(SnowSplat0, 2)), _MainTexMult), 1 - _Metallic0);
									float3 SnowSplat1 = _Splat1.Sample(my_linear_repeat_sampler, i.uv * (_TerrainScale.xz / _Splat1_ST.xy));
									SnowSplat1 = lerp(pow(SnowSplat1, 0.4) * _Specular1 * 2, lerp(1, saturate(pow(SnowSplat1, 2)), _MainTexMult), 1 - _Metallic1);
									float3 SnowSplat2 = _Splat2.Sample(my_linear_repeat_sampler, i.uv * (_TerrainScale.xz / _Splat2_ST.xy));
									SnowSplat2 = lerp(pow(SnowSplat2, 0.4) * _Specular2 * 2, lerp(1, saturate(pow(SnowSplat2, 2)), _MainTexMult), 1 - _Metallic2);
									float3 SnowSplat3 = _Splat3.Sample(my_linear_repeat_sampler, i.uv * (_TerrainScale.xz / _Splat3_ST.xy));
									SnowSplat3 = lerp(pow(SnowSplat3, 0.4) * _Specular3 * 2, lerp(1, saturate(pow(SnowSplat3, 2)), _MainTexMult), 1 - _Metallic3);

	#ifdef USE_COMPLEX_T 
									float3 SnowSplat4 = _Splat4.Sample(my_linear_repeat_sampler, float4(i.uv * (float2(_Splat4_STn.x, _Splat4_STn.y)),0,0));
									SnowSplat4 = lerp(pow(SnowSplat4, 0.4) * _Specular4 * 2, lerp(1, saturate(pow(SnowSplat4, 2)), _MainTexMult), 1 - _Metallic4);
									float3 SnowSplat5 = _Splat5.Sample(my_linear_repeat_sampler, i.uv * (float2(_Splat5_STn.x, _Splat5_STn.y)));
									SnowSplat5 = lerp(pow(SnowSplat5, 0.4) * _Specular5 * 2, lerp(1, saturate(pow(SnowSplat5, 2)), _MainTexMult), 1 - _Metallic5);
									float3 SnowSplat6 = _Splat6.Sample(my_linear_repeat_sampler, i.uv * (float2(_Splat6_STn.x, _Splat6_STn.y)));
									SnowSplat6 = lerp(pow(SnowSplat6, 0.4) * _Specular6 * 2, lerp(1, saturate(pow(SnowSplat6, 2)), _MainTexMult), 1 - _Metallic6);
									float3 SnowSplat7 = _Splat7.Sample(my_linear_repeat_sampler, i.uv * (float2(_Splat7_STn.x, _Splat7_STn.y)));
									SnowSplat7 = lerp(pow(SnowSplat7, 0.4) * _Specular7 * 2, lerp(1, saturate(pow(SnowSplat7, 2)), _MainTexMult), 1 - _Metallic7);
	#endif

									float3 SnowNormal0 = UnpackScaleNormal(_Normal0.Sample(my_linear_repeat_sampler, i.uv * (_TerrainScale.xz / _Splat0_ST.xy)), _NormalScale0);
									SnowNormal0 = lerp(SnowNormal0, SnowNormal0, 1 - _Metallic0);
									float3 SnowNormal1 = UnpackScaleNormal(_Normal1.Sample(my_linear_repeat_sampler, i.uv * (_TerrainScale.xz / _Splat1_ST.xy)), _NormalScale1);
									SnowNormal1 = lerp(SnowNormal1, SnowNormal1, 1 - _Metallic1);
									float3 SnowNormal2 = UnpackScaleNormal(_Normal2.Sample(my_linear_repeat_sampler, i.uv * (_TerrainScale.xz / _Splat2_ST.xy)), _NormalScale2);
									SnowNormal2 = lerp(SnowNormal2, SnowNormal2, 1 - _Metallic2);
									float3 SnowNormal3 = UnpackScaleNormal(_Normal3.Sample(my_linear_repeat_sampler, i.uv * (_TerrainScale.xz / _Splat3_ST.xy)), _NormalScale3);
									SnowNormal3 = lerp(SnowNormal3, SnowNormal3, 1 - _Metallic3);

	#ifdef USE_COMPLEX_T
									float3 SnowNormal4 = UnpackScaleNormal(_Normal4.Sample(my_linear_repeat_sampler, i.uv * (float2(_Splat4_STn.x, _Splat4_STn.y))), _NormalScale4);
									SnowNormal4 = lerp(SnowNormal4, SnowNormal4, 1 - _Metallic4);
									float3 SnowNormal5 = UnpackScaleNormal(_Normal5.Sample(my_linear_repeat_sampler, i.uv * (float2(_Splat5_STn.x, _Splat5_STn.y))), _NormalScale5);
									SnowNormal5 = lerp(SnowNormal5, SnowNormal5, 1 - _Metallic5);
									float3 SnowNormal6 = UnpackScaleNormal(_Normal6.Sample(my_linear_repeat_sampler, i.uv * (float2(_Splat6_STn.x, _Splat6_STn.y))), _NormalScale6);
									SnowNormal6 = lerp(SnowNormal6, SnowNormal6, 1 - _Metallic6);
									float3 SnowNormal7 = UnpackScaleNormal(_Normal7.Sample(my_linear_repeat_sampler, i.uv * (float2(_Splat7_STn.x, _Splat7_STn.y))), _NormalScale7);
									SnowNormal7 = lerp(SnowNormal7, SnowNormal7, 1 - _Metallic7);
	#endif

									// SNOW NORMAL //
									float3 normal = lerp(0, SnowNormal0, splat_control.r * (1 - _Metallic0));
									normal = lerp(normal, SnowNormal1, saturate(splat_control.g * (1 - _Metallic1)));
									normal = lerp(normal, SnowNormal2, saturate(splat_control.b * (1 - _Metallic2)));
									normal = lerp(normal, SnowNormal3, saturate(splat_control.a * (1 - _Metallic3)));
	#ifdef USE_COMPLEX_T
									normal = lerp(normal, SnowNormal4, saturate(splat_control1.r * (1 - _Metallic4)));
									normal = lerp(normal, SnowNormal5, saturate(splat_control1.g * (1 - _Metallic5)));
									normal = lerp(normal, SnowNormal6, saturate(splat_control1.b * (1 - _Metallic6)));
									normal = lerp(normal, SnowNormal7, saturate(splat_control1.a * (1 - _Metallic7)));
	#endif

									// ICE NORMAL //
									half3 normalTex = lerp(UnpackScaleNormal(_Normal0.Sample(my_linear_repeat_sampler, i.uv * _IceScale), _NormalScale), SnowNormal0, splat_control.r * (_Metallic0));
									normalTex = lerp(normalTex, SnowNormal1 * _NormalScale1, splat_control.g * (_Metallic1));
									normalTex = lerp(normalTex, SnowNormal2 * _NormalScale2, splat_control.b * (_Metallic2));
									normalTex = lerp(normalTex, SnowNormal3 * _NormalScale3, splat_control.a * (_Metallic3));
	#ifdef USE_COMPLEX_T
									normalTex = lerp(normalTex, SnowNormal4 * _NormalScale4, splat_control1.r * (_Metallic4));
									normalTex = lerp(normalTex, SnowNormal5 * _NormalScale5, splat_control1.g * (_Metallic5));
									normalTex = lerp(normalTex, SnowNormal6 * _NormalScale6, splat_control1.b * (_Metallic6));
									normalTex = lerp(normalTex, SnowNormal7 * _NormalScale7, splat_control1.a * (_Metallic7));
	#endif
						#else
	#ifdef IS_SNOW
									iceValue = 1;
	#else
									iceValue = saturate(pow(saturate(i.color.g) , 1 + clamp(abs((snowHeight - 0.5) * 20) * -_TransitionPower * (saturate(i.color.g)), -1, 1)) * 1.25);
	#endif

									float3 normal = UnpackScaleNormal(tex2D(_BumpMap, (snowUV)*_SnowNormalScale * _SnowScale), _NormalMultiplier * 2).rgb * iceValue - i.normal;
									half3 normalTex = UnpackScaleNormal(_NormalTex.Sample(my_linear_repeat_sampler, iceUV * _IceScale), _NormalScale);
						#endif

						#ifdef IS_T
									half4 c = _Specular0;
									c = lerp(c,_Specular1, splat_control.g * (iceValue) * (1 - _Metallic1));
									c = lerp(c,_Specular2, splat_control.b * (iceValue) * (1 - _Metallic2));
									c = lerp(c,_Specular3, splat_control.a * (iceValue) * (1 - _Metallic3));

									#ifdef USE_COMPLEX_T
									c = lerp(c,_Specular4, splat_control1.r * (iceValue) * (1 - _Metallic4));
									c = lerp(c,_Specular5, splat_control1.g * (iceValue) * (1 - _Metallic5));
									c = lerp(c,_Specular6, splat_control1.b * (iceValue) * (1 - _Metallic6));
									c = lerp(c,_Specular7, splat_control1.a * (iceValue) * (1 - _Metallic7));

									#endif
						#else
									half4 c = _Color;
						#endif

						#ifdef IS_ICE
						#else
						#ifdef USE_RT

									if (_HasRT == 1)
									{
										if (i.color.b > 0.95 && i.color.g < 0.05)
										{
										}
										else
										{
											ripples2 = lerp(rippleMain.y, rippleMainAdditional.y, uvRTValue);
											ripples3 = lerp(rippleMain.z, rippleMainAdditional.z, uvRTValue);
											calculatedNormal = calcNormal(uv, _GlobalEffectRT);
											calculatedNormal.y = lerp(calculatedNormal.y, 0, saturate(ripples3 * 5));
											calculatedNormalAdd = calcNormal(uvAdd, _GlobalEffectRTAdditional);
											calculatedNormal = lerp(calculatedNormal, 0, uvRTValue);
										}

										c = lerp(
											c,
											_BotColor * 2 - 1,
											ripples3);

										c = lerp(
											c,
											c + _MountColor,
											saturate(saturate(ripples2 - ripples3) * saturate(snowHeight + 0.5) * 1));

										c.rgb = lerp(c.rgb, c.rgb * _BotColor, clamp(ripples3 * saturate(calculatedNormalAdd.r - 0.15) * _NormalRTStrength * 1, 0, 1));
									}
						#endif	
									c.rgb = c.rgb * lightColor; // HERE

									c.rgb = lerp(c.rgb * _Color * _DisplacementColorMult, c.rgb, snowHeightReal);
						#endif	
									float3 normalEffect = i.normal;
						#ifdef IS_T
									// SNOW LERP //
									float3 snowColor = SnowSplat0;
									snowColor = lerp(snowColor, SnowSplat1, saturate(splat_control.g * (1 - _Metallic1)));
									snowColor = lerp(snowColor, SnowSplat2, saturate(splat_control.b * (1 - _Metallic2)));
									snowColor = lerp(snowColor, SnowSplat3, saturate(splat_control.a * (1 - _Metallic3)));

									#ifdef USE_COMPLEX_T
									snowColor = lerp(snowColor, SnowSplat4, saturate(splat_control1.r * (1 - _Metallic4)));
									snowColor = lerp(snowColor, SnowSplat5, saturate(splat_control1.g * (1 - _Metallic5)));
									snowColor = lerp(snowColor, SnowSplat6, saturate(splat_control1.b * (1 - _Metallic6)));
									snowColor = lerp(snowColor, SnowSplat7, saturate(splat_control1.a * (1 - _Metallic7)));
									#endif	

									c.rgb *= snowColor;
						#else
									c *= lerp(1, saturate(pow(tex2D(_MainTex, snowUV * _SnowScale) + _MainTexMult * 0.225, 2)), _MainTexMult);


						#endif
						#ifdef USE_FOG
									// ADD SNOW FOG WITH DUST //
									half3 flowVal = (_FlowTex.Sample(my_bilinear_repeat_sampler, i.uv)) * _FlowMultiplier;

									float dif1 = frac(_Time.y * 0.15 + 0.5);
									float dif2 = frac(_Time.y * 0.15);

									half lerpVal = abs((0.5 - dif1) / 0.5);

									//_FogDirection
									half3 col1 = _FogTex.Sample(my_bilinear_repeat_sampler, i.uv * _FogScale - flowVal.xy * dif1 + (normalize(_FogDirection.xy) * _Time.y * -0.02 * _FogDirection.z));
									half3 col2 = _FogTex.Sample(my_bilinear_repeat_sampler, i.uv * _FogScale - flowVal.xy * dif2 + (normalize(_FogDirection.xy) * _Time.y * -0.02 * _FogDirection.z));

									half3 fogFlow = lerp(col1, col2, lerpVal);
									fogFlow = abs(pow(fogFlow, 5));
						#endif

									// ICE MATERIAL //
									float3 viewDirTangent = i.viewDir;
						#ifdef IS_T
									// ICE LERP //
									half4 IceTex = half4(SnowSplat1,1);
									IceTex.rgb = lerp(IceTex, SnowSplat1, splat_control.g * (_Metallic1));
									IceTex.rgb = lerp(IceTex, SnowSplat2, splat_control.b * (_Metallic2));
									IceTex.rgb = lerp(IceTex, SnowSplat3, splat_control.a * (_Metallic3));

						#ifdef USE_COMPLEX_T
									IceTex.rgb = lerp(IceTex, SnowSplat4, splat_control1.r * (_Metallic4));
									IceTex.rgb = lerp(IceTex, SnowSplat5, splat_control1.g * (_Metallic5));
									IceTex.rgb = lerp(IceTex, SnowSplat6, splat_control1.b * (_Metallic6));
									IceTex.rgb = lerp(IceTex, SnowSplat7, splat_control1.a * (_Metallic7));
						#endif
						#else

						#ifdef USE_PR
									half4 IceTex = half4(lerp(1, pow(lerp(tex2D(_IceTex, iceUV * _IceScale),tex2DStochastic(_IceTex, iceUV * _IceScale),dist), 0.4), _IceTint.a) * _IceTint.rgb * 2 ,1);
						#else
									half4 IceTex = half4(lerp(1, pow(tex2D(_IceTex, iceUV * _IceScale), 0.4), _IceTint.a) * _IceTint.rgb * 2 ,1);
						#endif

						#endif
									IceTex.rgb = IceTex.rgb * lightColor;

									float3 viewDirection = normalize(_WorldSpaceCameraPos - i.worldPos.xyz);
									float3 normalDirection = normalize(i.normalDir);

	#ifdef IS_T
									float3 normalDirectionWithNormal = normalize(i.normalDir) + normalize(i.normalDir) * normal;
	#else
									float3 normalDirectionWithNormal = normalize(i.normalDir) + normalize(i.normalDir) * normalize(abs(tex2D(_BumpMap, snowUV * _SnowNormalScale * _SnowScale))) * _NormalMultiplier;
	#endif

									half fresnelValue = lerp(0, 1, saturate(dot(normalDirection, viewDirection)));
									_OffsetScale = max(0, _OffsetScale);

									half parallax = 0;

									float3 newRoughnessTex = _Roughness.Sample(my_linear_repeat_sampler, iceUV * _IceScale).rgb;
									float alphaIce = 1;
									alphaIce = saturate(1 - saturate((newRoughnessTex.r + newRoughnessTex.g + newRoughnessTex.b) / 3));

									float underWaterFish = 0;

									float rotationAngle = _FishAngle * UNITY_PI / 180.0;
									float sina, cosa;
									sincos(rotationAngle, sina, cosa);
									float2x2 m = float2x2(cosa, -sina, sina, cosa);


						#ifdef IS_SNOW
						float3 albedo = c.rgb;

						#else

						#ifdef USE_LOW
										float customRatio = 0.5;
						#ifdef IS_T
											float3 j1UV = lerp(0, _OffsetScale, customRatio) * normalize(viewDirTangent) + normalTex;
											float j1parallax = _Mask0.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat0_ST.xy) + j1UV).r * _ParallaxStrength.y * _Metallic0 * splat_control.r;
											j1parallax += _Mask1.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat1_ST.xy) + j1UV).r * _ParallaxStrength.y * _Metallic1 * splat_control.g;
											j1parallax += _Mask2.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat2_ST.xy) + j1UV).r * _ParallaxStrength.y * _Metallic2 * splat_control.b;
											j1parallax += _Mask3.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat3_ST.xy) + j1UV).r * _ParallaxStrength.y * _Metallic3 * splat_control.a;
						#ifdef USE_COMPLEX_T
											j1parallax += _Mask4.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat4_STn.x, _Splat4_STn.y)) + j1UV).r * _ParallaxStrength.y * _Metallic4 * splat_control1.r;
											j1parallax += _Mask5.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat5_STn.x, _Splat5_STn.y)) + j1UV).r * _ParallaxStrength.y * _Metallic5 * splat_control1.g;
											j1parallax += _Mask6.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat6_STn.x, _Splat6_STn.y)) + j1UV).r * _ParallaxStrength.y * _Metallic6 * splat_control1.b;
											j1parallax += _Mask7.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat7_STn.x, _Splat7_STn.y)) + j1UV).r * _ParallaxStrength.y * _Metallic7 * splat_control1.a;
						#endif

											parallax += j1parallax;

											float2 compressedUV = i.uv * _IceScale * _FishScale * _OverallScale * (_TerrainScale.xz / _Splat1_ST.xy) + j1UV;
											float2 fishUV = mul(m, compressedUV);

											underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * _FishSpeed + fishUV).r;
											underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);

						#else
										// First layer of cracks.
										parallax += tex2D(_ParallaxLayers, iceUV * _IceScale + lerp(0, _OffsetScale, customRatio / 2) * normalize(viewDirTangent) + normalTex).r * _ParallaxStrength.y * alphaIce;
										for (int k = 0; k < 1; k++)
										{
											parallax += tex2D(_ParallaxLayers, iceUV * _IceScale + lerp(0, _OffsetScale, customRatio / 2 + k * 0.04) * normalize(viewDirTangent) + normalTex).r * _ParallaxStrength.y * alphaIce;

										}

										float2 compressedUV = iceUV * _IceScale * _FishScale + lerp(0, _OffsetScale * 0.5, customRatio) * normalize(viewDirTangent) + normalTex;
										float2 fishUV = mul(m, compressedUV);

										underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * _FishSpeed + fishUV).r;
										underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);

					#endif

					#else
								for (int j = 1; j < 4; j++)
								{
									float ratio = (float)j / (float)4;

									if (j == 1)
									{
					#ifdef IS_T
										float3 j1UV = lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex;

										float j1parallax = _Mask0.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat0_ST.xy) + j1UV).r * _ParallaxStrength.y * _Metallic0 * splat_control.r;
										j1parallax += _Mask1.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat1_ST.xy) + j1UV).r * _ParallaxStrength.y * _Metallic1 * splat_control.g;
										j1parallax += _Mask2.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat2_ST.xy) + j1UV).r * _ParallaxStrength.y * _Metallic2 * splat_control.b;
										j1parallax += _Mask3.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat3_ST.xy) + j1UV).r * _ParallaxStrength.y * _Metallic3 * splat_control.a;
					#ifdef USE_COMPLEX_T
										j1parallax += _Mask4.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat4_STn.x, _Splat4_STn.y)) + j1UV).r * _ParallaxStrength.y * _Metallic4 * splat_control1.r;
										j1parallax += _Mask5.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat5_STn.x, _Splat5_STn.y)) + j1UV).r * _ParallaxStrength.y * _Metallic5 * splat_control1.g;
										j1parallax += _Mask6.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat6_STn.x, _Splat6_STn.y)) + j1UV).r * _ParallaxStrength.y * _Metallic6 * splat_control1.b;
										j1parallax += _Mask7.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat7_STn.x, _Splat7_STn.y)) + j1UV).r * _ParallaxStrength.y * _Metallic7 * splat_control1.a;
					#endif

										parallax += j1parallax;

										float2 compressedUV = iceUV * _IceScale * _FishScale * _OverallScale * (_TerrainScale.xz / _Splat1_ST.xy) + j1UV;
										float2 fishUV = mul(m, compressedUV);

										underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * _FishSpeed + fishUV).r;
										underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);

					#else
										// First layer of cracks.
										parallax += tex2D(_ParallaxLayers, iceUV * _IceScale + lerp(0, _OffsetScale, ratio / 2) * normalize(viewDirTangent) + normalTex).r * _ParallaxStrength.y * alphaIce;
										for (int k = 0; k < 5; k++)
										{
											parallax += tex2D(_ParallaxLayers, iceUV * _IceScale + lerp(0, _OffsetScale, ratio / 2 + k * 0.04) * normalize(viewDirTangent) + normalTex).r * _ParallaxStrength.y * alphaIce;
										}

										float2 compressedUV = iceUV * _IceScale * _FishScale + lerp(0, _OffsetScale * 0.5, ratio) * normalize(viewDirTangent) + normalTex;
										float2 fishUV = mul(m, compressedUV);

										underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * _FishSpeed + fishUV).r;
										underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);
					#endif
									}
									else if (j == 2)
									{
					#ifdef IS_T
										float3 j2UV = lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex;

										float j2parallax = _Mask0.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat0_ST.xy) + j2UV).b * _ParallaxStrength.z * _Metallic0 * splat_control.r;
										j2parallax += _Mask1.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat1_ST.xy) + j2UV).b * _ParallaxStrength.z * _Metallic1 * splat_control.g;
										j2parallax += _Mask2.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat2_ST.xy) + j2UV).b * _ParallaxStrength.z * _Metallic2 * splat_control.b;
										j2parallax += _Mask3.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat3_ST.xy) + j2UV).b * _ParallaxStrength.z * _Metallic3 * splat_control.a;
					#ifdef USE_COMPLEX_T
										j2parallax += _Mask4.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat4_STn.x, _Splat4_STn.y)) + j2UV).b * _ParallaxStrength.z * _Metallic4 * splat_control1.r;
										j2parallax += _Mask5.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat5_STn.x, _Splat5_STn.y)) + j2UV).b * _ParallaxStrength.z * _Metallic5 * splat_control1.g;
										j2parallax += _Mask6.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat6_STn.x, _Splat6_STn.y)) + j2UV).b * _ParallaxStrength.z * _Metallic6 * splat_control1.b;
										j2parallax += _Mask7.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat7_STn.x, _Splat7_STn.y)) + j2UV).b * _ParallaxStrength.z * _Metallic7 * splat_control1.a;
					#endif


										parallax += j2parallax;



										float2 compressedUV = iceUV * _IceScale * _FishScale * _OverallScale * (_TerrainScale.xz / _Splat0_ST.xy) + lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex;
										float2 fishUV = mul(m, compressedUV);

										underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * _FishSpeed + fishUV).g;
										underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);
					#else
										// Second layer of cracks.
										parallax += tex2D(_ParallaxLayers, iceUV * _IceScale + lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex).b * _ParallaxStrength.z * alphaIce;
										for (int k = 0; k < 5; k++)
										{
											parallax += tex2D(_ParallaxLayers, iceUV * _IceScale + lerp(0, _OffsetScale, ratio + k * 0.04) * normalize(viewDirTangent) + normalTex).b * _ParallaxStrength.z * alphaIce;
										}

										float2 compressedUV = iceUV * _IceScale * _FishScale + lerp(0, _OffsetScale * 1, ratio) * normalize(viewDirTangent) + normalTex;
										float2 fishUV = mul(m, compressedUV);

										underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * 1.4 * _FishSpeed + fishUV).g * 0.75;
										underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);
					#endif
									}
									else if (j == 3)
									{
					#ifdef IS_T
										float3 j3UV = lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex;

										float j3parallax = _Mask0.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat0_ST.xy) + j3UV).g * _ParallaxStrength.w * _Metallic0 * splat_control.r;
										j3parallax += _Mask1.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat1_ST.xy) + j3UV).g * _ParallaxStrength.w * _Metallic1 * splat_control.g;
										j3parallax += _Mask2.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat2_ST.xy) + j3UV).g * _ParallaxStrength.w * _Metallic2 * splat_control.b;
										j3parallax += _Mask3.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat3_ST.xy) + j3UV).g * _ParallaxStrength.w * _Metallic3 * splat_control.a;
					#ifdef USE_COMPLEX_T
										j3parallax += _Mask4.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat4_STn.x, _Splat4_STn.y)) + j3UV).g * _ParallaxStrength.w * _Metallic4 * splat_control1.r;
										j3parallax += _Mask5.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat5_STn.x, _Splat5_STn.y)) + j3UV).g * _ParallaxStrength.w * _Metallic5 * splat_control1.g;
										j3parallax += _Mask6.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat6_STn.x, _Splat6_STn.y)) + j3UV).g * _ParallaxStrength.w * _Metallic6 * splat_control1.b;
										j3parallax += _Mask7.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat7_STn.x, _Splat7_STn.y)) + j3UV).g * _ParallaxStrength.w * _Metallic7 * splat_control1.a;
					#endif

										parallax += j3parallax;

										float2 compressedUV = iceUV * _IceScale * _FishScale * _OverallScale * (_TerrainScale.xz / _Splat0_ST.xy) + lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex;
										float2 fishUV = mul(m, compressedUV);

										underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * _FishSpeed + fishUV).b;
										underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);
					#else
										// Third layer of cracks.
										parallax += tex2D(_ParallaxLayers, iceUV * _IceScale + lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex).g * _ParallaxStrength.w * alphaIce - fresnelValue * 0.02;
										for (int k = 0; k < 5; k++)
										{
											parallax += tex2D(_ParallaxLayers, iceUV * _IceScale + lerp(0, _OffsetScale, ratio + k * 0.04) * normalize(viewDirTangent) + normalTex).g * _ParallaxStrength.w * alphaIce - fresnelValue * 0.02;
										}

										float2 compressedUV = iceUV * _IceScale * _FishScale + lerp(0, _OffsetScale * 2, ratio) * normalize(viewDirTangent) + normalTex;
										float2 fishUV = mul(m, compressedUV);

										underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * 2 * _FishSpeed + fishUV).b * 0.55;
										underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);
					#endif
									}
								}

					#endif

								parallax *= 1.5;
								parallax *= (1 + _ParallaxStrength.x);
								parallax = clamp(parallax, -2, 2);

								half parallaxDepth = clamp(parallax * pow(max(0.0, dot(reflect(-viewDirection, normalDirection * 1.3) * 0.4,viewDirection)), 1)  ,-2,2);

								#ifdef USE_LOW
								parallaxDepth -= 0.1;
								#endif

								half4 blended = 0;

								if (i.color.b > 0.95 && i.color.g < 0.05)
								{
									blended = blendMultiply(IceTex, parallax + parallaxDepth * 1, _ParalaxColorScale);
								}
								else
								{
									blended = blendMultiply(IceTex, parallax + parallaxDepth * 1, _ParalaxColorScale) - (_IceTrail * ripples3 * _IceTrail.a);
								}

								blended.rgb = lerp(blended.rgb + newRoughnessTex * 0.4, blended.rgb, alphaIce);

								float3 albedo = 1;
					#ifdef	IS_ICE
								albedo = blended * 0.5 + lerp(0, 1, parallaxDepth);

								if (parallaxDepth < 0)
								{
									albedo.rgb = lerp(albedo.rgb, _ParalaxColor.rgb - underWaterFish * _FishIntensity, saturate(abs(parallaxDepth)));
								}
								else
								{
									albedo.rgb = lerp(albedo.rgb, albedo.rgb + _ParalaxColor.rgb, saturate(abs(parallaxDepth)));
								}
					#else
					#ifdef IS_ADD
								albedo = lerp(c.rgb * _TransitionColor, c.rgb, saturate(pow(iceValue, 3)));
					#else
								albedo = lerp(blended * 0.5 + lerp(0, 1, parallaxDepth), c.rgb, saturate(pow(iceValue, 3)));

								if (parallaxDepth < 0)
								{
									albedo.rgb = lerp(albedo.rgb, _ParalaxColor.rgb - underWaterFish * _FishIntensity, saturate(abs(parallaxDepth)) * saturate(1 - pow(iceValue, 5)));
								}
								else
								{
#ifdef IS_T
									albedo.rgb = lerp(albedo.rgb, albedo.rgb + _ParalaxColor.rgb - underWaterFish * _FishIntensity, saturate(abs(parallaxDepth)) * saturate(1 - pow(iceValue, 5)));
#else
									albedo.rgb = lerp(albedo.rgb, albedo.rgb + _ParalaxColor.rgb , saturate(abs(parallaxDepth)) * saturate(1 - pow(iceValue, 5)));
#endif
								}
					#endif
					#endif

					#endif

								float3 lightDirection;
								float attenuation;

								half3 worldNormal;
#if !UNITY_COLORSPACE_GAMMA
								shadowmap = pow(shadowmap, gamma);
#endif

								float3 bumpMapLerp = lerp(normalTex, UnpackScaleNormal(tex2D(_BumpMap, snowUV * _SnowScale), _NormalMultiplier).rgb * 3 , iceValue);
								worldNormal.x = dot(normalDirection.x, bumpMapLerp);
								worldNormal.y = dot(normalDirection.y, bumpMapLerp);
								worldNormal.z = dot(normalDirection.z, bumpMapLerp);

								// basic lighting from sun pos //
								float diff = 0;

								float3 N = normalize(normalDirection);
								float3 fragmentToLight = _WorldSpaceLightPos0.xyz - i.worldPos.xyz;
								//float  distanceToLight = length(fragmentToLight);
								//float  atten = pow(2, -0.1 * distanceToLight * distanceToLight) * _WorldSpaceLightPos0.w + 1 - _WorldSpaceLightPos0.w;
								float3 L = normalize(fragmentToLight) * _WorldSpaceLightPos0.w + normalize(_WorldSpaceLightPos0.xyz) * (1 - _WorldSpaceLightPos0.w);

								diff = dot(N, L);
								half3 reflection = 0;
#ifdef USE_LOW
#else
								half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
								half3 worldRefl = reflect(-worldViewDir, worldNormal);
								half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldRefl);
								reflection = DecodeHDR(skyData, unity_SpecCube0_HDR);
								half reflectionMultiplier = lerp(1, 2, saturate(1 - shadowmap));
#endif

#if !UNITY_COLORSPACE_GAMMA
								reflection = pow(reflection, gamma);
#endif

								_ShininessIce = max(0.1, _ShininessIce);
								_ShininessSnow = max(0.1, _ShininessSnow);

								if (0.0 == _WorldSpaceLightPos0.w) // directional light
								{
									attenuation = 1.0; // no attenuation
									lightDirection = normalize(_WorldSpaceLightPos0.xyz);
								}
								else // point or spot light
								{
									float3 vertexToLightSource =
										_WorldSpaceLightPos0.xyz - i.worldPos.xyz;
									float distance = length(vertexToLightSource);
									attenuation = 1.0 / distance; // linear attenuation 
									lightDirection = normalize(vertexToLightSource);
								}

								float3 ambientLighting =
									UNITY_LIGHTMODEL_AMBIENT.rgb;
#if !UNITY_COLORSPACE_GAMMA
								ambientLighting = pow(ambientLighting, gamma);
#endif
								ambientLighting *= _Color.rgb;

								float3 diffuseReflection =
									attenuation * lightColor * _Color.rgb
									* max(0.0, dot(normalDirection, lightDirection));

#ifdef USE_LOW
#else
								diffuseReflection = diffuseReflection + reflection * reflection * reflectionMultiplier * _ReflectionOpacity;
#endif

								float3 specularReflection;
								if (dot(normalDirection, lightDirection) < 0.0)
									// light source on the wrong side
								{
									specularReflection = float3(0.0, 0.0, 0.0);
									// no specular reflection
								}
								else // light source on the right side
								{
									specularReflection = attenuation * lightColor
										* _SpecColor.rgb * pow(max(0.0, dot(
											reflect(-lightDirection, normalDirection),
											viewDirection)), lerp(_ShininessIce, _ShininessSnow, iceValue));
#ifdef USE_LOW
#else
									specularReflection = specularReflection + reflection * 0.2 * reflectionMultiplier;
#endif
								}

								float NdotL = 1;
#ifdef LIGHTMAP_ON
								NdotL = 1;
#else
								NdotL = 0.5 * (dot(_WorldSpaceLightPos0.xyz, normalDirectionWithNormal)) + 0.5; // Lambert Normal adjustement
								NdotL = lerp(NdotL, NdotL + saturate(snowHeightReal) * 0.1 * _DisplacementStrength - saturate(1 - snowHeightReal) * 0.1 * _DisplacementStrength, iceValue * _DisplacementShadowMult);
								NdotL = saturate(NdotL);
#endif


								float lightIntensity = smoothstep(0.1 + _LightOffset * clamp((_LightHardness + 0.5) * 2,1,10), (0.101 + _LightOffset) * clamp((_LightHardness + 0.5) * 2, 1, 10), NdotL * shadowmap);
								_SpecForce = max(0.1, _SpecForce);

								#ifdef IS_UNLIT
								lightIntensity = 1;
								#endif

								half3 shadowmapColor = lerp(_ProjectedShadowColor, 1, lightIntensity);

								float zDist = dot(_WorldSpaceCameraPos - i.worldPos, UNITY_MATRIX_V[2].xyz);
								float fadeDist = saturate(1 - UnityComputeShadowFade(UnityComputeShadowFadeDistance(i.worldPos, zDist)) * diff);
								shadowmapColor = lerp(1,shadowmapColor , saturate(fadeDist));

								albedo.xyz = albedo.xyz * saturate(shadowmapColor);

					#ifdef IS_T
								float4 specGloss = pow(tex2D(_SpecGlossMap, i.uv * 2 * (_TerrainScale.xz / _Splat0_ST.xy)), _SpecForce);
								float4 littleSpec = _LittleSpec.Sample(my_linear_repeat_sampler, i.uv * (_TerrainScale.xz / _Splat0_ST.xy)) * saturate(1 - ripples3);
					#else
								float4 specGloss = pow(tex2D(_SpecGlossMap, snowUV * 2 * _SnowScale), _SpecForce);
								float4 littleSpec = _LittleSpec.Sample(my_linear_repeat_sampler, snowUV * _SnowScale) * saturate(1 - ripples3);
					#endif

					#ifdef IS_SNOW
								half rougnessTex = 0;
					#else

					#ifdef IS_T
								half rougnessTemp = _Mask0.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat0_ST.xy)).r * 2 * _RoughnessStrength;

								rougnessTemp = lerp(rougnessTemp, _Mask1.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat1_ST.xy)).r * 2 * _RoughnessStrength, _Metallic1 * splat_control.g);
								rougnessTemp = lerp(rougnessTemp, _Mask2.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat2_ST.xy)).r * 2 * _RoughnessStrength, _Metallic2 * splat_control.b);
								rougnessTemp = lerp(rougnessTemp, _Mask3.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (_TerrainScale.xz / _Splat3_ST.xy)).r * 2 * _RoughnessStrength, _Metallic3 * splat_control.a);

					#ifdef USE_COMPLEX_T
								rougnessTemp = lerp(rougnessTemp, _Mask4.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat4_STn.x, _Splat4_STn.y))).r * 2 * _RoughnessStrength, _Metallic4 * splat_control1.r);
								rougnessTemp = lerp(rougnessTemp, _Mask5.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat5_STn.x, _Splat5_STn.y))).r * 2 * _RoughnessStrength, _Metallic5 * splat_control1.g);
								rougnessTemp = lerp(rougnessTemp, _Mask6.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat6_STn.x, _Splat6_STn.y))).r * 2 * _RoughnessStrength, _Metallic6 * splat_control1.b);
								rougnessTemp = lerp(rougnessTemp, _Mask7.Sample(my_linear_repeat_sampler, i.uv * _IceScale * (float2(_Splat7_STn.x, _Splat7_STn.y))).r * 2 * _RoughnessStrength, _Metallic7 * splat_control1.a);
					#endif

					#ifdef	IS_ICE
								half rougnessTex = rougnessTemp * saturate(1 - ripples3);
					#else
								half rougnessTex = rougnessTemp * saturate(1 - ripples3) * (1 - iceValue);
					#endif

					#else

					#ifdef	IS_ICE

					#ifdef USE_PR
								half rougnessTex = lerp(tex2D(_ParallaxLayers, iceUV * _IceScale).r,tex2DStochastic(_SnowHeight, iceUV * _IceScale).r * 0.4,dist) * 2 * _RoughnessStrength * saturate(1 - ripples3) * 1;
					#else
								half rougnessTex = tex2D(_ParallaxLayers, iceUV * _IceScale).r * 2 * _RoughnessStrength * saturate(1 - ripples3) * 1;
					#endif

					#else

					#ifdef USE_PR
								half rougnessTex = lerp(tex2D(_ParallaxLayers, iceUV * _IceScale).r,tex2DStochastic(_SnowHeight, iceUV * _IceScale).r * 0.4,dist) * 2 * _RoughnessStrength * saturate(1 - ripples3) * (1 - iceValue);
					#else
								half rougnessTex = tex2D(_ParallaxLayers, iceUV * _IceScale).r * 2 * _RoughnessStrength * saturate(1 - ripples3) * (1 - iceValue);
					#endif

					#endif
					#endif

					rougnessTex = lerp(0,rougnessTex, saturate(max(0.3,lightIntensity * 4) * lightColor.a));

					#endif

					#ifdef	IS_ICE
								specGloss.r = specGloss.r * saturate(normal);
					#else
					#ifdef USE_RT
								if (_HasRT == 1)
								{
									specGloss.r = specGloss.r * saturate(normal) + saturate(ripples3 * 30) * lerp(0, 1, saturate(saturate(1 - ripples3 * 5) * calculatedNormal.x * reflect(-lightDirection, normalDirection)).x * _NormalRTStrength * saturate(shadowmapColor * 2));
									specGloss.r = specGloss.r * saturate(normal) + saturate(ripples2 * 30) * lerp(0, 0.1, saturate(saturate(1 - ripples2 * 5) * calculatedNormal.y * reflect(lightDirection, -normalDirection)).x * _NormalRTStrength * saturate(shadowmapColor * 2));
								}
								else
								{
									specGloss.r = specGloss.r * saturate(normal);
								}
					#else
								specGloss.r = specGloss.r * saturate(normal);
					#endif
					#endif
								_LittleSpecForce = max(0, _LittleSpecForce);

					#ifdef	IS_ICE
								specularReflection = parallax + specularReflection;
					#else
								specularReflection = lerp(parallax + specularReflection, specularReflection * (specGloss.r + lerp(littleSpec.g * _LittleSpecForce * 0.2 , littleSpec.g * _LittleSpecForce, specularReflection)), saturate(iceValue * 3)); // multiply the *3 for a better snow ice transition
					#endif
								float smallSpec = pow(max(0.0, dot(reflect(-lightDirection , normalDirection + parallax * 0.01), viewDirection)), 800);
								smallSpec = saturate(smallSpec * saturate(UnpackScaleNormal(_NormalTex.Sample(my_linear_repeat_sampler, iceUV * _IceScale), 20) + 0.8));

					#ifdef	IS_ICE
								specularReflection = diffuseReflection * 0.1 + specularReflection * rougnessTex + smallSpec;
					#else
					#ifdef USE_RT
								if (_HasRT == 1)
								{
									specularReflection = specularReflection - lerp(0, saturate(0.075), saturate(saturate(_LightColor0.a * lightIntensity + _LightColor0.a * 0.35) * saturate(1 - ripples3 * 4) * calculatedNormal.x * reflect(lightDirection, normalDirection)).x * _NormalRTStrength);
									specularReflection = specularReflection + lerp(0, saturate(0.125), saturate(saturate(_LightColor0.a * lightIntensity + _LightColor0.a * 0.35) * saturate(1 - ripples3 * 8) * calculatedNormal.x * reflect(-lightDirection, normalDirection)).x * _NormalRTStrength);
									specularReflection = specularReflection - lerp(0, saturate(0.1), saturate(saturate(1 - ripples2 * 1) * calculatedNormal.y * reflect(-lightDirection, normalDirection)).x * _NormalRTStrength);
									specularReflection = specularReflection + lerp(0, saturate(0.1), saturate(saturate(1 - ripples2 * 6) * calculatedNormal.y * reflect(lightDirection, normalDirection)).x * _NormalRTStrength * 0.5);
								}
					#endif

					#ifdef USE_LS
								specularReflection = lerp(specularReflection, diffuseReflection * 0.1 + specularReflection * rougnessTex + smallSpec * lightColor.a * ((lightColor.r + lightColor.b + lightColor.g) / 3), saturate(pow(1 - iceValue,2) * 3));
					#else
								specularReflection = lerp(specularReflection, diffuseReflection * 0.1 + specularReflection * rougnessTex, saturate(pow(1 - iceValue, 2) * 3));
					#endif
					#endif

					#ifdef USE_AL

								half3 ambientColor = ShadeSH9(half4(lerp(normalDirection, normalDirection + normalEffect * 2.5, saturate(ripples3)), 1));
#if !UNITY_COLORSPACE_GAMMA
								ambientColor = pow(ambientColor, gamma) * 1.0;
#endif
								albedo.rgb = saturate(albedo.rgb + (ambientColor - 0.5) * 0.75);
					#endif
								half fresnelRefl = lerp(1, 0, saturate(dot(normalDirection, viewDirection) * 2.65 * _ReflectionColor.a));

					#ifdef	IS_ICE
								albedo.rgb = lerp(albedo.rgb, albedo.rgb + reflection * _ReflectionStrength + _ReflectionColor.rgb, saturate(fresnelRefl));
					#else
#ifdef USE_LOW
#else
								albedo.rgb = lerp(albedo.rgb, albedo.rgb + reflection * _ReflectionStrength + _ReflectionColor.rgb, saturate((1 - iceValue) * (fresnelRefl + normalTex * fresnelRefl)));
#endif
								albedo.rgb = lerp(albedo.rgb, albedo.rgb + _RimColor, saturate(iceValue * (fresnelRefl + normal * fresnelRefl * 0.2)));
					#endif

#ifdef USE_LOW
#else
								albedo.rgb = lerp((albedo.rgb + reflection * 0.15 + 0.2) , albedo.rgb, saturate(1 - fresnelRefl * 0.25));
#endif
								albedo += float4(specularReflection.r, specularReflection.r, specularReflection.r, 1.0) * _SpecColor.rgb;


					#ifdef USE_FOG
								// ADD CUSTOM FOG //
								albedo.rgb += fogFlow * _FogIntensity;
					#endif


								half transparency = 1;
					#ifdef	IS_ADD
								transparency = saturate(lerp(-0.5,2, saturate(pow(iceValue,1))));
								if (iceValue < 0.30)
								{
									discard;
								}
					#else
								transparency = saturate(lerp(_TransparencyValue, 1, saturate(pow(iceValue, 2))));
					#endif

						albedo = max(0, albedo);
						UNITY_APPLY_FOG(i.fogCoord, albedo);

						return float4(albedo, transparency);
					}

								ENDCG
					}


					// SHADOW CASTER PASS
					Pass{
					Tags {
						"LightMode" = "ShadowCaster"
					}

					CGPROGRAM

					#pragma target 3.0


							#pragma multi_compile _ LOD_FADE_CROSSFADE

							#pragma multi_compile_fwdbase
							#pragma multi_compile_fog

							#pragma vertex vert
							#pragma fragment frag

							#define FORWARD_BASE_PASS
							#pragma shader_feature USE_AL
							#pragma shader_feature USE_RT
							#pragma shader_feature IS_ADD
							#pragma shader_feature USE_INTER
							#pragma shader_feature USE_WC

							#include "UnityPBSLighting.cginc"
							#include "AutoLight.cginc"

					sampler2D  _DetailTex, _DetailMask;
					float4 _MainTex_ST, _DetailTex_ST;

					// Render Texture Effects //
					uniform sampler2D _GlobalEffectRT;
					uniform float3 _Position;
					uniform float _OrthographicCamSize;
					uniform sampler2D _GlobalEffectRTAdditional;
					uniform float3 _PositionAdd;
					uniform float _OrthographicCamSizeAdditional;

					sampler2D _MainTex;
					float _HasRT;

					float _UpVector, _NormalVector;
					float _AddSnowStrength, _RemoveSnowStrength, _DisplacementStrength;

					//ICE Variables
					sampler2D _SnowHeight;
					Texture2D _SnowTransition;
					float _TransitionScale;
					float _TransitionPower;
					float _HeightScale, _SnowScale;

					half _OverallScale;

					half _DisplacementOffset;

					struct VertexData //appdata
					{
						float4 vertex : POSITION;
						float3 normal : NORMAL;
						float4 tangent : TANGENT;
						float2 uv : TEXCOORD0;
						float4 color : COLOR;

					#ifdef SHADOWS_SCREEN
						SHADOW_COORDS(1)
					#endif
#ifdef USE_VR
							UNITY_VERTEX_INPUT_INSTANCE_ID
#endif

#ifdef IS_ADD
#ifdef USE_INTER
							float2 uv3 : TEXCOORD3;
						float2 uv4 : TEXCOORD4;
						float2 uv6 : TEXCOORD6;
						float2 uv7 : TEXCOORD7;
#endif
#endif
					};

					struct InterpolatorsVertex
					{
						float4 pos : SV_POSITION;
						float3 normal : TEXCOORD1;
						float4 uv : TEXCOORD0;
						float4 color : COLOR;
						float3 worldPos : TEXCOORD2;
						float3 normalDir: TEXCOORD3;

					#ifdef SHADOWS_SCREEN
						SHADOW_COORDS(4)
					#endif
#ifdef USE_VR
							UNITY_VERTEX_OUTPUT_STEREO
#endif
					};



					InterpolatorsVertex vert(VertexData v) {
						InterpolatorsVertex i;

#ifdef USE_VR
						UNITY_SETUP_INSTANCE_ID(v);
						UNITY_INITIALIZE_OUTPUT(InterpolatorsVertex, i);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(i);
#endif

#ifdef IS_T
						_HeightScale = _HeightScale * 0.066; // correct the heightScale to what it was before update 1.3
#endif
						float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
						float3 originalPos = worldPos;

						//RT Cam effects
						float2 uv = worldPos.xz - _Position.xz;
						uv = uv / (_OrthographicCamSize * 2);
						uv += 0.5;

						float2 uvAdd = worldPos.xz - _PositionAdd.xz;
						uvAdd = uvAdd / (_OrthographicCamSizeAdditional * 2);
						uvAdd += 0.5;

						float3 rippleMain = 0;
						float3 rippleMainAdditional = 0;

						float ripples = 0;
						float ripples2 = 0;
						float ripples3 = 0;

						float uvRTValue = 0;
								_OverallScale = _OverallScale * 1;


					#ifdef USE_RT
						if (_HasRT == 1)
						{
							// .b(lue) = Snow Dig / .r(ed) = Snow To Ice / .g(reen) = Snow Mount
							rippleMain = tex2Dlod(_GlobalEffectRT, float4(uv, 0, 0));
							rippleMainAdditional = tex2Dlod(_GlobalEffectRTAdditional, float4(uvAdd, 0, 0));
						}

					#ifdef IS_ICE
					#else
						float2 uvGradient = smoothstep(0, 5, length(max(abs(_Position.xz - worldPos.xz) - _OrthographicCamSize + 5, 0.0)));
						uvRTValue = saturate(uvGradient.x);

						ripples = lerp(rippleMain.x, rippleMainAdditional.x, uvRTValue);
						ripples2 = lerp(rippleMain.y, rippleMainAdditional.y, uvRTValue);
						ripples3 = lerp(rippleMain.z, rippleMainAdditional.z, uvRTValue);
					#endif

					#endif
						float slopeValue = 0;
					#ifdef IS_T
						half4 splat_control = tex2Dlod(_Control0, float4(v.uv, 0, 0));
						#ifdef USE_COMPLEX_T
						half4 splat_control1 = tex2Dlod(_Control1, float4(v.uv, 0, 0));
						#endif

#ifdef USE_COMPLEX_T
						float iceValue = saturate(1 - splat_control.r * _Metallic0 - splat_control.g * _Metallic1 - splat_control.b * _Metallic2 - splat_control.a * _Metallic3
							- splat_control1.r * _Metallic4 - splat_control1.g * _Metallic5 - splat_control1.b * _Metallic6 - splat_control1.a * _Metallic7
							- ripples);
#else
						float iceValue = saturate(1 - splat_control.r * _Metallic0 - splat_control.g * _Metallic1 - splat_control.b * _Metallic2 - splat_control.a * _Metallic3
							- ripples);
#endif

						float snowHeightNew = _Mask0.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r;
						snowHeightNew = lerp(snowHeightNew, _Mask1.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control.g * (1 - _Metallic1)));
						snowHeightNew = lerp(snowHeightNew, _Mask2.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control.b * (1 - _Metallic2)));
						snowHeightNew = lerp(snowHeightNew, _Mask3.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control.a * (1 - _Metallic3)));
#ifdef USE_COMPLEX_T
						snowHeightNew = lerp(snowHeightNew, _Mask4.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control1.r * (1 - _Metallic4)));
						snowHeightNew = lerp(snowHeightNew, _Mask5.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control1.g * (1 - _Metallic5)));
						snowHeightNew = lerp(snowHeightNew, _Mask6.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control1.b * (1 - _Metallic6)));
						snowHeightNew = lerp(snowHeightNew, _Mask7.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control1.a * (1 - _Metallic7)));
#endif
						float snowHeight = snowHeightNew;
					#else
						float iceValue = saturate((v.color.g + v.color.b) / 2 - ripples);

#ifdef USE_INTER
#ifdef IS_ADD			// custom intersection and slope value //
						float4 midPoint = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0));

						float4 quaternion = float4(v.uv6.x, -v.uv6.y, -v.uv7.x, -v.uv7.y);
						float3 offsetPoint = worldPos.xyz - midPoint;

						float3 rotatedVert = rotateVector(quaternion, -offsetPoint);
						float manualLerp = 0;

						manualLerp = v.uv4.x;

						rotatedVert = RotateAroundZInDegrees(float4(rotatedVert, 0), lerp(6, -6, (manualLerp)));
						rotatedVert = RotateAroundXInDegrees(float4(rotatedVert, 0), lerp(-55, 55, (manualLerp))) + midPoint;

						slopeValue = ((v.color.a) - (rotatedVert.y - 0.5));

						if (slopeValue > 0.0)
						{
							v.color.g = saturate(v.color.g + saturate(slopeValue * 3));
							v.color.b = saturate(v.color.b + saturate(slopeValue * 3));
						}
#endif
#endif


						if (v.color.b > 0.6 && v.color.g < 0.4)
						{
							iceValue = saturate(1 - v.color.b);
						}
						else
						{
							iceValue = saturate((v.color.g + v.color.b) / 2 - ripples);
						}


					#ifdef USE_WC
						float snowHeight = tex2Dlod(_SnowHeight, float4(originalPos.xz, 0, 0) * _HeightScale * 0.1 * _SnowScale * _OverallScale).r;
					#else
						float2 snowUV = float2(v.uv.x * _MainTex_ST.x + _MainTex_ST.z, v.uv.y * _MainTex_ST.y + _MainTex_ST.w);
						float snowHeight = tex2Dlod(_SnowHeight, float4(snowUV, 0, 0) * _HeightScale * _SnowScale * _OverallScale).r;
					#endif
					#endif

						i.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject));
					#ifdef IS_ICE
					#else


						v.color = lerp(v.color, saturate(float4(1, 0, 0, 0)), ripples);
						i.normal = normalize(v.normal);

					#ifdef IS_ADD
						float3 newNormal = normalize(i.normalDir);
						worldPos += ((float4(0, -_RemoveSnowStrength, 0, 0) * _UpVector - newNormal * _RemoveSnowStrength * _NormalVector) * ripples3 + (float4(0, _AddSnowStrength * snowHeight, 0, 0) * _UpVector + newNormal * _AddSnowStrength * snowHeight * _NormalVector) * ripples2 * saturate(1 - ripples3)) * saturate(iceValue * 3);
						worldPos += (float4(0, _DisplacementOffset, 0, 0) * _UpVector + newNormal * _DisplacementOffset * _NormalVector) * saturate(iceValue * 2.5);
						worldPos += (float4(0, 2 * _DisplacementStrength * snowHeight, 0, 0) * _UpVector) + (newNormal * 2 * _DisplacementStrength * snowHeight * _NormalVector * clamp(slopeValue * 20, 1, 2)) * saturate(saturate(iceValue * 2.5));

						worldPos = lerp(worldPos, mul(unity_ObjectToWorld, v.vertex), saturate(v.color.g - saturate(v.color.r + v.color.b)));

						v.vertex.xyz = lerp(mul(unity_WorldToObject, float4(originalPos, 1)).xyz, mul(unity_WorldToObject, float4(worldPos, 1)).xyz, iceValue);
				#else
						float3 newNormal = normalize(i.normalDir);
						worldPos += ((float4(0, -_RemoveSnowStrength, 0, 0) * _UpVector - newNormal * _RemoveSnowStrength * _NormalVector) * ripples3 + (float4(0, _AddSnowStrength * snowHeight, 0, 0) * _UpVector + newNormal * _AddSnowStrength * snowHeight * _NormalVector) * ripples2 * saturate(1 - ripples3)) * saturate(iceValue * 3);
						worldPos += (float4(0, _DisplacementOffset, 0, 0) * _UpVector + newNormal * _DisplacementOffset * _NormalVector) * saturate(iceValue * 2.5);
						worldPos += (float4(0, 2 * _DisplacementStrength * snowHeight, 0, 0) * _UpVector) + (newNormal * 2 * _DisplacementStrength * snowHeight * _NormalVector) * saturate(saturate(iceValue * 2.5));

						worldPos = lerp(worldPos, mul(unity_ObjectToWorld, v.vertex), saturate(v.color.g - saturate(v.color.r + v.color.b)));

						v.vertex.xyz = lerp(mul(unity_WorldToObject, float4(originalPos, 1)).xyz, mul(unity_WorldToObject, float4(worldPos, 1)).xyz, iceValue);

					#endif
					#endif

						i.pos = UnityObjectToClipPos(v.vertex);

						i.worldPos.xyz = mul(unity_ObjectToWorld, v.vertex);
						i.color = v.color;

					#ifdef IS_T
						i.uv.xy = v.uv;
					#else
						i.uv.xy = v.uv * _OverallScale;
					#endif
						i.uv.zw = TRANSFORM_TEX(v.uv, _DetailTex);


						TRANSFER_SHADOW_CASTER_NORMALOFFSET(i)

						return i;
					}

								float4 frag(InterpolatorsVertex i) : SV_Target
								{
#ifdef USE_VR
								UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i)
#endif
#ifdef IS_T
						_HeightScale = _HeightScale * 0.066; // correct the heightScale to what it was before update 1.3
#endif
					#ifdef	IS_ADD

						float3 worldPos = mul(unity_ObjectToWorld, i.pos);
						float3 originalPos = worldPos;

						float2 uv = worldPos.xz - _Position.xz;
						uv = uv / (_OrthographicCamSize * 2);
						uv += 0.5;

						float2 uvAdd = worldPos.xz - _PositionAdd.xz;
						uvAdd = uvAdd / (_OrthographicCamSizeAdditional * 2);
						uvAdd += 0.5;

						float3 rippleMain = 0;
						float3 rippleMainAdditional = 0;
						rippleMain = tex2Dlod(_GlobalEffectRT, float4(uv, 0, 0));
						rippleMainAdditional = tex2Dlod(_GlobalEffectRTAdditional, float4(uvAdd, 0, 0));

								_OverallScale = _OverallScale * 1;

							#ifdef USE_WT
								float2 snowUV = float2(i.worldPos.x + _MainTex_ST.z, i.worldPos.z + _MainTex_ST.w) * _HeightScale * 0.1 * _OverallScale;
							#else
								float2 snowUV = float2(i.uv.x * _MainTex_ST.x + _MainTex_ST.z, i.uv.y * _MainTex_ST.y + _MainTex_ST.w);
							#endif

						float snowHeight = saturate(_SnowTransition.Sample(my_linear_repeat_sampler, snowUV * _TransitionScale * _SnowScale).r);

						float iceValue = saturate(pow((i.color.g + i.color.b) / 2, 0.35 + clamp((snowHeight - 0.5) * -_TransitionPower * (saturate(i.color.g + i.color.b)), -0.34, 1)));
						if (iceValue < 0.30)
						{
							discard;
						}
					#endif
								SHADOW_CASTER_FRAGMENT(i)
								}



								ENDCG
							}

						// ADDITIONAL LIGHT PASS
						Pass{
						Tags {
							"LightMode" = "ForwardAdd"
						}
							Blend One One // Additive

						CGPROGRAM

						#pragma target 3.0

								#pragma multi_compile _ LOD_FADE_CROSSFADE

									//#pragma multi_compile_fwdbase
									#pragma multi_compile_fog

									#pragma vertex vert
									#pragma fragment frag

									#define FORWARD_BASE_PASS
									#pragma shader_feature USE_AL
									#pragma shader_feature USE_RT
									#pragma shader_feature IS_ADD
									#pragma shader_feature USE_INTER
									#pragma shader_feature USE_WC
									#pragma shader_feature USE_WT

								#pragma multi_compile_fwdadd_fullshadows
								#include "Lighting.cginc"
								#include "AutoLight.cginc"

								//uniform float4x4 unity_WorldToLight;
								//uniform sampler2D _LightTexture0;
								uniform float _LightIntensity;
								//uniform float4 _LightColor0;

								sampler2D  _DetailTex, _DetailMask;
								float4 _MainTex_ST, _DetailTex_ST;

								// Render Texture Effects //
								uniform sampler2D _GlobalEffectRT;
								uniform float3 _Position;
								uniform float _OrthographicCamSize;
								uniform sampler2D _GlobalEffectRTAdditional;
								uniform float3 _PositionAdd;
								uniform float _OrthographicCamSizeAdditional;

								float _HasRT;
								sampler2D _MainTex;

								float _UpVector, _NormalVector;
								float _AddSnowStrength, _RemoveSnowStrength, _DisplacementStrength;

								//ICE Variables
								sampler2D _SnowHeight;
								sampler2D _SnowTransition;
								float _TransitionScale;
								float _TransitionPower;
								float _HeightScale, _SnowScale;

								half _OverallScale;

								half _DisplacementOffset;

							struct VertexData //appdata
							{
								float4 vertex : POSITION;
								float3 normal : NORMAL;
								float4 tangent : TANGENT;
								float2 uv : TEXCOORD0;
								float4 color : COLOR;
								UNITY_FOG_COORDS(1)
								float4 posLight : TEXCOORD2;
#ifdef USE_VR
								UNITY_VERTEX_INPUT_INSTANCE_ID
#endif

#ifdef IS_ADD
#ifdef USE_INTER
									float2 uv3 : TEXCOORD3;
								float2 uv4 : TEXCOORD4;
								float2 uv6 : TEXCOORD6;
								float2 uv7 : TEXCOORD7;
#endif
#endif
							};

							struct InterpolatorsVertex
							{
								float4 pos : SV_POSITION;
								float3 normal : TEXCOORD1;
								float4 tangent : TANGENT;
								float4 uv : TEXCOORD0;
								float4 color : COLOR;
								float3 worldPos : TEXCOORD2;
								float3 viewDir: POSITION1;
								float3 normalDir: TEXCOORD3;
								UNITY_FOG_COORDS(4)
								float4 posLight : TEXCOORD5;
#ifdef USE_VR
								UNITY_VERTEX_OUTPUT_STEREO
#endif
							};

							InterpolatorsVertex vert(VertexData v) {
								InterpolatorsVertex i;

#ifdef USE_VR
								UNITY_SETUP_INSTANCE_ID(v);
								UNITY_INITIALIZE_OUTPUT(InterpolatorsVertex, i);
								UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(i);
#endif

#ifdef IS_T
								_HeightScale = _HeightScale * 0.066; // correct the heightScale to what it was before update 1.3
#endif
								float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
								float3 originalPos = worldPos;

								//RT Cam effects
								float2 uv = worldPos.xz - _Position.xz;
								uv = uv / (_OrthographicCamSize * 2);
								uv += 0.5;

								float2 uvAdd = worldPos.xz - _PositionAdd.xz;
								uvAdd = uvAdd / (_OrthographicCamSizeAdditional * 2);
								uvAdd += 0.5;

								float3 rippleMain = 0;
								float3 rippleMainAdditional = 0;

								float ripples = 0;
								float ripples2 = 0;
								float ripples3 = 0;

								_OverallScale = _OverallScale * 1;

								float uvRTValue = 0;

								//float viewDistance = distance(worldPos, _WorldSpaceCameraPos);
					#ifdef USE_RT
								if (_HasRT == 1)
								{
									// .b(lue) = Snow Dig / .r(ed) = Snow To Ice / .g(reen) = Snow Mount
									rippleMain = tex2Dlod(_GlobalEffectRT, float4(uv, 0, 0));
									rippleMainAdditional = tex2Dlod(_GlobalEffectRTAdditional, float4(uvAdd, 0, 0));
								}

					#ifdef IS_ICE
					#else
								float2 uvGradient = smoothstep(0, 5, length(max(abs(_Position.xz - worldPos.xz) - _OrthographicCamSize + 5, 0.0)));
								uvRTValue = saturate(uvGradient.x);

								ripples = lerp(rippleMain.x, rippleMainAdditional.x, uvRTValue);
								ripples2 = lerp(rippleMain.y, rippleMainAdditional.y, uvRTValue);
								ripples3 = lerp(rippleMain.z, rippleMainAdditional.z, uvRTValue);
					#endif

					#endif
								float slopeValue = 0;
					#ifdef IS_T
								half4 splat_control = tex2Dlod(_Control0, float4(v.uv, 0, 0));
						#ifdef USE_COMPLEX_T
								half4 splat_control1 = tex2Dlod(_Control1, float4(v.uv, 0, 0));
						#endif

#ifdef USE_COMPLEX_T
						float iceValue = saturate(1 - splat_control.r * _Metallic0 - splat_control.g * _Metallic1 - splat_control.b * _Metallic2 - splat_control.a * _Metallic3
							- splat_control1.r * _Metallic4 - splat_control1.g * _Metallic5 - splat_control1.b * _Metallic6 - splat_control1.a * _Metallic7
							- ripples);
#else
						float iceValue = saturate(1 - splat_control.r * _Metallic0 - splat_control.g * _Metallic1 - splat_control.b * _Metallic2 - splat_control.a * _Metallic3
							- ripples);
#endif

								float snowHeightNew = _Mask0.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r;
								snowHeightNew = lerp(snowHeightNew, _Mask1.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control.g * (1 - _Metallic1)));
								snowHeightNew = lerp(snowHeightNew, _Mask2.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control.b * (1 - _Metallic2)));
								snowHeightNew = lerp(snowHeightNew, _Mask3.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control.a * (1 - _Metallic3)));
#ifdef USE_COMPLEX_T
								snowHeightNew = lerp(snowHeightNew, _Mask4.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control1.r * (1 - _Metallic4)));
								snowHeightNew = lerp(snowHeightNew, _Mask5.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control1.g * (1 - _Metallic5)));
								snowHeightNew = lerp(snowHeightNew, _Mask6.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control1.b * (1 - _Metallic6)));
								snowHeightNew = lerp(snowHeightNew, _Mask7.SampleLevel(my_linear_repeat_sampler, float4(originalPos.xz, 0, 0) * _HeightScale * _OverallScale * _SnowScale * 0.1,0).r, saturate(splat_control1.a * (1 - _Metallic7)));
#endif
								float snowHeight = snowHeightNew;
					#else
								float iceValue = saturate((v.color.g + v.color.b) / 2 - ripples);
#ifdef USE_INTER
#ifdef IS_ADD			// custom intersection and slope value //
								float4 midPoint = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0));

								float4 quaternion = float4(v.uv6.x, -v.uv6.y, -v.uv7.x, -v.uv7.y);
								float3 offsetPoint = worldPos.xyz - midPoint;

								float3 rotatedVert = rotateVector(quaternion, -offsetPoint);
								float manualLerp = 0;

								manualLerp = v.uv4.x;

								rotatedVert = RotateAroundZInDegrees(float4(rotatedVert, 0), lerp(6, -6, (manualLerp)));
								rotatedVert = RotateAroundXInDegrees(float4(rotatedVert, 0), lerp(-55, 55, (manualLerp))) + midPoint;

								slopeValue = ((v.color.a) - (rotatedVert.y - 0.5));

								if (slopeValue > 0.0)
								{
									v.color.g = saturate(v.color.g + saturate(slopeValue * 3));
									v.color.b = saturate(v.color.b + saturate(slopeValue * 3));
								}
#endif
#endif

								if (v.color.b > 0.6 && v.color.g < 0.4)
								{
									iceValue = saturate(1 - v.color.b);
								}
								else
								{
									iceValue = saturate((v.color.g + v.color.b) / 2 - ripples);
								}


					#ifdef USE_WC
						float snowHeight = tex2Dlod(_SnowHeight, float4(originalPos.xz, 0, 0) * _HeightScale * 0.1 * _SnowScale * _OverallScale).r;
					#else
						float2 snowUV = float2(v.uv.x * _MainTex_ST.x + _MainTex_ST.z, v.uv.y * _MainTex_ST.y + _MainTex_ST.w);
						float snowHeight = tex2Dlod(_SnowHeight, float4(snowUV, 0, 0) * _HeightScale * _SnowScale * _OverallScale).r;
					#endif

					#endif

								i.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject));
					#ifdef IS_ICE
					#else

								v.color = lerp(v.color, saturate(float4(1, 0, 0, 0)), ripples);
								i.normal = normalize(v.normal);

					#ifdef IS_ADD
								float3 newNormal = normalize(i.normalDir);
								worldPos += ((float4(0, -_RemoveSnowStrength, 0, 0) * _UpVector - newNormal * _RemoveSnowStrength * _NormalVector) * ripples3 + (float4(0, _AddSnowStrength * snowHeight, 0, 0) * _UpVector + newNormal * _AddSnowStrength * snowHeight * _NormalVector) * ripples2 * saturate(1 - ripples3)) * saturate(iceValue * 3);
								worldPos += (float4(0, _DisplacementOffset, 0, 0) * _UpVector + newNormal * _DisplacementOffset * _NormalVector) * saturate(iceValue * 2.5);
								worldPos += (float4(0, 2 * _DisplacementStrength * snowHeight, 0, 0) * _UpVector) + (newNormal * 2 * _DisplacementStrength * snowHeight * _NormalVector * clamp(slopeValue * 20, 1, 2)) * saturate(saturate(iceValue * 2.5));

								worldPos = lerp(worldPos, mul(unity_ObjectToWorld, v.vertex), saturate(v.color.g - saturate(v.color.r + v.color.b)));

								v.vertex.xyz = lerp(mul(unity_WorldToObject, float4(originalPos, 1)).xyz, mul(unity_WorldToObject, float4(worldPos, 1)).xyz, iceValue);
					#else
								float3 newNormal = normalize(i.normalDir);
								worldPos += ((float4(0, -_RemoveSnowStrength, 0, 0) * _UpVector - newNormal * _RemoveSnowStrength * _NormalVector) * ripples3 + (float4(0, _AddSnowStrength * snowHeight, 0, 0) * _UpVector + newNormal * _AddSnowStrength * snowHeight * _NormalVector) * ripples2 * saturate(1 - ripples3)) * saturate(iceValue * 3);
								worldPos += (float4(0, _DisplacementOffset, 0, 0) * _UpVector + newNormal * _DisplacementOffset * _NormalVector) * saturate(iceValue * 2.5);
								worldPos += (float4(0, 2 * _DisplacementStrength * snowHeight, 0, 0) * _UpVector) + (newNormal * 2 * _DisplacementStrength * snowHeight * _NormalVector) * saturate(saturate(iceValue * 2.5));

								worldPos = lerp(worldPos, mul(unity_ObjectToWorld, v.vertex), saturate(v.color.g - saturate(v.color.r + v.color.b)));

								v.vertex.xyz = lerp(mul(unity_WorldToObject, float4(originalPos, 1)).xyz, mul(unity_WorldToObject, float4(worldPos, 1)).xyz, iceValue);

					#endif
					#endif

								i.pos = UnityObjectToClipPos(v.vertex);

								float4 objCam = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1.0));
								float3 viewDir = v.vertex.xyz - objCam.xyz;

					#ifdef IS_T
								float4 tangent = float4 (1.0, 0.0, 0.0, -1.0);
								tangent.xyz = tangent.xyz - v.normal * dot(v.normal, tangent.xyz); // Orthogonalize tangent to normal.

								float tangentSign = tangent.w * unity_WorldTransformParams.w;
								float3 bitangent = cross(v.normal.xyz, tangent.xyz) * tangentSign;

								i.viewDir = float3(
									dot(viewDir, tangent.xyz),
									dot(viewDir, bitangent.xyz),
									dot(viewDir, v.normal.xyz)
									);

								i.worldPos.xyz = mul(unity_ObjectToWorld, v.vertex);
								i.tangent = tangent;

					#else
								float tangentSign = v.tangent.w * unity_WorldTransformParams.w;
								float3 bitangent = cross(v.normal.xyz, v.tangent.xyz) * tangentSign;

								i.viewDir = float3(
									dot(viewDir, v.tangent.xyz),
									dot(viewDir, bitangent.xyz),
									dot(viewDir, v.normal.xyz)
									);

								i.worldPos.xyz = mul(unity_ObjectToWorld, v.vertex);
								i.tangent = v.tangent;
					#endif

								i.color = v.color;

					#ifdef IS_T
								i.uv.xy = v.uv;
					#else
								i.uv.xy = v.uv * _OverallScale;
					#endif
								i.uv.zw = TRANSFORM_TEX(v.uv, _DetailTex);

					#if defined(SPOT) || defined(POINT)
								i.posLight = mul(unity_WorldToLight, mul(unity_ObjectToWorld, v.vertex));
					#else
								i.posLight = mul(unity_ObjectToWorld, v.vertex);
					#endif


								UNITY_TRANSFER_FOG(i, i.pos);
								return i;
							}

										float4 frag(InterpolatorsVertex i) : SV_Target
										{
#ifdef USE_VR
								UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i)
#endif
#ifdef IS_T
						_HeightScale = _HeightScale * 0.066; // correct the heightScale to what it was before update 1.3
#endif
								float3 worldPos = i.worldPos;
								float3 originalPos = worldPos;
							#ifdef	IS_ADD

								float2 uv = worldPos.xz - _Position.xz;
								uv = uv / (_OrthographicCamSize * 2);
								uv += 0.5;

								float2 uvAdd = worldPos.xz - _PositionAdd.xz;
								uvAdd = uvAdd / (_OrthographicCamSizeAdditional * 2);
								uvAdd += 0.5;

								float3 rippleMain = 0;
								float3 rippleMainAdditional = 0;

								_OverallScale = _OverallScale * 1;


						float2 snowUV = float2(i.uv.x * _MainTex_ST.x + _MainTex_ST.z, i.uv.y * _MainTex_ST.y + _MainTex_ST.w);

								rippleMain = tex2Dlod(_GlobalEffectRT, float4(uv, 0, 0));
								rippleMainAdditional = tex2Dlod(_GlobalEffectRTAdditional, float4(uvAdd, 0, 0));

								float snowHeight = tex2D(_SnowTransition, (snowUV * _TransitionScale * _SnowScale)).r;
								float iceValue = saturate(pow((i.color.g + i.color.b) / 2, 0.35 + clamp((snowHeight - 0.5) * -_TransitionPower * (saturate(i.color.g + i.color.b)), -0.34, 1)));
								if (iceValue < 0.30)
								{
									discard;
								}
							#endif

									 float3 normalDirection = normalize(i.normalDir);
									 float3 lightDirection;
									 float attenuation = 1.0;
									 float cookieAttenuation = 1.0;
					#if defined(SPOT) || defined(POINT)
									 if (0.0 == _WorldSpaceLightPos0.w) // directional light
									 {
										attenuation = 1.0; // no attenuation
										lightDirection = normalize(_WorldSpaceLightPos0.xyz);
										cookieAttenuation = tex2D(_LightTexture0, i.posLight.xy).a;
									 }
									 else if (1.0 != unity_WorldToLight[3][3]) // spot light
									 {
										 attenuation = 1.0; // no attenuation
										 UNITY_LIGHT_ATTENUATION(atten, i, worldPos.xyz);
										 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
										 cookieAttenuation = tex2D(_LightTexture0,i.posLight.xy / i.posLight.w + float2(0.5, 0.5)).a;
										 attenuation = atten;
									 }
									 else // point light
									 {
										float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - originalPos.xyz;
										lightDirection = normalize(vertexToLightSource);

										half ndotl = saturate(dot(normalDirection, lightDirection));
										UNITY_LIGHT_ATTENUATION(atten, i, worldPos.xyz);
										attenuation = ndotl * atten;
									 }
					#else
									 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
					#endif
									 float3 diffuseReflection = attenuation * _LightColor0.rgb * max(0.0, dot(normalDirection, lightDirection));
									 float3 finalLightColor = cookieAttenuation * diffuseReflection;
									 finalLightColor *= _LightIntensity;

									 UNITY_APPLY_FOG(i.fogCoord, finalLightColor);
									 return float4(saturate(finalLightColor),1);
										}

										ENDCG
							}
				}
}