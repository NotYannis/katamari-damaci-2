using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    public float speedBall=1;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    public void SetSpeed(Slider speed)
    {
        speedBall = speed.value*20;
        Debug.Log(speedBall);
    }
}
