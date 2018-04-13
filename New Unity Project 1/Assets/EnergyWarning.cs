using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnergyBar))]
[RequireComponent(typeof(Material))]
public class EnergyWarning : MonoBehaviour {

	public Color WarningColor = Color.red;

	private EnergyBar energyBar;
	private Material mat;
	private Color baseColor;

	private bool inWarningMode = false;

	// Use this for initialization
	void Start () {
		energyBar = GetComponent<EnergyBar> ();
		mat = gameObject.GetComponent<Renderer> ().material;
		if (mat == null) {
			throw new MissingReferenceException ("No material found");
		}

		baseColor = mat.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (!inWarningMode) {
			if (energyBar.PossibleEnergyRatio == 0) {
				mat.color = WarningColor;
				inWarningMode = true;
			}
		} else if (energyBar.PossibleEnergyRatio > 0) {
			mat.color = baseColor;
			inWarningMode = false;
		}
	}

	void OnApplicationQuit() {
		mat.color = baseColor;
	}
}
