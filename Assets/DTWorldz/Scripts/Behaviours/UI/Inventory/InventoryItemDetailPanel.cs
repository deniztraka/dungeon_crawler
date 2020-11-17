using System.Collections;
using System.Collections.Generic;
using DTWorldz.ScriptableObjects.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemDetailPanel : MonoBehaviour {
    public Image ItemIcon;
    public Text ItemName;
    public Text ItemDesc;

    public void ShowItem(BaseItem item)
    {
        transform.localScale = Vector3.one;
        ItemIcon.sprite = item.Icon;
        ItemName.text = item.Name;
        ItemDesc.text = item.Description;
    }

    public void ClosePanel()
    {

       transform.localScale = Vector3.zero;
        ItemIcon.sprite = null;
        ItemName.text = null;
    }

    
}
