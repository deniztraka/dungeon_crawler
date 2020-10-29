using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Player;
using DTWorldz.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{
    public class MainMenuScreenBehaviour : MonoBehaviour
    {
        public PlayerBehaviour Player;
        public Button NewGameButton;
        public Button CreateCharacterButton;
        public Canvas SavedCharacterCanvas;
        public Canvas NewCharacterCanvas;
        public InputField TitleInputField;
        public Text CharacterTitleText;
        private SaveSystemManager saveSystemManager;
        private string characterName;

        void Start()
        {
            saveSystemManager = GameObject.FindObjectOfType<SaveSystemManager>();
            var hasSavedGame = saveSystemManager.HasSavedGame();
            NewGameButton.interactable = hasSavedGame;
            SavedCharacterCanvas.enabled = hasSavedGame;
            NewCharacterCanvas.enabled = !hasSavedGame;
            CreateCharacterButton.interactable = hasSavedGame;

            TitleInputField.onEndEdit.AddListener(OnEndEdit);
            Player.OnAfterDataLoad += new PlayerBehaviour.DataLoaderHandler(UpdateCharacterPanel);
            Player.Load();
        }

        private void UpdateCharacterPanel()
        {            
            var playerDataModel = Player.GetDataModel();

            if (playerDataModel != null)
            {
                CharacterTitleText.text = playerDataModel.Name;
            }
        }

        private void OnEndEdit(string characterName)
        {
            if (characterName.Length >= 3)
            {
                this.characterName = characterName;
                CreateCharacterButton.interactable = true;
            }
        }

        public void NewGame()
        {
            saveSystemManager.ClearSaveData();
            var asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
            asyncSceneLoader.LoadScene("MainMenuScene", false, false);
        }

        public void SelectCharacter()
        {
            var asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
            asyncSceneLoader.LoadScene("Act1Scene", false, false);
        }

        public void CreateCharacter()
        {
            NewCharacterCanvas.enabled = false;
            SavedCharacterCanvas.enabled = true;
            Player.CreateDataModel();
            Player.SetCharacterName(characterName);
            Player.Save();
            var asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
            asyncSceneLoader.LoadScene("MainMenuScene", false, false);

        }



        public void Exit()
        {
            var gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.Quit();
        }
    }
}