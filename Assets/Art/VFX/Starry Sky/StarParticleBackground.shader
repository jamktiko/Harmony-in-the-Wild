Shader "Custom/StarParticleBackground"
{
    Properties
    {
        _MainTex ("Particle Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        ZTest LEqual
        ZWrite Off

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

                float4 worldPos = mul(unity_ObjectToWorld, IN.vertex);

                float4 clipPos = TransformWorldToHClip(worldPos.xyz);

                clipPos.z = clipPos.w * 0.0001;

                OUT.position = clipPos;
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                half4 col = tex2D(_MainTex, IN.uv) * _Color;
                return col;
            }
            ENDHLSL
        }
    }
    FallBack Off
}
