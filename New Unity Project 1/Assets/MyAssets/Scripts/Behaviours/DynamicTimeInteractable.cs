using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DynamicTimeInteractable : TimeInteractable {
	protected List<ITimeState> states = new List<ITimeState>(); //FIXME: Probably wanna preallocate this
	protected int currentStateIndex = -1;

	protected override void RewindFixedUpdate() {
		currentStateIndex = Mathf.Max (0, currentStateIndex - rewindSpeed);
		RestoreLastState ();
	}
	protected override void SlowFixedUpdate() {
		StoreStates (1);
	}
	protected override void NormalFixedUpdate() {
		StoreStates (slowFactor);
	}

	private void StoreStates(int numberOfStates = 1) {
		Debug.Assert (numberOfStates > 0);

		ITimeState currentState = GetCurrentState ();
		if (currentStateIndex >= 0 && numberOfStates > 1) {
			ITimeState previousState = states [currentStateIndex];
			for (int i = 1; i < numberOfStates; ++i) {
				++currentStateIndex;
				float ratio = (float)i/(float)numberOfStates;
				ITimeState midState = GetInterpolatedState (previousState, currentState, ratio);
					//
				if (states.Count <= currentStateIndex) { 
					states.Add (midState);
				} else {
					states [currentStateIndex] = midState;
				}
			}
		}

		++currentStateIndex;
		if (states.Count <= currentStateIndex) { 
			states.Add (currentState);
		} else {
			states [currentStateIndex] = currentState;
		}
	}
		
	protected abstract ITimeState GetCurrentState ();

	protected abstract ITimeState GetInterpolatedState (ITimeState previousState, ITimeState nextState, float ratio);

	protected abstract void RestoreLastState ();
}