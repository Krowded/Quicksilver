using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndingMethods : MonoBehaviour {
	public void Win() {
		Debug.Log ("You win!");
		Destroy (this); //FIXME: Obviously don't keep this...
	}

	public void Lose() {
		Debug.Log ("You lose!");
	}
}
