using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using UnityEditor;
using UnityEngine;

namespace DTWorldz.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerAreaStack", menuName = "ScriptableObjects/PlayerAreaStack", order = 4)]
    public class PlayerAreaStack : ScriptableObject
    {
        [SerializeField]
        private Stack<PlayerAreaStackModel> lastPlayerAreas;

        public PlayerAreaStackModel Pop()
        {
            if (lastPlayerAreas == null || lastPlayerAreas.Count == 0)
            {
                return null;
            }
            return lastPlayerAreas.Pop();
        }

        public PlayerAreaStackModel Peek()
        {
            if (lastPlayerAreas == null || lastPlayerAreas.Count == 0)
            {
                return null;
            }
            return lastPlayerAreas.Peek();
        }

        public void Push(PlayerAreaStackModel model)
        {
            if (lastPlayerAreas == null)
            {
                lastPlayerAreas = new Stack<PlayerAreaStackModel>(); 
            }
            lastPlayerAreas.Push(model);
        }  

        public Stack<PlayerAreaStackModel> GetLastPlayerAreas(){
            return lastPlayerAreas;
        }      
    }
}