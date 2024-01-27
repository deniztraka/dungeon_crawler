using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Player;
using DTWorldz.Scripts.Managers;
using UnityEngine;
namespace DTWorldz.Items.SO
{
    [CreateAssetMenu(fileName = "HealthRegenerator", menuName = "DTWorldz.Items/Items/Consumables/HealthRegenerators", order = 1)]
    public class HealthRegeneratorSO : BaseConsumableItemSO
    {
        
        internal override void Use()
        {
            base.Use();
            GameManager.Instance.PlayerBehaviour.DrinkHealthPotion(this.RegenAmount);
        }
    }
}