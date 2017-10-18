using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Loader : MonoBehaviour {
    public Texture2D noise = null;

    // Use this for initialization
    void Start () {
        //Texture2D tex;
        //tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        //string path = Application.dataPath;
        //tex = LoadPNG("./PlayerController/Textures/T_Noise.png");
        noise.LoadImage(noise.GetRawTextureData());
        Color colorPixel = noise.GetPixel(0, 0);
        //Debug.Log("color pixel : " + colorPixel.ToString());
        Debug.Log("color pixel : " + noise.format);
    }

    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
}
