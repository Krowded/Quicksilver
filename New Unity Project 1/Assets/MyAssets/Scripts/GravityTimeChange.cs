using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityTimeChange : MonoBehaviour, TimeInteractor {
	public float TimeStopDrag = 20f;
	public float TimeStartDrag = 0f;
	private Rigidbody rb;

	void Start() {
		rb = gameObject.GetComponent<Rigidbody> ();
		rb.drag = TimeStopDrag;
	}

	public void StartTime() {
		rb.drag = TimeStartDrag;
	}

	public void StopTime() {
		rb.drag = TimeStopDrag;
	}
}
