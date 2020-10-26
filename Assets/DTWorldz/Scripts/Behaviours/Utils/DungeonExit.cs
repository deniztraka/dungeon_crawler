﻿using System.Collections;
using System.Collections.Generic;
using DTWorldz.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DTWorldz.Behaviours.Utils
{
    public class DungeonExit : MonoBehaviour
    {
        public PlayerAreaStack PlayerAreaStack;
        // Start is called before the first frame update
        void Start()
        {
            var playerGameObject = GameObject.FindWithTag("Player");
            playerGameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                var playerAreaModel = PlayerAreaStack.Peek();
                if (playerAreaModel != null)
                {
                    SceneManager.LoadSceneAsync(playerAreaModel.AreaName);
                } else {
                    Debug.Log("I didn't come from anywhere.");
                }
            }
        }
    }
}