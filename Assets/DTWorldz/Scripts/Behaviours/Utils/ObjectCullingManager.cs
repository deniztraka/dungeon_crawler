using System.Collections;
using System.Collections.Generic;
using DTWorldz.Scripts.Managers;
using UnityEngine;
namespace DTWorldz.Behaviours.Utils
{
    public class ObjectCullingManager : MonoBehaviour
    {
        // --------------------------------------------------
        // Variables:

        [SerializeField]
        private float distanceFromPlayer = 3f;

        private GameObject player;

        private List<ActivatorItem> activatorItems;

        public List<ActivatorItem> addList;

        // --------------------------------------------------

        void Awake()
        {
            player = GameManager.Instance.PlayerBehaviour.gameObject;
            activatorItems = new List<ActivatorItem>();
            addList = new List<ActivatorItem>();

            AddToList();

            StartCoroutine(DoCheck());
        }

        IEnumerator DoCheck()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                StartCoroutine(CheckActivation());
            }
        }

        void AddToList()
        {
            if (addList.Count > 0)
            {
                foreach (ActivatorItem item in addList)
                {
                    if (item.item != null)
                    {
                        activatorItems.Add(item);
                    }
                }

                addList.Clear();
            }
        }

        IEnumerator CheckActivation()
        {
            List<ActivatorItem> removeList = new List<ActivatorItem>();

            if (activatorItems.Count > 0)
            {
                foreach (ActivatorItem item in activatorItems)
                {
                    if (item.item == null)
                    {
                        removeList.Add(item);
                    }
                    else if (Vector2.Distance(player.transform.position, item.item.transform.position) > distanceFromPlayer)
                    {
                        item.item.SetActive(false);
                    }
                    else
                    {
                        item.item.SetActive(true);
                    }
                }
            }

            yield return new WaitForSeconds(0.1f);
            if (removeList.Count > 0)
            {
                foreach (ActivatorItem item in removeList)
                {
                    activatorItems.Remove(item);
                }
            }

            yield return new WaitForSeconds(0.1f);

            AddToList();
        }
    }

    public class ActivatorItem
    {
        public GameObject item;
    }
}