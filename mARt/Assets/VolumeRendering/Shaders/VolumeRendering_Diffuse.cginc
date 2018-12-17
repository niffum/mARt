#ifndef __VOLUME_RENDERING_INCLUDED__
#define __VOLUME_RENDERING_INCLUDED__

#include "UnityCG.cginc"

#ifndef ITERATIONS
#define ITERATIONS 100
#endif

half4 _Color;
half4 _ColorMask;
sampler3D _Volume;
sampler3D _VolumeMask;
sampler2D _TransferColor;
half _Intensity, _Threshold, _IntensityMask;
half3 _SliceMin, _SliceMax;
float4x4 _AxisRotationMatrix;
float _ShowMask;

struct Ray {
  float3 origin;
  float3 dir;
};

struct AABB {
  float3 min;
  float3 max;
};

bool intersect(Ray r, AABB aabb, out float t0, out float t1)
{
  float3 invR = 1.0 / r.dir;
  float3 tbot = invR * (aabb.min - r.origin);
  float3 ttop = invR * (aabb.max - r.origin);
  float3 tmin = min(ttop, tbot);
  float3 tmax = max(ttop, tbot);
  float2 t = max(tmin.xx, tmin.yz);
  t0 = max(t.x, t.y);
  t = min(tmax.xx, tmax.yz);
  t1 = min(t.x, t.y);
  return t0 <= t1;
}

float3 localize(float3 p) {
  return mul(unity_WorldToObject, float4(p, 1)).xyz;
}

float3 get_uv(float3 p) {
  // float3 local = localize(p);
  return (p + 0.5);
}

float sample_volume(float3 uv, float3 p)
{
  float v = tex3D(_Volume, uv).r * _Intensity;

  float3 axis = mul(_AxisRotationMatrix, float4(p, 0)).xyz;
  axis = get_uv(axis);
  float min = step(_SliceMin.x, axis.x) * step(_SliceMin.y, axis.y) * step(_SliceMin.z, axis.z);
  float max = step(axis.x, _SliceMax.x) * step(axis.y, _SliceMax.y) * step(axis.z, _SliceMax.z);

  return v * min * max;
}

float sample_volume_mask(float3 uv, float3 p)
{
  float v = tex3D(_VolumeMask, uv).a * _IntensityMask;

  float3 axis = mul(_AxisRotationMatrix, float4(p, 0)).xyz;
  axis = get_uv(axis);
  float min = step(_SliceMin.x, axis.x) * step(_SliceMin.y, axis.y) * step(_SliceMin.z, axis.z);
  float max = step(axis.x, _SliceMax.x) * step(axis.y, _SliceMax.y) * step(axis.z, _SliceMax.z);

  return v * min * max;
}

float4 get_transferColor(float isovalue)
{
  float2 xy = float2(isovalue, 0);

  return  tex2D(_TransferColor, xy);
}

bool outside(float3 uv)
{
  const float EPSILON = 0.01;
  float lower = -EPSILON;
  float upper = 1 + EPSILON;
  return (
			uv.x < lower || uv.y < lower || uv.z < lower ||
			uv.x > upper || uv.y > upper || uv.z > upper
		);
}

struct appdata
{
  float4 vertex : POSITION;
  float2 uv : TEXCOORD0;
};

struct v2f
{
  float4 vertex : SV_POSITION;
  float2 uv : TEXCOORD0;
  float3 world : TEXCOORD1;
  float3 local : TEXCOORD2;
};

v2f vert(appdata v)
{
  v2f o;
  o.vertex = UnityObjectToClipPos(v.vertex);
  o.uv = v.uv;
  o.world = mul(unity_ObjectToWorld, v.vertex).xyz;
  o.local = v.vertex.xyz;
  return o;
}

fixed4 phong(float3 gradient, float4 diffuse, float3 local)
{
	float specularIntensity = 0.0;
	
	float AmbientFactor = 0.2;
	float4 specular = float4(1.0, 1.0, 1.0, 0.8);
	float specularStrength = 0.9;

	// Calculates the light's direction in world space.
	float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);

	float3 normal = gradient;
	float3 viewDirection = normalize(local);

	// Calculates the light's intensity on the vertex.
	float intensity = max(dot(normal, lightDirection), 0.0);

	if (intensity > 0)
	{
		// Calculates the half vector between the light vector and the view vector.
		float3 h = normalize(lightDirection + viewDirection);

		// Calculates the specular intensity, based on the reflection direction and view direction.
		specularIntensity = pow(max(dot(normal, h), 0.0), specularStrength);
	}

	// Gets the color of the texture on the current UV coordinates.
	// textureColor = float4(isoValue, isoValue, isoValue, isoValue);

	// Applies a tint to the texture color, based on the passed Diffuse Color.
	//fixed4 textureTint = textureColor * diffuse;

	// Calculates the vetex's ambient color, based on its texture tint color.
	fixed4 ambientColor = diffuse * AmbientFactor;

	return max(diffuse*intensity + specular * specularIntensity, ambientColor);
}

// =============================================================================
// Fragment Function
// =============================================================================


fixed4 frag(v2f i) : SV_Target
{
  Ray ray;
  // ray.origin = localize(i.world);
  ray.origin = i.local;

  // world space direction to object space
  float3 dir = (i.world - _WorldSpaceCameraPos);
  ray.dir = normalize(mul(unity_WorldToObject, dir));

  AABB aabb;
  aabb.min = float3(-0.5, -0.5, -0.5);
  aabb.max = float3(0.5, 0.5, 0.5);

  float tnear;
  float tfar;
  intersect(ray, aabb, tnear, tfar);

  tnear = max(0.0, tnear);

  // float3 start = ray.origin + ray.dir * tnear;
  float3 start = ray.origin;
  float3 end = ray.origin + ray.dir * tfar;
  float dist = abs(tfar - tnear); // float dist = distance(start, end);
  float step_size = dist / float(ITERATIONS);
  float3 ds = normalize(end - start) * step_size;

  float4 dst = float4(0, 0, 0, 0);
  float4 dstMask = float4(0, 0, 0, 0);
  float3 p = start;

  [unroll]
  for (int iter = 0; iter < ITERATIONS; iter++)
  {
    float3 uv = get_uv(p);
    float v = sample_volume(uv, p);
    float4 src = float4(v, v, v, v);
    // Y
    if(v != 0.0)
    {
      src = get_transferColor(v);
    } 
    
    src.a *= 0.5;
    src.rgb *= src.a;


    // blend
    dst = (1.0 - dst.a) * src + dst;
   
    // Sample mask
    if(_ShowMask == 1)
    {
    
      float vMask = sample_volume_mask(uv, p);
      float4 srcMask = float4(vMask, vMask, vMask, vMask);

      srcMask.a *= 0.5;
      srcMask.rgb *= srcMask.a;

      dstMask = (1.0 - dstMask.a) * srcMask + dstMask;
    }

    p += ds;

    if (dst.a > _Threshold) break;
  }

  /*
  if(dst.a > dstMask.a)
  {
    return saturate(dst) * _Color;
  }
  else
  {
    return saturate(dstMask) * _ColorMask;
  }
  */

  //return max(saturate(dst) * _Color , saturate(dstMask) * _ColorMask);
  //return max(saturate(dst), saturate(dstMask) * _ColorMask);

  return saturate(dst);
  // diffuse is transfer color 
  // normal is gradient
  //return phong(,saturate(dst), i.local);

  
}

#endif 
