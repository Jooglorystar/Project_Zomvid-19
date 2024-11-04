Shader "Custom/BlendPanoramicSkybox"
{
    Properties
    {
        _Tex1 ("Panorama Texture 1", 2D) = "white" {}
        _Tex2 ("Panorama Texture 2", 2D) = "white" {}
        _Tex3 ("Panorama Texture 3", 2D) = "white" {}
        _Exposure1 ("Exposure 1", Range(0, 5)) = 1.0
        _Exposure2 ("Exposure 2", Range(0, 5)) = 1.0
        _Exposure3 ("Exposure 3", Range(0, 5)) = 1.0
        _Blend ("Blend Factor", Range(0, 1)) = 0.5
        _Blend2 ("Blend Factor 2", Range(0, 1)) = 0.0
    }
    SubShader
    {
        Tags { "Queue" = "Background" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            sampler2D _Tex1;
            sampler2D _Tex2;
            sampler2D _Tex3;
            float _Exposure1;
            float _Exposure2;
            float _Exposure3;
            float _Blend;
            float _Blend2;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float theta = atan2(i.worldPos.z, i.worldPos.x);
                float phi = acos(i.worldPos.y / length(i.worldPos));
                
                float2 uv;
                uv.x = - (theta / (2.0 * UNITY_PI)) + 0.5;
                uv.y = 1.0 - (phi / UNITY_PI); // v 좌표 반전

                fixed4 col1 = tex2D(_Tex1, uv) * _Exposure1;
                fixed4 col2 = tex2D(_Tex2, uv) * _Exposure2;
                fixed4 col3 = tex2D(_Tex3, uv) * _Exposure3;

                // 두 텍스처를 먼저 혼합
                fixed4 blendedColor = lerp(col1, col2, _Blend);

                // _Blend2 값이 0이 아니면 세 번째 텍스처를 추가로 혼합
                if (_Blend2 != 0.0)
                {
                    blendedColor = lerp(blendedColor, col3, _Blend2);
                }

                return blendedColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
