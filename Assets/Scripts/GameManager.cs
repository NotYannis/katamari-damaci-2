using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject pauseMenu;

    static public float squashRate = 0.5f;
    private float _lastSquashRate = 0.5f;
    
    void Update() {
        
        if(Input.GetKeyDown(KeyCode.P)) {
            if(pauseMenu.activeSelf) {
                pauseMenu.SetActive(false);
            } else {
                pauseMenu.SetActive(true);
            }
        }

        if (squashRate != _lastSquashRate) {
            EventManager.TriggerEvent("squashValueChanged");
        }

        _lastSquashRate = squashRate;
    }
}
