using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorlds.Behaviours.Effects
{
    public class BloodStainsPool : MonoBehaviour
    {

        public static BloodStainsPool Instance { get; private set; }

        public List<GameObject> BloodStainsList;
        public float Chance = 0.5f;
        private Queue<GameObject> bloodStainsPool;

        public int ReadyCount = 50;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            PrepareBloodPool();
        }

        private void PrepareBloodPool()
        {
            bloodStainsPool = new Queue<GameObject>();
            for (int i = 0; i < ReadyCount; i++)
            {
                var randomBloodStainIndex = Random.Range(0, BloodStainsList.Count);
                var bloodStainObject = GameObject.Instantiate(BloodStainsList[randomBloodStainIndex]);
                bloodStainObject.SetActive(false);
                bloodStainsPool.Enqueue(bloodStainObject);
            }
        }

        public void Create(Vector3 position)
        {
            if (UnityEngine.Random.value > Chance)
            {
                var dequedObject = bloodStainsPool.Dequeue();
                dequedObject.SetActive(true);
                dequedObject.transform.position = position;
                dequedObject.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
                bloodStainsPool.Enqueue(dequedObject);
            }
        }
    }

}