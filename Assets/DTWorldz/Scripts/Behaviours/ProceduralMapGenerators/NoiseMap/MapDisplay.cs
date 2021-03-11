using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.ProceduralGeneration;
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
            //TextureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
        }

        public void DrawTileMap(Tile[] tileMapAssets, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    TileMap.SetTile(new Vector3Int(x, y, 0), tileMapAssets[y * width + x]);
                    //yield return new WaitForSeconds(0.1f);
                }
            }
        }

        public void  DrawTileMap(TerrainType[] regions, float[,] noiseMap, int width, int height)
        {
            bool[,] isSetMap = new bool[width, height];
            
            Array.Sort(regions, (a, b) => a.Height.CompareTo(b.Height));

            for (int i = 0; i < regions.Length; i++)
            {
                var region = regions[i];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        float currentHeight = noiseMap[x, y];
                        if (!isSetMap[x,y] && currentHeight <= region.Height)
                        {
                            region.Tilemap.SetTile(new Vector3Int(x, y, 0), region.Tile);
                            isSetMap[x,y] = true;
                            //yield return new WaitForSeconds(0);
                        }                        
                    }
                }
            }
        }
        public void ClearTileMap(TerrainType[] regions)
        {
            for (int i = 0; i < regions.Length; i++)
            {
                var region = regions[i];
                region.Tilemap.ClearAllTiles();
            }
            
        }
    }
}