using System;
using UnityEngine;

public class FloatPropertyCompare : MonoBehaviour {
	[SerializeField] private FloatProperty Property1;
	[SerializeField] private CompareMode Operator;
	[SerializeField] private FloatProperty Property2;

	private void Awake() {
		Property1.OnChange.AddListener(a => Compare(a, Property2.Value));
		Property2.OnChange.AddListener(b => Compare(Property1.Value, b));
		Compare(Property1.Value, Property2.Value);
	}

	private void Compare(float a, float b) {
		bool success = Operator switch {
			CompareMode.Equals => Mathf.Abs(a - b) < Mathf.Epsilon,
			CompareMode.NotEqual => Mathf.Abs(a - b) > Mathf.Epsilon,
			CompareMode.LT => a < b,
			CompareMode.LTE => a <= b,
			CompareMode.GT => a > b,
			CompareMode.GTE => a >= b,
			_ => throw new ArgumentOutOfRangeException(),
		};
		gameObject.SetActive(success);
	}
}