Shader "Unlit/SpecialFX/Liquide"
{
    Properties
    {
        _Tint ("Teinte", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _FillAmount ("Niveau de Remplissage", Range(-10,10)) = 0.0
        [HideInInspector] _WobbleX ("OscillationX", Range(-1,1)) = 0.0
        [HideInInspector] _WobbleZ ("OscillationZ", Range(-1,1)) = 0.0
        _TopColor ("Couleur Supérieure", Color) = (1,1,1,1)
        _FoamColor ("Couleur de la Mousse", Color) = (1,1,1,1)
        _Rim ("Épaisseur de la Ligne de Mousse", Range(0,0.1)) = 0.0    
        _RimColor ("Couleur du Contour", Color) = (1,1,1,1)
        _RimPower ("Puissance du Contour", Range(0,10)) = 0.0
    }
 
    SubShader
    {
        Tags {
            "Queue"="Geometry"
            "DisableBatching"="True"
            "RenderType"="Opaque"
            "PreviewType"="Plane"
            "VRCFallback"="Hidden"
        }

        Stencil {
            Ref 1
            Comp Always
            Pass Replace
        }
  
        Pass
        {
            Zwrite On
            Cull Off
            AlphaToMask On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON
           
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 viewDir : COLOR;
                float3 normal : COLOR2;
                float fillEdge : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FillAmount, _WobbleX, _WobbleZ;
            float4 _TopColor, _RimColor, _FoamColor, _Tint;
            float _Rim, _RimPower;
           
            float4 RotateAroundYInDegrees (float4 vertex, float degrees)
            {
                float alpha = degrees * UNITY_PI / 180;
                float sina, cosa;
                sincos(alpha, sina, cosa);
                float2x2 m = float2x2(cosa, sina, -sina, cosa);
                return float4(vertex.yz , mul(m, vertex.xz)).xzyw;
            }

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);
                float3 worldPosX = RotateAroundYInDegrees(float4(worldPos,0), 360);
                float3 worldPosZ = float3(worldPosX.y, worldPosX.z, worldPosX.x);
                float3 worldPosAdjusted = worldPos + (worldPosX * _WobbleX) + (worldPosZ * _WobbleZ);
                o.fillEdge = worldPosAdjusted.y + _FillAmount;

                o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                o.normal = v.normal;
                return o;
            }
           
            fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                fixed4 col = tex2D(_MainTex, i.uv) * _Tint;
                UNITY_APPLY_FOG(i.fogCoord, col);
           
                float dotProduct = 1 - pow(dot(i.normal, i.viewDir), _RimPower);
                float4 RimResult = smoothstep(0.5, 1.0, dotProduct);
                RimResult *= _RimColor;

                float4 foam = (step(i.fillEdge, 0.5) - step(i.fillEdge, (0.5 - _Rim)));
                float4 foamColored = foam * (_FoamColor * 0.9);
                float4 result = step(i.fillEdge, 0.5) - foam;
                float4 resultColored = result * col;
                float4 finalResult = resultColored + foamColored;
                finalResult.rgb += RimResult;

                float4 topColor = _TopColor * (foam + result);
                return facing > 0 ? finalResult : topColor;
            }
            ENDCG
        }
    }
}