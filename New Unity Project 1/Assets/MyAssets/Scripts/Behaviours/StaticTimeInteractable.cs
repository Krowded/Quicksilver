using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class StaticTimeInteractable : TimeInteractable {
	private List<ITimeState> states = new List<ITimeState>();
	private List<int> timeStamps = new List<int>();
	private int stateIndex = -1;

	protected override void RewindFixedUpdate() {
		if (stateIndex > -1 && globalTimeStamp <= timeStamps [stateIndex]) {
			RestoreState ();
		}
	}

	public void StoreState() {
		++stateIndex;
		ITimeState temp = GetState ();
		if (states.Count <= stateIndex) {
			states.Add (temp);
			timeStamps.Add (globalTimeStamp);
		} else {
			states [stateIndex] = temp;
			timeStamps [stateIndex] = globalTimeStamp;
		}
	}

	private void RestoreState() {
		SetToState (states[stateIndex]);
		--stateIndex;
	}

	protected abstract ITimeState GetState ();

	protected abstract void SetToState (ITimeState s);
}

