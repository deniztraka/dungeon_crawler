using System.Collections;
using System.Collections.Generic;
using DTWorldz.SaveSystem;
using UnityEngine;

namespace DTWorldz.Behaviours.UI
{
    public class MainMenuScreenBehaviour : MonoBehaviour
    {
        void Start()
        {

        }

        public void NewGame()
        {
            var asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
            asyncSceneLoader.LoadScene("Act1Scene", false, false);
            var saveSystemManager = GameObject.FindObjectOfType<SaveSystemManager>();
            saveSystemManager.ClearSaveData();
        }

        public void SelectCharacter()
        {
            var asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
            asyncSceneLoader.LoadScene("Act1Scene", false, false);
        }

        public void Exit()
        {
            var gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.Quit();
        }
    }
}