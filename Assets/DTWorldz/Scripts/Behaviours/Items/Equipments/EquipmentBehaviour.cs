using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using DTWorldz.Models.MobileStats;
using DTWorldz.ScriptableObjects.Items;
using UnityEngine;
namespace DTWorldz.Behaviours.Items
{
    public class EquipmentBehaviour : BaseItemBehaviour
    {        

        public override void SetCount(int count)
        {
            count = 1;
        }

        public override void OnTriggerEnter2D(Collider2D collider)
        {
            base.OnTriggerEnter2D(collider);            
        }

        public override void SetModifiers(int minStatCount, int maxStatCount, StatQuality statQuality)
        {
            base.SetModifiers(minStatCount, maxStatCount, statQuality);            
        }

        public new EquipmentItemModel GetModel()
        {
            throw new System.NotImplementedException();
        }
    }
}