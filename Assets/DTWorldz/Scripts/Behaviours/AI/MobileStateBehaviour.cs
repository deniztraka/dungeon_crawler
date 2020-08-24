using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.AI.States;
using UnityEngine;

namespace DTWorldz.Behaviours.AI
{
    
    public class MobileStateBehaviour : MonoBehaviour
    {
        public bool DrawGizmos = false;        
        public float IdleChance = 0.5f;
        public float WanderChance = 0.5f;        
        public int MinDecisionDelay = 1;
        public int MaxDecisionDelay = 5;        
        public float FollowRefreshFrequency = 1;

        private string state;        

        public void SetState(string state){
            this.state = state;
        }

        public string GetState(){
            return state;
        }
    }
}