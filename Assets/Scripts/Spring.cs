using UnityEngine;

public class Spring : MonoBehaviour {
	[SerializeField] private Vector2 MaxScale;
	[SerializeField] private float AnimationTime;
	
	private float springTime;
	private Vector2 baseScale;

	private void Awake() {
		baseScale = transform.localScale;
	}

	private void JumpedOn() {
		springTime = AnimationTime;
	}

	private void Update() {
		if(springTime <= 0)
			return;

		float relativeTime = springTime / AnimationTime; //time 0-1
		float sinTime = relativeTime * Mathf.PI; //time 0-PI
		float amplitude = Mathf.Sin(sinTime); //walk half period of sine
		transform.localScale = Vector2.Lerp(baseScale, MaxScale, amplitude);

		springTime -= Time.deltaTime;
		if(springTime <= 0)
			transform.localScale = baseScale; //proper reset since we can overshoot
	}
}