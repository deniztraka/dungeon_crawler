using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Player;
using DTWorldz.Items.Behaviours.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DTWorldz.Items.Behaviours
{
    public class GroundItemDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private GameObject DragHelperObject;
        private SpriteRenderer draggedItemSprite;
        private Color tempColor;
        private GameObject tempDraggingObject;
        private ItemBehaviour item;
        private bool dragStarted = false;
        private BackpackCanvasBehaviour backpackCanvasBehaviour;
        public Vector2 DraggingOffset = new Vector2(1, 1);

        void Start()
        {
            item = GetComponent<ItemBehaviour>(); ;
            draggedItemSprite = GetComponentInChildren<SpriteRenderer>();
            backpackCanvasBehaviour = FindObjectOfType<BackpackCanvasBehaviour>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            var playerObj = GameObject.FindWithTag("Player");
            var playerBehaviour = playerObj.GetComponent<PlayerBehaviour>();
            if (playerBehaviour.InteractionDistance <= Vector2.Distance(transform.position, playerObj.transform.position))
            {
                return;
            }

            dragStarted = true;
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tempColor = draggedItemSprite.color;
            tempDraggingObject = Instantiate(DragHelperObject, pos, Quaternion.identity);

            tempDraggingObject.GetComponent<SpriteRenderer>().sprite = draggedItemSprite.sprite;
            draggedItemSprite.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.5f);
        }

        private bool IsOverBackpack()
        {   
            //Debug.Log(Input.mousePosition);
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var updatedPos = new Vector3(mousePos.x, mousePos.y, 0f) + new Vector3(DraggingOffset.x, DraggingOffset.y, 0f);
            tempDraggingObject.transform.position = updatedPos;

            var isOver = false;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // write debug info on the screen point of raycast
            var screenPoint = Camera.main.WorldToScreenPoint(ray.origin);
            var colliders = Physics2D.OverlapCircleAll(screenPoint, 1f);
            if (colliders != null && colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.gameObject.tag == "BackpackUI")
                    {
                        isOver = true;
                        break;
                    }
                }
            }

             

            return isOver;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (tempDraggingObject != null && backpackCanvasBehaviour != null)
            {
                backpackCanvasBehaviour.SetStatus(IsOverBackpack());
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!dragStarted)
            {
                return;
            }
            draggedItemSprite.color = tempColor;
            if (tempDraggingObject != null)
            {
                Destroy(tempDraggingObject);
            }

            if (IsOverBackpack())
            {
                var playerObj = GameObject.FindWithTag("Player");
                var playerInventory = playerObj.GetComponent<InventoryBehaviour>();
                playerInventory.AddItem(item);
            }

            if (backpackCanvasBehaviour != null)
            {
                backpackCanvasBehaviour.SetStatus(false);
            }
        }
    }
}