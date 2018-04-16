#ifndef COLOR_MANIP
#define COLOR_MANIP

#include "UnityCG.cginc"

float4 HueCorrectedColor(float4 colorValues, float ratio) {
	//float temp = colorValues.w;
	ratio = max(ratio, 1 - ratio);
	colorValues.xyz /= ratio;
	//colorValues.w = temp;
	return colorValues;
}

float4 LerpColorsCorrected(float4 color1, float4 color2, float ratio) {
	return HueCorrectedColor (lerp (color1, color2, ratio), ratio);
}

#endif