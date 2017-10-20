using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    public float speedBall=8;
    public float speedCamera=10;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    public void SetSpeed(Slider speed)
    {
        speedBall = speed.value*20;
        if (speedBall < 3) speedBall = 3;
        Debug.Log(speedBall);
    }
    public void SetSpeedCamera(Slider speed)
    {
        speedCamera = speed.value * 20;
        if (speedCamera < 3) speedCamera = 3;
        Debug.Log(speedCamera);
    }
}
