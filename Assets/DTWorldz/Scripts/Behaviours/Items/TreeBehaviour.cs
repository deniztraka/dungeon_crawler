using System;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Behaviours.Utils;
using DTWorldz.Scripts.Managers;
using UnityEngine;
namespace DTWorldz.Behaviours
{
    public class TreeBehaviour : MonoBehaviour
    {
        private Animator animator;
        private AudioManager audioManager;
        private HealthBehaviour health;
        private int animatorHitStateHash;
        [SerializeField]
        private Vector3 DamageTakenParticleOffset = Vector3.zero;

        public ParticleSystem DamageTakenParticles;
        public ParticleSystem DeadParticles;

         private Interactable interactable;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            audioManager = GetComponent<AudioManager>();
            health = GetComponent<HealthBehaviour>();

            health.OnDamageTaken += new HealthBehaviour.DamageTaken(OnDamageTaken);
            health.OnDeath += new Interfaces.HealthChanged(OnDeath);
            animatorHitStateHash = Animator.StringToHash("Hit");

            interactable = GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.OnInteraction += new Interactable.InteractHandler(OnInteraction);
            }
        }

        private void OnInteraction()
        {
            var playerMovementBehaviour = GameManager.Instance.PlayerBehaviour.GetComponent<PlayerMovementBehaviour>();
            if(playerMovementBehaviour != null){
                playerMovementBehaviour.Attack(health);
            }
        }

        private void OnDeath(float currentHealth, float maxHealth)
        {
            if (DeadParticles != null)
            {
                var spawnedParticles = Instantiate(DeadParticles, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
            }
            Destroy(gameObject, 0.1f);
        }

        private void OnDamageTaken(float damageAmount, DamageType type)
        {
            if (audioManager != null)
            {
                audioManager.Play("Hit");
            }

            if (DamageTakenParticles != null)
            {
                var spawnedParticles = Instantiate(DamageTakenParticles, transform.position + DamageTakenParticleOffset, Quaternion.identity);
            }

            if (animator != null)
            {
                animator.SetTrigger(animatorHitStateHash);
            }
        }
    }
}