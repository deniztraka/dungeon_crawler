using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DTWorldz.Behaviours.UI
{
    public class CharacterBarCanvas : MonoBehaviour
    {
        public Text CharacterTitleText;
        public void UpdateCharacterTitleText(string text)
        {
            CharacterTitleText.text = text;
        }
    }
}