Shader "Custom/StarParticleBackground"
{
    Properties
    {
        // A texture with an alpha channel that contains your star shape
        _MainTex ("Particle Texture", 2D) = "white" {}
        
        // A color tint (including alpha, if desired)
        _Color ("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        // We want to draw these after all opaque objects (RenderQueue ~2000)
        // but still behind other transparent objects if necessary. 
        // "Transparent" sets the RenderQueue to 3000 by default.
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        // We allow the depth test so that anything in front occludes the stars,
        // but we do NOT write to the depth buffer.
        ZTest LEqual
        ZWrite Off

        // Standard alpha blending: the alpha of the fragment controls how 
        // much of the background shows through.
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Include URP's core library (for TransformWorldToHClip, etc.)
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                half2  uv       : TEXCOORD0;
            };

            sampler2D _MainTex;
            half4     _Color;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                // Convert from object space to world space
                float4 worldPos = mul(unity_ObjectToWorld, IN.vertex);

                // Convert that to clip space using URP’s helper
                float4 clipPos = TransformWorldToHClip(worldPos.xyz);

                // Force the particle’s Z to the far clip plane
                // so anything in front (terrain, trees) will occlude it.
                clipPos.z = clipPos.w * 0.0001;

                OUT.position = clipPos;
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // Sample the texture and tint by _Color
                half4 col = tex2D(_MainTex, IN.uv) * _Color;
                return col;
            }
            ENDHLSL
        }
    }

    // No fallback needed in URP; you can leave it out or specify a blank fallback.
    FallBack Off
}
