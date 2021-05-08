using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Player;
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
            var playerGameObject = GameObject.FindGameObjectWithTag("Player");
            var player = playerGameObject.GetComponent<PlayerBehaviour>();
            player.Eat(this);
        }
    }
}