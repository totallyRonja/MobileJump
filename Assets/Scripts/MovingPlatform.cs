using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
	[SerializeField] private float From;
	[SerializeField] private float To;
	[SerializeField] private float Speed;

	private float progress;
	private Transform trans;
	private bool newPlatform;

	private void Awake() {
		trans = transform;
	}

	private void ResetValues() {
		newPlatform = true;
	}

	private void Update() {
		//we need to do this here because we need to use the position *after* instantiation, not during
		if (newPlatform) {
			progress = Mathf.InverseLerp(From, To, trans.position.x) / Speed;
			newPlatform = false;
		}
		progress += Time.deltaTime;
		var relProgress = Mathf.PingPong(progress * Speed, 1);
		var pos = Mathf.Lerp(From, To, relProgress);
		trans.position = trans.position.X(pos);
	}

	private void OnDrawGizmos() {
		var height = transform.position.y;
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(new Vector3(From, height, 0), new Vector3(To, height, 0));
	}
}