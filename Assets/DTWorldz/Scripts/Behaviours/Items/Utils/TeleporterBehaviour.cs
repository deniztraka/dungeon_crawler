using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.ProceduralMapGenerators;
using UnityEngine;
namespace DTWorldz.Behaviours.Items.Utils
{
    public class TeleporterBehaviour : MonoBehaviour
    {
        public Vector3 TeleportTo;
        public Vector3 TeleportPosition;
        private LevelBehaviour levelBehaviour;

        private void Start()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                other.transform.position = TeleportTo;
                var playerMovementBehaviour = other.GetComponent<PlayerMovementBehaviour>();
                playerMovementBehaviour.SetMovementGrid(levelBehaviour.WallMap);
            }
        }

        internal void SetTeleportsToLevel(LevelBehaviour levelBehaviour)
        {
            this.levelBehaviour = levelBehaviour;
        }
    }
}