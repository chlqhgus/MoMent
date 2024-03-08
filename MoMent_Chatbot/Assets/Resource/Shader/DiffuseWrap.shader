Shader "Custom/DiffuseWrap" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RampTex ("ramp (RGBA)", 2D) = "black" {}
		_RimColor ("Rim Color", Color) = (0.235, 0.235, 0.196, 0.0)
		_RimPower ("RimPower", Float) = 4.0
		
		
    }
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" "IgnoreProjector"="True"}
		LOD 200
		
		
		CGPROGRAM
		#pragma surface surf DiffuseWrap
		
		fixed4 _RimColor;
		fixed _RimPower;
		sampler2D _RampTex;
		float _Light;
				
		half4 LightingDiffuseWrap (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) 
		{
			float NdotL = saturate(dot(s.Normal, lightDir))* 0.5 + 0.5;
			float4 ramp = tex2D(_RampTex, float2(NdotL  ,0.5 ))*0.9;
					
          	float4 final;
          	final.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten *2);
          	final.a = s.Alpha;
            return final;
      	}
		
		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;
			float3 viewDir;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			float rim = 1- saturate(dot (normalize(IN.viewDir), o.Normal));
          	o.Emission = _RimColor.rgb * pow (rim, _RimPower);//*c.rgb;
          	o.Gloss = 1;
		}
		ENDCG
	} 
	FallBack "VertexLit"
}
