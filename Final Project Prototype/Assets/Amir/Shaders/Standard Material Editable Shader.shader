// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Standard Material Editable Shader"
{
	Properties
	{
		[NoScaleOffset]_BaseColorRGBOutlineWidthA("Base Color (RGB) Outline Width (A)", 2D) = "gray" {}
		_BaseTint("Base Tint", Color) = (1,1,1,0)
		_BaseCellSharpness("Base Cell Sharpness", Range( 0.01 , 1)) = 0.01
		_BaseCellOffset("Base Cell Offset", Range( -1 , 1)) = 0
		_IndirectDiffuseContribution("Indirect Diffuse Contribution", Range( 0 , 1)) = 1
		_ShadowContribution("Shadow Contribution", Range( 0 , 1)) = 0.5
		[NoScaleOffset]_Highlight("Highlight", 2D) = "white" {}
		[HDR]_HighlightTint("Highlight Tint", Color) = (1,1,1,1)
		_HighlightCellOffset("Highlight Cell Offset", Range( -1 , -0.5)) = -0.95
		_HighlightCellSharpness("Highlight Cell Sharpness", Range( 0.001 , 1)) = 0.01
		_IndirectSpecularContribution("Indirect Specular Contribution", Range( 0 , 1)) = 1
		[Toggle(_STATICHIGHLIGHTS_ON)] _StaticHighLights("Static HighLights", Float) = 0
		[Normal][NoScaleOffset]_NormalMap("Normal Map", 2D) = "bump" {}
		_NormalScale("Normal Scale", Range( 0 , 1)) = 1
		[HDR]_RimColor("Rim Color", Color) = (1,1,1,0)
		_RimPower("Rim Power", Range( 0.01 , 1)) = 0.4
		_RimOffset("Rim Offset", Range( 0 , 1)) = 0.6
		_OutlineTint("Outline Tint", Color) = (0.5294118,0.5294118,0.5294118,0)
		_OutlineWidth("Outline Width", Range( 0.02 , 0.2)) = 0.02
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_BaseColorRGBOutlineWidthA202 = v.texcoord;
			float4 tex2DNode202 = tex2Dlod( _BaseColorRGBOutlineWidthA, float4( uv_BaseColorRGBOutlineWidthA202, 0, 0.0) );
			float OutlineCustomWidth161 = tex2DNode202.a;
			float outlineVar = ( _OutlineWidth * OutlineCustomWidth161 );
			v.vertex.xyz += ( v.normal * outlineVar );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float3 temp_cast_0 = (1.0).xxx;
			float2 uv_NormalMap115 = i.uv_texcoord;
			float3 normalizeResult117 = normalize( (WorldNormalVector( i , UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap115 ), _NormalScale ) )) );
			float3 NewNormals118 = normalizeResult117;
			float3 lerpResult204 = lerp( temp_cast_0 , NewNormals118 , _IndirectDiffuseContribution);
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float temp_output_201_0 = ( 1.0 - ( ( 1.0 - 0.0 ) * _WorldSpaceLightPos0.w ) );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult121 = dot( NewNormals118 , ase_worldlightDir );
			float NdotL122 = dotResult121;
			float lerpResult205 = lerp( temp_output_201_0 , ( saturate( ( ( NdotL122 + _BaseCellOffset ) / _BaseCellSharpness ) ) * 0.0 ) , _ShadowContribution);
			float2 uv_BaseColorRGBOutlineWidthA202 = i.uv_texcoord;
			float4 tex2DNode202 = tex2D( _BaseColorRGBOutlineWidthA, uv_BaseColorRGBOutlineWidthA202 );
			float3 BaseColor160 = ( ( ( lerpResult204 * ase_lightColor.a * temp_output_201_0 ) + ( ase_lightColor.rgb * lerpResult205 ) ) * (( tex2DNode202 * _BaseTint )).rgb );
			o.Emission = ( BaseColor160 * (_OutlineTint).rgb );
			o.Normal = float3(0,0,-1);
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _STATICHIGHLIGHTS_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float3 worldPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _NormalScale;
		uniform sampler2D _NormalMap;
		uniform float _IndirectDiffuseContribution;
		uniform float _BaseCellOffset;
		uniform float _BaseCellSharpness;
		uniform float _ShadowContribution;
		uniform sampler2D _BaseColorRGBOutlineWidthA;
		uniform float4 _BaseTint;
		uniform float _IndirectSpecularContribution;
		uniform float4 _HighlightTint;
		uniform sampler2D _Highlight;
		uniform float _HighlightCellOffset;
		uniform float _HighlightCellSharpness;
		uniform float _RimOffset;
		uniform float _RimPower;
		uniform float4 _RimColor;
		uniform float _OutlineWidth;
		uniform float4 _OutlineTint;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float lerpResult172 = lerp( 1.0 , 0.0 , _IndirectSpecularContribution);
			float2 uv_Highlight124 = i.uv_texcoord;
			float4 temp_output_133_0 = ( _HighlightTint * tex2D( _Highlight, uv_Highlight124 ) );
			float3 HighlightColor165 = (temp_output_133_0).rgb;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 LightColorFalloff162 = ( ase_lightColor.rgb * float3( 0,0,0 ) );
			float temp_output_139_0 = (temp_output_133_0).a;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult4_g4 = normalize( ( ase_worldViewDir + ase_worldlightDir ) );
			float2 uv_NormalMap115 = i.uv_texcoord;
			float3 normalizeResult117 = normalize( (WorldNormalVector( i , UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap115 ), _NormalScale ) )) );
			float3 NewNormals118 = normalizeResult117;
			float dotResult140 = dot( normalizeResult4_g4 , NewNormals118 );
			float dotResult121 = dot( NewNormals118 , ase_worldlightDir );
			float NdotL122 = dotResult121;
			#ifdef _STATICHIGHLIGHTS_ON
				float staticSwitch150 = NdotL122;
			#else
				float staticSwitch150 = dotResult140;
			#endif
			float3 temp_cast_1 = (1.0).xxx;
			float3 lerpResult204 = lerp( temp_cast_1 , NewNormals118 , _IndirectDiffuseContribution);
			float temp_output_201_0 = ( 1.0 - ( ( 1.0 - 0.0 ) * _WorldSpaceLightPos0.w ) );
			float lerpResult205 = lerp( temp_output_201_0 , ( saturate( ( ( NdotL122 + _BaseCellOffset ) / _BaseCellSharpness ) ) * 0.0 ) , _ShadowContribution);
			float2 uv_BaseColorRGBOutlineWidthA202 = i.uv_texcoord;
			float4 tex2DNode202 = tex2D( _BaseColorRGBOutlineWidthA, uv_BaseColorRGBOutlineWidthA202 );
			float3 BaseColor160 = ( ( ( lerpResult204 * ase_lightColor.a * temp_output_201_0 ) + ( ase_lightColor.rgb * lerpResult205 ) ) * (( tex2DNode202 * _BaseTint )).rgb );
			float dotResult131 = dot( NewNormals118 , ase_worldViewDir );
			c.rgb = ( ( lerpResult172 * HighlightColor165 * LightColorFalloff162 * pow( temp_output_139_0 , 1.5 ) * saturate( ( ( staticSwitch150 + _HighlightCellOffset ) / ( ( 1.0 - temp_output_139_0 ) * _HighlightCellSharpness ) ) ) ) + BaseColor160 + ( ( saturate( NdotL122 ) * pow( ( 1.0 - saturate( ( dotResult131 + _RimOffset ) ) ) , _RimPower ) ) * HighlightColor165 * LightColorFalloff162 * (_RimColor).rgb ) );
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
			float3 temp_cast_0 = (1.0).xxx;
			float2 uv_NormalMap115 = i.uv_texcoord;
			float3 normalizeResult117 = normalize( (WorldNormalVector( i , UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap115 ), _NormalScale ) )) );
			float3 NewNormals118 = normalizeResult117;
			float3 lerpResult204 = lerp( temp_cast_0 , NewNormals118 , _IndirectDiffuseContribution);
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float temp_output_201_0 = ( 1.0 - ( ( 1.0 - 0.0 ) * _WorldSpaceLightPos0.w ) );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult121 = dot( NewNormals118 , ase_worldlightDir );
			float NdotL122 = dotResult121;
			float lerpResult205 = lerp( temp_output_201_0 , ( saturate( ( ( NdotL122 + _BaseCellOffset ) / _BaseCellSharpness ) ) * 0.0 ) , _ShadowContribution);
			float2 uv_BaseColorRGBOutlineWidthA202 = i.uv_texcoord;
			float4 tex2DNode202 = tex2D( _BaseColorRGBOutlineWidthA, uv_BaseColorRGBOutlineWidthA202 );
			float3 BaseColor160 = ( ( ( lerpResult204 * ase_lightColor.a * temp_output_201_0 ) + ( ase_lightColor.rgb * lerpResult205 ) ) * (( tex2DNode202 * _BaseTint )).rgb );
			o.Albedo = BaseColor160;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15800
