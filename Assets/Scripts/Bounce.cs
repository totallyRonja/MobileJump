using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Bounce : MonoBehaviour {

	[SerializeField] private float BounceDuration;
	[SerializeField] private bool NormalizedTime;
	[SerializeField] private string PropertyName;
	[SerializeField] private Material BounceMaterial;
	private Material regularMaterial;
	private Renderer render;
	private float bounceStart = -1;
	private int property;

	private void Awake() {
		render = GetComponent<Renderer>();
		regularMaterial = render.sharedMaterial;
		property = Shader.PropertyToID(PropertyName);
		//copy the bounce material to avoid multiple sources setting variables
		BounceMaterial = new Material(BounceMaterial);
	}

	public void DoBounce() {
		bounceStart = Time.time;
		BounceMaterial.SetFloat(property, 0);
		render.sharedMaterial = BounceMaterial;  
	}

	private void Update() {
		if(bounceStart < 0) return;

		var elapsedTime = Time.time - bounceStart;
		if (elapsedTime > BounceDuration) {
			EndBounce();
			return;
		}
		if (NormalizedTime) elapsedTime /= BounceDuration;
		BounceMaterial.SetFloat(property, elapsedTime);
	}

	public void EndBounce() {
		bounceStart = -1;
		render.sharedMaterial = regularMaterial;  
	}
}