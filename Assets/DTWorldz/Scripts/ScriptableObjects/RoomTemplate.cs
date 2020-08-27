using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace DTWorldz.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RoomTemplate", menuName = "ScriptableObjects/RoomTemplate", order = 2)]
    public class RoomTemplate : ScriptableObject
    {
        public float DecorationChance = .25f;
        public float ObjectsChance = .1f;
        public float ContainerChance = .1f;
        public float TrapChance = .1f;
        public TileBase[] FloorDecorations;
        public TileBase[] UpperWallDecorations;
        public TileBase[] LeftWallDecorations;
        public TileBase[] RightWallDecorations;

        public GameObject[] Objects;
        public GameObject[] Containers;
        public GameObject[] Treasures;
        public TrapTemplate TrapTemplate;

        public GameObject TopLight;
        public GameObject BottomLight;
        public GameObject RightLight;
        public GameObject LeftLight;
    }
}