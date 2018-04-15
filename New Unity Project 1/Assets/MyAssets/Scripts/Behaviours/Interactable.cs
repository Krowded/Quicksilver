using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
	public MeshFilter MeshFilter;
	public float Cost;
	public float Diameter;
	protected bool grabbed = false;
	public abstract Vector3[] linePositions { get; }
	public Color HighlightColor = new Color(0f, 0f, 0f, 0);

	private bool standardGravityState;
	private Rigidbody rb;

	public void Grab() {
		if (grabbed) {
			throw new UnassignedReferenceException ("Object already grabbed");
		}
		TurnGravityOff ();
		grabbed = true;
		ProtectedGrab ();
	}

	protected virtual void ProtectedGrab() {}

	public void Drop() {
		if (!grabbed) {
			throw new UnassignedReferenceException ("Can't drop non-grabbed object");
		}
		ResetGravity ();
		grabbed = false;
		ProtectedDrop ();
	}

	protected virtual void ProtectedDrop() {}

	public void DropAndReset() {
		Drop ();
		ProtectedDropAndReset ();
	}

	protected virtual void ProtectedDropAndReset() {
		
	}

	public virtual float GetCost() {
		if (!grabbed) {
			throw new UnassignedReferenceException ("Can't get cost from non-grabbed objects");
		}
		return ProtectedGetCost ();
	}

	protected virtual float ProtectedGetCost() {
		return 0;
	}


	private Color startColor;
	private bool colorSet = false;
	public virtual void SetColor(Color color) {
		if (colorSet) {
			throw new Exception ("Color already set");
		} else {
			Material temp = gameObject.GetComponent<Material> ();
			startColor = temp.color;
			temp.color *= color;
			colorSet = true;
		}
	}

	public virtual void ResetColor(Color color) {
		if (!colorSet) {
			gameObject.GetComponent<Material> ().color = startColor;
			colorSet = false;
		} else {
			#warning Color not set
		}
	}

	private void TurnGravityOff() {
		rb = gameObject.GetComponent<Rigidbody> (); //Regetting the rigidbody every time here, just to avoid bugs if it gains one while grabbed
		if (rb != null) {
			standardGravityState = rb.useGravity;
			rb.useGravity = false;
		}
	}
		
	private void ResetGravity() {
		if (rb != null) {
			rb.useGravity = standardGravityState;
			rb = null;
		}
	}
}