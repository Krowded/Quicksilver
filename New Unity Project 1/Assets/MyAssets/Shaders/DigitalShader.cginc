#include "UnityCG.cginc"

uniform float4 _Color;
uniform float _XLimit;
uniform float _YLimit;

struct appdataDigital
{
	float4 vertex : POSITION;
};

struct v2fDigital
{
	float4 vertex : POSITION;
	float4 color : COLOR;
};


v2fDigital vertDigital (appdataDigital v)
{
	v2fDigital o;
	o.vertex = UnityObjectToClipPos(v.vertex);

	o.color = _Color;
	o.color.w = (frac(_Time.z*v.vertex.x) < _XLimit) || (frac(_Time.z*v.vertex.y) < _YLimit);

	return o;
}

fixed4 fragDigital (v2fDigital i) : SV_Target
{
	i.color.w *= (i.color.w >= 0.95) ? 1 : 0.4 * i.color.w;
	return i.color;
}