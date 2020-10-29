using System.Collections;
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
            var newPos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            var cameraHolderObject = GameObject.Find("CameraHolder");
            playerGameObject.transform.position = newPos;
            cameraHolderObject.transform.position = new Vector3(newPos.x, newPos.y, -1);
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {                
                var playerAreaModel = PlayerAreaStack.Peek();
                if (playerAreaModel != null)
                {
                    var asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
                    asyncSceneLoader.LoadScene(playerAreaModel.AreaName, true);
                }
                else
                {
                    Debug.Log("I didn't come from anywhere.");
                }
            }
        }
    }
}