Shader "Unlit/Minimap"
{
    Properties
    {
        _MainTex("Minimap", 2D) = "white" {}
        _Zoom("Zoom", Float) = 1
        _Radius("Radius", Float) = 1
    }
    SubShader
    {
        Tags {"Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
 
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
            half _Zoom;
            half _Radius;
            half3 _PlayerPos;
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
 
            //return 0 to 1 value based on the uv coordinate to the center of the uv
            half circle(half2 _uv, half _r)
            {
                half2 dist = _uv - half2(.5, .5);
                return 1.0 - smoothstep(_r - (_r*0.01), _r + (_r*0.01), dot(dist, dist)*4.0);
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //offset the texture to the player position and add the offset made by the _Zoom relative to the center of the uv
                fixed4 col = tex2D(_MainTex, (_Zoom * i.uv) + half2(_PlayerPos.z + (.5-(_Zoom / 2)), _PlayerPos.x*-1 + (.5-(_Zoom / 2))));
             
//lerp over the alpha with the value we get from the circle function, so value 0 to _r * 0.01 will render a gradient fade to all above that will be our texture
                col.a = circle(i.uv, _Radius);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}