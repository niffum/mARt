//#ifndef __VOLUME_RENDERING_INCLUDED__
//#define __VOLUME_RENDERING_INCLUDED__

#include "UnityCG.cginc"


sampler2D _TransferColor;
float4 _TransferColor_ST;

fixed4 get_transferColor(float2 isovalue)
{
  //float2 xy = float2(isovalue, 0);

  return  tex2D(_TransferColor, isovalue);
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
  //float3 world : TEXCOORD1;
  //float3 local : TEXCOORD2;
};

v2f vert(appdata v)
{
  v2f o;
  o.vertex = UnityObjectToClipPos(v.vertex);
  o.uv = v.uv;
  //o.world = mul(unity_ObjectToWorld, v.vertex).xyz;
  //o.local = v.vertex.xyz;
  return o;
}

fixed4 frag(v2f i) : SV_Target
{
  //return get_transferColor(i.uv);
  //return float4(i.uv.x,0.0,0.0,1.0);
  fixed4 col = tex2D(_TransferColor, i.uv);
  return col;
}

//#endif 
