using TMPro;
using UnityEngine;


[RequireComponent(typeof(TMP_Text))]
public class HeightDisplay : MonoBehaviour {
    [SerializeField] private string DisplayString = "{0:F2}m";
    
    void Start() {
        var text = GetComponent<TMP_Text>();
        GameManager.Instance.maxHeight.AddListener(height => text.text = string.Format(DisplayString, height), true);
    }
}
