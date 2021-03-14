using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTWorldz.ProceduralGeneration
{
    [Serializable]
    public class TileMapTerrain
    {
        public TerrainTypeTemplate Template;
        public Tilemap Tilemap;
    }
}