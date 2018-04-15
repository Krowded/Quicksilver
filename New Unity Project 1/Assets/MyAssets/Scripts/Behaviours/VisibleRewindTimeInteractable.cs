using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(RewindDrawer))]
public class VisibleRewindTimeInteractable : PhysicsTimeInteractable
{
	private RewindDrawer rewindDrawer;

	void Start() {
		rewindDrawer = gameObject.GetComponent<RewindDrawer> ();
		if (!rewindDrawer.MeshIsSet()) {
			MeshFilter temp = gameObject.GetComponent<MeshFilter> ();
			if (temp == null) {
				throw new MissingComponentException ("Found no MeshFilter");
			} else {
				rewindDrawer.SetMesh (temp.mesh);
			}
		}
	}

	protected override void ProtectedRewindTime ()
	{
		base.ProtectedRewindTime ();
		DrawRewind ();
	}

	protected override void ProtectedStopRewind ()
	{
		base.ProtectedStopRewind ();
		StopDrawingRewind ();
	}

	protected override void ProtectedResetState ()
	{
		base.ProtectedResetState ();
		StopDrawingRewind ();
	}

	private void DrawRewind() {
		rewindDrawer.StartColor = Color.gray;
		rewindDrawer.EndColor = Color.gray*1.5f;
		rewindDrawer.UpdatePositions (tf, states, currentStateIndex+1); //Need to set colors before updating positions
		rewindDrawer.StartDrawing (rewindSpeed);
	}

	private void StopDrawingRewind() {
		if (rewindDrawer != null) //Need to check, cause problem before start is called
			rewindDrawer.StopDrawing();
	}
}

