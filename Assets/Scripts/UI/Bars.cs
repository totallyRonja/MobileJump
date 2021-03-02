using UnityEngine;

public class Bars : MonoBehaviour {
	private void Start() {
		var trans = (RectTransform) transform;
		// ReSharper disable once Unity.NoNullPropagation
		var aspectHelper = AspectHelper.Instance;
		if (aspectHelper != null) {
			trans.sizeDelta = aspectHelper.CanvasSize;
		} else {
			var canvasTrans = (RectTransform)GetComponentInParent<Canvas>().transform;
			trans.sizeDelta = canvasTrans.sizeDelta;
		}
	}
}
