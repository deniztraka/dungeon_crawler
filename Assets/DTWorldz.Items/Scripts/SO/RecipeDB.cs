using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTWorldz.Items.SO
{
    [CreateAssetMenu(fileName = "RecipeDB", menuName = "DTWorldz.Items/RecipeDB", order = 0)]
    public class RecipeDB : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<int> ids;
        [SerializeField]
        private List<Recipe> recipes;

        private Dictionary<int, Recipe> recipeDictionary = new Dictionary<int, Recipe>();

        public List<Recipe> GetAll()
        {
            return recipes;
        }

        public void OnBeforeSerialize()
        {
            ids = new List<int>();
            recipes = new List<Recipe>();

            foreach (var kvp in recipeDictionary)
            {
                ids.Add(kvp.Key);
                recipes.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            recipeDictionary = new Dictionary<int, Recipe>();
            ids = new List<int>();
            for (int i = 0; i < recipes.Count; i++)
            {
                ids.Add(i);
                if (recipes[i] != null)
                {
                    recipes[i].Id = i;
                }

                recipeDictionary.Add(ids[i], recipes[i]);
            }
        }
    }
}