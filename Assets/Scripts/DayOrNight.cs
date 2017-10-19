using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayOrNight : MonoBehaviour {

    public Material dayBox;
    public Material nightBox;

	// Use this for initialization
	void Start () {
        if(Random.Range(0, 2) == 0)
        {
            RenderSettings.skybox = dayBox;
        }
        else
        {
            RenderSettings.skybox = nightBox;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
