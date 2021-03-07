using System.Collections;
using System.Collections.Generic;
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
                }
            }
        }
        public void ClearTileMap()
        {
            TileMap.ClearAllTiles();
        }
    }
}