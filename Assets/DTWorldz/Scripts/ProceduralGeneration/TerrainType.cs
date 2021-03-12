using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTWorldz.ProceduralGeneration
{
    [Serializable]
    public struct TerrainType
    {
        public string Name;
        public float Height;
        public Color Color;
        public Tile Tile;
        public Tilemap Tilemap;
        public List<GameObject> TreeTypes;
        public float TreeFrequency;
        public List<GameObject> BushTypes;
        public float BushFrequency;
    }
}