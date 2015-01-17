Shader "Custom/EnergyShield"
{
    SubShader
	{
        Pass
		{
			Blend SrcAlpha DstAlpha
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct VertexInput
			{
				float4 pos : SV_POSITION;
				float3 color : COLOR0;
			};

			VertexInput vert (appdata_base v)
			{
				VertexInput o;

				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);

				o.color = v.normal * 0.5 + 0.5;

				return o;
			}

			float4 frag (VertexInput i) : COLOR
			{
				return float4(1,0,0,0.2f);
			}

			ENDCG
        }
    }
    Fallback "VertexLit"
}