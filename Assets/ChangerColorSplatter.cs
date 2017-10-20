using SplatterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerColorSplatter : MonoBehaviour {

    public SplatterSettings[] splatterColors = new SplatterSettings[4];
	// Use this for initialization
	void Start () {
		
	}
	
    public void ChangeColor(TerrainType terrainType)
    {
        GetComponent<MeshSplatterManager>().defaultSettings = splatterColors[(int)terrainType];
    }
}
