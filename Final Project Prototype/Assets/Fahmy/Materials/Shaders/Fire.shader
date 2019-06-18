// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fire and Ring"
{
	Properties
	{
		_Fire("Fire", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_PlayerPos("PlayerPos", Vector) = (0,0,0,0)
		_FireDistance("Fire Distance", Float) = 0.1
		_RingSize("RingSize", Range( 0 , 0.98)) = 0.98
		_Float1("Float 1", Float) = 1
		_RingIntensity("RingIntensity", Float) = 1
		_Fire2("Fire2", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _Fire;
		uniform sampler2D _Fire2;
		uniform float4 _Fire2_ST;
		uniform float _Float1;
		uniform float3 _PlayerPos;
		uniform float _FireDistance;
		uniform float _RingIntensity;
		uniform float _RingSize;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner5 = ( 1.0 * _Time.y * float2( 0.05,0.05 ) + i.uv_texcoord);
			float2 uv_Fire2 = i.uv_texcoord * _Fire2_ST.xy + _Fire2_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_5_0_g1 = ( float3( (ase_worldPos).xz ,  0.0 ) + _PlayerPos );
			float clampResult11_g1 = clamp( ( ( 1.0 - distance( temp_output_5_0_g1 , ( temp_output_5_0_g1 * float3( -1,-1,-1 ) ) ) ) + _FireDistance ) , 0.0 , 1.0 );
			float temp_output_99_0 = clampResult11_g1;
			float temp_output_83_0 = ( 1.0 - temp_output_99_0 );
			float4 temp_output_21_0 = ( ( tex2D( _Fire, panner5 ) * tex2D( _Fire2, uv_Fire2 ) * float4(1,0.9158285,0.1650943,0) * _Float1 ) * temp_output_83_0 );
			o.Emission = ( temp_output_21_0 + ( temp_output_21_0 * ( _RingIntensity * ( step( temp_output_99_0 , 1.0 ) * step( temp_output_83_0 , _RingSize ) ) * 6.28318548202515 ) ) ).rgb;
			o.Alpha = 1;
			clip( temp_output_83_0 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15800
627.2;73.6;628;446;-341.5841;243.9235;2.163215;False;False
Node;AmplifyShaderEditor.CommentaryNode;1;-802.1705,-523.5842;Float;False;1279.169;551.5795;Normal Maps/Wave Movement;7;7;6;5;2;10;11;96;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;98;-810.9556,128.1965;Float;False;1659.124;753.0027;Ring;9;86;83;84;72;92;90;77;91;93;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-762.4841,-447.8747;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;99;-760.9556,178.1965;Float;True;InnerCircle;2;;1;3a59fa5512bd1fa4797d757ca2d3d56c;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;5;-480.6667,-469.3452;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.05,0.05;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;86;-387.7132,650.1971;Float;False;Property;_RingSize;RingSize;5;0;Create;True;0;0;False;0;0.98;0;0;0.98;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;83;-334.6661,335.4688;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;11;-93.32657,-111.5489;Float;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;False;0;1,0.9158285,0.1650943,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-580.0203,-240.1618;Float;True;Property;_Fire2;Fire2;8;0;Create;True;0;0;False;0;22cbf6d2c37622c499b36b016f427263;fb6566c21f717904f83743a5a76dd0b0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-159.0773,-459.6078;Float;True;Property;_Fire;Fire;0;0;Create;True;0;0;False;0;f7e96904e8667e1439548f0f86389447;fb6566c21f717904f83743a5a76dd0b0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;84;12.38927,438.0649;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;72;4.316693,204.8219;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-257.0583,-80.09526;Float;False;Property;_Float1;Float 1;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;92;333.5602,771.5993;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;232.7445,556.8279;Float;False;Property;_RingIntensity;RingIntensity;7;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;328.3003,301.5461;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;180.1949,-257.8174;Float;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;593.0842,-89.44599;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;521.8903,452.0457;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;612.7687,199.5946;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;94;866.5836,-24.18131;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;102;1175.894,-23.96427;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Fire and Ring;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;2;0
WireConnection;83;0;99;0
WireConnection;6;1;5;0
WireConnection;84;0;83;0
WireConnection;84;1;86;0
WireConnection;72;0;99;0
WireConnection;77;0;72;0
WireConnection;77;1;84;0
WireConnection;10;0;6;0
WireConnection;10;1;7;0
WireConnection;10;2;11;0
WireConnection;10;3;96;0
WireConnection;21;0;10;0
WireConnection;21;1;83;0
WireConnection;91;0;90;0
WireConnection;91;1;77;0
WireConnection;91;2;92;0
WireConnection;93;0;21;0
WireConnection;93;1;91;0
WireConnection;94;0;21;0
WireConnection;94;1;93;0
WireConnection;102;2;94;0
WireConnection;102;10;83;0
ASEEND*/
//CHKSM=5A77C9D78CC64FB2BC15CD40F00B22CC2332AC63