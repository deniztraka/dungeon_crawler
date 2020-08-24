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
            if (mobileStateBehaviour.DrawGizmos)
            {
                Gizmos.color = Color.red;
                var positionY = mobileStateBehaviour.transform.position.y;
                var positionX = mobileStateBehaviour.transform.position.x;
                var gizmoPosition = new Vector3(positionX + 0.3f, positionY + 0.6f, 0);
                Handles.Label(gizmoPosition, "mState:" + mobileStateBehaviour.GetState());            
            }
        }
    }
}