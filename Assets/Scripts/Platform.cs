using System;
using UnityEngine;

public class Platform : MonoBehaviour {
	[SerializeField] private IntProperty JumpCount;
	
	private Bounce[] bouncers;
	private bool jumpedOn = false;
    
	private void Awake() {
		bouncers = GetComponentsInChildren<Bounce>();
	}

	private void ResetValues() {
		jumpedOn = false;
	}

	private void JumpedOn() {
		if (!jumpedOn) {
			JumpCount.Value++;
			jumpedOn = true;
		}

		foreach (var bouncer in bouncers) {
			bouncer.DoBounce();
		}
	}
}