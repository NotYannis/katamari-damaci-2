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
            SoundManager.instance.SetCycle("day");
        }
        else
        {
            RenderSettings.skybox = nightBox;
            SoundManager.instance.SetCycle("night");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
