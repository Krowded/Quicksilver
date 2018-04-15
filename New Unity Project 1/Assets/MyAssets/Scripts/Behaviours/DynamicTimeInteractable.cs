using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DynamicTimeInteractable : TimeInteractable {
	protected List<ITimeState> states = new List<ITimeState>(); //FIXME: Probably wanna preallocate this

	protected override sealed void FixedUpdate() {
		switch (timeState) {
			case TimeState.Rewind:
				RewindFixedUpdate ();
				break;
			case TimeState.Slow:
				StoreStates ();
				break;
			case TimeState.Stop:
				//Do nothing
				break;
			case TimeState.Normal:
				StoreStates (slowFactor);
				break;
			default:
				throw new NotImplementedException ("Unknown TimeState: " + timeState);
				//break;
		}
	}

	private void RewindFixedUpdate() {
		if (currentStateIndex >= 0) {
			RestoreLastState ();
		} else {
			throw new IndexOutOfRangeException ("Overshot rewind. Current state index: " + currentStateIndex);
		}
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

	protected abstract ITimeState GetInterpolatedState (ITimeState previousState, ITimeState currentState, float ratio);

	protected abstract void RestoreLastState ();
}