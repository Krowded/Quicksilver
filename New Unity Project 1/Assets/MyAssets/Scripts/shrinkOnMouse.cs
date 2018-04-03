using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrinkOnMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			gameObject.transform.localScale = gameObject.transform.localScale - (new Vector3 (0f, 0.1f, 0f));
			gameObject.transform.position -= Vector3.up * 0.1f;
		}
	}
}
