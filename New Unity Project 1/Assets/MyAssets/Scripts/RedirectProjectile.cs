using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectProjectile : MonoBehaviour {
	void OnCollisionEnter(Collision col) {
		Debug.Log ("Collision detected");
		if (col.gameObject.tag == "Projectile") {
			Vector3 normal;
			if (Vector3.Dot(transform.right, col.transform.position - transform.position) > 0) {
				normal = transform.right;
			} else {
				normal = -transform.right;
			}

			Vector3 movementDirection = col.rigidbody.velocity.normalized;
			Quaternion globalRotation = Quaternion.FromToRotation (movementDirection, Vector3.Reflect (movementDirection, normal));
			col.transform.Rotate(globalRotation.eulerAngles, Space.World);
		}
	}
}
