﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AfterImageDrawer))]
public class InteractorScript : MonoBehaviour {
	//Interaction components
	public EnergyBar energyBar;
	private AfterImageDrawer afterImage;
	public GameObject HolderObj;
	public Transform parentTransform;

	//Values that matter
	public float DistanceFromFace = 1f;

	//Keeping track of interactions
	[SerializeField] private GameObject HeldObject;
	[SerializeField] private GameObject LookedAtObject;
	private Collider HeldCollider;
	private Interactable HeldInteractable;
	private HolderScript Holder;

	//State machine
	private bool searchNewObject = false;
	private bool justPickedUp = true;
	private bool justDropped = false;

	void Start () {
		Holder = HolderObj.GetComponent<HolderScript> ();
		afterImage = GetComponent<AfterImageDrawer> ();

		//Make sure we have the transform of the owner of the interactor
		if (parentTransform == null) {
			parentTransform = gameObject.transform.parent;
		}
	}
		
	void UpdateLine() {
		afterImage.UpdatePositions (baseTransform: HeldObject.transform, 
					    startPosition: HeldInteractable.linePositions [0],
					    endPosition: HeldInteractable.linePositions[HeldInteractable.linePositions.Length-1]);
		afterImage.StartColor = energyBar.InterpolatedColor;
		afterImage.EndColor = energyBar.InterpolatedPossibleColor;
	}
	void UpdateCost() {
		energyBar.PossibleCost = HeldInteractable.GetCost();
	}

	void Update () {
		gameObject.transform.position = parentTransform.position + parentTransform.forward * DistanceFromFace;
		if (HeldObject != null) {
			HandleHeldObject ();
			Holder.UpdateParameters (1 - energyBar.PossibleEnergyRatio);
		} else {
			justDropped = false;
		}
	}

	public void PickUpOrDrop() {
		if (HeldObject == null) {
			if (LookedAtObject != null && !justDropped) {
				PickUp ();
			}
		} else {
			if (!justPickedUp && energyBar.HaveEnoughEnergy ()) {
				DropHere ();
			}
		}
	}

	public void Reset() {
		if (HeldObject != null) {
			DropCancel ();
		}
	}

	void HandleHeldObject() {
		HeldObject.transform.position = gameObject.transform.position;
		justPickedUp = false;
		UpdateCost();
		UpdateLine();
	}

	private void DropHere() {
		HeldInteractable.Drop();
		ResetHeldObject ();
		RemoveHolder ();
		energyBar.ApplyCost ();
		energyBar.PossibleCost = 0;
		justDropped = true;

		Debug.Log ("Dropped with new values!");
	}

	private void DropCancel() {
		HeldInteractable.DropAndReset ();
		TriggerExit (HeldObject); //Manually, since teleportation isn't registered
		ResetHeldObject ();
		RemoveHolder ();
		energyBar.PossibleCost = 0;
		justDropped = true;
		Debug.Log ("Dropped and reset!");
	}

	private void ResetHeldObject() {
		HeldCollider.enabled = true;
		HeldCollider = null;
		HeldInteractable = null;
		HeldObject = null;
		afterImage.StopDrawing ();
	}

	private void SetHeldObject(GameObject obj) 
	{
		HeldObject = obj;
		HeldCollider = HeldObject.GetComponent<Collider> ();
		HeldCollider.enabled = false;
		HeldInteractable = HeldObject.GetComponent<Interactable> ();
		HeldInteractable.Grab();
		Mesh m = HeldInteractable.MeshFilter.mesh;
		if (m != null) {
			afterImage.SetMesh (m);
			afterImage.StartDrawing ();
		} else {
			//HeldInteractable.Mesh ();
			throw new UnityException ("Picked up object without mesh");
		}
	}

	private void PickUp() {
		SetHeldObject (LookedAtObject);
		DisplayHolder ();
		justPickedUp = true;
		Debug.Log ("Picked up!");
	}

	void ColorChildren(Transform tf, Color color) {
		MeshRenderer m = tf.gameObject.GetComponent<MeshRenderer> ();
		if (m != null) {
			m.material.color += color;
		}

		foreach (Transform child in tf) {
			ColorChildren (child, color);
		}
	}

	void OnTriggerEnter(Collider col) {
		if (LookedAtObject == null) {
			Interactable obj = col.GetComponent<Interactable> ();
			if (obj != null && LookedAtObject == null) {
				LookedAtObject = col.gameObject;
				ColorChildren (LookedAtObject.transform, obj.HighlightColor);
				searchNewObject = false;
			}
		}
	}
		
	void OnTriggerExit(Collider col) {
		TriggerExit (col.gameObject);
	}

	void TriggerExit(GameObject go) {
		if (go == LookedAtObject) {
			ColorChildren (LookedAtObject.transform, -1f*go.GetComponent<Interactable>().HighlightColor);
			LookedAtObject = null;
			searchNewObject = true;
		}
	}

	void OnTriggerStay(Collider col) {
		if (searchNewObject) {
			OnTriggerEnter(col);
			return;
		}
	}


	void DisplayHolder() {
		HolderObj.SetActive (true);
		Holder.Initialize (HeldInteractable);
	}

	void RemoveHolder() {
		HolderObj.SetActive (false);
	}
}
