Shader "Rishaan.nz/DiffuseColor" 
{
	Properties 
	{
		[HDR]_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		
		[Header(Stencil)]
		_Stencil ("Stencil ID [0;255]", Float) = 0
		_ReadMask ("ReadMask [0;255]", Int) = 255
		_WriteMask ("WriteMask [0;255]", Int) = 255
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation", Int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilFail ("Stencil Fail", Int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilZFail ("Stencil ZFail", Int) = 0
		
		[Header(Rendering)]
		[Enum(UnityEngine.Rendering.CullMode)] _Culling ("Culling", Int) = 2
		[Enum(Off,0,On,1)] _ZWrite("ZWrite", Int) = 1
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Int) = 4
		_Offset("Offset", Float) = 0
		[Enum(None,0,Alpha,1,Red,8,Green,4,Blue,2,RGB,14,RGBA,15)] _ColorMask("Writing Color Mask", Int) = 15
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	
	sampler2D _MainTex;
	half4 _Color;

	struct Input 
	{
		float2 uv_MainTex;
	};

	void surf (Input IN, inout SurfaceOutput o) 
	{
		half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
	
	SubShader 
	{
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_ReadMask]
			WriteMask [_WriteMask]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			Fail [_StencilFail]
			ZFail [_StencilZFail]
		}
		
		Tags { "RenderType"="Opaque" }
		LOD 150
		Cull [_Culling]
		Offset [_Offset], [_Offset]
		ZWrite [_ZWrite]
		ZTest [_ZTest]
		ColorMask [_ColorMask]

		CGPROGRAM
		#pragma surface surf Lambert
		ENDCG
	}
	Fallback "Mobile/VertexLit"
}
