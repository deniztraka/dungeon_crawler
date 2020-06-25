using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BSPGenerator : Generator
{
    readonly int MaxLeafSize, MinLeafSize, HallsWidth;
    readonly List<Node> leafsList;
    public static bool RandomHallWidth;
    public static int MinLeafSizeStatic;
    public static int HallsWidthStatic;


    public BSPGenerator(int mapWidth, int mapHeight, int maxLeafSize, int minLeafSize, int hallsWidth, bool randomHallWidth,
                        Tilemap floorMap, Tilemap wallMap,
                        TileBase floorTile, TileBase wallTile)
    {
        Width = mapWidth;
        Height = mapHeight;
        MaxLeafSize = maxLeafSize;
        MinLeafSize = minLeafSize;
        HallsWidth = hallsWidth;        
        RandomHallWidth = randomHallWidth;
        MinLeafSizeStatic = MinLeafSize;
        HallsWidthStatic = HallsWidth;
        FloorTile = floorTile;
        WallTile = wallTile;
        FloorMap = floorMap;
        WallMap = wallMap;
        MapData = new bool[Height, Width];
        MinLeafSizeStatic = MinLeafSize;
        leafsList = new List<Node>();
    }

    public override void GenerateMap()
    {
        var root = new Node(new Point(0, 0), Width, Height);
        leafsList.Add(root);

        bool didSplit = true;

        while (didSplit)
        {
            didSplit = false;
            for (int i = 0; i < leafsList.Count; i++)
            {
                if (leafsList[i].LeftChild == null && leafsList[i].RightChild == null)
                {
                    if (leafsList[i].Width > MaxLeafSize || leafsList[i].Height > MaxLeafSize || Random.Range(0f, 1f) > 0.25)
                    {
                        if (leafsList[i].Split())
                        {
                            leafsList.Add(leafsList[i].LeftChild);
                            leafsList.Add(leafsList[i].RightChild);
                            didSplit = true;
                        }
                    }
                }
            }
        }
        root.CreateRooms();
        ConverNodesToArray();
    }

    void ConverNodesToArray()
    {
        foreach (var l in leafsList)
        {
            for (int i = (int)l.Room.xMin; i <= l.Room.xMax; i++)
                for (int j = (int)l.Room.yMin; j <= l.Room.yMax; j++)
                {
                    if (i == 0 && j == 0) continue;
                    MapData[j, i] = false;
                }

            if (l.Halls != null)
            {
                foreach (var hall in l.Halls)
                {
                    for (int i = (int)hall.xMin; i <= hall.xMax; i++)
                        for (int j = (int)hall.yMin; j <= hall.yMax; j++)
                            MapData[j, i] = false;
                }
            }
        }
    }
}