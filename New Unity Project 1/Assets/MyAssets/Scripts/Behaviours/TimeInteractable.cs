using UnityEngine;
using System.Collections;
using System;

public abstract class TimeInteractable : MonoBehaviour {
	public TimeState timeState = TimeState.Normal;

	protected int slowFactor;
	protected int rewindSpeed;
	protected int globalTimeStamp = -1;

	//Need to do setup in awake, since we make a call to each TimeInteractable at start
	protected virtual void Awake () {
		ResetState ();
	}

	private void FixedUpdate() {		
		switch (timeState) {
			case TimeState.Normal:
				globalTimeStamp += slowFactor;
				NormalFixedUpdate ();
				break;
			case TimeState.Slow:
				++globalTimeStamp;
				SlowFixedUpdate ();
				break;
			case TimeState.Stop:
				StopFixedUpdate ();
				break;
			case TimeState.Rewind:
				globalTimeStamp = Mathf.Max (0, globalTimeStamp - rewindSpeed);
				RewindFixedUpdate ();
				break;
			default:
				throw new NotImplementedException ("Unknown TimeState: " + timeState);
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

