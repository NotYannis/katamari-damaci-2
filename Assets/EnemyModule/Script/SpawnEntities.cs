using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEntities : MonoBehaviour {

	public List<GameObject> entities = new List<GameObject>();
	public GameObject terrainParent;

	// Use this for initialization
	void Start () 
	{
		Vector2 minT, maxT;
		foreach (Terrain t in terrainParent.GetComponentsInChildren<Terrain>()) 
		{
			minT = new Vector2 (t.transform.position.x+t.terrainData.size.x, t.transform.position.z+t.terrainData.size.z);
			maxT = new Vector2 (t.transform.position.x, t.transform.position.z);
			Debug.Log (t.name + " : " + minT + " / " + maxT);
			foreach (GameObject e in entities) 
			{
				for (int i = 0; i < 10; i++) 
				{
					GameObject g = Instantiate (e);
					g.transform.position = new Vector3 (Random.Range (minT.x, maxT.x), 0, Random.Range (minT.y, maxT.y));
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
