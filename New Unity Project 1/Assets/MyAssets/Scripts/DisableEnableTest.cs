using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEnableTest : MonoBehaviour {

	private int count = 0;

	void Awake() {
		Debug.Log ("Awake " + count++);
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("Start " + count++);
		
	}

	void OnEnable() {
		Debug.Log ("OnEnable " + count++);
	}
}
