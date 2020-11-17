using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Player;
using UnityEngine;
namespace DTWorldz.Behaviours.UI
{
    public class ItemLabelCanvas : MonoBehaviour
    {
        public GameObject PickupButtonsPanel;
        AudioManager audioManager;
        BaseItemBehaviour itemBehaviour;
        Canvas labelCanvas;
        // Start is called before the first frame update
        void Start()
        {
            audioManager = GetComponent<AudioManager>();
            labelCanvas = GetComponent<Canvas>();
            itemBehaviour = transform.GetComponentInParent<BaseItemBehaviour>();
        }

        public void OnLabelClicked()
        {
            PickupButtonsPanel.SetActive(true);
        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            labelCanvas.enabled = true;
        }

        public virtual void OnTriggerExit2D(Collider2D collider)
        {
            labelCanvas.enabled = false;
            PickupButtonsPanel.SetActive(false);
        }

        public void OnPickupClicked()
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
            var isSucceed = player.PickupItem(itemBehaviour);
            if (isSucceed)
            {
                Destroy(itemBehaviour.gameObject);
            }
            PickupButtonsPanel.SetActive(false);
        }

        public void OnCancelClicked()
        {
            PickupButtonsPanel.SetActive(false);
        }
    }
}