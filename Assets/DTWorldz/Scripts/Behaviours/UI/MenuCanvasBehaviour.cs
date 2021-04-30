using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasBehaviour : MonoBehaviour
{
    private bool isOpen = false;
    // public void ReturnToMenu()
    // {
    //     var asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
    //     asyncSceneLoader.LoadScene("MainMenuScene", false, true);
    // }

    internal void SetStatus(bool isOpen)
    {
        if(this.isOpen != isOpen){
            this.isOpen = isOpen;
        }
    }
}
