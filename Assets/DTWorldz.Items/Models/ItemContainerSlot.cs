using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.SO;
using UnityEngine;
namespace DTWorldz.Items.Models
{
    [Serializable]
    public class ItemContainerSlot
    {
        public BaseItemSO ItemSO;
        public int Quantity;

        public ItemContainerSlot(BaseItemSO itemSO, int quantity)
        {
            ItemSO = itemSO;
            Quantity = quantity;
        }
    }
}