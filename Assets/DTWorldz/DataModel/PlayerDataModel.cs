using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DTWorldz.SaveSystem;
using UnityEngine;
namespace DTWorldz.DataModel
{

    [Serializable]
    public class PlayerDataModel : BaseDataModel
    {
        public int GoldAmount;        


        public PlayerDataModel(SaveSystemManager saveSystemManager) : base(saveSystemManager, "player")
        {
        }

        public bool Load()
        {
            var tempModel = base.OnLoad<PlayerDataModel>();
            if (tempModel != null)
            {
                this.GoldAmount = tempModel.GoldAmount;
                return true;
            }
            return false;
        }

        public void Save()
        {
            base.OnSave<PlayerDataModel>(this);
        }
    }

}