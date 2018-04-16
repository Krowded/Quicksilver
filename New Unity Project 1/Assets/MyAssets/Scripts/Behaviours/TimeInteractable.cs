using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class TimeInteractable : MonoBehaviour {
	static public List<TimeInteractable> AllInstances;
	static TimeInteractable() {
		AllInstances = new List<TimeInteractable> (); //Maybe preallocate?
	}
	private void OnEnable() {
		AllInstances.Add (this);
	}
	private void OnDisable() {
		AllInstances.Remove(this);
	}

	//ALlowing to take item out for individual manipulation
	public void Take() {
		AllInstances.Remove(this);
	}
	public void Return() {
		AllInstances.Add(this);
	}


	//Actual class starts here
	public TimeState timeState = TimeState.Normal;

	protected int slowFactor;
	protected int rewindSpeed;
	protected int instanceTime = -1;

	//Need to do setup in awake, since we make a call to each TimeInteractable at start
	protected virtual void Awake () {
		ResetState ();
	}

	private void FixedUpdate() {		
		switch (timeState) {
			case TimeState.Normal:
				instanceTime += slowFactor;
				NormalFixedUpdate ();
				break;
			case TimeState.Slow:
				++instanceTime;
				SlowFixedUpdate ();
				break;
			case TimeState.Stop:
				StopFixedUpdate ();
				break;
			case TimeState.Rewind:
				instanceTime = Mathf.Max (0, instanceTime - rewindSpeed);
				RewindFixedUpdate ();
				break;
			default:
				throw new NotImplementedException ("Haven't implemented TimeState: " + timeState);
		}
	}

	protected virtual void NormalFixedUpdate() {}
	protected virtual void SlowFixedUpdate() {}
	protected virtual void StopFixedUpdate() {}
	protected virtual void RewindFixedUpdate() {}

	public void StartTime() {
		ResetState ();
		ProtectedStartTime ();
	}

	protected virtual void ProtectedStartTime() {}

	public void SlowTime(int slowFactor) {
		ResetState ();
		this.slowFactor = slowFactor;
		timeState = TimeState.Slow;
		ProtectedSlowTime ();
	}

	protected virtual void ProtectedSlowTime () {}

	public void StopTime() {
		ResetState ();
		timeState = TimeState.Stop;
		ProtectedStopTime ();
	}

	protected virtual void ProtectedStopTime () {}

	public void RewindTime(int rewindSpeed = 1) {
		this.rewindSpeed = rewindSpeed;
		timeState = TimeState.Rewind;
		ProtectedRewindTime ();
	}

	protected virtual void ProtectedRewindTime () {}

	public void StopRewind() {
		timeState = TimeState.Normal;
		ProtectedStopRewind ();
	}

	protected virtual void ProtectedStopRewind() {}

	protected void ResetState () {
		timeState = TimeState.Normal;
		ProtectedResetState ();
	}

	protected virtual void ProtectedResetState () {}
}

