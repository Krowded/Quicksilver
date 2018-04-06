using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHierarchyEmpty : LevelEvent {
	public Transform Hierarchy;

	public override bool ConditionMet() {
		return Hierarchy.childCount == 0;
	}
}
