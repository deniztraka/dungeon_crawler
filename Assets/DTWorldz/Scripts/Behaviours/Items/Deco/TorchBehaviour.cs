using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using UnityEngine;
namespace DTWorldz.Behaviours.Items.Deco
{
    public class TorchBehaviour : MonoBehaviour
    {
        private bool active;
        public bool IsActive
        {
            get { return active; }
        }

        public FlickeringLightBehaviour FlickeringLight;
        private AudioManager audioManager;
        // Start is called before the first frame update
        void Start()
        {
            audioManager = GetComponent<AudioManager>();
            Ignite();
        }

        public void Ignite()
        {
            if (!this.IsActive)
            {
                StartCoroutine(PlaySounds(1));
                active = true;
            }
        }

        IEnumerator PlaySounds(float delay)
        {
            if (audioManager != null)
            {
                audioManager.Play("Ignite");
            }
            yield return new WaitForSeconds(delay);
            if (audioManager != null)
            {
                audioManager.Play("Idle");
            }
        }

        public void Extinguish()
        {
            if (IsActive)
            {
                if (audioManager != null)
                {
                    audioManager.Stop("Idle");
                    audioManager.Play("Extinguish");
                }
                active = false;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}