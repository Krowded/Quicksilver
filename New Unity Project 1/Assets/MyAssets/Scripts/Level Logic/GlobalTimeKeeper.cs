/*
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GlobalTimeKeeper : TimeInteractable
{
	public TimeState currentTimeState {
		get {
			return states[currentStateIndex];
		}
	}
	List<TimeState> states;

	protected override void Awake () {
		GetComponent<Rigidbody> ().isKinematic = true;
		GetComponent<Rigidbody> ().useGravity = false;
		timeState = TimeState.Normal;

		states = new List<TimeState> ();
	}

	public override void StartTime() {
		timeState = TimeState.Normal;
	}

	public override void SlowTime(int ignoredValue) {
		timeState = TimeState.Slow;
	}

	public override void StopTime() {
		timeState = TimeState.Stop;
	}

	public override void RewindTime() {
		timeState = TimeState.Rewind;
	}

	public override void StopRewind() {
		timeState = states[currentStateIndex];
	}

	protected override void FixedUpdate() {
		if (timeState == TimeState.Rewind) {
			RewindFixedUpdate ();
		} else {
			StoreState ();
		}
	}

	private void RewindFixedUpdate() {
		if (currentStateIndex >= 0) {
			RestoreLastState ();
		} else {
			throw new IndexOutOfRangeException ("Overshot rewind. Current state index: " + currentStateIndex);
		}
	}

	private void StoreState() {
		++currentStateIndex;
		if (states.Count <= currentStateIndex) {
			states.Add (timeState);
		} else {
			states [currentStateIndex] = timeState;
		}
	}

	private void RestoreLastState() {
		--currentStateIndex;
	}
}
*/