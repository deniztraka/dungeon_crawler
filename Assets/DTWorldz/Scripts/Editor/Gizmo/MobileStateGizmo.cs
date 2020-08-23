using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.AI;
using UnityEditor;
using UnityEngine;

namespace DTWorldz.Editor.Gizmo
{
    public class MobileStateGizmo : MonoBehaviour
    {
        // Start is called before the first frame update
        [DrawGizmo(GizmoType.Selected | GizmoType.NotInSelectionHierarchy)]
        static void DrawStateGizmo(MobileStateBehaviour mobileStateBehaviour, GizmoType gizmoType)
        {
            Gizmos.color = Color.red;
            var positionY = mobileStateBehaviour.transform.position.y;
            var positionX = mobileStateBehaviour.transform.position.x;
            var gizmoPosition = new Vector3(positionX, positionY + 0.6f, 0);
            Handles.Label(gizmoPosition, mobileStateBehaviour.GetState());
        }
    }
}