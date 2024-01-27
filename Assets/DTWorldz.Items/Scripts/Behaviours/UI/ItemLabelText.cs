using System.Collections;
using System.Collections.Generic;
using DTWorldz.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Items.Behaviours.UI
{
    public class ItemLabelText : MonoBehaviour
    {
        private Transform playerTransform;
        private Text text;
        // Start is called before the first frame update
        void Start()
        {
            var itemBehaviour = GetComponentInParent<ItemBehaviour>();
            if (itemBehaviour == null || itemBehaviour.ItemSO == null)
            {
                return;
            }

            text = GetComponentInChildren<Text>();
            if (text == null)
            {
                return;
            }

            var canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                return;
            }

            canvas.worldCamera = Camera.main;

            playerTransform = GameManager.Instance.PlayerBehaviour.transform;

            text.text = itemBehaviour.ItemSO.Name;
        }

        void Update()
        {
            if (playerTransform == null || text == null)
            {
                return;
            }

            text.enabled = Vector2.Distance(playerTransform.position, transform.position) < 3;

        }
    }
}