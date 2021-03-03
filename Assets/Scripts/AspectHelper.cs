using System;
using UnityEngine;
using UnityEngine.UI;

public class AspectHelper : MonoBehaviour {
	public static AspectHelper Instance;
	
	[SerializeField] [Tooltip("Enter Aspect ratio as width/height")] private Vector2 MinAspect;
	[SerializeField] [Tooltip("Enter Aspect ratio as width/height")] private Vector2 MaxAspect;
	[SerializeField] private CanvasScaler CanvasScaler;

	[NonSerialized] public float Aspect;
	[NonSerialized] public Vector2 CanvasSize;
	[NonSerialized] public Vector2 WorldSize;

	private void Awake() {
		var minAspect = MinAspect.x / MinAspect.y;
		var maxAspect = MaxAspect.x / MaxAspect.y;
		if (minAspect > maxAspect)
			(minAspect, maxAspect) = (maxAspect, minAspect);

		var cam = GetComponent<Camera>();
		var aspect = cam.aspect;
		var canvasSize = CanvasScaler.referenceResolution;
		canvasSize = Vector2.Lerp(
			canvasSize.Y(canvasSize.x / aspect),
			canvasSize.X(canvasSize.y * aspect),
			CanvasScaler.matchWidthOrHeight);
		Aspect = Mathf.Clamp(aspect, minAspect, maxAspect);

		var cameraHeight = cam.orthographicSize;
		WorldSize = new Vector2(cameraHeight * Aspect, cameraHeight);
		if (maxAspect < aspect) {
			CanvasSize = new Vector2(canvasSize.y * Aspect, canvasSize.y);
		} else if (minAspect > aspect) {
			CanvasSize = new Vector2(canvasSize.x, canvasSize.x / Aspect);
			cam.orthographicSize = 2 * WorldSize.x * Aspect / aspect;
		} else {
			CanvasSize = Vector2.Lerp(
				canvasSize.Y(canvasSize.x / aspect),
				canvasSize.X(canvasSize.y * aspect),
				CanvasScaler.matchWidthOrHeight);
		}
	}
	
	private void OnEnable() {
		if(Instance != null)
			Debug.LogWarning("There are two aspect helpers in your scene, this shouldn't happen!");
		Instance = this;
	}

	private void OnDisable() {
		if (Instance == this)
			Instance = null;
	}
}
