using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEntities : MonoBehaviour {

	public List<GameObject> entities = new List<GameObject>();
	public GameObject terrainParent;
	public GameObject player;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 minT, maxT;
		foreach (KeyValuePair<Vector3Int, TerrainChunk> t in terrainParent.GetComponent<TerrainGeneration>().chunkLoaded) 
		{
			if (!t.Value.entitiesLoaded) {
				Vector3 boundsSize = t.Value.Terrain.GetComponent<MeshFilter> ().mesh.bounds.size;
				minT = new Vector2 (t.Value.Terrain.transform.position.x - boundsSize.x/2, t.Value.Terrain.transform.position.z - boundsSize.z/2);
				maxT = new Vector2 (t.Value.Terrain.transform.position.x + boundsSize.x/2, t.Value.Terrain.transform.position.z + boundsSize.z/2);
				foreach (GameObject e in entities) {
					for (int i = 0; i < 1; i++) {
						GameObject g = Instantiate (e);
						g.transform.position = new Vector3 (Random.Range (minT.x, maxT.x), t.Value.Terrain.transform.position.y, Random.Range (minT.y, maxT.y));
						g.transform.SetParent (t.Value.Terrain.transform);
					}
				}
				t.Value.entitiesLoaded = true;
			}
		}
	}
}
