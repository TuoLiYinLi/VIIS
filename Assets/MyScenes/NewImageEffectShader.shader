Shader "Custom/Rainbow" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Float) = 1
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags {"Queue" = "Transparent" "RenderType"="Opaque"}
        
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
                float4 spacePos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _Speed;
            float4 _Color;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.spacePos = ComputeScreenPos(o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target {
                float4 color = tex2D(_MainTex, i.uv);
                color.r = sin((_Time.y + i.spacePos.x) * _Speed);
                color.g = sin((_Time.y + i.spacePos.y) * _Speed + 1.57);
                color.b = sin((_Time.y + i.spacePos.z) * _Speed + 3.14);
                return color * _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}