using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnCollision : MonoBehaviour {
	void OnCollisionEnter (Collision collision) {
		Collider col = collision.collider;
		Debug.Log ("Collision detected between " + gameObject.name + " and " + col.name);

		DeathScript ds = col.gameObject.GetComponent<DeathScript> ();
		if (ds != null) {
			ds.Kill ();
		}
	}
}
