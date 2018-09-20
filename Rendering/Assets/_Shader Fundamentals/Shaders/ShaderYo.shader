// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/ShaderYo"
{
	//Using properties to make the Shader adjustable
	Properties
	{
		_Tint("Tint", Color) = (1, 1, 1, 1)
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

			float4 VertexProgramYo(float4 position : POSITION, out float3 localPosition : TEXCOORD0) : SV_POSITION
			{
				localPosition = position.xyz;
				return UnityObjectToClipPos(position);
			}

			float4 FragmentProgramYo(float4 position : SV_POSITION, float3 localPosition : TEXCOORD0) : SV_TARGET
			{
				return float4(localPosition, 1);
			}

			//Ending the code here
			ENDCG
		}
	}
}
