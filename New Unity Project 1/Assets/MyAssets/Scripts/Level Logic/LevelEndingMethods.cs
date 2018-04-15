﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeController))]
public class LevelEndingMethods : MonoBehaviour {

	private TimeController tc;

	void Start() {
		tc = GetComponent<TimeController> ();
	}

	public void Win() {
		Debug.Log ("You win!");
		Destroy (this); //FIXME: Obviously don't keep this...
	}
		
	public void Lose() {
		if (tc.timeState == TimeState.Rewind || tc.timeState == TimeState.Stop) {
			return;
		}

		tc.StopTime (gameObject.transform);
		StartCoroutine(WaitThenRewindToBeginning(gameObject.transform, timeInSeconds: 0.3f));
	}

	private IEnumerator WaitThenRewindToBeginning(Transform hierarchy, float timeInSeconds) {
		yield return new WaitForSecondsRealtime (timeInSeconds);
		tc.StartTime (hierarchy);
		tc.RewindTime (hierarchy, customRewindSpeed: 20);
	}
}