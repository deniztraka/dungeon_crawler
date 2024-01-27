using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.Models;
using UnityEngine;

namespace DTWorldz.Items.SO
{
    [CreateAssetMenu(fileName = "CraftingRecipe", menuName = "DTWorldz.Items/Crafting Recipe", order = 1)]
    public class Recipe : ScriptableObject, IRecipe
    {
        [SerializeField]
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name;
        public string Description;
        public float CraftingTime;

        [SerializeField]
        private List<RecipeRequirement> requirements;
        public List<RecipeRequirement> Requirements
        {
            get { return requirements; }
            set { requirements = value; }
        }


        [SerializeField]
        private BaseItemSO output;
        public BaseItemSO Output
        {
            get { return output; }
            set { output = value; }
        }
    }
}
