// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/ShaderYo"
{
	//Using properties to make the Shader adjustable
	Properties
	{
		_Tint("Tint", Color) = (1, 1, 1, 1)
		_MainTex("Texture", 2D) = "white" {}
	}

	//You can use SubShaders to group multiple shader variants together
	SubShader
	{
		//A shader pass is where an object actually gets rendered
		Pass
		{
			//Indicates the start of our code
			CGPROGRAM

			//We have to tell the compiler which programs/functions to use, via pragma directives
			#pragma vertex VertexProgramYo
			#pragma fragment FragmentProgramYo

			//Including Unity Shader file that defines common variables, functions, and other things
			#include "UnityCG.cginc"

			float4 _Tint;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			//Setting up this struct so the FragmentProgram values are not that intimidating
			struct Interpolators 
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			struct VertexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			Interpolators VertexProgramYo(VertexData v)
			{
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return i;
			}

			float4 FragmentProgramYo(Interpolators i) : SV_TARGET
			{
				return tex2D(_MainTex, i.uv);
			}

			//Ending the code here
			ENDCG
		}
	}
}
