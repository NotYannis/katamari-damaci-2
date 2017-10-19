using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnButton : MonoBehaviour
{
    public GameObject BOff;

    public void OnClikButton()
    {
        Debug.Log("toto");
        Button bOff = BOff.GetComponent<Button>();
        ColorBlock cbOff = bOff.colors;
        cbOff.normalColor = new Color((float)128 / 255, (float)128 / 255, (float)128 / 255, (float)150 / 255);
        bOff.colors = cbOff;
        Button bOn = GetComponent<Button>();
        ColorBlock cbOn = bOn.colors;
        cbOn.normalColor = new Color((float)100 / 255, (float)146 / 255, (float)210 / 255);
        cbOn.highlightedColor = new Color((float)100 / 255, (float)146 / 255, (float)210 / 255);
        bOn.colors = cbOn;

    }
}
