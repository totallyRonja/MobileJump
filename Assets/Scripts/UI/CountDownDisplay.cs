using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class CountDownDisplay : MonoBehaviour {
    [SerializeField] private FloatProperty Timer;
    [SerializeField] private float FromScale = 1;
    [SerializeField] private float ToScale = 1;

    private TMP_Text text;
    private RectTransform trans;

    private void Awake() {
        trans = (RectTransform) transform;
        text = GetComponent<TMP_Text>();
        
        text = GetComponent<TMP_Text>();
        Timer.OnChange.AddListener(UpdateText);
        UpdateText(3f);
    }

    private void UpdateText(float number) {
        var whole = Mathf.CeilToInt(number); //we count to 1, not to 0
        var fraction = number % 1;
        var scale = Mathf.Lerp(FromScale, ToScale, 1 - fraction); //inversed because count *down*
        trans.localScale = Vector3.one * scale;
        text.text = whole.ToString();
    }
}
