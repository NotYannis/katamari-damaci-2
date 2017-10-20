using SplatterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerColorSplatter : MonoBehaviour {

    public TerrainGeneration terrain;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (terrain.IsOnNewChunkPosition())
        {
            Vector3Int playerpos = terrain.GetChunkPosition(terrain.player.transform.position);
            TerrainChunk chunk;
            terrain.chunkLoaded.TryGetValue(playerpos, out chunk);
            if (chunk != null)
            {
                switch (chunk.type)
                {
                    case TerrainType.Dirt:
                        GetComponent<MeshSplatterManager>().defaultSettings.colors[0] = new Color((float)225 / 255, (float)188 / 255, (float)164 / 255);
                        Debug.Log("color dirt !!!");
                        break;
                    case TerrainType.Grass:
                        GetComponent<MeshSplatterManager>().defaultSettings.colors[0] = new Color((float)(135 / 255), (float)(251 / 255), (float)(145 / 255));
                        Debug.Log("color grass !!!");
                        break;
                    case TerrainType.Water:
                        GetComponent<MeshSplatterManager>().defaultSettings.colors[0] = new Color((float)(104 / 255), (float)(145 / 255), (float)(244 / 255));
                        Debug.Log("color water !!!");
                        ;
                        break;
                    case TerrainType.Leaves:
                        GetComponent<MeshSplatterManager>().defaultSettings.colors[0] = new Color((float)(165 / 255), (float)(128 / 255), (float)(87 / 255));
                        Debug.Log("color leaves !!!");
                        ;
                        break;
                }
            }
        }
    }
}
