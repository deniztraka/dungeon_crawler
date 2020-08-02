using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DTWorldz.Behaviours;

namespace DTWorldz.Editor.Gizmo
{
    public class AttackZoneGizmo : MonoBehaviour
    {
        [DrawGizmo(GizmoType.Selected)]
        static void DrawAttackZone(AttackBehaviour attackBehaviour, GizmoType gizmoType){
            var collider = attackBehaviour.gameObject.GetComponent<BoxCollider2D>();
            Gizmos.color = Color.red;
            var positionY = attackBehaviour.transform.position.y + collider.offset.y;
            var positionX = attackBehaviour.transform.position.x + collider.offset.x;
            var gizmoPosition = new Vector3(positionX, positionY, 0);
            Gizmos.DrawWireCube(gizmoPosition, collider.size);            
        }
    }
}