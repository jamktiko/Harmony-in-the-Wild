Shader "Custom/SkyParticlesUnlit_URP"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        [MainColor]   _BaseColor("Base Color", Color) = (1,1,1,1)
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        _BumpMap("Normal Map", 2D) = "bump" {}
        [HDR] _EmissionColor("Color", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}

        _SoftParticlesNearFadeDistance("Soft Particles Near Fade", Float) = 0.0
        _SoftParticlesFarFadeDistance("Soft Particles Far Fade", Float) = 1.0
        _CameraNearFadeDistance("Camera Near Fade", Float) = 1.0
        _CameraFarFadeDistance("Camera Far Fade", Float) = 2.0
        _DistortionBlend("Distortion Blend", Range(0.0, 1.0)) = 0.5
        _DistortionStrength("Distortion Strength", Float) = 1.0

        // Hidden / Advanced
        _Surface("__surface", Float) = 0.0
        _Blend("__mode", Float) = 0.0
        _Cull("__cull", Float) = 2.0
        [ToggleUI] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _BlendOp("__blendop", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _SrcBlendAlpha("__srcA", Float) = 1.0
        [HideInInspector] _DstBlendAlpha("__dstA", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        [HideInInspector] _AlphaToMask("__alphaToMask", Float) = 0.0

        _ColorMode("_ColorMode", Float) = 0.0
        [HideInInspector] _BaseColorAddSubDiff("_ColorMode", Vector) = (0,0,0,0)
        [ToggleOff] _FlipbookBlending("__flipbookblending", Float) = 0.0
        [ToggleUI] _SoftParticlesEnabled("__softparticlesenabled", Float) = 0.0
        [ToggleUI] _CameraFadingEnabled("__camerafadingenabled", Float) = 0.0
        [ToggleUI] _DistortionEnabled("__distortionenabled", Float) = 0.0
        [HideInInspector] _SoftParticleFadeParams("__softparticlefadeparams", Vector) = (0,0,0,0)
        [HideInInspector] _CameraFadeParams("__camerafadeparams", Vector) = (0,0,0,0)
        [HideInInspector] _DistortionStrengthScaled("Distortion Strength Scaled", Float) = 0.1

        _QueueOffset("Queue offset", Float) = 0.0

        [HideInInspector] _FlipbookMode("flipbook", Float) = 0
        [HideInInspector] _Mode("mode", Float) = 0
        [HideInInspector] _Color("color", Color) = (1,1,1,1)
    }

    HLSLINCLUDE

    // Comment this out if compiler indicates unknown pragma:
    #pragma never_use_dxc

    // --- Includes from URP.
    #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesUnlitInput.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesUnlitForwardPass.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesDepthOnlyPass.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesDepthNormalsPass.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesEditorPass.hlsl"

    //---------------------------------------------------------------------
    // OVERRIDE VERTEX FUNCTIONS: push clipPos.z -> near 0 for reversed-Z, near 1 for standard-Z.
    //---------------------------------------------------------------------
    
    // ------------------------------------------------------------------
    //  Forward pass.
    VaryingsParticle Star_vertParticleUnlit(AttributesParticle input)
    {
        VaryingsParticle o = vertParticleUnlit(input);
        // Reversed-Z: push clipPos.z to near 0.
        o.clipPos.z = o.clipPos.w * 0.0001;
        return o;
    }

    // ------------------------------------------------------------------
    //  Depth Only pass.
    VaryingsDepthOnlyParticle Star_DepthOnlyVertex(AttributesDepthOnlyParticle input)
    {
        VaryingsDepthOnlyParticle o = DepthOnlyVertex(input);
        o.clipPos.z = o.clipPos.w * 0.0001;
        return o;
    }

    // ------------------------------------------------------------------
    //  Depth Normals pass
    VaryingsDepthNormalsParticle Star_DepthNormalsVertex(AttributesDepthNormalsParticle input)
    {
        VaryingsDepthNormalsParticle o = DepthNormalsVertex(input);
        o.clipPos.z = o.clipPos.w * 0.0001;
        return o;
    }

    // -------------------------------------
    // Scene Selection pass
    VaryingsParticle Star_vertParticleEditor(AttributesParticle input)
    {
        VaryingsParticle o = vertParticleEditor(input);
        o.clipPos.z = o.clipPos.w * 0.0001;
        return o;
    }

    // -------------------------------------
    // Scene Picking pass
    VaryingsParticle Star_vertParticleEditorPicking(AttributesParticle input)
    {
        VaryingsParticle o = vertParticleEditor(input);
        o.clipPos.z = o.clipPos.w * 0.0001;
        return o;
    }

    ENDHLSL

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "PerformanceChecks"="False"
        }

        //------------------------------------------------------------------
        // (1) FORWARD LIT PASS
        Pass
        {
            Name "ForwardLit"
            BlendOp [_BlendOp]
            Blend [_SrcBlend][_DstBlend] , [_SrcBlendAlpha][_DstBlendAlpha]
            ZWrite [_ZWrite]
            Cull [_Cull]
            AlphaToMask [_AlphaToMask]

            HLSLPROGRAM
            // If URP warns "unknown pragma," comment these out:
            #pragma target 2.0
            #pragma vertex Star_vertParticleUnlit
            #pragma fragment fragParticleUnlit

            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local _FLIPBOOKBLENDING_ON
            #pragma shader_feature_local _SOFTPARTICLES_ON
            #pragma shader_feature_local _FADING_ON
            #pragma shader_feature_local _DISTORTION_ON
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
            #pragma shader_feature_local_fragment _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
            #pragma multi_compile_instancing
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile_fragment _ DEBUG_DISPLAY
            #pragma instancing_options procedural:ParticleInstancingSetup

            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesUnlitForwardPass.hlsl"
            ENDHLSL
        }

        //------------------------------------------------------------------
        // (2) DEPTH ONLY PASS
        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode"="DepthOnly" }
            ZWrite On
            ColorMask R
            Cull [_Cull]

            HLSLPROGRAM
            #pragma vertex Star_DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            #pragma multi_compile_instancing
            #pragma instancing_options procedural:ParticleInstancingSetup

            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesDepthOnlyPass.hlsl"
            ENDHLSL
        }

        //------------------------------------------------------------------
        // (3) DEPTH NORMALS PASS
        Pass
        {
            Name "DepthNormalsOnly"
            Tags { "LightMode"="DepthNormalsOnly" }
            ZWrite On
            Cull [_Cull]

            HLSLPROGRAM
            // #pragma target 4.5, comment out if triggers unknown pragma
            #pragma vertex Star_DepthNormalsVertex
            #pragma fragment DepthNormalsFragment

            #pragma multi_compile_instancing
            #pragma instancing_options procedural:ParticleInstancingSetup

            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesDepthNormalsPass.hlsl"
            ENDHLSL
        }

        //------------------------------------------------------------------
        // (4) SCENE SELECTION PASS
        Pass
        {
            Name "SceneSelectionPass"
            Tags { "LightMode"="SceneSelectionPass" }
            BlendOp Add
            Blend One Zero
            ZWrite On
            Cull Off

            HLSLPROGRAM
            #define PARTICLES_EDITOR_META_PASS
            #pragma vertex Star_vertParticleEditor
            #pragma fragment fragParticleSceneHighlight

            #pragma multi_compile_instancing
            #pragma instancing_options procedural:ParticleInstancingSetup

            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesEditorPass.hlsl"
            ENDHLSL
        }

        //------------------------------------------------------------------
        // (5) SCENE PICKING PASS
        Pass
        {
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }
            BlendOp Add
            Blend One Zero
            ZWrite On
            Cull Off

            HLSLPROGRAM
            #define PARTICLES_EDITOR_META_PASS
            #pragma vertex Star_vertParticleEditorPicking
            #pragma fragment fragParticleScenePicking

            #pragma multi_compile_instancing
            #pragma instancing_options procedural:ParticleInstancingSetup

            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesEditorPass.hlsl"
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
    CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.ParticlesUnlitShader"
}
