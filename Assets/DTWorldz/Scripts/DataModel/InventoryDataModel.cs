using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using DTWorldz.SaveSystem;
using DTWorldz.ScriptableObjects.Items;
using UnityEngine;
namespace DTWorldz.DataModel
{

    [Serializable]
    public class InventoryDataModel : BaseDataModel
    {
        public List<ItemModel> ItemModels;
        public InventoryDataModel(SaveSystemManager saveSystemManager) : base(saveSystemManager, "inventory")
        {
            ItemModels = new List<ItemModel>();
        }

        public bool Load()
        {
            var tempModel = base.OnLoad<InventoryDataModel>();
            if (tempModel != null)
            {
                foreach (var item in tempModel.ItemModels)
                {
                    ItemModels.Add(item);
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