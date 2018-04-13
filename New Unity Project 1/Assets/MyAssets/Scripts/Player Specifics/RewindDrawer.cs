using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindDrawer : AfterImageDrawer {
	private GameObject tempObject2;

	private List<int> usedIndices;
	private int totalPoints = -1;

	protected override void ProtectedStart() {
		tempObject2 = new GameObject ();
		tempObject2.hideFlags |= HideFlags.HideInHierarchy;

		usedIndices = new List<int> ();
	}

	protected override void Update() {
		if (count <= 0) {
			this.StopDrawing ();
		}

		base.Update ();
	}

	private void FixedUpdate() { //Rewind, so we have one state per fixedupdate guaranteed
		if (totalPoints >= 0) {
			--totalPoints;
			if (totalPoints < usedIndices [count - 1]) {
				--count;
			}
		}
	}

	public void UpdatePositions(Transform baseTransform, List<TimeInteractable.FrameState> states, int size = -1) {
		if (size < -1) {
			totalPoints = states.Count;
		} else {
			totalPoints = size;
		}

		count = 0;
		if (totalPoints <= 0) {
			return;
		}

		usedIndices.Clear ();

		tempObject2.transform.localScale = baseTransform.localScale*0.99f; //Slightly smaller, so it can hide inside object if necessary
		Transform tempTransform = tempObject2.transform;

		int positionSublist = -1;
		int sublistStartIndex = 0;
		Vector3 previousPos = Vector3.zero;
		for (int i = 0; i < totalPoints; ++i) {
			if (i == 0 || count - sublistStartIndex == 1023) {
				++positionSublist;
				AllocateInstanceMemory (positionSublist+1);
				sublistStartIndex = positionSublist * 1023;
			}

			if (i == 0 || Vector3.Magnitude(previousPos - states[i].position) > DistanceBetweenDraws) {
				tempTransform.position = states[i].position;
				tempTransform.rotation = states[i].rotation;
				positions[positionSublist][count - sublistStartIndex] = tempTransform.localToWorldMatrix;
				usedIndices.Add (i);
				++count;

				previousPos = tempTransform.position;
			}


		}

		CalculateAndSetColors ();
	}
}