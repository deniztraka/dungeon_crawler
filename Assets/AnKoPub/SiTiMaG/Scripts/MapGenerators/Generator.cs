using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Generator
{
    public int Width, Height, Scale;
    public bool[,] MapData;
    public List<GameObject> WallObjects;
    public Tilemap FloorMap;
    public Tilemap WallMap;
    public TileBase FloorTile;
    public TileBase WallTile;

    public virtual void InitialiseMap()
    {
        for (int i = 0; i < Width; i++)
            for (int j = 0; j < Height; j++)
                MapData[j, i] = true;
    }

    public virtual void NewMap()
    {
        InitialiseMap();
        BuildMap();
    }

    public virtual void GenerateMap() { }

    public virtual void BuildMap()
    {
        GenerateMap();
        FloorMap.BoxFill(new Vector3Int(Width - 1, Height - 1, 0), FloorTile, 0, 0, Width, Height);
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                if (MapData[y, x])
                {
                    FloorMap.SetTile(new Vector3Int(x, y, 0), null);
                    WallMap.SetTile(new Vector3Int(x, y, 0), WallTile);
                }
    }
    
    //public void ClearMap()
    //{
    //    var maps = GameObject.FindObjectsOfType(typeof(Tilemap));
    //    foreach (Tilemap m in maps)
    //        m.ClearAllTiles();
    //}
}
