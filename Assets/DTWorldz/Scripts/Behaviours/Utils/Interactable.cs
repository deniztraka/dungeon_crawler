using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.Utils
{
    public class Interactable : MonoBehaviour
    {
        public float Cooldown = 1f;
        public Sprite Image;
        public delegate void InteractHandler();
        public event InteractHandler OnInteraction;
        void Start()
        {

        }

        void OnMouseDown()
        {
            Interact();
        }

        public virtual void Interact()
        {
            if(OnInteraction != null){
                OnInteraction();
            }
        }
    }
}