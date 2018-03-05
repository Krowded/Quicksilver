using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour {
	public Transform Entities;
	private bool ToggledOn = false;

	void Update () {
		if (Input.GetKeyDown(KeyCode.F)) {
			Debug.Log ("Hit F. Toggle: " + ToggledOn);
			if (ToggledOn) {
				StopTime (Entities);
			} else {
				StartTime (Entities);
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
