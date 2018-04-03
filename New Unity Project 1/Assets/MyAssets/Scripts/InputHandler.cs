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

	[HideInInspector] public bool pickKeyDown = false;
	[HideInInspector] public bool timeKeyDown = false;
	[HideInInspector] public bool forwardKeyDown = false;
	[HideInInspector] public bool leftKeyDown = false;
	[HideInInspector] public bool backKeyDown = false;
	[HideInInspector] public bool rightKeyDown = false;
	[HideInInspector] public bool resetKeyDown = false;

	void Start() {
		//TODO: Move this somewhere more appropriate (fixes light at reload)
		DynamicGI.UpdateEnvironment ();
	}

	void Update () {
		pickKeyDown = Input.GetKeyDown(PickUpKey);
		timeKeyDown = Input.GetKeyDown(TimeKey);
		forwardKeyDown = Input.GetKeyDown(ForwardKey);
		leftKeyDown = Input.GetKeyDown(LeftKey);
		backKeyDown = Input.GetKeyDown(BackKey);
		rightKeyDown = Input.GetKeyDown(RightKey);
		resetKeyDown = Input.GetKeyDown (ResetKey);

		//TODO: Move this somewhere more appropriate (e.g. loader)
		if (Input.GetKeyDown (RestartKey)) {
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}
	}
}
