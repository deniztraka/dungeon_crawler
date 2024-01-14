using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Behaviours.Utils;
using UnityEngine;
namespace DTWorldz.Behaviours
{
    public class MushroomBehaviour : MonoBehaviour
    {

        private Interactable interactable;
        // Start is called before the first frame update
        void Start()
        {
            interactable = GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.OnInteraction += new Interactable.InteractHandler(OnInteraction);
            }
        }

        private void OnInteraction()
        {
            var playerMovementBehaviour = GameObject.FindWithTag("Player").GetComponent<PlayerMovementBehaviour>();
            if (playerMovementBehaviour != null)
            {
                playerMovementBehaviour.Attack(GetComponent<HealthBehaviour>());
            }
        }
    }
}