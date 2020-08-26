using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.AI.States.Traps
{
    public class IdleTrapStateBehaviour : BaseTrapStateBehaviour
    {
        float refreshTime = 0;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateName = "Idle";
            base.OnStateEnter(animator, stateInfo, layerIndex);
            if (SpriteRenderer != null)
            {
                SpriteRenderer.sprite = IdleSprite != null ? IdleSprite : null;
            }

            if (Collider != null)
            {
                Collider.enabled = false;
            }

            refreshTime = TrapBehaviour.RefreshTime;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (refreshTime <= 0)
            {
                refreshTime = TrapBehaviour.RefreshTime;
                animator.SetTrigger("Ready");
            }
            else
            {
                refreshTime -= Time.deltaTime;
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}