using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

	private string[] scenes = { 
		"level1", 
		"level2" };
	private int sceneIndex = 0;

	void Start () {
		
	}

	void Update () {
		if (SceneManager.sceneCount < 2) {
			if (sceneIndex >= scenes.Length) {
				Application.Quit ();
				return;
			}

			SceneManager.LoadScene (scenes [sceneIndex++], LoadSceneMode.Additive);
		}
	}
}
