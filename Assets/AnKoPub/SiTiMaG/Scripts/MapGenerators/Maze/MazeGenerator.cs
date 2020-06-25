using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : Generator
{
    readonly float chanceOfEmptySpace;

    public MazeGenerator(int mapWidth, int mapHeight, float chanceOfEmpty, Tilemap floorMap, Tilemap wallMap, TileBase floorTile, TileBase wallTile)
    {
        Width = mapWidth;
        Height = mapHeight;        
        MapData = new bool[Height, Width];
        chanceOfEmptySpace = chanceOfEmpty;
        FloorTile = floorTile;
        WallTile = wallTile;
        FloorMap = floorMap;
        WallMap = wallMap;
    }

    public override void NewMap()
    {
        BuildMap();
    }

    public override void GenerateMap()
    {
        int rMax = MapData.GetUpperBound(0);
        int cMax = MapData.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (i == 0 || j == 0 || i == rMax || j == cMax)
                {
                    MapData[i, j] = true;
                }

                else if (i % 2 == 0 && j % 2 == 0)
                {
                    if (Random.value > chanceOfEmptySpace)
                    {
                        MapData[i, j] = true;

                        int a = Random.value < 0.5 ? 0 : (Random.value < 0.5 ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < 0.5 ? -1 : 1);
                        MapData[i + a, j + b] = true;
                    }
                }
            }
        }
    }
}