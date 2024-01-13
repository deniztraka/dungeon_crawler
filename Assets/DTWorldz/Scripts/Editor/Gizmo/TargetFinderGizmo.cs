using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DTWorldz.Behaviours;
using DTWorldz.Behaviours.Utils;

namespace DTWorldz.Editor.Gizmo
{
    public class TargetFinderGizmo : MonoBehaviour
    {
        [DrawGizmo(GizmoType.Selected)]
        static void DrawTargetFinderArea(TargetFinder targetFinder, GizmoType gizmoType)
        {
            var collider = targetFinder.gameObject.GetComponent<BoxCollider2D>();
            Gizmos.color = Color.red;
            var positionY = targetFinder.transform.position.y + collider.offset.y;
            var positionX = targetFinder.transform.position.x + collider.offset.x;
            var gizmoPosition = new Vector3(positionX, positionY, 0);
            Gizmos.DrawWireCube(gizmoPosition, collider.size);


        }
    }
}