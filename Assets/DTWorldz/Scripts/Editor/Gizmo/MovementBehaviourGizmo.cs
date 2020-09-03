using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours;
using DTWorldz.Behaviours.Mobiles;
using UnityEditor;
using UnityEngine;

namespace DTWorldz.Editor.Gizmo
{
    public class MovementBehaviourGizmo : MonoBehaviour
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.NotInSelectionHierarchy)]
        static void DrawFollowZoneGizmo(MovementBehaviour movementBehaviour, GizmoType gizmoType)
        {
            if (movementBehaviour.DrawGizmos)
            {
                var closeDistanceColor = Color.red;
                closeDistanceColor.a = 0.25f;
                Gizmos.color = closeDistanceColor;
                Gizmos.DrawSphere(movementBehaviour.transform.position, movementBehaviour.CloseDistance);

                var awareDistanceColor = Color.yellow;
                awareDistanceColor.a = 0.05f;
                Gizmos.color = awareDistanceColor;
                Gizmos.DrawSphere(movementBehaviour.transform.position, movementBehaviour.AwareDistance);

                

                Gizmos.color = Color.white;
                var positionY = movementBehaviour.transform.position.y;
                var positionX = movementBehaviour.transform.position.x;
                var gizmoPosition = new Vector3(positionX + 0.3f, positionY + 1f, 0);
                Handles.Label(gizmoPosition, "mSpeed:" + movementBehaviour.GetResultingSpeed());
            }
        }
    }
}
