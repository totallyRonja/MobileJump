using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;

	[SerializeField] private Transform HeightMeasurement;
	
	[NonSerialized] public Observable<float> maxHeight = new Observable<float>();

	private float heightZero;

	private void Awake() {
		heightZero = HeightMeasurement.position.y;
	}

	private void LateUpdate() {
		var height = HeightMeasurement.position.y - heightZero;
		if (height > maxHeight.Value)
			maxHeight.Value = height;
	}

	private void OnEnable() {
		if (Instance != null)
			Debug.LogWarning("There are two pool managers in your scene, this shouldn't happen!");
		Instance = this;
	}

	private void OnDisable() {
		if (Instance == this)
			Instance = null;
	}
}