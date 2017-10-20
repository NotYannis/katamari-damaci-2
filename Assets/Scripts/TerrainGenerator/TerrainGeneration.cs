using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise.Generator;
using System.Linq;

public interface INoiseProvider
{
	float GetValue(float x, float z);
}

public class TerrainChunkNeighborhood
{
	public TerrainChunk XUp { get; set; }

	public TerrainChunk XDown { get; set; }

	public TerrainChunk ZUp { get; set; }

	public TerrainChunk ZDown { get; set; }
}

public enum TerrainNeighbor
{
	XUp,
	XDown,
	ZUp,
	ZDown
}

public class TerrainChunkSettings
{
	public int HeightmapResolution { get; private set; }
	public int AlphamapResolution { get; private set; }

	public int Length { get; private set; }
	public int Height { get; private set; }
	public GameObject parentTerrain;

	public TerrainChunkSettings(int _heightmap, int _alphamap, int _length, int _height, GameObject _parentTerrain)
	{
		HeightmapResolution = _heightmap;
		AlphamapResolution = _alphamap;
		Length = _length;
		Height = _height;
		parentTerrain = _parentTerrain;
	}
}

public class TerrainChunk {
	public Vector3Int Position { get; private set; }

	public GameObject Terrain { get; set; }
	private TerrainChunkSettings Settings { get; set; }

	public bool entitiesLoaded = false;

    public TerrainType type;

	public TerrainChunk(TerrainChunkSettings _settings, TerrainType _type, int _x, int _z, GameObject _terrain)
	{
		Settings = _settings;
		Position = new Vector3Int(_x, 0, _z);
		Terrain = _terrain;
        type = _type;
	}

	public void CreateTerrain()
	{
		Terrain = GameObject.Instantiate(Terrain, new Vector3(Position.x * Settings.Length, 0.0f, Position.z * Settings.Length), Quaternion.identity);
		Terrain.transform.SetParent (Settings.parentTerrain.transform);
	}

	public void RemoveTerrain()
	{
		Settings = null;

		if(Terrain != null)
			GameObject.Destroy(Terrain.gameObject);
	}
}

public enum TerrainType
{
    Dirt, Grass, Leaves, Water
}

public class TerrainGeneration : MonoBehaviour {
	public int terrainSize = 4;
	public GameObject player;
	private Vector3Int position;
	private Vector3Int lastPosition;
	private TerrainChunkSettings settings;
    private TerrainType lastChunkType;

	internal Dictionary<Vector3Int, TerrainChunk> chunkLoaded = new Dictionary<Vector3Int, TerrainChunk>();
	private Dictionary<Vector3Int, TerrainChunk> requestedChunks = new Dictionary<Vector3Int, TerrainChunk>();
	private Dictionary<Vector3Int, TerrainChunk> removeChunk = new Dictionary<Vector3Int, TerrainChunk>();

	public GameObject[] terrains = new GameObject[4];

	// Use this for initialization
	void Start () {
		settings = new TerrainChunkSettings(129, 129, 100, 0, this.gameObject);
		lastPosition = Vector3Int.down;
		position = GetChunkPosition(player.transform.position);
		GetChunks(player.transform.position, terrainSize);
		Generate();
	}

	// Update is called once per frame
	void Update () {
		if (IsOnNewChunkPosition())
		{
			GetChunks(player.transform.position, terrainSize);
			Generate();

            Vector3Int playerpos = new Vector3Int((int)(player.transform.position.x + settings.Length / 2), (int)player.transform.position.y, (int)(player.transform.position.z - settings.Length / 2));
            playerpos = GetChunkPosition(transform.position);
            print("Player :" + playerpos);
            TerrainChunk chunk;
            chunkLoaded.TryGetValue(playerpos, out chunk);
            if(chunk != null)
            {
                print("chunk" + chunk.Terrain.transform.position);
                if(chunk.type != lastChunkType){
                    switch (chunk.type)
                    {
                        case TerrainType.Dirt:
                        case TerrainType.Grass:
                            EventManager.TriggerEvent("OnPlayerEnterGrass");
                            break;
                        case TerrainType.Water:
                            EventManager.TriggerEvent("OnPlayerEnterWater");
                            break;
                        case TerrainType.Leaves:
                            EventManager.TriggerEvent("OnPlayerEnterLeaf");
                            break;
                    }
                }
                lastChunkType = chunk.type;
            }

		}
	}

	TerrainChunk GenerateChunk(int x, int z)
	{
        int terrainType = Random.Range(0, terrains.Length);


		var terrain = new TerrainChunk(settings, (TerrainType)terrainType, x, z, terrains[terrainType]);

		return terrain;
	}

	private List<Vector3Int> GetChunckPositionInRadius(Vector3Int position, int radius)
	{
		List<Vector3Int> result = new List<Vector3Int>();
		position = GetChunkPosition(position);

		for(int z = -radius; z <= radius; ++z)
		{
			for(int x = -radius; x <= radius; ++x)
			{
				if(x * x + z * z < radius * radius)
				{
					result.Add(new Vector3Int(position.x + x, 0, position.z + z));
				}
			}
		}

		return result;
	}

	public void GetChunks(Vector3 worldPosition, int radius)
	{
		Dictionary<Vector3Int, TerrainChunk> toRemove = new Dictionary<Vector3Int, TerrainChunk>();

		Vector3Int worldPositionInt = new Vector3Int((int)worldPosition.x, (int)worldPosition.y, (int)worldPosition.z);
		var chunksPosition = GetChunckPositionInRadius(worldPositionInt, radius);

		foreach (Vector3Int pos in chunksPosition)
		{
			var chunk = GenerateChunk(pos.x, pos.z);

			if (!chunkLoaded.ContainsKey(pos))
			{
				requestedChunks.Add(pos, chunk);
				chunkLoaded.Add(pos, chunk);
			}

			toRemove.Add(pos, chunk);
		}

		removeChunk = chunkLoaded.Where(x => !toRemove.ContainsKey(x.Key))
			.ToDictionary(x => x.Key, x => x.Value);
	}

	private void Generate()
	{
		foreach(var chunks in requestedChunks)
		{
			chunks.Value.CreateTerrain();
		}

		requestedChunks.Clear();

		foreach (var chunks in removeChunk)
		{
			chunks.Value.RemoveTerrain();
			chunkLoaded.Remove(chunks.Key);
		}

		removeChunk.Clear();
	}


	public Vector3Int GetChunkPosition(Vector3 worldPosition)
	{
		var x = (int)Mathf.Floor(worldPosition.x / settings.Length);
		var z = (int)Mathf.Floor(worldPosition.z / settings.Length);
		return new Vector3Int(x, 0, z);
	}

	public bool IsOnNewChunkPosition()
	{
		bool result = false;

		lastPosition = position;
        position = new Vector3Int((int)(player.transform.position.x - settings.Length / 2), (int)player.transform.position.y, (int)(player.transform.position.z - settings.Length / 2));
		position = GetChunkPosition(position);

        if (lastPosition != position)
		{
			result = true;
		}

		return result;
	}


}