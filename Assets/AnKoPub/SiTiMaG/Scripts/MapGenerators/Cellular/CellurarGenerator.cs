using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cellurar : Generator
{
    readonly int DeathLimit, BirthLimit, NumberOfSteps;
    readonly float ChanceToStartAlive = 0.45f;

    public Cellurar(int mapWidth, int mapHeight, int deathLimit, int birthLimit,
                    int numberOfSteps, float chanceToStartAlive, Tilemap floorMap, Tilemap wallMap, TileBase floorTile, TileBase wallTile)
    {
        Width = mapWidth;
        Height = mapHeight;        
        DeathLimit = deathLimit;
        BirthLimit = birthLimit;
        NumberOfSteps = numberOfSteps;
        ChanceToStartAlive = chanceToStartAlive;
        FloorTile = floorTile;
        WallTile = wallTile;
        FloorMap = floorMap;
        WallMap = wallMap;
        MapData = new bool[Height, Width];
    }

    public override void InitialiseMap()
    {
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                if (Random.Range(0f, 1f) < ChanceToStartAlive)
                    MapData[y, x] = true;
        FillBorder();
    }

    public void FillBorder()
    {
        for (int x = 0; x < Width; x++)
        {
            MapData[0, x] = true;
            MapData[Height - 1, x] = true;
        }
        for (int y = 0; y < Height; y++)
        {
            MapData[y, y] = true;
            MapData[y, Width - 1] = true;
        }
    }

    bool[,] DoSimulationStep(bool[,] oldMap) //Do one pass through map
    {
        bool[,] newMap = new bool[Width, Height];

        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                int neigCount = CountAliveNeighbours(oldMap, x, y);
                if (oldMap[y, x])
                {
                    if (neigCount >= DeathLimit) newMap[y, x] = true;
                    else newMap[y, x] = false;
                }
                else
                {
                    if (neigCount >= BirthLimit) newMap[y, x] = true;
                    else newMap[y, x] = false;
                }
            }
        return newMap;
    }

    int CountAliveNeighbours(bool[,] map, int x, int y)
    {
        var count = 0;

        for (int i = -1; i < 2; i++)
            for (int j = -1; j < 2; j++)
            {
                int neighbourX = x + i;
                int neighbourY = y + j;
                if (i == 0 && j == 0) continue;
                else if (neighbourX < 0 || neighbourY < 0 || neighbourX >= Width || neighbourY >= Height) count += 1;
                else if (map[neighbourX, neighbourY]) count += 1;
            }

        return count;
    }

    public override void GenerateMap()
    {
        for (int i = 0; i < NumberOfSteps; i++)
            MapData = DoSimulationStep(MapData);
    }    
}
