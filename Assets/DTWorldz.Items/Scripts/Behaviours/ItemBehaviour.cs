using System.Collections;
using DTWorldz.Items.SO;
using DTWorldz.Scripts.Managers;
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
        private Vector3 tempPosition;
        private bool dragging;

        [SerializeField]
        private Material outlineMaterial;
        private Material tempMaterial;
        private Transform playerTransform;
        private Canvas labelTextCanvas;

        void Start()
        {
            playerTransform = GameManager.Instance.PlayerBehaviour.transform;
            labelTextCanvas = GetComponentInChildren<Canvas>();
            if (labelTextCanvas != null)
            {
                labelTextCanvas.gameObject.SetActive(false);
            }
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                tempMaterial = spriteRenderer.material;
            }
            dragging = false;

            StartCoroutine(CheckDistanceFromPlayer());
        }

        void OnMouseDown()
        {
            if (labelTextCanvas != null)
            {
                StartCoroutine(ShowLabel());
            }
        }

        private IEnumerator ShowLabel()
        {
            labelTextCanvas.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            labelTextCanvas.gameObject.SetActive(false);
        }

        private IEnumerator CheckDistanceFromPlayer()
        {
            while (true)
            {

                yield return new WaitForSeconds(0.5f);

                if (playerTransform != null && outlineMaterial != null && spriteRenderer != null && tempMaterial != null)
                {
                    var closeEnough = Vector2.Distance(playerTransform.position, transform.position) < 2;
                    spriteRenderer.material = closeEnough ? outlineMaterial : tempMaterial;
                }
            }
        }

        public void SetItem(BaseItemSO itemSO, int quantity)
        {
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