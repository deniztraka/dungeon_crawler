using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.AI.States.Traps
{
    public class DamageTrapStateBehaviour : BaseTrapStateBehaviour
    {
        private float refreshTime;
        private int spriteIndex;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateName = "Damage";
            base.OnStateEnter(animator, stateInfo, layerIndex);
            refreshTime = 0;
            spriteIndex = 0;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            if (TrapBehaviour.AnimationSprites != null && TrapBehaviour.AnimationSprites.Count > 0 && SpriteRenderer != null)
            {
                if (spriteIndex < TrapBehaviour.AnimationSprites.Count)
                {
                    if (refreshTime <= 0)
                    {                        
                        refreshTime = TrapBehaviour.AnimationFrequency;
                        SpriteRenderer.sprite = TrapBehaviour.AnimationSprites[spriteIndex];
                        spriteIndex++;
                    }
                    else
                    {
                        refreshTime -= Time.deltaTime;
                    }
                }
                else
                {
                    animator.SetTrigger("Idle");
                }
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}