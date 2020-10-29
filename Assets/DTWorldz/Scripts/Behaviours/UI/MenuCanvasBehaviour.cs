using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasBehaviour : MonoBehaviour
{
    public void ReturnToMenu()
    {
        var asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
        asyncSceneLoader.LoadScene("MainMenuScene", false, true);
    }
}
