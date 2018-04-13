using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class TimeInteractable : MonoBehaviour {
	private RewindDrawer rewindDrawer;

	//Local space start values
	public Vector3 StartVelocity;
	public Vector3 StartAngularVelocity;

	private int slowFactor;
	private int rewindSpeed;

	private Transform tf;
	private Rigidbody rb;
	private bool baseKinematicState;
	private bool baseGravityState;

	public TimeState timeState = TimeState.Slow;

	private float storedVelocity = 0;
	private float storedAngularVelocity = 0;
	private float previousVelocity;
	private float previousAngularVelocity;

	public struct FrameState {
		public FrameState(Transform tf, Rigidbody rb, TimeState ts, float storedVelocity, float storedAngularVelocity) {
			this.position = tf.position;
			this.rotation = tf.rotation;
			this.velocity = rb.velocity.magnitude;
			this.velocityDirection = rb.velocity.normalized;
			this.angularVelocity = rb.angularVelocity.magnitude;
			this.angularVelocityDirection = rb.angularVelocity.normalized;

			this.storedVelocity = storedVelocity;
			this.storedAngularVelocity = storedAngularVelocity;
			this.timeState = ts; //Not necessary currently. But if we change things later?
		}

		public FrameState(FrameState previousState, FrameState nextState, float lerpRatio) 
		{
			this.position = Vector3.Lerp (previousState.position, nextState.position, lerpRatio);
			this.rotation = Quaternion.Lerp (previousState.rotation, nextState.rotation, lerpRatio);
			this.velocity = Mathf.Lerp (previousState.velocity, nextState.velocity, lerpRatio);
			this.velocityDirection = Vector3.Slerp (previousState.velocityDirection, nextState.velocityDirection, lerpRatio);
			this.angularVelocity = Mathf.Lerp (previousState.angularVelocity, nextState.angularVelocity, lerpRatio);
			this.angularVelocityDirection = Vector3.Slerp(previousState.angularVelocityDirection, nextState.angularVelocityDirection, lerpRatio);

			this.storedVelocity = Mathf.Lerp (previousState.storedVelocity, nextState.storedVelocity, lerpRatio);
			this.storedAngularVelocity = Mathf.Lerp (previousState.storedAngularVelocity, nextState.storedAngularVelocity, lerpRatio);
			this.timeState = nextState.timeState; //Can't interpolate very well, so just grabbing the next one since that is usually what determines the current state
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

	private List<FrameState> states = new List<FrameState>(); //FIXME: Probably wanna preallocate this
	protected int currentStateIndex = -1;

	//Need to do setup in awake, since we make a call to each TimeInteractable at start
	protected virtual void Awake () {
		tf = gameObject.transform;
		rb = GetComponent<Rigidbody> ();

		baseKinematicState = rb.isKinematic;
		baseGravityState = rb.useGravity;

		rb.velocity = StartVelocity;
		rb.angularVelocity = StartAngularVelocity;

		previousVelocity = rb.velocity.magnitude;
		previousAngularVelocity = rb.angularVelocity.magnitude;

		rewindDrawer = GetComponent<RewindDrawer> ();
		if (rewindDrawer != null && !rewindDrawer.MeshIsSet()) {
			rewindDrawer.SetMesh(this.GetComponent<MeshFilter>().mesh);
		}

		ResetState ();
	}

	protected virtual void Update() {}

	public virtual void StartTime() {
		ResetState ();
	}

	public virtual void SlowTime(int slowFactor) {
		ResetState ();
		this.slowFactor = slowFactor;
		StoreVelocity (1f/(float)slowFactor);
		timeState = TimeState.Slow;
	}

	public virtual void StopTime() {
		ResetState ();
		StoreVelocity(0);

		//FIXME: Possible bug source here. We change this directly both here and in interactor. Combine the effects somehow?
		rb.isKinematic = true;
		rb.useGravity = false;

		timeState = TimeState.Stop;
	}

	public virtual void RewindTime(int rewindSpeed = 1) {
		timeState = TimeState.Rewind;
		this.rewindSpeed = rewindSpeed;
		DrawRewind ();
	}

	public virtual void StopRewind() {
		timeState = states[currentStateIndex].timeState;
		StopDrawingRewind ();
	}

	protected virtual void DrawRewind() {
		if (rewindDrawer != null) {
			rewindDrawer.StartColor = Color.gray;
			rewindDrawer.EndColor = Color.gray*1.5f;
			rewindDrawer.UpdatePositions (tf, states, currentStateIndex+1); //Need to set colors before updating positions
			rewindDrawer.StartDrawing (rewindSpeed);
		}
	}

	protected virtual void StopDrawingRewind() {
		if (rewindDrawer != null) {
			rewindDrawer.StopDrawing();
		}
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
				StoreRelativeVelocity (1f / (float)slowFactor);
				StoreState ();
			} else if (timeState == TimeState.Stop) {
				//Do nothing? Don't store state?
			} else {
				StoreState (slowFactor);
				//StoreState ();
			}
		}
	}

	private void RewindFixedUpdate() {
		if (currentStateIndex >= 0) {
			RestoreLastState ();
		} else {
			throw new IndexOutOfRangeException ("Overshot rewind. Current state index: " + currentStateIndex);
		}
	}

	private void StoreState(int numberOfStates = 1) {
		UnityEngine.Assertions.Assert.IsTrue (numberOfStates > 0);


		FrameState currentState = new FrameState (tf, rb, timeState, storedVelocity, storedAngularVelocity);
		if (currentStateIndex >= 0 && numberOfStates > 1) {
			//Interpolate states between previous and now
			FrameState previousState = states [currentStateIndex];
			for (int i = 1; i < numberOfStates; ++i) {
				++currentStateIndex;
				float ratio = (float)i/(float)numberOfStates;
				FrameState midState = new FrameState (previousState, currentState, ratio);
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

	private void RestoreLastState() {
		if (currentStateIndex > 0) {
			currentStateIndex = Mathf.Max (0, currentStateIndex - rewindSpeed);

			tf.position = states [currentStateIndex].position;
			tf.rotation = states [currentStateIndex].rotation;
			rb.velocity = states [currentStateIndex].velocity * states [currentStateIndex].velocityDirection;
			rb.angularVelocity = states [currentStateIndex].angularVelocity * states [currentStateIndex].angularVelocityDirection;
			storedVelocity = states [currentStateIndex].storedVelocity;
			storedAngularVelocity = states [currentStateIndex].storedAngularVelocity;
			//No timeState restoration, since we are in rewind mode. Set at end of rewind.
		}
	}
		
	private void ResetState () {
		timeState = TimeState.Normal;
		rb.isKinematic = baseKinematicState;
		rb.useGravity = baseGravityState;
		RestoreVelocity ();

		StopDrawingRewind ();
	}
}