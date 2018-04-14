using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RewindDrawer))]
public class VisibleRewindTimeInteractable : DynamicTimeInteractable
{
	private RewindDrawer rewindDrawer;

	protected override void Awake() {
		rewindDrawer = gameObject.GetComponent<RewindDrawer> ();
		if (!rewindDrawer.MeshIsSet()) {
			rewindDrawer.SetMesh(this.GetComponent<MeshFilter>().mesh);
		}

		base.Awake ();
	}

	private void DrawRewind() {
		rewindDrawer.StartColor = Color.gray;
		rewindDrawer.EndColor = Color.gray*1.5f;
		rewindDrawer.UpdatePositions (tf, states, currentStateIndex+1); //Need to set colors before updating positions
		rewindDrawer.StartDrawing (rewindSpeed);
	}

	private void StopDrawingRewind() {
		rewindDrawer.StopDrawing();
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
}

