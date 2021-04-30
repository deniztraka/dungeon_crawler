using DTWorldz.Items.SO;
using UnityEngine;

namespace DTWorldz.Items.Behaviours
{
    public class ItemBehaviour : MonoBehaviour
    {
        public BaseItemSO ItemSO;
        public int Quantity;
        [SerializeField]
        private Sprite unsetSprite;
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Vector3 tempPosition;
        private bool dragging;

        void Start()
        {
            animator = GetComponent<Animator>();
            dragging = false;
        }

        public void SetItem(BaseItemSO itemSO, int quantity){
            ItemSO = itemSO;
            Quantity = quantity;
            SetSprite();
        }

        void OnValidate()
        {
            SetSprite();
        }

        private void SetSprite()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            if (ItemSO != null)
            {
                spriteRenderer.sprite = ItemSO.Icon;
            }
            else
            {
                spriteRenderer.sprite = unsetSprite;
            }
        }
    }
}