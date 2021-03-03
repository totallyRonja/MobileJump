using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EventProperty", menuName = "Properties/Event", order = 135)]
public class EventProperty : ScriptableObject {
	[NonSerialized] public Event Event = new Event();

	public void Invoke() {
		Event?.Invoke();
	}
}