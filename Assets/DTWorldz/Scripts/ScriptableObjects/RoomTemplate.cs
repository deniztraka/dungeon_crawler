using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace DTWorldz.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RoomTemplate", menuName = "ScriptableObjects/RoomTemplate", order = 1)]
    public class RoomTemplate : ScriptableObject
    {
        public float DecorationChance = .25f;
        public TileBase[] FloorDecorations;
        public TileBase[] UpperWallDecorations;
        public TileBase[] LeftWallDecorations;
        public TileBase[] RightWallDecorations;
    }
}