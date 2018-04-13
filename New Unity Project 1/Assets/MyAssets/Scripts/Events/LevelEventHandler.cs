using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventHandler : MonoBehaviour {
	public GameObject EventHierarchy;
	public List<LevelEvent> Events = new List<LevelEvent> (); //Event are checked and fired in hierarchy order

	void Start() {
		UpdateEvents ();
	}

	void FixedUpdate () {
		foreach (LevelEvent _event in Events) {
			_event.FireIfConditionMet ();
		}
	}

	public void UpdateEvents() {
		Events.Clear ();

		//BFS for events => events trigger in hierarchical order
		Stack<GameObject> eventHierarchy = new Stack<GameObject> ();
		eventHierarchy.Push (EventHierarchy);

		while (eventHierarchy.Count > 0) {
			GameObject temp = eventHierarchy.Pop ();
			foreach (LevelEvent _event in temp.GetComponents<LevelEvent>()) {
				Events.Add (_event);
			}

			for (int i = 0; i < temp.transform.childCount; ++i) {
				eventHierarchy.Push (temp.transform.GetChild (i).gameObject);
			}
		}
	}
}
