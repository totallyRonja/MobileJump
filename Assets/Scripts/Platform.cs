using UnityEngine;

public class Platform : MonoBehaviour {
    private Bounce[] bouncers;
    
    private void Awake() {
        bouncers = GetComponentsInChildren<Bounce>();
    }

    private void JumpedOn() {
        foreach (var bouncer in bouncers) {
            bouncer.DoBounce();
        }
    }
}
