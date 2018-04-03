using System;
using UnityEngine;

public static class ColorManipulation
{
	public static Color HueCorrectedColor(Vector3 colorValues, float ratio) {
		if (ratio <= 0.5) {
			ratio = 1 - ratio;
		}
		colorValues /= ratio;
		return new Color(colorValues.x,colorValues.y,colorValues.z);
	}

	public static Color HueCorrectedColor(Vector4 colorValues, float ratio) {
		float temp = colorValues.w;
		colorValues = HueCorrectedColor ((Vector3)colorValues, ratio);
		colorValues.w = temp;
		return colorValues;
	}

	public static Color LerpColorsCorrected(Color color1, Color color2, float ratio) {
		return HueCorrectedColor (Color.Lerp (color1, color2, ratio), ratio);
	}

}

