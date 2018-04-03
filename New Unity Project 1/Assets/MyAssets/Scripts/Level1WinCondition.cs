using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Level;

namespace Level {
	public class Level1WinCondition : Level.WinCondition {
		private Transform goodTeam;
		private Transform evilTeam;
		private int startNumber;

		public void Start() {
			goodTeam = UnityEngine.GameObject.Find("GoodTeam").transform;
			evilTeam = UnityEngine.GameObject.Find("EvilTeam").transform;
			startNumber = goodTeam.childCount;
		}

		public override void Update() {
			if (goodTeam.childCount < startNumber) {
				Debug.Log ("You lost");
				this.CloseScene();
			}

			if (evilTeam.childCount == 0) {
				Debug.Log ("You won");
				this.CloseScene();
			}
		}
	}
}
