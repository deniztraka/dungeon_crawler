using System.Collections;
using System.Collections.Generic;
using DTWorldz.ScriptableObjects;
using UnityEngine;
namespace DTWorldz.Behaviours
{
    public class ActManager : MonoBehaviour
    {
        public GameObject StartingPoint;
        public PlayerAreaStack PlayerAreaStack;
        public GameObject CameraHolder;
        public bool SetCurrentAct;

        [SerializeField]
        private string currentSceneName;


        private Vector3 startingPoint;

        // Start is called before the first frame update
        void Start()
        {
            if (StartingPoint != null && !PlayerAreaStack.IsActive)
            {
                startingPoint = StartingPoint.transform.position;
                var playerObj = GameObject.FindWithTag("Player");
                playerObj.transform.position = startingPoint;
                CameraHolder.transform.position = new Vector3(startingPoint.x, startingPoint.y, CameraHolder.transform.position.z);
            }

            currentSceneName = gameObject.name;

            if (SetCurrentAct)
            {
                GameManager.Instance.SetCurrentAct(currentSceneName);
            }

        }

        public string GetCurrentSceneName()
        {
            return currentSceneName;
        }
    }
}