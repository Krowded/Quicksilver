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

		Vector3 previousPos = Vector3.zero;
		for(int i = 0, tempCount = totalPoints; tempCount > 0; ++i, tempCount -= 1023) {
			AllocateInstanceMemory (i+1);
			int startIndex = i * 1023;
			for (int j = startIndex; j < totalPoints; ++j) {
				if (i+j == 0 || Vector3.Magnitude(previousPos - states[i].position) > DistanceBetweenDraws) {
					tempTransform.position = states[startIndex+j].position;
					tempTransform.rotation = states[startIndex+j].rotation;
					positions[i][count - startIndex] = tempTransform.localToWorldMatrix;
					usedIndices.Add (startIndex+j);
					++count;
				}
				previousPos = states [startIndex+j].position;
			}
		}

		/*
		int tempCount = count;
		for(int i = 0; i <= (count-1)/1023; ++i) {
			for (int j = 0; j < count; ++j) {
				colors[i][j] = ColorManipulation.LerpColorsCorrected (StartColor, EndColor, (float)(i*1023+j)/(float)count);
			}
			propBlocks[i].Clear ();
			propBlocks[i].SetVectorArray ("_Color", colors[i]);
		}*/
		CalculateAndSetColors ();
	}
}