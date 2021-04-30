using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.SO;
using UnityEngine;
namespace DTWorldz.Items.Behaviours
{
    public class ItemDBBehaviour : MonoBehaviour
    {
        public static ItemDBBehaviour Instance { get; private set; }

        public ItemDB DB;

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
    }
}