using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHierarchyDead : LevelEvent {
	public Transform Hierarchy;

	public override bool ConditionMet() {
		return HierarchyIsDead (Hierarchy);
	}

	private bool HierarchyIsDead(Transform hierarchy) {
		DeathScript ds = hierarchy.GetComponent<DeathScript> ();
		if (ds != null && !ds.IsDead ()) {
			return false;
		} else {

			foreach (Transform child in hierarchy) {
				if (!HierarchyIsDead (child)) {
					return false;
				}
			}
			return true;
		}
	}
}
