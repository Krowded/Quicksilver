using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TimeInteractable : MonoBehaviour {

	public Vector3 startVelocity;

	[Range(0,1)]
	public float NormalSpeed = 1;
	[Range(0,1)]
	public float SlowSpeed = 0.2f;

	private Transform tf;
	private Rigidbody rb;
	private bool baseKinematicState;
	private bool baseGravityState;

	private bool isSlow = false;
	private bool isStopped = false;

	private Vector3 storedVelocity = Vector3.zero;
	private Vector3 storedAngularVelocity = Vector3.zero;
	private Vector3 previousVelocity;
	private Vector3 previousAngularVelocity;

	private struct FrameState {
		public FrameState(Transform tf, Rigidbody rb) {
			this.position = tf.position;
			this.rotation = tf.rotation;
			this.velocity = rb.velocity;
			this.angularVelocity = rb.angularVelocity;
		}
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 velocity;
		public Vector3 angularVelocity;
	}

	private List<FrameState> states = new List<FrameState>();
	private int currentStateIndex = -1;
	private bool rewind = false;
	private int targetStateIndex = -1;

	protected virtual void Start () {
		tf = gameObject.transform;
		rb = GetComponent<Rigidbody> ();

		baseKinematicState = rb.isKinematic;
		baseGravityState = rb.useGravity;

		//rb.velocity = startVelocity; //TODO: Remove this?

		previousVelocity = rb.velocity;
		previousAngularVelocity = rb.angularVelocity;
	}

	protected virtual void Update() {}

	private void RewindFixedUpdate() {
		if (currentStateIndex > targetStateIndex) {
			tf.position = states [currentStateIndex].position;
			tf.rotation = states [currentStateIndex].rotation;
			rb.velocity = states [currentStateIndex].velocity;
			rb.angularVelocity = states [currentStateIndex].angularVelocity;
			--currentStateIndex;
		} else {
			rewind = false;
			SlowTime ();
		}
	}

	private void StoreForce(float scale) {
		Vector3 velocityToStore = (1-scale) * (rb.velocity - previousVelocity);
		storedVelocity += velocityToStore;
		rb.velocity -= velocityToStore;
		previousVelocity = rb.velocity;

		Vector3 angularVelocityToStore =  (1 - scale) * rb.angularVelocity - previousAngularVelocity;
		storedAngularVelocity += angularVelocityToStore;
		rb.angularVelocity -= angularVelocityToStore;
		previousAngularVelocity = rb.angularVelocity;
	}

	private void RestoreForce() {
		if (isSlow && Vector3.Magnitude(rb.velocity) < 0.000001f) {
			return;
		}

		rb.velocity += storedVelocity;
		storedVelocity = Vector3.zero;
		rb.angularVelocity = storedAngularVelocity;
		storedAngularVelocity = Vector3.zero;
	}

	protected virtual void FixedUpdate() {
		if (rewind) {
			RewindFixedUpdate ();
		} else {
			//Update state
			if (isSlow) {
				StoreForce (SlowSpeed);
			} else if (isStopped) {
				//Do nothing? Don't store state?
			}

			//Save state
			++currentStateIndex;
			FrameState currentState = new FrameState (tf, rb);
			if (states.Count <= currentStateIndex) { 
				states.Add (currentState);
			} else {
				states [currentStateIndex] = currentState;
			}

		}
	}
		
	private void ResetNaturalState () {
		isStopped = false;
		isSlow = false;
		rb.isKinematic = baseKinematicState;
		rb.useGravity = baseGravityState;
		RestoreForce ();
	}

	public virtual void StartTime() {
		ResetNaturalState ();
	}

	public virtual void SlowTime() {
		ResetNaturalState ();
		StoreForce (SlowSpeed);
		isSlow = true;
	}

	public virtual void StopTime() {
		ResetNaturalState ();
		StoreForce (0);

		//Possible bugs here. We change this directly both here and in interactor. Combine the effects somehow?
		rb.isKinematic = true;
		rb.useGravity = false;

		isStopped = true;
	}

	public virtual void RewindTime(int ticks) {
		rewind = true;
		targetStateIndex = Mathf.Max (-1, currentStateIndex - ticks);
	}
}