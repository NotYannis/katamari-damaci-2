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

    public TerrainChunkSettings(int _heightmap, int _alphamap, int _length, int _height)
    {
        HeightmapResolution = _heightmap;
        AlphamapResolution = _alphamap;
        Length = _length;
        Height = _height;
    }
}

public class TerrainChunk {
    public Vector2i Position { get; private set; }

    private Terrain Terrain { get; set; }
    private TerrainChunkSettings Settings { get; set; }
    private TerrainChunkNeighborhood neighbors;

    private NoiseProvider NoiseProvider { get; set; }

    public TerrainChunk(TerrainChunkSettings _settings, NoiseProvider _noise, int _x, int _z)
    {
        Settings = _settings;
        NoiseProvider = _noise;
        Position = new Vector2i(_x, _z);
    }

    public void CreateTerrain()
    {
        TerrainData terrainData = new TerrainData()
        {
            heightmapResolution = Settings.HeightmapResolution,
            alphamapResolution = Settings.AlphamapResolution,
            size = new Vector3(Settings.Length, Settings.Height, Settings.Length),
        };

        GameObject terrainGameObject = Terrain.CreateTerrainGameObject(terrainData);
        terrainGameObject.transform.position = new Vector3(Position.X * Settings.Length, 0.0f, Position.Z * Settings.Length);
        Terrain = terrainGameObject.GetComponent<Terrain>();
        Terrain.Flush();
    }

    private float[,] GetHeightmap()
    {
        var heightmap = new float[Settings.HeightmapResolution, Settings.HeightmapResolution];

        for (var zRes = 0; zRes < Settings.HeightmapResolution; zRes++)
        {
            for (var xRes = 0; xRes < Settings.HeightmapResolution; xRes++)
            {
                var xCoordinate = Position.X + (float)xRes / (Settings.HeightmapResolution - 1);
                var zCoordinate = Position.Z + (float)zRes / (Settings.HeightmapResolution - 1);

                heightmap[zRes, xRes] = NoiseProvider.GetValue(xCoordinate, zCoordinate);
            }
        }

        return heightmap;
    }

    public void SetNeighbors(TerrainChunk neighbor, TerrainNeighbor pos)
    {
        switch (pos)
        {
            case TerrainNeighbor.XDown:
                neighbors.XDown = neighbor;
                break;
            case TerrainNeighbor.XUp:
                neighbors.XUp = neighbor;
                break;
            case TerrainNeighbor.ZDown:
                neighbors.ZDown = neighbor;
                break;
            case TerrainNeighbor.ZUp:
                neighbors.ZUp = neighbor;
                break;
        }
    }

    public void RemoveTerrain()
    {
        Settings = null;

        if(Terrain != null)
            GameObject.Destroy(Terrain.gameObject);
    }
}

public class NoiseProvider : INoiseProvider
{
    private Perlin PerlinNoiseGenerator;

    public NoiseProvider()
    {
        PerlinNoiseGenerator = new Perlin();
    }

    public float GetValue(float x, float z)
    {
        return (float)(PerlinNoiseGenerator.GetValue(x, 0, z) / 2f) + 0.5f;
    }
}

public class TerrainGeneration : MonoBehaviour {
    private int terrainSize;
    public GameObject player;
    private Vector3Int position;
    private Vector3Int lastPosition;
    private TerrainChunkSettings settings;

    private Dictionary<Vector3Int, TerrainChunk> chunkLoaded = new Dictionary<Vector3Int, TerrainChunk>();
    private Dictionary<Vector3Int, TerrainChunk> requestedChunks = new Dictionary<Vector3Int, TerrainChunk>();
    private Dictionary<Vector3Int, TerrainChunk> removeChunk = new Dictionary<Vector3Int, TerrainChunk>();

    // Use this for initialization
    void Start () {
        settings = new TerrainChunkSettings(129, 129, 100, 0);
        lastPosition = Vector3Int.down;
        position = GetChunkPosition(player.transform.position);
        GetChunks(player.transform.position, 5);
        Generate();
    }

    // Update is called once per frame
    void Update () {
        if (IsOnNewChunkPosition())
        {
            GetChunks(player.transform.position, 5);
            Generate();
        }
	}

    void GenerateChunk(int x, int z)
    {
        var noiseProvider = new NoiseProvider();
        
        var terrain = new TerrainChunk(settings, noiseProvider, x, z);
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
            var noiseProvider = new NoiseProvider();

            var chunk = new TerrainChunk(settings, noiseProvider, pos.x, pos.z);

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

    public void UpdateTerrain(Vector3 worldPosition, int radius)
    {
        Vector3Int worldPositionInt = new Vector3Int((int)worldPosition.x, (int)worldPosition.y, (int)worldPosition.z);
        var chunks = GetChunckPositionInRadius(worldPositionInt, radius);

        foreach(Vector3Int pos in chunks)
        {
            GenerateChunk(pos.x, pos.z);
        }
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
        position = GetChunkPosition(player.transform.position);

        if(lastPosition != position)
        {
            result = true;
        }

        return result;
    }

    private void SetChunkNeighborhood(TerrainChunk chunk)
    {
        TerrainChunk xUp;
        TerrainChunk xDown;
        TerrainChunk zUp;
        TerrainChunk zDown;

        chunkLoaded.TryGetValue(new Vector3Int(chunk.Position.X + 1, 0, chunk.Position.Z), out xUp);
        chunkLoaded.TryGetValue(new Vector3Int(chunk.Position.X - 1, 0, chunk.Position.Z), out xDown);
        chunkLoaded.TryGetValue(new Vector3Int(chunk.Position.X, 0, chunk.Position.Z + 1), out zUp);
        chunkLoaded.TryGetValue(new Vector3Int(chunk.Position.X, 0, chunk.Position.Z - 1), out zDown);

        if (xUp != null)
        {
            chunk.SetNeighbors(xUp, TerrainNeighbor.XUp);
            xUp.SetNeighbors(chunk, TerrainNeighbor.XDown);
        }
        if (xDown != null)
        {
            chunk.SetNeighbors(xDown, TerrainNeighbor.XDown);
            xDown.SetNeighbors(chunk, TerrainNeighbor.XUp);
        }
        if (zUp != null)
        {
            chunk.SetNeighbors(zUp, TerrainNeighbor.ZUp);
            zUp.SetNeighbors(chunk, TerrainNeighbor.ZDown);
        }
        if (zDown != null)
        {
            chunk.SetNeighbors(zDown, TerrainNeighbor.ZDown);
            zDown.SetNeighbors(chunk, TerrainNeighbor.ZUp);
        }
    }

}
