using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class LevelEvent : MonoBehaviour {
	public string Name = "";
	public UnityEvent Effect;

	protected virtual void Start() {
		Name = this.GetType ().ToString ();
		if (Effect == null) {
			throw new MissingReferenceException ("No effect assigned to event");
		}
	}

	public void FireIfConditionMet() {
		if (ConditionMet()) {
			Effect.Invoke ();
		}
	}

	public abstract bool ConditionMet ();
}