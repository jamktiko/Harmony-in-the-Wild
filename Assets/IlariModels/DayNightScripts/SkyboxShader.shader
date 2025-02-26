Shader "RenderFX/Skybox Blended With Fog" {
    Properties{
        _FogColor("Fog Color", Color) = (.5, .5, .5, .5)
        _Fog("Fog Intensity", Range(0.0,1.0)) = 1.0
        _Blend("Blend", Range(0.0,1.0)) = 0.5
        _FrontTex("Front (+Z)", 2D) = "white" {}
        _BackTex("Back (-Z)", 2D) = "white" {}
        _LeftTex("Left (+X)", 2D) = "white" {}
        _RightTex("Right (-X)", 2D) = "white" {}
        _UpTex("Up (+Y)", 2D) = "white" {}
        _DownTex("Down (-Y)", 2D) = "white" {}
        _FrontTex2("Front (+Z)", 2D) = "white" {}
        _BackTex2("Back (-Z)", 2D) = "white" {}
        _LeftTex2("Left (+X)", 2D) = "white" {}
        _RightTex2("Right (-X)", 2D) = "white" {}
        _UpTex2("Up (+Y)", 2D) = "white" {}
        _DownTex2("Down (-Y)", 2D) = "white" {}
    }

        SubShader{
            Tags { "Queue" = "Background" "RenderType" = "Background" }
            Cull Off ZWrite Off Fog { Mode Off }

            CGINCLUDE
            #include "UnityCG.cginc"

            fixed4 _FogColor;
            fixed _Blend;
            fixed _Fog;

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
            struct v2f {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }
            fixed4 skybox_frag(v2f i, sampler2D sky1, sampler2D sky2, half fogBase) {
                // calculate fog value: 100% at horizon, 0% at the top face
                half fog = _Fog * saturate(lerp(2, 0, i.texcoord.y));
                // mix skyboxes 1 and 2 according to _Blend
                fixed4 tex = lerp(tex2D(sky1, i.texcoord), tex2D(sky2, i.texcoord), _Blend);
                // apply fog: fogBase forces 100% if >=1, 0% if <= -1
                tex = lerp(tex, _FogColor, saturate(fog + fogBase));
                return tex;
            }
            ENDCG

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                sampler2D _FrontTex;
                sampler2D _FrontTex2;
                fixed4 frag(v2f i) : COLOR { return skybox_frag(i, _FrontTex, _FrontTex2, 0); }
                ENDCG
            }
            Pass{
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                sampler2D _BackTex;
                sampler2D _BackTex2;
                fixed4 frag(v2f i) : COLOR { return skybox_frag(i, _BackTex, _BackTex2, 0); }
                ENDCG
            }
            Pass{
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                sampler2D _LeftTex;
                sampler2D _LeftTex2;
                fixed4 frag(v2f i) : COLOR { return skybox_frag(i, _LeftTex, _LeftTex2, 0); }
                ENDCG
            }
            Pass{
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                sampler2D _RightTex;
                sampler2D _RightTex2;
                fixed4 frag(v2f i) : COLOR { return skybox_frag(i, _RightTex, _RightTex2, 0); }
                ENDCG
            }
            Pass{
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                sampler2D _UpTex;
                sampler2D _UpTex2;
                fixed4 frag(v2f i) : COLOR { return skybox_frag(i, _UpTex, _UpTex2, -1); } // no fog at top
                ENDCG
            }
            Pass{
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                sampler2D _DownTex;
                sampler2D _DownTex2;
                fixed4 frag(v2f i) : COLOR { return skybox_frag(i, _DownTex, _DownTex2, -1); } // 100% fog at bottom
                ENDCG
            }
    }
}