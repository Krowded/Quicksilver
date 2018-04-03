using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StartVelocity : MonoBehaviour, TimeInteractor {

	[SerializeField] public float Speed1;
	[SerializeField] public float Speed2;

	private Rigidbody rb;
	private Transform tf;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody> ();
		tf = gameObject.GetComponent<Transform> ();
		rb.velocity = tf.up*Speed1;
	}

	// Update is called once per frame
	void Update () {
		rb.velocity = tf.up*Speed1;
	}

	public void StartTime() {
		Debug.Log ("Started");
		float temp = Speed1;
		Speed1 = Speed2;
		Speed2 = temp;
	}

	public void StopTime() {
		Debug.Log ("Stopped");
		float temp = Speed1;
		Speed1 = Speed2;
		Speed2 = temp;
	}
}
