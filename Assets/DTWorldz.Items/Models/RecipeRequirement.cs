using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.SO;
using UnityEngine;
namespace DTWorldz.Items.Models
{
    [Serializable]
    public class RecipeRequirement
    {
        public BaseItemSO ItemSO;
        public int Quantity;
    }
}