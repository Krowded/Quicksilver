using UnityEngine;

public class Timer : TimeInteractable {
	public float TimeUntilTimeout = 10;

	public bool IsTimeout() {
		return globalTimeStamp > TimeUntilTimeout;
	}
}
