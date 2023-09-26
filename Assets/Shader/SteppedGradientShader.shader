Shader "Custom/SteppedGradientShader" {
    Properties {
        _Color1 ("Color1", Color) = (1, 0, 0, 1)
        _Color2 ("Color2", Color) = (0, 1, 0, 1)
        _Steps ("Steps", Range(1, 100)) = 10
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
            float _Steps;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float gradient = smoothstep(0.0, 1.0, i.uv.y);
                gradient = floor(gradient * _Steps) / _Steps;
                fixed4 finalColor = lerp(_Color1, _Color2, gradient);
                return finalColor;
            }
            ENDCG
        }
    }
}
