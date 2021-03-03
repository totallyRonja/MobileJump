using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class FloatDisplay : MonoBehaviour {
	[SerializeField] private FloatProperty Property;
	[SerializeField] private string DisplayString = "it is {0}";

	private TMP_Text text;

	private void Start() {
		text = GetComponent<TMP_Text>();
		UpdateText(Property);
		Property.OnChange.AddListener(UpdateText);
	}

	private void UpdateText(float number) {
		text.text = string.Format(DisplayString, number);
	}
}