using System.Collections;
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
	private int timeCount = 0; //Need to keep track of time so we don't overshoot on rewind. Perhaps get it from somewhere else?

	void Start() {
		input = GetComponent<InputHandler> ();
		baseGravity = Physics.gravity;
		SlowTime (); //Start slow
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
				StopRewind ();
			}
		} else {
			if (input.timeKeyDown) {
				if (timeState == TimeState.Normal) {
					SlowTime ();
				} else {
					StartTime ();
				}

				SetState ();
			} else if (input.rewindKeyDown) {
				RewindTime ();
			}
		}
	}

	void FixedUpdate() {
		switch (timeState) {
			case TimeState.Normal:
				timeCount += currentSlowFactor;
				break;
			case TimeState.Slow:
				++timeCount;
				break;
			case TimeState.Stop:
				//Do nothing
				break;
			case TimeState.Rewind:
				if (timeCount == 0) {
					StopRewind ();
				}
				timeCount = Mathf.Max (0, timeCount - currentRewindSpeed);
				break;
			default:
				throw new MissingComponentException ("Unknown TimeState: " + timeState);
		}
	}

	public void StartTime() {
		ApplyToEveryInstance((timeInteractable) => { timeInteractable.StartTime ();});
		timeState = TimeState.Normal;
	}

	public void StopTime(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (timeInteractable) => {
			timeInteractable.StopTime ();
		});

		timeState = TimeState.Stop;
	}

	public void StopTime() {
		ApplyToEveryInstance((timeInteractable) => { timeInteractable.StopTime (); });
		timeState = TimeState.Stop;
	}

	public void SlowTime(int customSlowFactor = -1) {
		if (customSlowFactor == -1) {
			currentSlowFactor = SlowTimeFactor;
		} else {
			currentSlowFactor = customSlowFactor;
		}

		ApplyToEveryInstance ((timeInteractable) => { timeInteractable.SlowTime (currentSlowFactor); });
		timeState = TimeState.Slow;
	}

	public void RewindTime(int customRewindSpeed = -1) {
		if (customRewindSpeed == -1) {
			currentRewindSpeed = RewindSpeed;
		} else {
			currentRewindSpeed = customRewindSpeed;
		}

		ApplyToEveryInstance((timeInteractable) => { timeInteractable.RewindTime (currentRewindSpeed); });
		timeState = TimeState.Rewind;
	}

	public void StopRewind() {
		ApplyToEveryInstance ((timeInteractable) => { timeInteractable.StopRewind (); });
		SlowTime (); //Feels better if the game starts back up at slow speed, and also solves problem of not knowing our timestate. (Could possibly be stop instead)
	}

	delegate void OnEachTimeFunction(TimeInteractable timeInteractable);
	void ApplyToEveryInstance(OnEachTimeFunction f) {
		foreach (TimeInteractable ti in TimeInteractable.AllInstances) {
			f (ti);
		}
	}




	public void StartTime(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (timeInteractable) => {
			timeInteractable.StartTime ();
		});

		timeState = TimeState.Normal;
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

	public void StopRewind(Transform hierarchy) {
		TraverseAndApplyToTimeHierarchy (hierarchy, (timeInteractable) => {
			timeInteractable.StopRewind ();
		});

		SlowTime (hierarchy); //Feels better if the game starts back up at slow speed, and also solves problem of not knowing our timestate
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

	delegate void OnEachFunction(Transform tf);
	void TraverseAndApplyToHierarchy(Transform hierarchy, OnEachFunction f) {
		f (hierarchy);

		foreach (Transform child in hierarchy) {
			TraverseAndApplyToHierarchy (child, f);
		}
	}

	void TraverseAndApplyToTimeHierarchy(Transform hierarchy, OnEachTimeFunction f) {
		TraverseAndApplyToHierarchy (hierarchy, (transform) => { 
			TimeInteractable timeInteractable = transform.GetComponent<TimeInteractable> ();
			if (timeInteractable != null) {
				f(timeInteractable);
			}
		});
	}
}
