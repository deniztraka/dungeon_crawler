using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.ProceduralGeneration;
using DTWorldz.ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTWorldz.Behaviours.ProceduralMapGenerators.NoiseMap
{
    public class MapDisplay : MonoBehaviour
    {
        public Renderer TextureRenderer;
        public Tilemap TileMap;
        public void DrawTexture(Texture2D texture)
        {
            TextureRenderer.sharedMaterial.mainTexture = texture;
        }

        public void DrawTileMap(Tile[] tileMapAssets, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    TileMap.SetTile(new Vector3Int(x, y, 0), tileMapAssets[y * width + x]);
                }
            }
        }

        public void DrawTileMap(TileMapTerrain[] terrains, float[,] noiseMap, int width, int height)
        {
            //bool[,] isSetMap = new bool[width, height];

            Array.Sort(terrains, (a, b) => a.Template.Height.CompareTo(b.Template.Height));

            bool[] isSetMap = new bool[width * height];

            // Assuming terrains are sorted by height
            for (int i = 0; i < terrains.Length; i++)
            {
                var terrain = terrains[i];
                if (terrain.Template.name == "WaterShadow")
                {
                    continue;
                }

                bool allAboveCurrentTerrain = true;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int index = y * width + x;
                        if (!isSetMap[index])
                        {
                            float currentHeight = noiseMap[x, y];
                            if (currentHeight <= terrain.Template.Height)
                            {
                                terrain.Tilemap.SetTile(new Vector3Int(x, y, 0), terrain.Template.Tile);
                                isSetMap[index] = true;
                            }
                            else
                            {
                                allAboveCurrentTerrain = false;
                            }
                        }
                    }
                }

                if (allAboveCurrentTerrain)
                {
                    break;
                }
            }


        }
        public void ClearTileMap(TileMapTerrain[] terrains)
        {
            for (int i = 0; i < terrains.Length; i++)
            {
                var terrain = terrains[i];
                terrain.Tilemap.ClearAllTiles();
            }

        }
    }
}