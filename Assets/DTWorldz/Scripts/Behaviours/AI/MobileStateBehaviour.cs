using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.AI.States;
using UnityEngine;

namespace DTWorldz.Behaviours.AI
{
    
    public class MobileStateBehaviour : MonoBehaviour
    {
        public bool DrawGizmos = false;
        private string state;        

        public void SetState(string state){
            this.state = state;
        }

        public string GetState(){
            return state;
        }
    }
}