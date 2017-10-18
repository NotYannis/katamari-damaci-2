using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueUpdate : MonoBehaviour {
    public Text text;

    private Slider slider;

    void Awake() {
        slider = GetComponent<Slider>();
    }

    void OnEnable() {
        slider.onValueChanged.AddListener(ChangeValue);
        ChangeValue(slider.value);
    }

    void OnDisable() {
        slider.onValueChanged.RemoveAllListeners();
    }

    void ChangeValue(float value) {
        text.text = value.ToString();
        GameManager.squashRate = value;
        EventManager.TriggerEvent("squashValueChanged");
    }
}
