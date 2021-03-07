using System;
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
    }
}