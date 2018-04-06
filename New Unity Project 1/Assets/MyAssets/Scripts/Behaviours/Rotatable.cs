using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotatable : Interactable {

	public Transform parentTransform;
	private Quaternion startRotation;

	public override Vector3[] linePositions {
		get {
			return new Vector3[]{ parentTransform.position, gameObject.transform.position };
		}
	}

	void Start() {
		Cost = 40f;
		Diameter = 4f;
	}

	void Update() {
		if (grabbed) {
			//parentTransform.rotation.SetLookRotation (Vector3.Normalize (parentTransform.position - gameObject.transform.position));
			Debug.Log(Vector3.Angle ((parentTransform.position - gameObject.transform.position).normalized, parentTransform.GetComponent<Rigidbody>().velocity.normalized));
			if (Quaternion.Angle (startRotation, parentTransform.rotation) > 0.1) {
				parentTransform.rotation *= Quaternion.FromToRotation (parentTransform.GetComponent<Rigidbody>().velocity.normalized, (parentTransform.position - gameObject.transform.position).normalized);
			}
			//parentTransform.rotation *= Quaternion.FromToRotation (parentTransform.TransformDirection(parentTransform.forward), Vector3.Normalize (parentTransform.position - gameObject.transform.position));
			//parentTransform.rotation.SetLookRotation (Vector3.Normalize (parentTransform.position - gameObject.transform.position));
		}
	}

	protected override void ProtectedGrab() {
		startRotation = parentTransform.rotation;
	}

	protected override void ProtectedDrop() {
		gameObject.transform.position = parentTransform.position;
	}

	protected override void ProtectedDropAndReset() {
		parentTransform.rotation = startRotation;
	}

	protected override float ProtectedGetCost() {
		float angle = Quaternion.Angle (startRotation, parentTransform.rotation);
		return angle*Cost;
	}
}

