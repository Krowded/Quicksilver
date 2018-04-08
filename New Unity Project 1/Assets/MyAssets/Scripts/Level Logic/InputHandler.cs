using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour {
	public KeyCode PickUpKey = KeyCode.Mouse0;
	public KeyCode TimeKey = KeyCode.Mouse1;
	public KeyCode ForwardKey = KeyCode.W;
	public KeyCode LeftKey = KeyCode.A;
	public KeyCode BackKey = KeyCode.S;
	public KeyCode RightKey = KeyCode.D;
	public KeyCode ResetKey = KeyCode.E;
	public KeyCode RestartKey = KeyCode.R;
	public KeyCode RewindKey = KeyCode.E;

	[HideInInspector] public bool pickKeyDown = false;
	[HideInInspector] public bool timeKeyDown = false;
	[HideInInspector] public bool forwardKeyDown = false;
	[HideInInspector] public bool leftKeyDown = false;
	[HideInInspector] public bool backKeyDown = false;
	[HideInInspector] public bool rightKeyDown = false;
	[HideInInspector] public bool resetKeyDown = false;
	[HideInInspector] public bool restartKeyDown = false;
	[HideInInspector] public bool rewindKeyDown = false;

	void Start() {
		//TODO: Move this somewhere more appropriate (fixes light at reload)
		DynamicGI.UpdateEnvironment ();
	}

	//Collect here, so we don't miss inputs, and then reset in them at end of frame
	void Update() {
		pickKeyDown = Input.GetKeyDown(PickUpKey);
		timeKeyDown = Input.GetKeyDown(TimeKey);
		forwardKeyDown = Input.GetKeyDown(ForwardKey);
		leftKeyDown = Input.GetKeyDown(LeftKey);
		backKeyDown = Input.GetKeyDown(BackKey);
		rightKeyDown = Input.GetKeyDown(RightKey);
		resetKeyDown = Input.GetKeyDown (ResetKey);
		restartKeyDown = Input.GetKeyDown (RestartKey);
		rewindKeyDown = Input.GetKeyDown (RewindKey);

		//TODO: Move this somewhere more appropriate (e.g. loader)
		if (restartKeyDown) {
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}
	}
}
