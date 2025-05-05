// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/RippleShader" {
	Properties {
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BaseColor("Base Color", Color) = (0.1,0.1,0.1,1)
		_RippleColor("Ripple Color", Color) = (1,1,1,1)
		_IsRainbow("Use Rainbow Ripple (0: OFF, 1: ON)", Float) = 1
		_ColorChangeSpeed("Color Change Speed", Float) = 1
		_RippleWidth ("Ripple Width", Range(0, 0.999)) = 0.2
		_RippleHeight ("Ripple Height", Float) = 0.2
		_RippleInterval ("Ripple Interval", Float) = 3
		_RippleSpeed ("Ripple Speed", Float) = 100
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		fixed4 _BaseColor;
		fixed4 _RippleColor;
		float _IsRainbow;
		float _ColorChangeSpeed;
		float _RippleWidth;
		float _RippleHeight;
		float _RippleInterval;
		float _RippleSpeed;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		float3 hsvToRgb(float3 hsv)
		{
			float3 rgb = float3(0.0f, 0.0f, 0.0f);

			float h = hsv.x * 6.0f;
			float i = floor(h);
			float var_1 = hsv.z * (1.0f - hsv.y);
			float var_2 = hsv.z * (1.0f - hsv.y * (h - i));
			float var_3 = hsv.z * (1.0f - hsv.y * (1.0f - (h - i)));
			if (i == 0.0f) { rgb = float3(hsv.z, var_3, var_1); }
			else if (i == 1.0f) { rgb = float3(var_2, hsv.z, var_1); }
			else if (i == 2.0f) { rgb = float3(var_1, hsv.z, var_3); }
			else if (i == 3.0f) { rgb = float3(var_1, var_2, hsv.z); }
			else if (i == 4.0f) { rgb = float3(var_3, var_1, hsv.z); }
			else { rgb = float3(hsv.z, var_1, var_2); }

			return (rgb);
		}

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			float dist = distance(fixed3(0, 0, 0), worldPos);
			float val = abs(sin(dist*_RippleInterval - _Time * _RippleSpeed));
			
			float threshold = (1.0f - _RippleWidth);
			if (val > threshold)
			{
				// v.vertex.xyz = float3(v.vertex.x, v.vertex.y + _RippleHeight * (val - threshold) / (1.0f - threshold), v.vertex.z);
				v.vertex.xyz += _RippleHeight * (val - threshold) / (1.0f - threshold) * v.normal;
			}
		}

		void surf (Input IN, inout SurfaceOutput o) {

			float dist = distance(fixed3(0.0f, 0.0f, 0.0f), IN.worldPos);
			float val = abs(sin(dist*_RippleInterval - _Time * _RippleSpeed));

			float threshold = (1.0f - _RippleWidth);
			if (val > threshold)
			{
				if (_IsRainbow >= 1.0f)
				{
					float hue = frac(_Time * _ColorChangeSpeed);
					fixed3 hsvColor = fixed3(hue, 1.0f, 1.0f);
					fixed3 rgbColor = hsvToRgb(hsvColor);
					fixed4 rgbaColor = fixed4(rgbColor.r, rgbColor.g, rgbColor.b, 1.0f);
					fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * rgbaColor;
					o.Albedo = c.rgb;
					o.Alpha = c.a;
				}
				else
				{
					fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _RippleColor;
					o.Albedo = c.rgb;
					o.Alpha = c.a;
				}
			}
			else
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _BaseColor;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}
