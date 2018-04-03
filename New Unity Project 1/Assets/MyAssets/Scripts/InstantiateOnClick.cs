using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateOnClick : MonoBehaviour {

	public Camera cam;
	public GameObject prefab;

	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray ray = cam.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				Instantiate (prefab, hit.collider.transform.position, Quaternion.identity);
			}
		}*/


		if (Input.GetMouseButtonDown (0)) {
			Vector3 p = cam.ScreenToWorldPoint (Input.mousePosition) + cam.transform.forward*10f;
			Debug.Log ("Created new at: " + p);
			Instantiate (prefab, p, Quaternion.identity);
		}
	}
}
