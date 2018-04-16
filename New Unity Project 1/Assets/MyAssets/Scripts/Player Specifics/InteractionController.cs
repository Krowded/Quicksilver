using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InteractorScript))]
public class InteractionController : MonoBehaviour
{
	public InputHandler input;
	public TimeController timeController;
	private InteractorScript interactor;
	public int RewindSpeedOnCancel = 1;

	void Start() {
		interactor = GetComponent<InteractorScript> ();
	}

	void Update ()
	{
		if (input.pickKeyDown) {
			interactor.PickUpOrDrop ();
		} else if (input.resetKeyDown) {
			interactor.Reset ();
			timeController.RewindTime (RewindSpeedOnCancel);
		}
	}
}

