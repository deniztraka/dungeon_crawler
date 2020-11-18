using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DTWorldz.Models;
using DTWorldz.Models.MobileStats;
using DTWorldz.ScriptableObjects.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemDetailPanel : MonoBehaviour
{
    public Image ItemIcon;
    public Text ItemName;
    public Text ItemDesc;
    public Text QualityText;
    public Text StatModifiersText;

    public Color PoorQualityColor;
    public Color RegularQualityColor;
    public Color ExceptionalQualityColor;
    public Color RareQualityColor;
    public Color LegendaryQualityColor;

    public void ShowItem(ItemModel itemModel)
    {
        transform.localScale = Vector3.one;
        ItemIcon.sprite = itemModel.ItemTemplate.Icon;
        ItemName.text = itemModel.ItemTemplate.Name;
        ItemDesc.text = itemModel.ItemTemplate.Description;
        QualityText.text = itemModel.StatQuality.ToString();
        StatModifiersText.text = GetStatsText(itemModel);
        // switch (item.StatQuality)
        // {
        //     case StatQuality.Poor:
        //         QualityText.color = PoorQualityColor;
        //         break;
        //     case StatQuality.Regular:
        //         QualityText.color = RegularQualityColor;
        //         break;
        //     case StatQuality.Exceptional:
        //         QualityText.color = ExceptionalQualityColor;
        //         break;
        //     case StatQuality.Rare:
        //         QualityText.color = RareQualityColor;
        //         break;
        //     case StatQuality.Legendary:
        //         QualityText.color = LegendaryQualityColor;
        //         break;
        // }
    }

    private string GetStatsText(ItemModel itemModel)
    {
        var sb = new StringBuilder();
        if (itemModel.StrengthModifier != null && itemModel.StrengthModifier.Value != 0)
        {
            sb.AppendFormat("{0}{1} Strength", itemModel.StrengthModifier.Value > 0 ? "+ " : "- ", itemModel.StrengthModifier.Value);
            sb.AppendLine();
        }

        if (itemModel.DexterityModifier != null && itemModel.DexterityModifier.Value != 0)
        {
            sb.AppendFormat("{0}{1} Dexterity", itemModel.DexterityModifier.Value > 0 ? "+ " : "- ", itemModel.DexterityModifier.Value);
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public void ClosePanel()
    {

        transform.localScale = Vector3.zero;
        ItemIcon.sprite = null;
        ItemName.text = null;
    }


}
