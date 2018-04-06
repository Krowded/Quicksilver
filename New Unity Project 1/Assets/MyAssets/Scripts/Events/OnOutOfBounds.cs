using UnityEngine;
using System.Collections;

public class OnOutOfBounds : LevelEvent {
	public Collider Object;
	public Collider Bounds;

	public override bool ConditionMet () {
		return 	!(Bounds.bounds.Contains (Object.transform.position) || Bounds.bounds.Intersects (Object.bounds));
	}
}

