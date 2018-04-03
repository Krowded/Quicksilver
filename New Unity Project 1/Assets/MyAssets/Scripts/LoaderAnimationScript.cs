using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class LoaderAnimationScript : MonoBehaviour {

	[Range(0,10)]
	public float TimeLimit;
	private float CurrentTime;
	private Renderer rend;

	private Transform tf;
	private Quaternion fixedRotation;

	void Awake() {
		tf = gameObject.transform;
		if (tf != null)
		fixedRotation = tf.rotation;
	}
		
	void Start () {
		rend = GetComponent<Renderer> ();
		rend.material.SetFloat ("_XLimit",1);
		rend.material.SetFloat ("_YLimit",1);

		StartAnimation (); //Should maybe be called from outside or in OnEnable or something
	}

	void Update () {
		CurrentTime = Mathf.Max (0, CurrentTime - Time.deltaTime);
		float ratio = CurrentTime/TimeLimit;
		if (ratio < 0.00001) {
			gameObject.SetActive (false);
		}

		rend.material.SetFloat ("_XLimit", ratio);
		rend.material.SetFloat ("_YLimit", ratio);

		if (tf != null)
		tf.rotation = fixedRotation;
	}


	public void StartAnimation() {
		CurrentTime = TimeLimit;
	}
}
