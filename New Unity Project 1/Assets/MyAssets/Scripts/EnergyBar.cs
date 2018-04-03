using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour {

	public InputHandler input;

	public float Y = 25f;
	public float X = 25f;
	public float Height = 15f;
	public float MaxWidth = 200f;
	public float MaxEnergy = 100f;
	public float CurrentEnergy = 100f;

	public float RegenerationRate = 0.1f;

	public float PossibleCost = 0f;

	public Color EmptyColor;
	public Color FullColor;
	public Color PossibleCostColor;
	public Color WarningColor;
	public Color LineFullColor;
	public Color LineEmptyColor;


	private bool timeIsGoingFast = false;

	private GUIStyle style;
	private Texture2D pixel;

	private GUIStyle possibleStyle;
	private Texture2D pixel2;

	private GUIStyle warningStyle;

	public Color LineColor {
		get {
			return ColorManipulation.LerpColorsCorrected(LineEmptyColor, LineFullColor, PossibleEnergyRatio);
		}
	}

	public Color InterpolatedColor {
		get
		{
			return ColorManipulation.LerpColorsCorrected(EmptyColor, FullColor, CurrentEnergyRatio);
		}
	}

	public Color InterpolatedPossibleColor {
		get
		{
			return ColorManipulation.LerpColorsCorrected (EmptyColor, FullColor, PossibleEnergyRatio);
		}
	}
		
	public float PossibleEnergyRatio {
		get {
			return Mathf.Max (0, CurrentEnergy - PossibleCost)/(float)MaxEnergy;
		}
	}

	public float CurrentEnergyRatio {
		get {
			return Mathf.Max (0, (float)CurrentEnergy/(float)MaxEnergy);
		}
	}

	void Start () {
		if (input == null) {
			throw new UnassignedReferenceException ("Missing InputHandler");
		}

		//
		pixel = new Texture2D (1, 1);
		pixel.wrapMode = TextureWrapMode.Repeat;
		pixel.Apply();

		style = new GUIStyle ();
		style.normal.background = pixel;

		//
		pixel2 = new Texture2D (1, 1);
		pixel2.wrapMode = TextureWrapMode.Repeat;
		pixel2.SetPixel (0, 0, PossibleCostColor);
		pixel2.Apply ();

		possibleStyle = new GUIStyle ();
		possibleStyle.normal.background = pixel2;

		warningStyle = new GUIStyle ();
		warningStyle.normal.textColor = WarningColor;
		warningStyle.fontStyle = FontStyle.Bold;
	}

	void Update() {
		if (input.timeKeyDown) {
			timeIsGoingFast = !timeIsGoingFast;
		}

		if (timeIsGoingFast) {
			CurrentEnergy = Mathf.Min(MaxEnergy, CurrentEnergy+(RegenerationRate * Time.deltaTime));
		}
	}

	void OnGUI() {
		float width = MaxWidth * ((float)CurrentEnergy/(float)MaxEnergy);
		pixel.SetPixel (0, 0, InterpolatedColor);
		pixel.Apply ();

		GUI.Label(new Rect(X,Y+Height,width,Height),"", style);

		if (PossibleCost > 0.01f) {
			float poswidth = Mathf.Min(width, MaxWidth*((float)PossibleCost/(float)MaxEnergy));
			GUI.Label (new Rect(X+width-poswidth, Y+Height, poswidth, Height), "", possibleStyle);
			if (!HaveEnoughEnergy ()) {
				DisplayWarning ();
			}
		}
	}

	void DisplayWarning() {
		GUI.Label (new Rect(X+10, Y+Height+5, 1000, 1000), "OVER ENERGY LIMIT!", warningStyle);
	}

	public void ApplyCost() {
		CurrentEnergy -= PossibleCost;
	}

	public bool HaveEnoughEnergy() {
		return PossibleCost <= CurrentEnergy;
	}
}
