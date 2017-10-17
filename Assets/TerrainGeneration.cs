using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise.Generator;


public interface INoiseProvider
{
    float GetValue(float x, float z);
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
    public int X { get; private set; }
    public int Z { get; private set; }

    private Terrain Terrain { get; set; }
    private TerrainChunkSettings Settings { get; set; }

    private NoiseProvider NoiseProvider { get; set; }

    public TerrainChunk(TerrainChunkSettings _settings, NoiseProvider _noise, int _x, int _z)
    {
        Settings = _settings;
        NoiseProvider = _noise;
        X = _x;
        Z = _z;
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
        terrainGameObject.transform.position = new Vector3(X * Settings.Length, 0.0f, Z * Settings.Length);
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
                var xCoordinate = X + (float)xRes / (Settings.HeightmapResolution - 1);
                var zCoordinate = Z + (float)zRes / (Settings.HeightmapResolution - 1);

                heightmap[zRes, xRes] = NoiseProvider.GetValue(xCoordinate, zCoordinate);
            }
        }

        return heightmap;
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
    public int terrainSize;
    private TerrainChunkSettings settings;

	// Use this for initialization
	void Start () {
        settings = new TerrainChunkSettings(129, 129, 100, 0);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.G))
        {
            Generate();
        }
	}
    
    void Generate()
    {
        var noiseProvider = new NoiseProvider();
        
        for(int i = 0; i <4; ++i)
        {
            for(int j = 0; j < 4; ++j)
            {
                var terrain = new TerrainChunk(settings, noiseProvider, i, j);
                terrain.CreateTerrain();
            }
        }
    }

    void Test()
    {
        var settings = new TerrainChunkSettings(129, 129, 100, 0);
        var noiseProvider = new NoiseProvider();
        var terrain = new TerrainChunk(settings, noiseProvider, 0, 0);
        terrain.CreateTerrain();
    }

    private List<Vector3Int> GetChunckPositionInRadius(Vector3Int position, int radius)
    {
        List<Vector3Int> result = new List<Vector3Int>();

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

    public void UpdateTerrain(Vector3 worldPosition, int radius)
    {
    }


    public Vector3Int GetChunkPosition(Vector3 worldPosition)
    {
        var x = (int)Mathf.Floor(worldPosition.x / settings.Length);
        var z = (int)Mathf.Floor(worldPosition.z / settings.Length);

        return new Vector3Int(x, 0, z);
    }

}
