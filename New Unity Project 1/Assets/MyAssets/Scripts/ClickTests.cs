using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTests : MonoBehaviour {

	public GameObject obj;		
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			obj.SetActive (!obj.activeSelf);
		}
	}
}
