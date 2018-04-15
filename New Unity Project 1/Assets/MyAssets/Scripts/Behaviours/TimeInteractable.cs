using UnityEngine;
using System.Collections;

public abstract class TimeInteractable : MonoBehaviour {
	public TimeState timeState = TimeState.Normal;

	protected int slowFactor;
	protected int rewindSpeed;
	protected int currentStateIndex = -1;

	//Need to do setup in awake, since we make a call to each TimeInteractable at start
	protected virtual void Awake () {
		ResetState ();
	}

	protected virtual void FixedUpdate() {
		if (timeState == TimeState.Rewind) {
			currentStateIndex = Mathf.Max (0, currentStateIndex - rewindSpeed);
		} else {
			if (timeState == TimeState.Slow) {
				++currentStateIndex;
			} else if (timeState == TimeState.Stop) {
				//Do nothing (not storing state)
			} else {
				currentStateIndex += slowFactor;
			}
		}
	}

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

