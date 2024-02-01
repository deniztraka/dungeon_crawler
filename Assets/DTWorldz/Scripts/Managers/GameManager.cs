using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Player;
using DTWorldz.Behaviours.Utils;
using DTWorldz.Items.SO;
using UnityEngine;
namespace DTWorldz.Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public PlayerBehaviour PlayerBehaviour;
        public RecipeDB RecipeDB;

        public Transform PlayerEquipmentSlotsWrapper;

        internal void Quit()
        {
            Application.Quit();
        }
    }
}