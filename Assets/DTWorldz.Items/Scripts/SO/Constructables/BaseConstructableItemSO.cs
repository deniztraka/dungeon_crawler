using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.Behaviours.UI;
using UnityEngine;
namespace DTWorldz.Items.SO
{
    public abstract class BaseConstructableItemSO : BaseItemSO
    {
        internal virtual void Construct()
        {
            Debug.Log("Constructing");
            GameObject.Find("PlayerInventoryPanel").GetComponent<InventoryUI>().Close();
            GameObject.Find("ConstructionPanelCanvas").GetComponent<ConstructionPanelUI>().Open(this);
        }
    }
}