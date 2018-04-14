using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class StaticTimeInteractable : TimeInteractable {

	private struct State : ITimeState {
	}

	private List<ITimeState> states = new List<ITimeState>();
	private List<int> timeStamps = new List<int>();
	private int stateIndex = -1;

	public void StoreState() {
		++stateIndex;
		ITimeState temp = GetState ();
		if (states.Count <= stateIndex) {
			states.Add (temp);
			timeStamps.Add (currentStateIndex);
		} else {
			states [stateIndex] = temp;
			timeStamps [stateIndex] = currentStateIndex;
		}
	}

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		if (timeState == TimeState.Rewind) {
			if (stateIndex > -1 && currentStateIndex <= timeStamps [stateIndex]) {
				RestoreState ();
			}
		}
	}

	private void RestoreState() {
		SetToState (states[stateIndex]);
		--stateIndex;
	}

	protected abstract ITimeState GetState ();

	protected abstract void SetToState (ITimeState s);
}

