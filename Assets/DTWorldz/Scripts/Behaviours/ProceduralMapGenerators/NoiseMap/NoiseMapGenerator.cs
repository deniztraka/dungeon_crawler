using System.Collections;
using System.Collections.Generic;
using DTWorldz.ProceduralGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTWorldz.Behaviours.ProceduralMapGenerators.NoiseMap
{
    public class NoiseMapGenerator : MonoBehaviour
    {
        public enum DrawMode { NoiseMap, ColorMap, TileMap }
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
        public bool autoUpdate;

        public void GenerateMap()
        {
            float[,] noiseMap = Noise.GenerateNoiseMap(Seed, Width, Height, Scale, Octaves, Persistance, Lacunarity, OffSet, IsIsland, IslandHeightMapTexture, LandIntensisty);

            var mapDisplay = MapDisplay.FindObjectOfType<MapDisplay>();
            if (DrawingMode == DrawMode.NoiseMap)
            {
                mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
            }
            else if (DrawingMode == DrawMode.ColorMap)
            {
                mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(GetColorMap(noiseMap), Width, Height));
            }
            else if (DrawingMode == DrawMode.TileMap)
            {
                mapDisplay.DrawTileMap(GetTileMap(noiseMap), Width, Height);
            }
        }

        public void ClearTileMap()
        {
            var mapDisplay = MapDisplay.FindObjectOfType<MapDisplay>();
            mapDisplay.ClearTileMap();
        }

        private Color[] GetColorMap(float[,] noiseMap)
        {
            var colorMap = new Color[Width * Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float currentHeight = noiseMap[x, y];
                    for (int r = 0; r < Regions.Length; r++)
                    {
                        if (currentHeight <= Regions[r].Height)
                        {
                            colorMap[y * Width + x] = Regions[r].Color;
                            break;
                        }
                    }
                }
            }

            return colorMap;
        }

        private Tile[] GetTileMap(float[,] noiseMap)
        {
            var tileMap = new Tile[Width * Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float currentHeight = noiseMap[x, y];
                    for (int r = 0; r < Regions.Length; r++)
                    {
                        if (currentHeight <= Regions[r].Height)
                        {
                            tileMap[y * Width + x] = Regions[r].Tile;
                            break;
                        }
                    }
                }
            }

            return tileMap;
        }

        void OnValidate()
        {
            if (Width < 1)
            {
                Width = 1;
            }
            if (Height < 1)
            {
                Height = 1;
            }
            if (Lacunarity < 1)
            {
                Lacunarity = 1;
            }
            if (Octaves < 0)
            {
                Octaves = 0;
            }
        }
    }
}