﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace DTWorldz.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RoomTemplate", menuName = "ScriptableObjects/RoomTemplate", order = 1)]
    public class RoomTemplate : ScriptableObject
    {
        public float DecorationChance = .25f;
        public float ObjectsChance = .1f;
        public float ContainerChance = .1f;
        public TileBase[] FloorDecorations;
        public TileBase[] UpperWallDecorations;
        public TileBase[] LeftWallDecorations;
        public TileBase[] RightWallDecorations;

        public GameObject[] Objects;
        public GameObject[] Containers;
    }
}