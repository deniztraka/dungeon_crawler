using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Player;
using DTWorldz.Models.MobileStats;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{
    public class ItemLabelCanvas : MonoBehaviour
    {
        public Color PoorQualityColor;
        public Color RegularQualityColor;
        public Color ExceptionalQualityColor;
        public Color RareQualityColor;
        public Color LegendaryQualityColor;

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

        internal void SetLabelColor(StatQuality statQuality)
        {
            var itemLabelTransform = transform.Find("ButtonLabel").Find("ItemLabelText");
            if (itemLabelTransform != null)
            {
                
                var labelText = itemLabelTransform.GetComponent<Text>();
                switch (statQuality)
                {
                    case StatQuality.Poor:
                        labelText.color = PoorQualityColor;
                        break;
                    case StatQuality.Regular:
                        labelText.color = RegularQualityColor;
                        break;
                    case StatQuality.Exceptional:
                        labelText.color = ExceptionalQualityColor;
                        break;
                    case StatQuality.Rare:
                        labelText.color = RareQualityColor;
                        break;
                    case StatQuality.Legendary:
                        labelText.color = LegendaryQualityColor;
                        break;
                }
            }
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