using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level {
	public abstract class WinCondition : MonoBehaviour {

		protected void CloseScene() {
			Debug.Log ("Closing scene");
			UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(gameObject.scene);
		}

		public abstract void Update ();
	}

}