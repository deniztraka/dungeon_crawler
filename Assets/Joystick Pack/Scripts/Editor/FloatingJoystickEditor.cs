using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloatingJoystick))]
public class FloatingJoystickEditor : JoystickEditor
{
    private SerializedProperty showAlways;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (background != null)
        {
            RectTransform backgroundRect = (RectTransform)background.objectReferenceValue;
            backgroundRect.anchorMax = Vector2.zero;
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.pivot = center;
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        showAlways = serializedObject.FindProperty("showAlways");
    }

    protected override void DrawValues()
    {
        base.DrawValues();
        if (showAlways != null)
        {
            EditorGUILayout.PropertyField(showAlways, new GUIContent("Show Always", "Disables hiding feature while on mouse down"));
        }
    }
}