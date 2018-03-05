using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartVelocity : MonoBehaviour, TimeInteractor {

	[SerializeField] public float Speed;
	[SerializeField] private float LastSpeed;
	public Rigidbody rb;
	private Transform tf;

	// Use this for initialization
	void Start () {
		if (rb == null) {
			rb = gameObject.GetComponent<Rigidbody> ();
		}
		tf = gameObject.GetComponent<Transform> ();
		rb.velocity = tf.up*Speed;
	}

	// Update is called once per frame
	void Update () {
		rb.velocity = tf.up*Speed;
	}

	public void StartTime() {
		Debug.Log ("Started");
		Speed = LastSpeed;
	}

	public void StopTime() {
		Debug.Log ("Stopped");
		LastSpeed = Speed;
		Speed = 0;
	}
}
