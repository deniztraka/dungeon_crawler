using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTWorldz.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "TerrainTypeTemplate", menuName = "ScriptableObjects/TerrainTypeTemplate", order = 2)]
    public class TerrainTypeTemplate : ScriptableObject
    {
        public string Name;
        public float Height;
        public Color Color;
        public Tile Tile;
        public List<GameObject> TreeTypes;
        public float TreeFrequency;
        public List<GameObject> BushTypes;
        public float BushFrequency;
    }
}