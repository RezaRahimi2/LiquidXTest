Shader "Custom/UnlitTransparentShader" {
    Properties {
        _Color1 ("Color1", Color) = (1, 0, 0, 1)
        _Color2 ("Color2", Color) = (0, 1, 0, 1)
        _Slider ("Slider", Range(.5, 1)) = 0
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color1;
            fixed4 _Color2;
            float _Slider;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col1 = _Color1;
                fixed4 col2 = _Color2;
                float slider = _Slider;
                float gradient = smoothstep(0.0, 1.0, i.uv.y);
                float threshold = step(slider, gradient);
                fixed4 finalColor = lerp(col1, col2, threshold);
                finalColor.a *= step(0.01, finalColor.a);
                return finalColor;
            }
            ENDCG
        }
    }
}
