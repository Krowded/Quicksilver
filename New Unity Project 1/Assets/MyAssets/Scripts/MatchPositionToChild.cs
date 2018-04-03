using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPositionToChild : MonoBehaviour {

	private Transform childTransform;

	// Use this for initialization
	void Start () {
		childTransform = transform.GetChild (0);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = childTransform.position;
		childTransform.position = Vector3.zero;
	}
}
