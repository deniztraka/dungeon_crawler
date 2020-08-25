using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace DTWorldz.Editor.Gizmo
{
    public class SpawnerGizmo : MonoBehaviour
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.NotInSelectionHierarchy)]
        static void DrawStateGizmo(ObjectSpawnerBehaviour spawner, GizmoType gizmoType)
        {

            Gizmos.color = Color.red;
            var positionY = spawner.transform.position.y;
            var positionX = spawner.transform.position.x;
            var gizmoPosition = new Vector3(positionX, positionY, 0);
            Handles.Label(gizmoPosition, "spawner");

            var rangeColor = Color.red;
            rangeColor.a = 0.1f;
            Gizmos.color = spawner.Color;
            Gizmos.DrawCube(spawner.transform.position, new Vector3(spawner.RangeX, spawner.RangeY, 0));
        }
    }
}
