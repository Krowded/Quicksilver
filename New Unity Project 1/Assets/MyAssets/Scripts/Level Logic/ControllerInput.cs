using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class ControllerInput : MonoBehaviour {
	public Transform Entities;
	private bool ToggledOn = false;

	private InputHandler input;

	void Start() {
		input = GetComponent<InputHandler> ();
	}

	void Update () {
		if (input.timeKeyDown) {
			if (ToggledOn) {
				SlowTime (Entities);
				Debug.Log ("Time Slowed");
			} else {
				StartTime (Entities);
				Debug.Log ("Time Started");
			}

			ToggledOn = !ToggledOn;
		}
	}

	//Search through hierarchy (probably better to have a list, but it'll do for now)
	void StartTime(Transform Entity) {
		TimeInteractable ti = Entity.GetComponent<TimeInteractable> ();
		if (ti != null) {
			ti.StartTime();	
		}

		foreach (Transform child in Entity.transform) {
			StartTime (child);
		}
	}

	void SlowTime(Transform Entity) {
		TimeInteractable ti = Entity.GetComponent<TimeInteractable> ();
		if (ti != null) {
			ti.SlowTime();	
		}

		foreach (Transform child in Entity.transform) {
			SlowTime (child);
		}
	}
}
