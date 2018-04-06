using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterKiller : MonoBehaviour, DeathScript {
	public void Kill() {
		GameObject.Destroy (gameObject);
	}
}
