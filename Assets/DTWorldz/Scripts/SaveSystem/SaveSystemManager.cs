using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DTWorldz.SaveSystem
{
    public class SaveSystemManager : MonoBehaviour
    {
        public delegate void SaveSystemHandler();
        public event SaveSystemHandler OnGameSave;
        public event SaveSystemHandler OnGameLoad;
        public string SavePath = "SaveData";

        void Start()
        {
            //LoadGame();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        internal void ClearSaveData()
        {
            var persistentPath = Application.persistentDataPath;            
            string filePath = Path.Combine(persistentPath, SavePath);
            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath, true);
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

        public bool HasSavedGame()
        {
            var persistentPath = Application.persistentDataPath;
            string filePath = Path.Combine(persistentPath, SavePath);
            return Directory.Exists(filePath);
        }
    }
}