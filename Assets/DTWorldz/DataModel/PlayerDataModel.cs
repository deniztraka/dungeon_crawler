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


        public PlayerDataModel() : base("player")
        {
        }

        public void Load()
        {
            var tempModel = base.OnLoad<PlayerDataModel>();
            if (tempModel != null)
            {
                this.GoldAmount = tempModel.GoldAmount;
            }
        }

        public void Save()
        {
            base.OnSave<PlayerDataModel>(this);
        }
    }

}