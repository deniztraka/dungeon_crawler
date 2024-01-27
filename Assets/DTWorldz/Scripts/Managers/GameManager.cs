using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Player;
using DTWorldz.Behaviours.Utils;
using UnityEngine;
namespace DTWorldz.Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public PlayerBehaviour PlayerBehaviour;

        internal void Quit()
        {
            Application.Quit();
        }
    }
}