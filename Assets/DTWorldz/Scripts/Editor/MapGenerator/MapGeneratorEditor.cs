using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DTWorldz.Behaviours.ProceduralMapGenerators.NoiseMap;

[CustomEditor(typeof(NoiseMapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        var mapGen = (NoiseMapGenerator)target;
        System.Random prng = null;

        if (DrawDefaultInspector() && mapGen.autoUpdate)
        {
            prng = mapGen.GenerateMap();
        }

        if (GUILayout.Button("Generate"))
        {
            if (prng == null)
            {
                prng = new System.Random(mapGen.Seed);
            }
            prng = mapGen.GenerateMap();
        }

        if (GUILayout.Button("ClearTileMap"))
        {
            mapGen.ClearTileMap();
        }

        if (GUILayout.Button("PlaceTrees"))
        {
            if (prng == null)
            {
                prng = new System.Random(mapGen.Seed);
            }
            mapGen.PlaceTrees(prng);
        }

        if (GUILayout.Button("ClearTrees"))
        {
            mapGen.ClearTrees();
        }

        if (GUILayout.Button("PlaceBushes"))
        {
            if (prng == null)
            {
                prng = new System.Random(mapGen.Seed);
            }
            mapGen.PlaceBushes(prng);
        }

        if (GUILayout.Button("ClearBushes"))
        {
            mapGen.ClearBushes();
        }
    }
}
