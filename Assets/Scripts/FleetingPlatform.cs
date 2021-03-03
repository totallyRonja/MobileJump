using UnityEngine;

public class FleetingPlatform : MonoBehaviour {
	private void JumpedOn() {
		//todo: add small delay and maybe a nice effect
		gameObject.SetActive(false);
	}
}