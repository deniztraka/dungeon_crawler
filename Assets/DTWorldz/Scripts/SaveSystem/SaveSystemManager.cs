using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTWorldz.SaveSystem
{
    public class SaveSystemManager : MonoBehaviour
    {
        public delegate void SaveSystemHandler();
        public event SaveSystemHandler OnGameSave;
        public event SaveSystemHandler OnGameLoad;

        public static SaveSystemManager Instance { get; private set; }
        public string SavePath = "SaveData";

        void Start(){
            LoadGame();
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SaveGame()
        {
            if (OnGameSave != null)
            {
                OnGameSave();
            }
        }

        public void LoadGame()
        {
            if (OnGameLoad != null)
            {
                OnGameLoad();
            }
        }
    }
}