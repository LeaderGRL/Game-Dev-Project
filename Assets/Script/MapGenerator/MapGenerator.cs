using System;
using UnityEngine;

namespace Script.MapGenerator
{
    public class MapGenerator : MonoBehaviour
    {
        public enum DrawMode
        {
            NoiseMap,
            ColourMap,
            Mesh
        }

        public DrawMode drawMode;

        public const int mapChunkSize = 241;
        [Range(0, 6)] [SerializeField] private int levelOfDetail;
        [SerializeField] private float noiseScale;
        [SerializeField] private int octaves;
        [Range(0, 1)] [SerializeField] private float persistance;
        [SerializeField] private float lacunarity;

        [SerializeField] private int seed;
        [SerializeField] private Vector2 offset;

        [SerializeField] private float meshHeightMultiplier;

        [SerializeField] private AnimationCurve meshHeightCurve;

        public bool autoUpdate;

        public TerrainType[] regions;

        public void GenerateMap()
        {
            float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves,
                persistance,
                lacunarity, offset);

            Color[] colorMap = new Color[mapChunkSize * mapChunkSize];

            for (int y = 0; y < mapChunkSize; y++)
            {
                for (int x = 0; x < mapChunkSize; x++)
                {
                    float currentHeight = noiseMap[x, y];
                    for (int i = 0; i < regions.Length; i++)
                    {
                        if (currentHeight <= regions[i].height)
                        {
                            colorMap[y * mapChunkSize + x] = regions[i].color;
                            break;
                        }
                    }
                }
            }

            MapDisplay display = FindObjectOfType<MapDisplay>();
            if (drawMode == DrawMode.NoiseMap)
                display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
            else if (drawMode == DrawMode.ColourMap)
                display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
            else if (drawMode == DrawMode.Mesh)
                display.DrawMesh(
                    MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail),
                    TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }

        public void OnValidate()
        {
            if (lacunarity < 1)
            {
                lacunarity = 1;
            }

            if (octaves < 0)
            {
                octaves = 0;
            }
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}