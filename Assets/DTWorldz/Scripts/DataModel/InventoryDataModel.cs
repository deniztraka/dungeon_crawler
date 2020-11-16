using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.SaveSystem;
using DTWorldz.ScriptableObjects.Items;
using UnityEngine;
namespace DTWorldz.DataModel
{

    [Serializable]
    public class InventoryDataModel : BaseDataModel
    {
        public List<BaseItem> Items;
        public InventoryDataModel(SaveSystemManager saveSystemManager) : base(saveSystemManager, "inventory")
        {
            Items = new List<BaseItem>();
        }

        public bool Load()
        {
            var tempModel = base.OnLoad<InventoryDataModel>();
            if (tempModel != null)
            {
                foreach (var item in tempModel.Items)
                {
                    Items.Add(item);
                }
                return true;
            }
            return false;
        }

        public void Save()
        {
            base.OnSave<InventoryDataModel>(this);
        }
    }
}