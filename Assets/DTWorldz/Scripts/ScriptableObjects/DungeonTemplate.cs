﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace DTWorldz.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DungeonTemplate", menuName = "ScriptableObjects/DungeonTemplate", order = 1)]
    public class DungeonTemplate : ScriptableObject
    {
        public int Seed = 0;

        public int Width = 20;
        public int Height = 20;
        public int MinRoomSize = 4;
        public int MaxRoomSize = 10;

        public TileBase FloorTile;
        public TileBase WallTile;
        public TileBase BackgroundTile;

        public TileBase TestTile;

        public RoomTemplate RoomTemplate;


    }
}