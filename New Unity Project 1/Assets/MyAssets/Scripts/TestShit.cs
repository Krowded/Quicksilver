using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShit : MonoBehaviour {
	/*
	public Camera cam;

	public float moveSpeed = 3f;
	public Vector3 direction;
	public Quaternion startRotation;

	// Use this for initialization
	void Start () {
		startRotation = gameObject.transform.rotation;
	}
		
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			direction = Input.mousePosition - cam.WorldToScreenPoint(gameObject.transform.position);
			direction.z = 0f;
			direction.Normalize();
			gameObject.transform.rotation = Quaternion.LookRotation (direction);
			gameObject.transform.RotateAround (gameObject.transform.position, gameObject.transform.up, 90);
			direction = gameObject.transform.InverseTransformDirection (direction);
		}
		Quaternion.LookRotation (new Vector3 (0f, 10f, 0f));
		gameObject.transform.Translate(direction * Time.deltaTime * moveSpeed);
	}*/

	public void TestClick() {
		Debug.Log ("BUttonclicked");
	}

}
