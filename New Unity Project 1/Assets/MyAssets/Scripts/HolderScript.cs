using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderScript : MonoBehaviour {

	public Color color1;
	public Color color2;

	private Rigidbody rb;
	private new Renderer renderer;
	private float ratio = 0;
	private bool updated = true;

	private Interactable papa;
	private Transform papaTransform;

	void Start () {
		renderer = GetComponent<Renderer>();
		rb = GetComponent<Rigidbody> ();
		rb.detectCollisions = false;
	}

	private float timePassed = 0;
	private float timeLimit = 0;
	void Update () {
		if (updated) {
			renderer.material.SetColor ("_Color", ColorManipulation.LerpColorsCorrected(color1, color2, ratio));
			updated = false;
		}

		transform.position = papaTransform.position;

		if (timePassed >= timeLimit) {
			timePassed = 0;
			timeLimit = Random.Range (0, 3);
			rb.AddRelativeTorque (Random.insideUnitSphere * Random.Range (10, 200));
		} else {
			timePassed += Time.deltaTime;
		}
	}

	public void Initialize(Interactable obj) {
		//transform.parent = obj.transform; //TODO: Make it so we get the actual object instead of a child and can just set the transform as parent
		papa = obj;
		papaTransform = papa.transform;
		transform.localScale = Vector3.one*(papa.Diameter);
		transform.rotation = Random.rotation;
	}

	public void UpdateParameters(float ratio) {
		this.ratio = ratio;
		updated = true;
	}
}
