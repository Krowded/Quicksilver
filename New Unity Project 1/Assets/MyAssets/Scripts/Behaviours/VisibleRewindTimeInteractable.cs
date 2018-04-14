using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RewindDrawer))]
public class VisibleRewindTimeInteractable : DynamicTimeInteractable
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

	public override void RewindTime (int rewindSpeed = 1)
	{
		base.RewindTime (rewindSpeed);
		DrawRewind ();
	}

	public override void StopRewind ()
	{
		base.StopRewind ();
		StopDrawingRewind ();
	}

	protected override void ResetState ()
	{
		base.ResetState ();
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

