using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public GameObject optionsPanel;

    void Update () {
	    if(Input.GetKeyDown(KeyCode.P)) {
            if(optionsPanel.activeSelf) {
                optionsPanel.SetActive(false);
            } else {
                optionsPanel.SetActive(true);
            }
        }	
	}
}
