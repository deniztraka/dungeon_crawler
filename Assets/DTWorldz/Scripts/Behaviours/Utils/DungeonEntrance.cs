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
        public string TargetSceneName;
        // Start is called before the first frame update
        void Start()
        {
            var lastArea = PlayerAreaStack.Peek();
            if (lastArea != null && lastArea.UniqueObjectName == gameObject.name)
            {
                lastArea = PlayerAreaStack.Pop();
                var newPos = new Vector3(lastArea.LastPosition.x, lastArea.LastPosition.y, lastArea.LastPosition.z);
                var playerObject = GameObject.FindWithTag("Player");
                var cameraHolderObject = GameObject.Find("CameraHolder");
                playerObject.transform.position = newPos;
                cameraHolderObject.transform.position = new Vector3(newPos.x, newPos.y, -1);         
            }
        }


        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                PlayerAreaStack.Push(new PlayerAreaStackModel(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), SceneManager.GetActiveScene().name, gameObject.name));                
                SceneManager.LoadSceneAsync(TargetSceneName);
            }
        }
    }
}