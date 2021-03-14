using System.Collections;
using System.Collections.Generic;
using DTWorldz.ProceduralGeneration;
using UnityEngine;
using static DTWorldz.Behaviours.ProceduralMapGenerators.NoiseMap.NoiseMapGenerator;

namespace DTWorldz.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MapTemplate", menuName = "ScriptableObjects/MapTemplate", order = 1)]
    public class MapTemplate : ScriptableObject
    {
        public DrawMode DrawingMode;
        public int Seed = 1;
        public int Width = 100;
        public int Height = 100;
        public float Scale = 0.3f;
        public int Octaves = 4;
        [Range(0, 1)]
        public float Persistance = 0.5f;
        public float Lacunarity = 2;
        public bool IsIsland = true;
        public Texture2D IslandHeightMapTexture;
        public float LandIntensisty = 4;
        public TerrainType[] Regions;

        public Vector2 OffSet = new Vector2(0, 0);
    }
}