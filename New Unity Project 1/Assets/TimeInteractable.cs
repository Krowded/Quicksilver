using UnityEngine;
using System.Collections;

public class TimeInteractable : MonoBehaviour {
	public TimeState timeState = TimeState.Slow;

	protected int slowFactor;
	protected int rewindSpeed;
	protected int currentStateIndex = -1;

	//Need to do setup in awake, since we make a call to each TimeInteractable at start
	protected virtual void Awake () {}

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

	public virtual void StartTime() {
		timeState = TimeState.Normal;
	}

	public virtual void SlowTime(int slowFactor) {
		this.slowFactor = slowFactor;
		timeState = TimeState.Slow;
	}

	public virtual void StopTime() {
		timeState = TimeState.Stop;
	}

	public virtual void RewindTime(int rewindSpeed = 1) {
		this.rewindSpeed = rewindSpeed;
		timeState = TimeState.Rewind;
	}

	public virtual void StopRewind() {
		timeState = TimeState.Normal;
	}
}

