using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Player;
using DTWorldz.Scripts.Managers;
using UnityEngine;
namespace DTWorldz.Items.SO
{
    [CreateAssetMenu(fileName = "Food", menuName = "DTWorldz.Items/Items/Consumables/Food", order = 1)]
    public class FoodItemSO : BaseConsumableItemSO
    {
        public bool IsCooked;
        internal override void Use()
        {
            base.Use();

            GameManager.Instance.PlayerBehaviour.Eat(this);
        }
    }
}