6;0.4;1524;796;126.8795;290.8604;3.166029;True;False
Node;AmplifyShaderEditor.CommentaryNode;104;-3354.934,346.4854;Float;False;1370.182;280;Comment;5;118;117;116;115;114;Normals;0.5220588,0.6044625,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-3304.934,422.9949;Float;False;Property;_NormalScale;Normal Scale;13;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;115;-2973.327,396.4854;Float;True;Property;_NormalMap;Normal Map;12;2;[Normal];[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;116;-2654.544,401.7255;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;117;-2412.908,400.7819;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;105;-3227.469,927.6155;Float;False;835.6508;341.2334;Comment;4;122;121;120;119;N dot L;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;118;-2227.752,402.2329;Float;False;NewNormals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;120;-3177.469,1089.849;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;119;-3150.41,977.6155;Float;False;118;NewNormals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;121;-2853.177,1014.414;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;122;-2634.818,1017.73;Float;False;NdotL;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;106;-1227.062,1021.001;Float;False;2744.931;803.0454;Comment;24;205;203;202;201;199;198;197;196;195;194;193;192;191;190;161;160;151;145;143;142;137;136;135;126;Base Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;190;-1177.062,1202.4;Float;False;Property;_BaseCellOffset;Base Cell Offset;3;0;Create;True;0;0;False;0;0;-0.716;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;191;-1176.854,1094.304;Float;False;122;NdotL;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;192;-874.9866,1192.086;Float;False;Property;_BaseCellSharpness;Base Cell Sharpness;2;0;Create;True;0;0;False;0;0.01;0.598;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;193;-893.6135,1095.575;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;194;-925.0876,1460.827;Float;False;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.OneMinusNode;196;-607.0186,1377.142;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;195;-594.5486,1098.418;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;197;-437.0957,1103.448;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;107;-1195.705,373.7631;Float;False;828.4254;361.0605;Comment;4;204;200;128;123;Indirect Diffuse;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;109;-1902.531,-668.3514;Float;False;2234.221;738.9581;Comment;19;185;179;173;168;165;159;155;152;150;149;148;146;140;138;134;133;130;127;124;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;198;-416.4746,1412.778;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;108;-823.2786,2122.898;Float;False;1926.522;520.1537;Comment;17;186;181;180;177;175;166;164;163;158;157;154;147;141;132;131;129;125;Rim Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-395.6855,1544.562;Float;False;Property;_ShadowContribution;Shadow Contribution;5;0;Create;True;0;0;False;0;0.5;0.425;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;128;-766.0925,423.763;Float;False;Constant;_Float4;Float 4;20;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;123;-1145.705,518.1434;Float;False;118;NewNormals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;127;-1221.616,-618.3514;Float;False;Property;_HighlightTint;Highlight Tint;7;1;[HDR];Create;True;0;0;False;0;1,1,1,1;0.745283,0.2495995,0.2495995,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;200;-891.7096,619.8235;Float;False;Property;_IndirectDiffuseContribution;Indirect Diffuse Contribution;4;0;Create;True;0;0;False;0;1;0.506;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-773.2786,2223.467;Float;False;118;NewNormals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;201;-226.5986,1415.763;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;129;-726.2476,2334.676;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;199;-199.9446,1102.333;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;124;-1290.549,-451.5374;Float;True;Property;_Highlight;Highlight;6;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightColorNode;203;75.80444,1102.861;Float;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;110;-658.7096,-997.6204;Float;False;287;165;Comment;1;139;Spec/Smooth;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;205;156.1104,1417.839;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;204;-551.2795,500.0783;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;135;424.6543,1640.337;Float;False;Property;_BaseTint;Base Tint;1;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;134;-1788.967,-44.39368;Float;False;118;NewNormals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;-891.8936,-496.2083;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;131;-421.1455,2269.784;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;130;-1852.531,-172.4974;Float;False;Blinn-Phong Half Vector;-1;;4;91a149ac9d615be429126c95e20753ce;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;132;-478.5447,2387.001;Float;False;Property;_RimOffset;Rim Offset;16;0;Create;True;0;0;False;0;0.6;0.306;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;202;386.2095,1436.607;Float;True;Property;_BaseColorRGBOutlineWidthA;Base Color (RGB) Outline Width (A);0;1;[NoScaleOffset];Create;True;0;0;False;0;None;ec1d3a3dc4b339c41a4f6dc0956c60d1;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;139;-608.7096,-947.6204;Float;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;138;-1547.602,-284.2661;Float;False;122;NdotL;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;141;-197.5457,2274.001;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;140;-1507.311,-118.0541;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;725.6765,1531.356;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;499.8384,1295.382;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;111;-3186.991,1504.479;Float;False;717.6841;295.7439;Comment;3;162;153;144;Light Falloff;0.9947262,1,0.6176471,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;497.5364,1071.001;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;145;756.6833,1184.126;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;147;-37.54565,2274.001;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;146;-529.5426,-324.6423;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;149;-752.0586,-87.01874;Float;False;Property;_HighlightCellSharpness;Highlight Cell Sharpness;9;0;Create;True;0;0;False;0;0.01;0.745;0.001;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;143;872.8445,1515.154;Float;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;148;-1145.32,-99.23584;Float;False;Property;_HighlightCellOffset;Highlight Cell Offset;8;0;Create;True;0;0;False;0;-0.95;-0.717;-1;-0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;144;-3091.288,1554.479;Float;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.StaticSwitch;150;-1279.979,-223.3687;Float;False;Property;_StaticHighLights;Static HighLights;11;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;-2902.992,1620.64;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;154;26.45435,2402.001;Float;False;Property;_RimPower;Rim Power;15;0;Create;True;0;0;False;0;0.4;1;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;113;1442.604,2102.855;Float;False;1039.617;429.9737;Comment;8;188;184;183;182;178;176;174;167;Custom Outline;1,0.6029412,0.7097364,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;-790.5426,-227.1487;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;112;-863.5135,-1539.678;Float;False;1008.755;365.3326;Comment;4;172;170;169;156;Indirect Specular;1,1,1,1;0;0
Node;AmplifyShaderEditor.ComponentMaskNode;152;-652.1936,-492.3914;Float;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;158;93.82251,2172.898;Float;False;122;NdotL;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;151;1057.74,1310.324;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;157;138.4543,2274.001;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;159;-434.6287,-93.68359;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;166;319.6775,2194.758;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;168;-442.3057,-234.8416;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;167;1503.116,2244.897;Float;False;Property;_OutlineTint;Outline Tint;17;0;Create;True;0;0;False;0;0.5294118,0.5294118,0.5294118,0;0.6509434,0.5004895,0.5004895,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;160;1250.472,1305.904;Float;False;BaseColor;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;161;838.8335,1647.105;Float;False;OutlineCustomWidth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;163;330.4543,2274.001;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;169;-456.6206,-1289.347;Float;False;Property;_IndirectSpecularContribution;Indirect Specular Contribution;10;0;Create;True;0;0;False;0;1;0.811;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;170;-362.8186,-1489.678;Float;False;Constant;_Float5;Float 5;20;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;164;339.8464,2435.405;Float;False;Property;_RimColor;Rim Color;14;1;[HDR];Create;True;0;0;False;0;1,1,1,0;21.92081,21.92081,21.92081,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;162;-2728.308,1617.107;Float;False;LightColorFalloff;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;165;-339.6245,-494.8824;Float;False;HighlightColor;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;174;1794.362,2442.384;Float;False;161;OutlineCustomWidth;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;173;-346.2627,-389.7676;Float;False;162;LightColorFalloff;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;182;1755.876,2157.097;Float;False;160;BaseColor;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;171;-273.7227,-802.7033;Float;False;2;0;FLOAT;0;False;1;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;614.5825,2202.775;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;178;1775.303,2346.122;Float;False;Property;_OutlineWidth;Outline Width;18;0;Create;True;0;0;False;0;0.02;0.1124;0.02;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;175;602.7783,2390.94;Float;False;162;LightColorFalloff;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;179;-226.4207,-231.1288;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;177;608.2234,2300.973;Float;False;165;HighlightColor;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;180;603.6785,2487.183;Float;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;176;1806.796,2247.138;Float;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;172;-38.75562,-1436.833;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;186;919.5364,2195.827;Float;False;4;4;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;184;2117.684,2351.191;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;183;2066.649,2199.138;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.3382353,0.3382353,0.3382353;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;185;162.6904,-497.9514;Float;False;5;5;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;156;-813.5135,-1419.038;Float;False;118;NewNormals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OutlineNode;188;2232.214,2200.916;Float;False;0;True;None;0;0;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;187;1976.394,1304.762;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;189;2433.033,1107.473;Float;False;160;BaseColor;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3683.888,1266.939;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Standard Material Editable Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;115;5;114;0
WireConnection;116;0;115;0
WireConnection;117;0;116;0
WireConnection;118;0;117;0
WireConnection;121;0;119;0
WireConnection;121;1;120;0
WireConnection;122;0;121;0
WireConnection;193;0;191;0
WireConnection;193;1;190;0
WireConnection;195;0;193;0
WireConnection;195;1;192;0
WireConnection;197;0;195;0
WireConnection;198;0;196;0
WireConnection;198;1;194;2
WireConnection;201;0;198;0
WireConnection;199;0;197;0
WireConnection;205;0;201;0
WireConnection;205;1;199;0
WireConnection;205;2;126;0
WireConnection;204;0;128;0
WireConnection;204;1;123;0
WireConnection;204;2;200;0
WireConnection;133;0;127;0
WireConnection;133;1;124;0
WireConnection;131;0;125;0
WireConnection;131;1;129;0
WireConnection;139;0;133;0
WireConnection;141;0;131;0
WireConnection;141;1;132;0
WireConnection;140;0;130;0
WireConnection;140;1;134;0
WireConnection;137;0;202;0
WireConnection;137;1;135;0
WireConnection;136;0;203;1
WireConnection;136;1;205;0
WireConnection;142;0;204;0
WireConnection;142;1;203;2
WireConnection;142;2;201;0
WireConnection;145;0;142;0
WireConnection;145;1;136;0
WireConnection;147;0;141;0
WireConnection;146;0;139;0
WireConnection;143;0;137;0
WireConnection;150;1;140;0
WireConnection;150;0;138;0
WireConnection;153;0;144;1
WireConnection;155;0;150;0
WireConnection;155;1;148;0
WireConnection;152;0;133;0
WireConnection;151;0;145;0
WireConnection;151;1;143;0
WireConnection;157;0;147;0
WireConnection;159;0;146;0
WireConnection;159;1;149;0
WireConnection;166;0;158;0
WireConnection;168;0;155;0
WireConnection;168;1;159;0
WireConnection;160;0;151;0
WireConnection;161;0;202;4
WireConnection;163;0;157;0
WireConnection;163;1;154;0
WireConnection;162;0;153;0
WireConnection;165;0;152;0
WireConnection;171;0;139;0
WireConnection;181;0;166;0
WireConnection;181;1;163;0
WireConnection;179;0;168;0
WireConnection;180;0;164;0
WireConnection;176;0;167;0
WireConnection;172;0;170;0
WireConnection;172;2;169;0
WireConnection;186;0;181;0
WireConnection;186;1;177;0
WireConnection;186;2;175;0
WireConnection;186;3;180;0
WireConnection;184;0;178;0
WireConnection;184;1;174;0
WireConnection;183;0;182;0
WireConnection;183;1;176;0
WireConnection;185;0;172;0
WireConnection;185;1;165;0
WireConnection;185;2;173;0
WireConnection;185;3;171;0
WireConnection;185;4;179;0
WireConnection;188;0;183;0
WireConnection;188;1;184;0
WireConnection;187;0;185;0
WireConnection;187;1;160;0
WireConnection;187;2;186;0
WireConnection;0;0;189;0
WireConnection;0;13;187;0
WireConnection;0;11;188;0
ASEEND*/
//CHKSM=E1B453BA006CC1222F3FA477A81CA001F124C336