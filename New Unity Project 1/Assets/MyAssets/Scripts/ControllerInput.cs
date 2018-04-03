using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class ControllerInput : MonoBehaviour {
	public Transform Entities;
	private bool ToggledOn = false;

	private InputHandler input;

	public Camera cam1;
	public Camera cam2;

	void Start() {
		input = GetComponent<InputHandler> ();
	}

	void Update () {
		if (input.timeKeyDown) {
			Debug.Log ("Hit F. Toggle: " + ToggledOn);
			if (ToggledOn) {
				StopTime (Entities);
//				cam1.enabled = !cam1.enabled;
//				cam2.enabled = !cam2.enabled;
			} else {
				StartTime (Entities);
//				cam1.enabled = !cam1.enabled;
//				cam2.enabled = !cam2.enabled;
			}

			ToggledOn = !ToggledOn;
			Debug.Log ("Toggle: " + ToggledOn);
		}
	}

	void StartTime(Transform Entity) {
		TimeInteractor st = Entity.GetComponent<TimeInteractor> ();
		if (st != null) {
			st.StartTime();	
		}

		foreach (Transform child in Entity.transform) {
			StartTime (child);
		}
	}

	void StopTime(Transform Entity) {
		TimeInteractor st = Entity.GetComponent<TimeInteractor> ();
		if (st != null) {
			st.StopTime();	
		}

		foreach (Transform child in Entity.transform) {
			StopTime (child);
		}
	}
}
