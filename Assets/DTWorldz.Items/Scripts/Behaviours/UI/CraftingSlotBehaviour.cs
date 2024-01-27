using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTWorldz.Items.Behaviours.UI
{
    public class CraftingSlotBehaviour : MonoBehaviour
    {
        public Image IconBg;
        public Image Icon;
        public Text Title;
        public Text Description;

        public Recipe Recipe;

        public delegate void RecipeSelected(Recipe recipe);
        public event RecipeSelected OnRecipeSelected;

        public void ButtonClicked()
        {
            if (OnRecipeSelected != null)
            {
                OnRecipeSelected.Invoke(Recipe);
            }
        }

        internal void RemoveItem()
        {
            Icon.sprite = null;
            Icon.enabled = false;
            Recipe = null;
            Description.text = System.String.Empty;
            Title.text = System.String.Empty;
        }

        internal void SetRecipe(Recipe recipe)
        {
            Recipe = recipe;
            Icon.sprite = recipe.Output.Icon;
            Icon.enabled = true;
            Description.text = recipe.Output.Description;
            Title.text = recipe.Output.Name;
        }

        internal void SetRequirementsMet(bool requirementsMet)
        {
            // UPDATE UI TO SHOW IF REQUIREMENTS MET
            Title.color = requirementsMet ? Color.white : Color.red;
            var tempColor = Title.color;
            tempColor.a = requirementsMet ? 1 : 0.5f;
            Title.color = tempColor;

            IconBg.color = requirementsMet ? Color.white : Color.red;
            var tempIconBgColor = IconBg.color;
            tempIconBgColor.a = 0.5f;
            IconBg.color = tempIconBgColor;
        }
    }
}