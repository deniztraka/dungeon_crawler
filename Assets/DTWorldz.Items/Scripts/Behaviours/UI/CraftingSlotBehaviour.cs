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
        public Image Icon;
        public Text Title;
        public Text Description;

        public Recipe Recipe;

        public delegate void RecipeSelected(Recipe recipe);
        public event RecipeSelected OnRecipeSelected;

        void Start()
        {
            //TODO: check if requirements met to create this item
            // better to do it in CraftingPanelUI since we can cache Player's inventory there
            
            // if (Recipe != null)
            // {
            //     var playerObject = GameObject.FindGameObjectWithTag("Player");
            //     if (playerObject != null)
            //     {
            //         var inventoryBehaviour = playerObject.GetComponent<InventoryBehaviour>();
            //         if (inventoryBehaviour != null && inventoryBehaviour.ItemContainer != null)
            //         {
            //             var itemContainer = inventoryBehaviour.ItemContainer;
            //             var requirementsMet = true;
            //             foreach (var requirement in Recipe.Requirements)
            //             {
            //                 var item = itemContainer.GetItem(requirement.Item);
            //                 if (item == null || item.Amount < requirement.Amount)
            //                 {
            //                     requirementsMet = false;
            //                     break;
            //                 }
            //             }
            //             if (requirementsMet)
            //             {
            //                 createButton.interactable = true;
            //             }
            //         }
            //     }
            // }
        }

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
    }
}