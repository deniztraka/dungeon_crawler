using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using DTWorldz.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DTWorldz.Behaviours.Utils
{
    public class DungeonEntrance : MonoBehaviour
    {
        public PlayerAreaStack PlayerAreaStack;
        // Start is called before the first frame update
        void Start()
        {
            var lastArea = PlayerAreaStack.Peek();
            if (lastArea != null && lastArea.UniqueObjectName == gameObject.name)
            {
                lastArea = PlayerAreaStack.Pop();
                var playerObject = GameObject.FindWithTag("Player");
                playerObject.transform.position = new Vector3(lastArea.LastPosition.x, lastArea.LastPosition.y, lastArea.LastPosition.z);
            }
        }


        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                Debug.Log(transform.position);
                PlayerAreaStack.Push(new PlayerAreaStackModel(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), "Act1Scene", gameObject.name));
                
                SceneManager.LoadSceneAsync("Act1CemetaryScene");
            }
        }
    }
}