using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class TimeController : MonoBehaviour {
	public Transform Hierarchy;
	public float SlowTimeScale = 0.2f;
	private InputHandler input;
	private GlobalTimeKeeper timeKeeper;

	private Vector3 baseGravity;
	private bool timeIsNormal = false;
	private bool rewinding = false;
	private int frameCount = 0; //Need to keep track of frame count so we don't overshoot on rewind

	void Start() {
		input = GetComponent<InputHandler> ();
		timeKeeper = Hierarchy.GetComponent<GlobalTimeKeeper> ();
		if (timeKeeper == null) {
			throw new UnityException ("Hierarchy missing GlobalTimeKeeper");
		}

		baseGravity = Physics.gravity;
	}

	void SetState() {
		if (timeIsNormal) {
			Physics.gravity = baseGravity;
		} else {
			Physics.gravity = SlowTimeScale * baseGravity;
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
					Debug.Log ("Time Slowed");
				} else {
					StartTime (Hierarchy);
					Debug.Log ("Time Started");
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
			--frameCount;
			if (frameCount <= 0) {
				StopRewind (Hierarchy);
			}
		} else {
			++frameCount;
		}
	}

	//Search through hierarchy (probably better to have a list, but it'll do for now)
	public void StartTime(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (ti) => {
			ti.StartTime ();
		});
	}

	public void SlowTime(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (ti) => {
			ti.SlowTime ();
		});
	}

	public void RewindTime(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (ti) => {
			ti.RewindTime ();
		});

		rewinding = true;
	}

	public void StopRewind(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (ti) => {
			ti.StopRewind ();
		});

		rewinding = false;



		/*
		//Find a timestate and steal it.. //FIXME: Either change how we do this or at least make it more clear that this happens
		timeIsNormal = (timeKeeper.currentTimeState == TimeInteractable.TimeState.Normal);
		*/

		SlowTime (hierarchy); //Feels better if the game starts back up at slow speed, and also solves problem of not knowing our timestate
	}

	delegate void OnEachFunction(Transform tf);
	void TraverseAndApplyToHierarchy(Transform hierarchy, OnEachFunction f) {
		f (hierarchy);

		foreach (Transform child in hierarchy) {
			TraverseAndApplyToHierarchy(child, f);
		}
	}

	delegate void OnEachTimeFunction(TimeInteractable ti);
	void TraverseAndApplyToTimeHierarchy(Transform hierarchy, OnEachTimeFunction f) {
		TraverseAndApplyToHierarchy (hierarchy, (x) => { 
			TimeInteractable ti = x.GetComponent<TimeInteractable> ();
			if (ti != null) {
				f(ti);
			}
		});
	}
}
