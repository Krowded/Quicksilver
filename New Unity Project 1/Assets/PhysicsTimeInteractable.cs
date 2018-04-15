﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsTimeInteractable : DynamicTimeInteractable
{
	//Local space start values
	public Vector3 StartVelocity;
	public Vector3 StartAngularVelocity;

	protected Transform tf;
	protected Rigidbody rb;
	private bool baseKinematicState;
	private bool baseGravityState;

	private float storedVelocity = 0;
	private float storedAngularVelocity = 0;
	private float previousVelocity;
	private float previousAngularVelocity;

	public struct PhysicsState : ITimeState {
		public PhysicsState(Transform tf, Rigidbody rb, TimeState ts, float storedVelocity, float storedAngularVelocity) {
			this.position = tf.position;
			this.rotation = tf.rotation;
			this.velocity = rb.velocity.magnitude;
			this.velocityDirection = rb.velocity.normalized;
			this.angularVelocity = rb.angularVelocity.magnitude;
			this.angularVelocityDirection = rb.angularVelocity.normalized;

			this.storedVelocity = storedVelocity;
			this.storedAngularVelocity = storedAngularVelocity;
		}

		public PhysicsState(PhysicsState previousState, PhysicsState nextState, float lerpRatio) 
		{
			this.position = Vector3.Lerp (previousState.position, nextState.position, lerpRatio);
			this.rotation = Quaternion.Slerp (previousState.rotation, nextState.rotation, lerpRatio); //TODO: Decide if we slerp or do discontinuous rotation (currently we do discontinuous in the game but slerp here)
			this.velocity = Mathf.Lerp (previousState.velocity, nextState.velocity, lerpRatio);
			this.velocityDirection = Vector3.Slerp (previousState.velocityDirection, nextState.velocityDirection, lerpRatio);
			this.angularVelocity = Mathf.Lerp (previousState.angularVelocity, nextState.angularVelocity, lerpRatio);
			this.angularVelocityDirection = Vector3.Slerp(previousState.angularVelocityDirection, nextState.angularVelocityDirection, lerpRatio);

			this.storedVelocity = Mathf.Lerp (previousState.storedVelocity, nextState.storedVelocity, lerpRatio);
			this.storedAngularVelocity = Mathf.Lerp (previousState.storedAngularVelocity, nextState.storedAngularVelocity, lerpRatio);
		}


		public Vector3 position;
		public Quaternion rotation;
		public Vector3 velocityDirection;
		public float velocity;
		public float angularVelocity;
		public Vector3 angularVelocityDirection;

		public float storedVelocity;
		public float storedAngularVelocity;
	}

	protected override void Awake ()
	{
		tf = gameObject.transform;
		rb = gameObject.GetComponent<Rigidbody> ();

		baseKinematicState = rb.isKinematic;
		baseGravityState = rb.useGravity;

		rb.velocity = StartVelocity;
		rb.angularVelocity = StartAngularVelocity;

		previousVelocity = rb.velocity.magnitude;
		previousAngularVelocity = rb.angularVelocity.magnitude;

		ResetState ();
	}

	protected override void ProtectedSlowTime ()
	{
		StoreVelocity (1f/(float)slowFactor);
	}
		

	protected override void ProtectedStopTime() {
		StoreVelocity(0);

		//FIXME: Possible bug source here. We change this directly both here and in interactor. Combine the effects somehow?
		rb.isKinematic = true;
		rb.useGravity = false;
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


	protected override ITimeState GetCurrentState () {
		if (timeState == TimeState.Slow)
			StoreRelativeVelocity (1f / (float)slowFactor);

		return new PhysicsState (tf, rb, timeState, storedVelocity, storedAngularVelocity);
	}

	protected override ITimeState GetInterpolatedState (ITimeState previousState, ITimeState currentState, float ratio) {
		return new PhysicsState ((PhysicsState)previousState, (PhysicsState)currentState, ratio);
	}
	protected override void RestoreLastState() {
		if (currentStateIndex > 0) {
			currentStateIndex = Mathf.Max (0, currentStateIndex - rewindSpeed);

			PhysicsState lastState = (PhysicsState)states [currentStateIndex];
			tf.position = lastState.position;
			tf.rotation = lastState.rotation;
			rb.velocity = lastState.velocity * lastState.velocityDirection;
			rb.angularVelocity = lastState.angularVelocity * lastState.angularVelocityDirection;
			storedVelocity = lastState.storedVelocity;
			storedAngularVelocity = lastState.storedAngularVelocity;
			//No timeState restoration, since we are in rewind mode. Set at end of rewind.
		}
	}

	protected override void ProtectedResetState() {
		rb.isKinematic = baseKinematicState;
		rb.useGravity = baseGravityState;
		RestoreVelocity ();
	}
}
