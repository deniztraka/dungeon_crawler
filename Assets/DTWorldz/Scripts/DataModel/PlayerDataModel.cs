using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DTWorldz.Models.MobileStats;
using DTWorldz.SaveSystem;
using UnityEngine;
namespace DTWorldz.DataModel
{

    [Serializable]
    public class PlayerDataModel : BaseDataModel
    {
        public int GoldAmount;
        public string Name;

        public Strength Strength;
        public Dexterity Dexterity;

        public PlayerDataModel(SaveSystemManager saveSystemManager) : base(saveSystemManager, "player")
        {            
        }


        public bool Load()
        {

            var tempModel = base.OnLoad<PlayerDataModel>();
            if (tempModel != null)
            {
                this.GoldAmount = tempModel.GoldAmount;
                this.Name = tempModel.Name;
                this.Dexterity = tempModel.Dexterity;
                this.Strength = tempModel.Strength;
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