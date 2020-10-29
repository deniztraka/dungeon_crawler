using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTWorldz.Behaviours.UI
{



    public class MainMenuScreenBehaviour : MonoBehaviour
    {
        public AsyncSceneLoader AsyncSceneLoader;
        public void NewGame()
        {
            AsyncSceneLoader.LoadScene("Act1Scene", false);
        }

        public void SelectCharacter()
        {
            AsyncSceneLoader.LoadScene("Act1Scene", false);
        }

        public void Exit()
        {
            GameManager.Instance.Quit();
        }
    }
}