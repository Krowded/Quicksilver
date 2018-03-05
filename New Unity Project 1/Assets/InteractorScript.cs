using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorScript : MonoBehaviour {
	[SerializeField] private GameObject HeldObject;
	[SerializeField] private GameObject LookedAtObject;

	public Transform parentTransform;
	public float DistanceFromFace = 1f;

	private bool searchNewObject = false;

	void Start () {
		if (parentTransform == null) {
			parentTransform = gameObject.transform.parent;
		}
	}

	void Update () {
		gameObject.transform.position = parentTransform.position + parentTransform.forward * DistanceFromFace;
		if (HeldObject != null) {
			HeldObject.transform.position = gameObject.transform.position;
		}
	}

	void ColorChildren(Transform tf, Color color) {
		MeshRenderer m = tf.gameObject.GetComponent<MeshRenderer> ();
		if (m != null) {
			m.material.color += color;
		}

		foreach (Transform child in tf) {
			ColorChildren (child, color);
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Floor") { return; }

		if (LookedAtObject == null) {
			LookedAtObject = col.gameObject;
			ColorChildren (LookedAtObject.transform, Color.red);
			searchNewObject = false;
		}
	}
		
	void OnTriggerExit(Collider col) {
		if (col.gameObject == LookedAtObject) {
			ColorChildren (LookedAtObject.transform, -1f*Color.red);
			LookedAtObject = null;
			searchNewObject = true;
		}
	}

	void OnTriggerStay(Collider col) {
		if (searchNewObject) {
			OnTriggerEnter (col);
			return;
		}

		if (col.gameObject != LookedAtObject) {
			return;
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			if (HeldObject != null) {
				//HeldObject.GetComponent<Rigidbody> ().isKinematic = false;
				HeldObject = null;
				Debug.Log ("Dropped!");
			} else {
				HeldObject = LookedAtObject;
				//HeldObject.GetComponent<Rigidbody> ().isKinematic = true;
				Debug.Log ("Picked up!");
			}
		}
	}
}
