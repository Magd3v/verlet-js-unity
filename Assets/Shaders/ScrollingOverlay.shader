// Shader created with Shader Forge v1.02 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.02;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.6906694,fgcg:1,fgcb:0.6323529,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32883,y:32688,varname:node_1,prsc:2|custl-240-RGB;n:type:ShaderForge.SFN_ScreenPos,id:3548,x:31902,y:32880,varname:node_3548,prsc:2,sctp:0;n:type:ShaderForge.SFN_Tex2d,id:240,x:32466,y:32872,ptovrint:False,ptlb:node_240,ptin:_node_240,varname:_node_240,prsc:2,tex:87e98f563747ec04e8cd93b3fc3b1a29,ntxv:0,isnm:False|UVIN-12-UVOUT;n:type:ShaderForge.SFN_Panner,id:12,x:32184,y:32938,varname:node_12,prsc:2,spu:1,spv:1|UVIN-3548-UVOUT,DIST-700-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1898,x:31738,y:33039,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:_Speed,prsc:2,glob:False,v1:0.5;n:type:ShaderForge.SFN_Time,id:4755,x:31738,y:33161,varname:node_4755,prsc:2;n:type:ShaderForge.SFN_Multiply,id:700,x:32009,y:33107,varname:node_700,prsc:2|A-1898-OUT,B-4755-T;proporder:240-1898;pass:END;sub:END;*/

Shader "Unlit/ScrollingTexture" {
    Properties {
        _node_240 ("node_240", 2D) = "white" {}
        _Speed ("Speed", Float ) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_240; uniform float4 _node_240_ST;
            uniform float _Speed;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.screenPos = o.pos;
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
/////// Vectors:
////// Lighting:
                float4 node_4755 = _Time + _TimeEditor;
                float2 node_12 = (i.screenPos.rg+(_Speed*node_4755.g)*float2(1,1));
                float4 _node_240_var = tex2D(_node_240,TRANSFORM_TEX(node_12, _node_240));
                float3 finalColor = _node_240_var.rgb;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
