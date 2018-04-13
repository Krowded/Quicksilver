using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class TimeController : MonoBehaviour {
	public Transform Hierarchy;
	public int SlowTimeFactor = 5;
	private InputHandler input;
	//private GlobalTimeKeeper timeKeeper;

	private Vector3 baseGravity;
	private bool timeIsNormal = false;
	private bool rewinding = false;
	private int frameCount = 0; //Need to keep track of frame count so we don't overshoot on rewind

	void Start() {
		input = GetComponent<InputHandler> ();
		baseGravity = Physics.gravity;
		SlowTime (Hierarchy); //Start slow
	}

	void SetState() {
		if (timeIsNormal) {
			Physics.gravity = baseGravity;
		} else {
			Physics.gravity = (1f/(float)SlowTimeFactor) * baseGravity;
		}
	}

	void Update () {
		if (rewinding) {
			if (input.rewindKeyDown) {
				StopRewind (Hierarchy);
			}
		} else {
			if (input.timeKeyDown) {
				if (timeIsNormal) {
					SlowTime (Hierarchy);
				} else {
					StartTime (Hierarchy);
				}

				timeIsNormal = !timeIsNormal;
				SetState ();
			} else if (input.rewindKeyDown) {
				RewindTime (Hierarchy);
			}
		}
	}

	void FixedUpdate() {
		if (rewinding) {
			if (frameCount < 0) {
				StopRewind (Hierarchy);
			}
			--frameCount;
		} else if (timeIsNormal) {
			frameCount += SlowTimeFactor;
		} else {
			++frameCount;
		}
	}

	//Search through hierarchy (probably better to have a list, but it'll do for now)
	public void StartTime(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (timeInteractable) => {
			timeInteractable.StartTime ();
		});
	}

	public void SlowTime(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (timeInteractable) => {
			timeInteractable.SlowTime (SlowTimeFactor);
		});
	}

	public void RewindTime(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (timeInteractable) => {
			timeInteractable.RewindTime ();
		});

		rewinding = true;
	}

	public void StopRewind(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (timeInteractable) => {
			timeInteractable.StopRewind ();
		});

		rewinding = false;

		SlowTime (hierarchy); //Feels better if the game starts back up at slow speed, and also solves problem of not knowing our timestate
	}

	delegate void OnEachFunction(Transform tf);
	void TraverseAndApplyToHierarchy(Transform hierarchy, OnEachFunction f) {
		f (hierarchy);

		foreach (Transform child in hierarchy) {
			TraverseAndApplyToHierarchy(child, f);
		}
	}

	delegate void OnEachTimeFunction(TimeInteractable timeInteractable);
	void TraverseAndApplyToTimeHierarchy(Transform hierarchy, OnEachTimeFunction f) {
		TraverseAndApplyToHierarchy (hierarchy, (transform) => { 
			TimeInteractable timeInteractable = transform.GetComponent<TimeInteractable> ();
			if (timeInteractable != null) {
				f(timeInteractable);
			}
		});
	}
}
