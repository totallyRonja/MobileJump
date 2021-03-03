using UnityEngine;

public class Pickup : MonoBehaviour {
	private void PickedUp() {
		//make object vanish when picked up
		//pooling will reenable it when its time
		gameObject.SetActive(false);
	}
}