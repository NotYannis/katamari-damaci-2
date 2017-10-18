using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    static public float squashRate = 0.05f;
    private float _lastSquashRate = 0.05f;

    void Update() {
        
        if (squashRate != _lastSquashRate) {
            EventManager.TriggerEvent("squashValueChanged");
        }

        _lastSquashRate = squashRate;
    }
}
