using UnityEngine;
using System.Collections;

public class OnTimeOut : LevelEvent {
	public Timer timer;

	public override bool ConditionMet() {
		return timer.IsTimeout ();
	}
}

