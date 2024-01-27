using System;
using System.Collections;
using DTWorldz.Behaviours.Player;
using DTWorldz.Behaviours.UI;
using DTWorldz.Items.SO;
using DTWorldz.Models;
using DTWorldz.Scripts.Managers;
using DTWorldz.Utils;
using UnityEngine;
using UnityEngine.UI;
namespace DTWorldz.Items.Behaviours.UI
{
    public class ConstructionPanelUI : MonoBehaviour
    {
        private Canvas canvas;

        private ActionButtonBehaviour createButton;
        private InventoryBehaviour inventoryBehaviour;
        private PlayerBehaviour playerBehaviour;
        private BaseConstructableItemSO constructableItemSO;

        private SpriteRenderer constructionObject;

        public LayerMask BlockingLayer;

        void Start()
        {
            playerBehaviour = GameManager.Instance.PlayerBehaviour;
            inventoryBehaviour = playerBehaviour.GetComponent<InventoryBehaviour>();

            InitUI();
        }



        private void InitUI()
        {
            canvas = GetComponent<Canvas>();
            createButton = transform.FindDeepChild("ConstructionActionButton").GetComponent<ActionButtonBehaviour>();
            createButton.Button.onClick.AddListener(Place);
            constructionObject = playerBehaviour.transform.FindDeepChild("ConstructionObject").GetComponent<SpriteRenderer>();

        }

        private void Place()
        {
            if (constructableItemSO != null)
            {
                // instantiate the item
                var item = Instantiate(constructableItemSO.Prefab, constructionObject.transform.position, Quaternion.identity);
                inventoryBehaviour.RemoveItem(constructableItemSO);
                Close();
            }
        }

        public void Open(BaseConstructableItemSO constructableItemSO)
        {
            canvas.enabled = true;
            this.constructableItemSO = constructableItemSO;
            createButton.ActionImage.sprite = constructableItemSO.Icon;
            constructionObject.enabled = true;
            constructionObject.sprite = constructableItemSO.Icon;
        }

        public void Close()
        {
            canvas.enabled = false;
            this.constructableItemSO = null;
            constructionObject.enabled = false;

        }

        void Update()
        {
            if (canvas.enabled && constructableItemSO != null)
            {
                var objectPosition = constructionObject.transform.position;
                switch (playerBehaviour.Direction)
                {
                    case Direction.Up:
                        objectPosition = playerBehaviour.transform.position + new Vector3(0, 2, 0);
                        break;
                    case Direction.Down:
                        objectPosition = playerBehaviour.transform.position + new Vector3(0, -1, 0);
                        break;
                    case Direction.Left:
                        objectPosition = playerBehaviour.transform.position + new Vector3(-1, 0, 0);
                        break;
                    case Direction.Right:
                        objectPosition = playerBehaviour.transform.position + new Vector3(1, 0, 0);
                        break;
                    default:
                        break;
                }

                constructionObject.transform.position = objectPosition;



                var colliders = Physics2D.OverlapBoxAll(constructionObject.transform.position, new Vector2(1, 1), 0f, BlockingLayer);
                if (colliders != null && colliders.Length > 0)
                {
                    createButton.Button.interactable = false;
                    constructionObject.color = Color.red;
                }
                else
                {
                    constructionObject.color = Color.white;
                    createButton.Button.interactable = true;
                }
            }
        }


    }
}