using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterKiller : StaticTimeInteractable, DeathScript {
	private struct State : ITimeState {
		public State(Vector3 position) {
			this.position = position;
		}
		public Vector3 position;
	}

	private Transform tf;
	private bool isDead = false;

	void Start() {
		tf = GetComponent<Transform> ();
	}

	public void Kill() {
		StoreState ();

		tf.position = Vector3.down * 1000f;
		Rigidbody rb = GetComponent<Rigidbody> ();
		if (rb != null) {
			rb.Sleep ();
		}
		isDead = true;
	}

	public bool IsDead() {
		return isDead;
	}

	protected override ITimeState GetState() {
		return new State (tf.position);
	}

	protected override void SetToState(ITimeState state) {
		UnityEngine.Assertions.Assert.IsTrue (state is State);

		tf.position = ((State)state).position;

		Rigidbody rb = GetComponent<Rigidbody> ();
		if (rb != null) {
			rb.WakeUp ();
		}
		isDead = false;
	}
}
