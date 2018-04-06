using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatingEnergyBar : MonoBehaviour {

	public float Distance = 1;
	public float Speed = 0.1f;
	public float MovementSpeed = 0.0000001f;
	public int Direction = 1;
	private float current;

	public EnergyBar energyBar;
	public Transform Bar;
	public Transform Shadow;

	private float length;

	void Start () {
		current = Distance;
		length = Bar.localScale.y;
	}

	void Update () {

		Bar.GetComponent<Renderer> ().material.SetColor("_OutlineColor", energyBar.InterpolatedPossibleColor);
		Bar.localScale = new Vector3 (Bar.localScale.x, energyBar.PossibleEnergyRatio*length, Bar.localScale.z);
		//Shadow.localScale = Bar.localScale;
		if (current >= 0) {
			current -= Speed * Time.deltaTime;
			Bar.position += new Vector3 (0, MovementSpeed * Direction, 0);
			//Shadow.position += new Vector3 (-MovementSpeed * Direction, 0, 0);
		} else {
			Direction = -Direction;
			current = Distance;
		}
	}
}
