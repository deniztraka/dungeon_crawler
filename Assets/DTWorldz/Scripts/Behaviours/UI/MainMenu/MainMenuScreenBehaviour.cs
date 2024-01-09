using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
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
        private AudioManager audioManager;

        void Start()
        {
            saveSystemManager = GameObject.FindObjectOfType<SaveSystemManager>();
            var hasSavedGame = saveSystemManager.HasSavedGame();
            NewGameButton.interactable = hasSavedGame;
            SavedCharacterCanvas.enabled = hasSavedGame;
            NewCharacterCanvas.enabled = !hasSavedGame;
            CreateCharacterButton.interactable = hasSavedGame;

            audioManager = GetComponent<AudioManager>();

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
            audioManager.Play("ButtonClick");
            StartCoroutine(ExecuteAfterTime(CreateNewGame, 0.6f));
        }

        private void CreateNewGame()
        {
            saveSystemManager.ClearSaveData();
            var asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
            asyncSceneLoader.LoadScene("MainMenuScene", false, false);
        }

        public void SelectCharacter()
        {
            audioManager.Play("ButtonClick");
            StartCoroutine(ExecuteAfterTime(LoadGameScene, 0.6f));
        }


        private void LoadGameScene()
        {
            var asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
            asyncSceneLoader.LoadScene("GameScene", false, false);
        }

        public void CreateCharacter()
        {
            audioManager.Play("ButtonClick");
            StartCoroutine(ExecuteAfterTime(CreateNewCharacter, 0.6f));
        }

        private void CreateNewCharacter()
        {
            NewCharacterCanvas.enabled = false;
            SavedCharacterCanvas.enabled = true;
            Player.CreateDataModel();
            Player.SetCharacterName(characterName);
            Player.Save();
            var asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
            asyncSceneLoader.LoadScene("MainMenuScene", false, false);
        }

        IEnumerator ExecuteAfterTime(Action task, float time)
        {
            yield return new WaitForSeconds(time);
            task.Invoke();
        }


        public void Exit()
        {
            audioManager.Play("ButtonClick");
            StartCoroutine(ExecuteAfterTime(ExitGame, 0.6f));
        }

        public void ExitGame()
        {
            var gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.Quit();
        }
    }
}