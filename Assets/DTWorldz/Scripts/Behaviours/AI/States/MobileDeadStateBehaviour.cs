using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.AI.States
{
    public class MobileDeadStateBehaviour : BaseMobileStateBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateName = "Dead";
            base.OnStateEnter(animator, stateInfo, layerIndex);

            
            GoDeadState(animator.gameObject);
            Destroy(animator.gameObject, 15);
        }

        //todo: just an example dead animation here
        private void GoDeadState(GameObject gameObject){
            MovementBehaviour.enabled = false;
            MobileHealth.enabled = false;
            // var spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
            // var tempColor = spriteRenderer.color;
            // tempColor.a = 0.5f;
            // spriteRenderer.color = tempColor;

            var collider = gameObject.GetComponentInChildren<Collider2D>();
            collider.enabled = false;

            // var rigidbody2d = gameObject.GetComponentInChildren<Rigidbody2D>();
            // rigidbody2d.AddForce(Vector2.up * 50, ForceMode2D.Force);

        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}
