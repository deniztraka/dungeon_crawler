using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.Models;
using UnityEngine;
using UnityEngine.UI;
namespace DTWorldz.Items.Behaviours.UI
{
    public class CraftingRequirementSlotBehaviour : MonoBehaviour
    {
        public Text QuantityText;
        public Text TitleText;
        public Image Image;

        internal void SetIsMissing(bool isMissing)
        {
            TitleText.color = isMissing ? Color.red : Color.white;
        }

        internal void SetRequirement(RecipeRequirement requirement)
        {
            QuantityText.text = requirement.Quantity.ToString();
            TitleText.text = requirement.ItemSO.Name;
            Image.sprite = requirement.ItemSO.Icon;
        }
    }
}