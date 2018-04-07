using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectProjectile : MonoBehaviour {

	private GameObject justCollidedWith = null;

	void Update() {
		justCollidedWith = null;
	}

	void OnCollisionEnter(Collision col) {
		Debug.Log ("Collision detected");
		if (col.gameObject.tag == "Projectile" && col.gameObject != justCollidedWith) {
			Vector3 normal;
			if (Vector3.Dot(transform.right, col.transform.position - transform.position) > 0) {
				normal = transform.right;
			} else {
				normal = -transform.right;
			}

			Vector3 movementDirection = col.rigidbody.velocity.normalized;
			Vector3 newDirection = Vector3.Reflect (movementDirection, normal);
			Quaternion globalRotation = Quaternion.FromToRotation (movementDirection, newDirection);
			Debug.Log("Before collision: " + col.rigidbody.velocity);
			col.rigidbody.rotation = globalRotation * col.rigidbody.rotation;
			col.rigidbody.velocity = col.rigidbody.velocity.magnitude * newDirection;

			//TODO: Rotate angularVelocity as well

			Debug.Log("After collision: " + col.rigidbody.velocity);

			justCollidedWith = col.gameObject;
		}
	}
}
