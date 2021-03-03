using System;
using UnityEngine;

public abstract class ValueProperty<T> : ScriptableObject {
	[SerializeField] [InspectorName("Value")] private T DefaultValue;
	
	//actual value is never serialized
	//this might not matter in game,
	//but doesnt pollute git changes in editor
	private T value;

	public T Value {
		get => value;
		set {
			OnChange?.Invoke(value);
			this.value = value;
		}
	}

	[HideInInspector] public readonly Event<T> OnChange = new Event<T>();

	//note that Awake isnt called between game runs in the editor so its good to call ResetValues at the start
	private void Awake() {
		ResetValues();
	}

	private void OnValidate() {
		ResetValues();
	}

	public void ResetValues() {
		Value = DefaultValue;
	}
}
