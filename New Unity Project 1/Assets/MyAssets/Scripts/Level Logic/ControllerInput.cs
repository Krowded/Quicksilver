using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class ControllerInput : MonoBehaviour {
	public Transform Entities;

	private InputHandler input;
	private bool timeToggle = false;
	private Vector3 baseGravity;

	void Start() {
		input = GetComponent<InputHandler> ();
		baseGravity = Physics.gravity;
	}

	void Update () {
		if (input.timeKeyDown) {
			if (timeToggle) {
				Physics.gravity = 0.2f * baseGravity;
				SlowTime (Entities);
				Debug.Log ("Time Slowed");
			} else {
				Physics.gravity = baseGravity;
				StartTime (Entities);
				Debug.Log ("Time Started");
			}

			timeToggle = !timeToggle;
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
