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

        if(DrawDefaultInspector() && mapGen.autoUpdate){
            mapGen.GenerateMap();
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }

        if (GUILayout.Button("ClearTileMap"))
        {
            mapGen.ClearTileMap();
        }

        if (GUILayout.Button("PlaceTrees"))
        {
            mapGen.PlaceTrees();
        }

        if (GUILayout.Button("ClearTrees"))
        {
            mapGen.ClearTrees();
        }
    }
}
