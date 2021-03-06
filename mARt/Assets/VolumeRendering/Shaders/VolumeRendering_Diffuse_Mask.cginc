﻿/*
 * Source: https://github.com/mattatz/unity-volume-rendering
 * Modified by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */

#ifndef __VOLUME_RENDERING_INCLUDED__
#define __VOLUME_RENDERING_INCLUDED__

#include "UnityCG.cginc"

#ifndef ITERATIONS
#define ITERATIONS 100
#endif

half4 _Color;
sampler3D _Volume;
half _Intensity, _Threshold;
half3 _SliceMin, _SliceMax;
float4x4 _AxisRotationMatrix;

// Added by Viola Jertschat ---------
half _IntensityMask, _Shininess, _Gamma;
sampler2D _TransferColor;
float _ShowMask;
sampler3D _VolumeMask;
half4 _ColorMask;
// ----------------------------------

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
	//tBottom and top are were the ray hits the planes (every Dimension is a t in the formula for where n the line hits one plane )
	float3 tBottom = (aabb.min - r.origin) / r.dir;//invR * (aabb.min - r.origin);
	float3 tTop = (aabb.max - r.origin) / r.dir; //invR * (aabb.max - r.origin);
	// depending on how the cube is turned
	// the longer ray is the far one
  float3 tnear = min(tTop, tBottom);
  float3 tfar = max(tTop, tBottom);

  // At this point we have 6 ts that are hitpoints with the planes when inserted to formula

  // What happens when ray is parallel to two of the planes? what is in the vectors in the dimension of these planes?

  // check if hitpoint really hits the box and not just the plane
  // The greatest t of the near plane hits is needed
  // The smallest t of the far plane
  float2 t = max(tnear.xx, tnear.yz);
  t0 = max(t.x, t.y);
  t = min(tfar.xx, tfar.yz);
  t1 = min(t.x, t.y);
  // if max tnear is greater than min tfar the ray does not intersect with the box
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
	//Get iso value from alpha channel of volume
  float v = tex3D(_Volume, uv).a * _Intensity;

  //Why?
  float3 axis = mul(_AxisRotationMatrix, float4(p, 0)).xyz;
  axis = get_uv(axis);
  //
  float min = step(_SliceMin.x, axis.x) * step(_SliceMin.y, axis.y) * step(_SliceMin.z, axis.z);
  float max = step(axis.x, _SliceMax.x) * step(axis.y, _SliceMax.y) * step(axis.z, _SliceMax.z);

  return v * min * max;
}

// Added by Viola Jertschat -----------------------------------------------
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

float3 get_gradient(float3 uv, float3 p)
{
	float3 gradient = tex3D(_Volume, uv);

	return gradient;
}
// ------------------------------------------------------------------------

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

// Added by Viola Jertschat -----------------------------------------------
float3 phong(float3 normal, float3 viewDir, float3 lightDir, float3 diffuse)
{

	// Diffuse
	float3 diffuseColor = diffuse.rgb;// float3(0.5, 1.0, 1.0);
	float diffuseIntensity = 1.0;

	float ndotl = dot(normal, lightDir);

	// no ndotl < 0 because we want everything visible

	// faking ambient
	float3 ambient = .1f * diffuseColor.rgb;

	float3 diffuseComponent = ndotl * diffuseColor.rgb + ambient;

	// Specular
	float3 specularColor = float3(1.0, 1.0, 1.0);
	float shininessPower = _Shininess; //5.0;

	float3 reflectionDir = 0.5 + reflect(-lightDir, normal);

	// max is important or everythinf back facing the light is black
	float rdotv = max(dot(reflectionDir, viewDir), 0.0);

	float3 specularComponent = specularColor.rgb  * pow(rdotv, shininessPower);

	// specular should not be visible on sourrounding box
	// by multiplying it with diffuse it gets darker/black when diffuse isn't visible either
	return diffuseComponent.rgb + specularComponent.rgb * diffuseComponent.rgb;
	//return float4(abs(normal), 1.0);
}

float3 gammaCorrection(float4 color) {
	// Gamma correction
	float gamma = _Gamma * 2;
	float3 gammaVector = float3(gamma, gamma, gamma);

	float3 rgb = pow(color.rgb, gammaVector);
	//float3 rgb = (outputColor.rgb - 0.5f) * (contrast)+0.5f;

	return rgb;
}
// ------------------------------------------------------------------------

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
  float3 stepRay = normalize(end - start) * step_size;

  float4 dst = float4(0, 0, 0, 0);
  float4 dstMask = float4(0, 0, 0, 0);
  float3 currentPoint = start;

  float3 lastUv = float3(0, 0, 0);

  [unroll]
  for (int iter = 0; iter < ITERATIONS; iter++)
  {
	// Sample Volume
    float3 uv = get_uv(currentPoint);
	lastUv = uv;
	// Get iso value
    float isoValue = sample_volume(uv, currentPoint);
    float4 src = float4(isoValue, isoValue, isoValue, isoValue);
    
	// Added by Viola Jertschat -----------------------------------------------
    if(isoValue != 0.0)
    {
		// Look up transfer function color
		//src = get_transferColor(isoValue);
		float4 transcolor = get_transferColor(isoValue);
		src.a = transcolor.a;
    } 

	float3 lightDir = 0.5 + normalize(_WorldSpaceLightPos0.xyz);//normalize((_WorldSpaceLightPos0 - i.vertex).xyz);
	float3 viewDir = 0.5 + normalize(i.local);//normalize(_WorldSpaceCameraPos - i.vertex.xyz);

	if (isoValue > 0.0)
	{
		float3 gradient = get_gradient(uv, currentPoint);
		src.rgb = phong(gradient, lightDir, viewDir, src.rgb);
	}
	// ------------------------------------------------------------------------

    src.a *= 0.5;
    src.rgb *= src.a;


    // blend
	// front to back
    dst = (1.0 - dst.a) * src + dst;

	// Added by Viola Jertschat -----------------------------------------------
	// Sample mask
	
	if (_ShowMask == 1)
	{

		float isoValueMask = sample_volume_mask(uv, currentPoint);
		float4 srcMask = float4(isoValueMask, isoValueMask, isoValueMask, isoValueMask);

		if (isoValueMask > 0.0)
		{
			float3 gradientMask = get_gradient(uv, currentPoint);
			srcMask.rgb = phong(gradientMask, lightDir, viewDir, srcMask.rgb);
		}

		srcMask.a *= 0.5;
		srcMask.rgb *= srcMask.a;

		dstMask = (1.0 - dstMask.a) * srcMask + dstMask;
	}
	// ------------------------------------------------------------------------
	
	currentPoint += stepRay;

    if (dst.a > _Threshold) break;
  }
  
  // Added by Viola Jertschat -----------------------------------------------
  dst.rgb = gammaCorrection(dst);

  if (dst.a > dstMask.a)
  {
	  return saturate(dst);
  }
  else
  {
	  if (_ShowMask == 1)
	  {
		  return saturate(dstMask) * _ColorMask + saturate(dst);
	  }
  }
  // ------------------------------------------------------------------------

  return saturate(dst);
}

#endif 
