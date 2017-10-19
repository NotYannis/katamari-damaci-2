using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetourButton : MonoBehaviour {
    //public GameObject monGameObject;
    public Canvas CanvasMenu, CanvasPause, CanvasOption;
    bool inGame;
    // Use this for initialization
    void Start () {
        /*monGameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate () {
            if (inGame)
            {
                //active pause;
            }
            else
            {
                //active menu principal
            }
        });*/
        }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetinGame(bool val)
    {
        inGame = val;
    }
    public void Retour()
    {
        
        if (inGame)
        {
            CanvasOption.gameObject.SetActive(false);
            CanvasPause.gameObject.SetActive(true);
        } else
        {
            
            CanvasOption.gameObject.SetActive(false);
            CanvasMenu.gameObject.SetActive(true);
        }
    }
}
