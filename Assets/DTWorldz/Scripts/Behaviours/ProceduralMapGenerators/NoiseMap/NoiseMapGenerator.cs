using System.Collections;
using System.Collections.Generic;
using DTWorldz.ProceduralGeneration;
using DTWorldz.ScriptableObjects;
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
        //public TerrainType[] Regions;
        public TileMapTerrain[] Terrains;
        public Vector2 OffSet = new Vector2(0, 0);

        public Transform TreesParentObject;
        public Transform BushesParentObject;
        public bool autoUpdate;
        public bool placeTrees;
        public bool placeBushes;

        public System.Random GenerateMap()
        {
            ClearTileMap();

            var prng = new System.Random(Seed);

            float[,] noiseMap = Noise.GenerateNoiseMap(prng, Width, Height, Scale, Octaves, Persistance, Lacunarity, OffSet, IsIsland, IslandHeightMapTexture, LandIntensisty);

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
                mapDisplay.DrawTileMap(Terrains, noiseMap, Width, Height);
            }

            if (placeTrees)
            {
                PlaceTrees(prng);
            }

            if (placeBushes)
            {
                PlaceBushes(prng);
            }
            return prng;
        }

        public void PlaceBushes(System.Random prng)
        {
            ClearBushes();

            if (BushesParentObject == null)
            {
                return;
            }

            foreach (var terrain in Terrains)
            {
                if (terrain.Template.BushFrequency == 0f || terrain.Template.BushTypes.Count == 0)
                {
                    continue;
                }

                var gridLayout = terrain.Tilemap.transform.GetComponentInParent<GridLayout>();

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        var chance = prng.NextDouble();
                        if (chance < terrain.Template.BushFrequency)
                        {
                            var regionTile = terrain.Tilemap.GetTile(new Vector3Int(x, y, 0));
                            if (regionTile != null)
                            {
                                var cellPosition = gridLayout.CellToWorld(new Vector3Int(x, y, 0));
                                cellPosition = new Vector3(cellPosition.x + 0.5f, cellPosition.y + 0.5f, 0);
                                var bush = Instantiate(terrain.Template.BushTypes[prng.Next(0, terrain.Template.BushTypes.Count)], cellPosition, Quaternion.identity, BushesParentObject);
                                bush.transform.localScale = new Vector3(UnityEngine.Random.Range(0.9f, 1.1f), UnityEngine.Random.Range(0.75f, 1.25f), 1f);
                            }
                        }
                    }
                }
            }
        }
        public void ClearBushes()
        {
            if (BushesParentObject == null)
            {
                return;
            }

            for (int i = BushesParentObject.childCount - 1; i >= 0; i--)
            {
                Transform child = BushesParentObject.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
        }

        public void PlaceTrees(System.Random prng)
        {
            ClearTrees();

            if (TreesParentObject == null)
            {
                return;
            }

            foreach (var terrain in Terrains)
            {
                if (terrain.Template.TreeFrequency == 0f || terrain.Template.TreeTypes.Count == 0)
                {
                    continue;
                }
                var gridLayout = terrain.Tilemap.transform.GetComponentInParent<GridLayout>();

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        var chance = prng.NextDouble();
                        if (chance < terrain.Template.TreeFrequency)
                        {
                            var terrainTile = terrain.Tilemap.GetTile(new Vector3Int(x, y, 0));
                            if (terrainTile != null)
                            {
                                var cellPosition = gridLayout.CellToWorld(new Vector3Int(x, y, 0));
                                cellPosition = new Vector3(cellPosition.x + 0.5f, cellPosition.y + 0.5f, 0);
                                var tree = Instantiate(terrain.Template.TreeTypes[prng.Next(0, terrain.Template.TreeTypes.Count)], cellPosition, Quaternion.identity, TreesParentObject);
                                UnityEngine.Random.InitState(prng.Next());
                                tree.transform.localScale = new Vector3(UnityEngine.Random.Range(1f, 1.5f), UnityEngine.Random.Range(1f, 1.5f), 1f);
                            }
                        }
                    }
                }
            }
        }

        public void ClearTrees()
        {
            if (TreesParentObject == null)
            {
                return;
            }

            for (int i = TreesParentObject.childCount - 1; i >= 0; i--)
            {
                Transform child = TreesParentObject.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
        }

        public void ClearTileMap()
        {
            var mapDisplay = MapDisplay.FindObjectOfType<MapDisplay>();
            mapDisplay.ClearTileMap(Terrains);
        }

        private Color[] GetColorMap(float[,] noiseMap)
        {
            var colorMap = new Color[Width * Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float currentHeight = noiseMap[x, y];
                    for (int r = 0; r < Terrains.Length; r++)
                    {
                        if (currentHeight <= Terrains[r].Template.Height)
                        {
                            colorMap[y * Width + x] = Terrains[r].Template.Color;
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
                    for (int r = 0; r < Terrains.Length; r++)
                    {
                        if (currentHeight <= Terrains[r].Template.Height)
                        {
                            tileMap[y * Width + x] = Terrains[r].Template.Tile;
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