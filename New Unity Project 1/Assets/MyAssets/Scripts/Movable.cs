using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movable : Interactable {
	private Vector3 startPosition;

	public override Vector3[] linePositions {
		get {
			return new Vector3[] { startPosition, gameObject.transform.position };
		}
	}
		
	public override void Grab() {
		base.Grab ();
		startPosition = gameObject.transform.position;
	}

	public override void DropAndReset() {
		base.DropAndReset ();
		gameObject.transform.position = startPosition;
	}

	public override float GetCost() {
		base.GetCost ();
		return Cost * Vector3.Magnitude (gameObject.transform.position - startPosition);
	}

}
