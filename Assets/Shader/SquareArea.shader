Shader "Rishaan.nz/SquareArea"
{
	Properties
	{
		[HDR] _Color("Main Color", Color) = (1,1,1,1)
		_SquareCenter("Cylinder Center", Vector) = (0,0,0,0)
		_SquareRadius("Cylinder Radius", Float) = 3

		[Header(Rendering)]
		[Enum(Off,0,On,1)] _ZWrite("ZWrite", Int) = 1
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Int) = 4
		_Offset("Offset", Float) = 0
		[Enum(None,0,Alpha,1,Red,8,Green,4,Blue,2,RGB,14,RGBA,15)] _ColorMask("Writing Color Mask", Int) = 15

		[Header(Backface Stencil)]
		_Stencil("Stencil ID [0;255]", Float) = 0
		_ReadMask("ReadMask [0;255]", Int) = 255
		_WriteMask("WriteMask [0;255]", Int) = 255
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Comparison", Int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOp("Stencil Operation", Int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilFail("Stencil Fail", Int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilZFail("Stencil ZFail", Int) = 0
	}

		SubShader
		{
			Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }
			LOD 200
			Cull Back
			Offset[_Offset],[_Offset]
			ZWrite[_ZWrite]
			ZTest[_ZTest]
			ColorMask[_ColorMask]

			CGPROGRAM
			#pragma surface surf Standard

			 struct Input
			 {
				 float3 worldPos;
			 };
			 half4 _Color;
			 float4 _SquareCenter;
			 float _SquareRadius;
			 void surf(Input IN, inout SurfaceOutputStandard o)
			 {
				 float3 dist = _SquareCenter.xyz - IN.worldPos;
				 if (dist.x > _SquareRadius || dist.z > _SquareRadius) discard;

				 o.Albedo = _Color;
			 }

			ENDCG

				// Fill stencil mask for cutting areas
				Pass
				{
					Name "Cut"
					Tags{ "Queue" = "Geometry" "IgnoreProjector" = "True" "RenderType" = "Opaque" }
					LOD 200
					Cull Front
					Offset[_Offset],[_Offset]
					ZWrite Off
					ZTest LEqual
					ColorMask 0

					Stencil
					{
					   Ref[_Stencil]
					   ReadMask[_ReadMask]
					   WriteMask[_WriteMask]
					   Comp[_StencilComp]
					   Pass[_StencilOp]
					   Fail[_StencilFail]
					   ZFail[_StencilZFail]
					}

					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag

					float4 _SquareCenter;
					float _SquareRadius;

					struct appdata_t {
						float4 vertex : POSITION;
					};

					struct v2f {
						float4 vertex : SV_POSITION;
						float3 worldPos : TEXCOORD0;
					};

					fixed4 _Color;

					v2f vert(appdata_t v)
					{
						v2f o;
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.worldPos = mul(unity_ObjectToWorld, v.vertex);
						return o;
					}

					fixed4 frag(v2f IN) : COLOR
					{
						float3 dist = _SquareCenter.xyz - IN.worldPos;
						if (dist.x > _SquareRadius || dist.z > _SquareRadius) discard;
						fixed4 col = fixed4(1,0,0,1);
						return col;
					}
					ENDCG
				}

				// shadow caster pass
				Pass
				{
					Name "Caster"
					Tags{ "LightMode" = "ShadowCaster" }
					Cull[_Culling]
					Offset[_Offset],[_Offset]
					ZWrite[_ZWrite]
					ZTest[_ZTest]
					ColorMask[_ColorMask]

					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 2.0
					#pragma multi_compile_shadowcaster
					#pragma multi_compile_instancing // allow instanced shadow pass for most of the shaders
					#include "UnityCG.cginc"

					float4 _SquareCenter;
					float _SquareRadius;

					struct v2f {
						V2F_SHADOW_CASTER;
						float3 worldPos : TEXCOORD0;
						UNITY_VERTEX_OUTPUT_STEREO
					};

					v2f vert(appdata_base v)
					{
						v2f o;
						UNITY_SETUP_INSTANCE_ID(v);
						o.worldPos = mul(unity_ObjectToWorld, v.vertex);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
						TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
						return o;
					}

					uniform sampler2D _MainTex;
					uniform half _Cutoff;

					float4 frag(v2f IN) : SV_Target
					{
						float3 dist = _SquareCenter.xyz - IN.worldPos;
						if (dist.x > _SquareRadius || dist.z > _SquareRadius) discard;

						SHADOW_CASTER_FRAGMENT(IN)
					}
					ENDCG
				}
		}
}