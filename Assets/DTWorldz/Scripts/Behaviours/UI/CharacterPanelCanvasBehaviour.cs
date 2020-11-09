using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.UI
{
    public class CharacterPanelCanvasBehaviour : MonoBehaviour
    {
        public void ClosePanel(){
            var canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }
    }
}