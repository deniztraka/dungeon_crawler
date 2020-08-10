using UnityEngine;
using UnityEditor;
using DTWorldz.Behaviours.ProceduralMapGenerators;

[CustomEditor(typeof(BPSDungeonGenerator))]
public class BPSDungeonGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BPSDungeonGenerator generator = (BPSDungeonGenerator)target;

        EditorGUILayout.BeginVertical();
        EditorGUILayout.HelpBox("Dungen Generation", MessageType.Info);
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Build")){
            generator.BuildMap();
        }

        if(GUILayout.Button("Clear")){
            generator.ClearMap();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

    }
}