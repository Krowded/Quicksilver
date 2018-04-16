using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class TimeController : MonoBehaviour {
	public Transform Hierarchy;
	public int SlowTimeFactor = 5;
	public int RewindSpeed = 1;
	public int MaxSlowDownFactor = 5;

	private int currentSlowFactor;
	private int currentRewindSpeed;

	private InputHandler input;

	private Vector3 baseGravity;
	public TimeState timeState;
	private int frameCount = 0; //Need to keep track of frame count so we don't overshoot on rewind

	void Start() {
		input = GetComponent<InputHandler> ();
		baseGravity = Physics.gravity;
		SlowTime (Hierarchy); //Start slow
	}

	void SetState() {
		if (timeState == TimeState.Normal) {
			Physics.gravity = baseGravity;
		} else if (timeState == TimeState.Slow) {
			Physics.gravity = (1f/(float)currentSlowFactor) * baseGravity;
		}
	}

	void Update () {
		if (timeState == TimeState.Rewind) {
			if (input.rewindKeyDown) {
				StopRewind (Hierarchy);
			}
		} else {
			if (input.timeKeyDown) {
				if (timeState == TimeState.Normal) {
					SlowTime (Hierarchy);
				} else {
					StartTime (Hierarchy);
				}
					
				SetState ();
			} else if (input.rewindKeyDown) {
				RewindTime (Hierarchy);
			}
		}
	}

	void FixedUpdate() {
		if (timeState == TimeState.Rewind) {
			if (frameCount == 0) {
				StopRewind (Hierarchy);
			}
			frameCount = Mathf.Max(0, frameCount-currentRewindSpeed);
		} else if (timeState == TimeState.Normal) {
			frameCount += currentSlowFactor;
		} else {
			++frameCount;
		}
	}

	//Search through hierarchy (probably better to have a list, but it'll do for now)
	public void StartTime(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (timeInteractable) => {
			timeInteractable.StartTime ();
		});

		timeState = TimeState.Normal;
	}

	public void StopTime(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (timeInteractable) => {
			timeInteractable.StopTime ();
		});

		timeState = TimeState.Stop;
	}

	public void SlowTime(Transform hierarchy, int customSlowFactor = -1) {
		if (customSlowFactor == -1) {
			currentSlowFactor = SlowTimeFactor;
		} else {
			currentSlowFactor = customSlowFactor;
		}

		TraverseAndApplyToTimeHierarchy (hierarchy, (timeInteractable) => {
			timeInteractable.SlowTime (currentSlowFactor);
		});

		timeState = TimeState.Slow;
	}

	public void RewindTime(Transform hierarchy, int customRewindSpeed = -1) {
		if (customRewindSpeed == -1) {
			currentRewindSpeed = RewindSpeed;
		} else {
			currentRewindSpeed = customRewindSpeed;
		}

		TraverseAndApplyToTimeHierarchy (hierarchy, (timeInteractable) => {
			timeInteractable.RewindTime (currentRewindSpeed);
		});

		timeState = TimeState.Rewind;
	}

	public void StopRewind(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (timeInteractable) => {
			timeInteractable.StopRewind ();
		});

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
