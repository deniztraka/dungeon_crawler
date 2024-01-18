using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.SO;
using DTWorldz.Utils;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
namespace DTWorldz.Items.Behaviours.UI
{
    public class CraftingPanelUI : MonoBehaviour
    {
        private Canvas canvas;
        public Transform SlotsContainer;
        public RecipeDB RecipeDB;

        public GameObject RecipePrefab;
        private UnityEngine.UI.Image outputImage;
        private Text outputTitle;
        private Text outputDescription;
        private Button createButton;

        void Start()
        {
            InitUI();
            InitRecipes();
            
        }

        

        private void InitUI()
        {
            canvas = GetComponent<Canvas>();
            outputImage = transform.FindDeepChild("OutputImage").GetComponent<UnityEngine.UI.Image>();
            outputTitle = transform.FindDeepChild("OutputTitle").GetComponent<Text>();
            outputDescription = transform.FindDeepChild("OutputDescription").GetComponent<Text>();
            createButton = transform.FindDeepChild("CreateButton").GetComponent<Button>();
            DisableCreationUI();
        }

        private void DisableCreationUI()
        {
            outputImage.sprite = null;
            outputImage.enabled = false;
            outputTitle.text = String.Empty;
            outputDescription.text = String.Empty;
            createButton.interactable = false;

            //TODO: ENABLE EXPLANATION TEXT FOR CRAFTING
        }

        private void InitRecipes()
        {
            // remove all existing recipes
            foreach (Transform child in SlotsContainer)
            {
                Destroy(child.gameObject);
            }

            // add all recipes from RecipeDB
            foreach (var recipe in RecipeDB.GetAll())
            {
                var recipeObject = Instantiate(RecipePrefab, SlotsContainer);
                var recipeSlotBehaviour = recipeObject.GetComponent<CraftingSlotBehaviour>();
                recipeSlotBehaviour.SetRecipe(recipe);
                //TODO: check if requirements met to create this item
                recipeSlotBehaviour.OnRecipeSelected += RecipeSelected;
            }
        }

        private void RecipeSelected(Recipe recipe)
        {
            Debug.Log("Recipe selected: " + recipe.Output.Name);
            SetOutputUI(recipe);
        }

        private void SetOutputUI(Recipe recipe)
        {
            //TODO: disable EXPLANATION TEXT FOR CRAFTING

            outputImage.sprite = recipe.Output.Icon;
            outputImage.enabled = true;
            outputTitle.text = recipe.Output.Name;
            outputDescription.text = recipe.Output.Description;
            createButton.interactable = true;
            createButton.onClick.RemoveAllListeners();
            createButton.onClick.AddListener(() => Craft(recipe));

            //TODO: set whats needed to craft
            //TODO: set red color to a requirement label if it's not met
        }

        private void Craft(Recipe recipe)
        {
            Debug.Log("Crafting: " + recipe.Output.Name);
            createButton.onClick.RemoveAllListeners();

            //TODO: create the item on players inventory
            DisableCreationUI();
        }

        public void Open()
        {
            canvas.enabled = true;
        }

        public void Close()
        {
            canvas.enabled = false;
        }
    }
}