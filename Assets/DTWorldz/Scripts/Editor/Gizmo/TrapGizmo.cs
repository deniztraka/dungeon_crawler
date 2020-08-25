using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Items.Deco;
using UnityEditor;
using UnityEngine;
namespace DTWorldz.Editor.Gizmo
{
    public class TrapGizmo : MonoBehaviour
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.NotInSelectionHierarchy)]
        static void DrawTrapGizmo(TrapBehaviour trap, GizmoType gizmoType)
        {
            Gizmos.color = Color.white;
            Handles.Label(trap.transform.position, trap.State);

            Gizmos.color = trap.Color;
            Gizmos.DrawCube(trap.transform.position, new Vector3(1, 1, 0));
        }
    }
}
