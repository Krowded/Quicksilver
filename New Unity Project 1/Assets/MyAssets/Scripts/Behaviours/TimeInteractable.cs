using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TimeInteractable : MonoBehaviour {

	public enum TimeState { Normal, Slow, Stop, Rewind };

	//Local space start values
	public Vector3 StartVelocity;
	public Vector3 StartAngularVelocity;

	[Range(0,1)]
	public float NormalSpeed = 1;
	[Range(0,1)]
	public float SlowSpeed = 0.2f;

	private Transform tf;
	private Rigidbody rb;
	private bool baseKinematicState;
	private bool baseGravityState;

	public TimeState timeState = TimeState.Slow;

	private float storedVelocity = 0;
	private float storedAngularVelocity = 0;
	private float previousVelocity;
	private float previousAngularVelocity;

	private struct FrameState {
		public FrameState(Transform tf, Rigidbody rb, TimeState ts, float storedVelocity, float storedAngularVelocity) {
			this.position = tf.position;
			this.rotation = tf.rotation;
			this.velocity = rb.velocity.magnitude;
			this.velocityDirection = rb.velocity.normalized;
			this.angularVelocity = rb.angularVelocity.magnitude;
			this.angularVelocityDirection = rb.angularVelocity.normalized;

			this.storedVelocity = storedVelocity;
			this.storedAngularVelocity = storedAngularVelocity;
			this.timeState = ts;
		}
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 velocityDirection;
		public float velocity;
		public float angularVelocity;
		public Vector3 angularVelocityDirection;

		public float storedVelocity;
		public float storedAngularVelocity;
		public TimeState timeState;
	}

	private List<FrameState> states = new List<FrameState>();
	protected int currentStateIndex = -1;


	protected virtual void Start () {
		tf = gameObject.transform;
		rb = GetComponent<Rigidbody> ();

		baseKinematicState = rb.isKinematic;
		baseGravityState = rb.useGravity;

		rb.velocity = StartVelocity;
		rb.angularVelocity = StartAngularVelocity;

		previousVelocity = rb.velocity.magnitude;
		previousAngularVelocity = rb.angularVelocity.magnitude;

		SlowTime ();
	}

	protected virtual void Update() {}

	public virtual void StartTime() {
		ResetState ();
	}

	public virtual void SlowTime() {
		ResetState ();
		StoreVelocity (SlowSpeed);
		timeState = TimeState.Slow;
	}

	public virtual void StopTime() {
		ResetState ();
		StoreVelocity(0);

		//FIXME: Possible bugs here. We change this directly both here and in interactor. Combine the effects somehow?
		rb.isKinematic = true;
		rb.useGravity = false;

		timeState = TimeState.Stop;
	}

	public virtual void RewindTime() {
		timeState = TimeState.Rewind;
	}

	public virtual void StopRewind() {
		timeState = states[currentStateIndex].timeState;
	}

	private void StoreRelativeVelocity(float scale) {
		//This is done a lot, and does a lot of normalization. If performance problem: switch to squared normalization

		float inverseScale = 1f - scale;
		float velocityToStore = inverseScale * (rb.velocity.magnitude - previousVelocity);
		storedVelocity += velocityToStore;
		rb.velocity -= velocityToStore*rb.velocity.normalized;
		previousVelocity = rb.velocity.magnitude;

		float angularVelocityToStore =  inverseScale * (rb.angularVelocity.magnitude - previousAngularVelocity);
		storedAngularVelocity += angularVelocityToStore;
		rb.angularVelocity -= angularVelocityToStore * rb.angularVelocity.normalized;
		previousAngularVelocity = rb.angularVelocity.magnitude;
	}

	//TODO: Find a better name for this. Confusing
	private void StoreVelocity(float scale) {
		float inverseScale = 1f - scale;
		storedVelocity += inverseScale * rb.velocity.magnitude;
		rb.velocity *= scale;
		previousVelocity = rb.velocity.magnitude;

		storedAngularVelocity += inverseScale * rb.angularVelocity.magnitude;
		rb.angularVelocity *= scale;
		previousAngularVelocity = rb.angularVelocity.magnitude;
	}

	private void RestoreVelocity() {
		rb.velocity += storedVelocity*rb.velocity.normalized;
		storedVelocity = 0;
		rb.angularVelocity = storedAngularVelocity * rb.angularVelocity.normalized;
		storedAngularVelocity = 0;
	}

	protected virtual void FixedUpdate() {
		if (timeState == TimeState.Rewind) {
			RewindFixedUpdate ();
		} else {
			//Update state
			if (timeState == TimeState.Slow) {
				StoreRelativeVelocity (SlowSpeed);
			} else if (timeState == TimeState.Stop) {
				//Do nothing? Don't store state?
			}

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
		FrameState currentState = new FrameState (tf, rb, timeState, storedVelocity, storedAngularVelocity);
		if (states.Count <= currentStateIndex) { 
			states.Add (currentState);
		} else {
			states [currentStateIndex] = currentState;
		}
	}

	private void RestoreLastState() {
		tf.position = states [currentStateIndex].position;
		tf.rotation = states [currentStateIndex].rotation;
		rb.velocity = states [currentStateIndex].velocity * states [currentStateIndex].velocityDirection;
		rb.angularVelocity = states [currentStateIndex].angularVelocity * states [currentStateIndex].angularVelocityDirection;
		storedVelocity = states [currentStateIndex].storedVelocity;
		storedAngularVelocity = states [currentStateIndex].storedAngularVelocity;
		//No timeState restoration, since we are in rewind mode. Set at end of rewind.
		--currentStateIndex;
	}
		
	private void ResetState () {
		timeState = TimeState.Normal;
		rb.isKinematic = baseKinematicState;
		rb.useGravity = baseGravityState;
		RestoreVelocity ();
	}
}