using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InteractorScript))]
public class InteractionController : MonoBehaviour
{
	public InputHandler input;
	public TimeController timeController;
	private InteractorScript interactor;

	void Start() {
		interactor = GetComponent<InteractorScript> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (input.pickKeyDown) {
			interactor.PickUpOrDrop ();
		} else if (input.resetKeyDown) {
			interactor.Reset ();
			timeController.RewindTime (timeController.Hierarchy);
		}
	}
}

