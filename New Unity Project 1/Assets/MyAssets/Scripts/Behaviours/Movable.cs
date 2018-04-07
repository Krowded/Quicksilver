using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movable : Interactable {
	private Vector3 startPosition;
	private bool gravityState;

	public override Vector3[] linePositions {
		get {
			return new Vector3[] { startPosition, gameObject.transform.position };
		}
	}
		
	protected override void ProtectedGrab() {
		startPosition = gameObject.transform.position;

		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
	}

	protected override void ProtectedDropAndReset() {
		gameObject.transform.position = startPosition;
	}

	protected override float ProtectedGetCost() {
		return Cost * Vector3.Magnitude (gameObject.transform.position - startPosition);
	}

}
