using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DTWorldz.Models;
using DTWorldz.SaveSystem;
using UnityEngine;
namespace DTWorldz.DataModel
{

    [Serializable]
    public class ItemSlotDataModel : BaseDataModel
    {

        public ItemSlotDataModel(SaveSystemManager saveSystemManager, string slotName) : base(saveSystemManager, slotName)
        {

        }        

        public void Save()
        {
            base.OnSave<ItemSlotDataModel>(this);
        }
        public void Save<T>(T obj)
        {
            base.OnSave<T>(obj);
        }
    }
}
