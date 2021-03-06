using System;
using UnityEngine;

public class IntPropertyCompare : MonoBehaviour {
	[SerializeField] private IntProperty Property1;
	[SerializeField] private CompareMode Operator;
	[SerializeField] private IntProperty Property2;

	private void Awake() {
		Property1.OnChange.AddListener(a => Compare(a, Property2));
		Property2.OnChange.AddListener(b => Compare(Property1, b));
		Compare(Property1, Property2);
	}

	private void Compare(int a, int b) {
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