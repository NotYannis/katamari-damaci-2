using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    public float speedBall=8;
    public float speedCamera=10;
    public bool mode = true;
    public bool postproc = true;
    public bool axe = true;
    public bool controle = true;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    public void SetSpeed(Slider speed)
    {
        speedBall = speed.value*20;
        if (speedBall < 3) speedBall = 3;
    }
    public void SetSpeedCamera(Slider speed)
    {
        speedCamera = speed.value * 20;
        if (speedCamera < 3) speedCamera = 3;
        Debug.Log(speedCamera);
    }

    public void SetMode(bool bmode)
    {
        mode = bmode;
    }

    public void SetPostProc(Toggle bproc)
    {
        postproc = bproc.isOn;
    }

    public void SetAxe(Toggle baxe)
    {
        axe = baxe.isOn;
    }

    public void SetControle(bool bcontrole)
    {
        controle = bcontrole;
    }
}
