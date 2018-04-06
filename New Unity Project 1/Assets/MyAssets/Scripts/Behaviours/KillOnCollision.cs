using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnCollision : MonoBehaviour {
	void OnCollisionEnter (Collision collision) {
		Collider col = collision.collider;
		Debug.Log ("Collision detected between " + gameObject.name + " and " + col.name);

		if (col.gameObject.CompareTag("Character")) {
			col.gameObject.GetComponent<DeathScript>().Kill();
			//GameObject.Destroy (gameObject);
		}
	}
}
