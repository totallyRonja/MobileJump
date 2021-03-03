using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class IntDisplay : MonoBehaviour {
	[SerializeField] private IntProperty Property;
	[SerializeField] private string DisplayString = "it is {0}";

	private TMP_Text text;

	private void Start() {
		text = GetComponent<TMP_Text>();
		UpdateText(Property.Value);
		Property.OnChange.AddListener(UpdateText);
	}

	private void UpdateText(int number) {
		text.text = string.Format(DisplayString, number);
	}
}