using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{
    public class CharacterPanelCanvasBehaviour : MonoBehaviour
    {
        public Text GoldText;
        public Text StrText;
        public Text DexterityText;
        public Text HealthText;
        public Text EnergyText;

        public void ClosePanel()
        {
            var canvas = GetComponent<Canvas>();
            canvas.enabled = false;

            var inventoryItemDetailPanel = transform.GetComponentInChildren<InventoryItemDetailPanel>();
            if (inventoryItemDetailPanel != null)
            {
                inventoryItemDetailPanel.ClosePanel();
            }
        }

        public void UpdateGoldText(int goldValue)
        {
            if (GoldText != null)
            {
                GoldText.text = goldValue.ToString();
            }
        }

        public void UpdateStats(int strCurrentValue, int dexCurrentValue)
        {
            StrText.text = strCurrentValue.ToString();
            HealthText.text = (strCurrentValue * 5).ToString();
            DexterityText.text = dexCurrentValue.ToString();
            EnergyText.text = (dexCurrentValue * 5).ToString();
        }
    }
}