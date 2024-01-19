using System;
using System.Collections;
using DTWorldz.Items.SO;
using DTWorldz.Utils;
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
        public GameObject RecipeRequirementPrefab;
        private UnityEngine.UI.Image outputImage;
        private Text outputTitle;
        private Text outputDescription;
        private Button createButton;
        private Transform requirementsContent;
        private InventoryBehaviour inventoryBehaviour;

        private GameObject loadingPanel;
        private Slider craftingProgressBar;

        void Start()
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                inventoryBehaviour = playerObject.GetComponent<InventoryBehaviour>();
            }
            InitUI();
            InitRecipes();
            ClearExistingRequirements();
        }



        private void InitUI()
        {
            canvas = GetComponent<Canvas>();
            outputImage = transform.FindDeepChild("OutputImage").GetComponent<UnityEngine.UI.Image>();
            outputTitle = transform.FindDeepChild("OutputTitle").GetComponent<Text>();
            outputDescription = transform.FindDeepChild("OutputDescription").GetComponent<Text>();
            createButton = transform.FindDeepChild("CreateButton").GetComponent<Button>();
            requirementsContent = transform.FindDeepChild("RequirementsContent");
            loadingPanel = transform.FindDeepChild("CraftingProgressBar").gameObject;
            craftingProgressBar = transform.FindDeepChild("CraftingSlider").GetComponent<Slider>();
            DisableCreationUI();
            loadingPanel.SetActive(false);
        }

        private void DisableCreationUI()
        {
            outputImage.sprite = null;
            outputImage.enabled = false;
            outputTitle.text = String.Empty;
            outputDescription.text = String.Empty;
            createButton.interactable = false;
        }

        private void InitRecipes()
        {
            // remove all existing recipes
            foreach (Transform child in SlotsContainer)
            {
                Destroy(child.gameObject);
            }

            var recipeCounter = 0;
            // add all recipes from RecipeDB
            foreach (var recipe in RecipeDB.GetAll())
            {
                var recipeObject = Instantiate(RecipePrefab, SlotsContainer);
                var recipeSlotBehaviour = recipeObject.GetComponent<CraftingSlotBehaviour>();
                recipeSlotBehaviour.SetRecipe(recipe);
                // check if requirements met
                recipeSlotBehaviour.SetRequirementsMet(CheckRecipeRequirements(recipe));
                recipeSlotBehaviour.OnRecipeSelected += RecipeSelected;
                recipeCounter++;
            }
        }

        bool CheckRecipeRequirements(Recipe recipe)
        {
            var requirementsMet = true;
            foreach (var requirement in recipe.Requirements)
            {
                inventoryBehaviour.HasItem(requirement.ItemSO, requirement.Quantity, (hasItem) =>
                {
                    if (!hasItem)
                    {
                        requirementsMet = false;
                    }
                });
            }

            return requirementsMet;
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

            ClearExistingRequirements();

            // add all requirements
            foreach (var requirement in recipe.Requirements)
            {
                var requirementObject = Instantiate(RecipeRequirementPrefab, requirementsContent);
                var requirementSlotBehaviour = requirementObject.GetComponent<CraftingRequirementSlotBehaviour>();
                requirementSlotBehaviour.SetRequirement(requirement);
                // get player's inventory and check if the item is there and if the amount is enough
                inventoryBehaviour.HasItem(requirement.ItemSO, requirement.Quantity, (hasItem) =>
                {
                    requirementSlotBehaviour.SetIsMissing(!hasItem);
                    if (!hasItem)
                    {
                        createButton.interactable = false;
                    }
                });
            }

            var rectTransform = requirementsContent.gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, recipe.Requirements.Count * 25);
        }

        void ClearExistingRequirements()
        {
            // remove all existing requirements
            foreach (Transform child in requirementsContent)
            {
                Destroy(child.gameObject);
            }
        }

        private void Craft(Recipe recipe)
        {

            StartCoroutine(UpdateProgressBar(recipe));

        }

        IEnumerator UpdateProgressBar(Recipe recipe)
        {
            // Example: simulate a process that takes 5 seconds
            float elapsed = 0f;
            loadingPanel.SetActive(true);

            while (elapsed < recipe.CraftingTime)
            {
                elapsed += Time.deltaTime;
                craftingProgressBar.value = elapsed / recipe.CraftingTime;
                yield return null; // Wait for the next frame
            }

            craftingProgressBar.value = 1f; // Ensure it's filled at the end

            // remove items from inventory
            foreach (var requirement in recipe.Requirements)
            {
                for (int i = 0; i < requirement.Quantity; i++)
                {
                    inventoryBehaviour.RemoveItem(requirement.ItemSO);
                }
            }

            //inventoryBehaviour.ItemContainer.AddItem(recipe.Output, 1);

            loadingPanel.SetActive(false);
            Reset();
            SetOutputUI(recipe);
        }

        public void Open()
        {
            canvas.enabled = true;
            DisableCreationUI();
            ClearExistingRequirements();
            InitRecipes();
        }

        private void Reset()
        {
            DisableCreationUI();
            ClearExistingRequirements();
            InitRecipes();
        }

        public void Close()
        {
            canvas.enabled = false;

        }
    }
}