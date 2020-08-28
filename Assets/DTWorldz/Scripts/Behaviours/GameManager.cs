﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours
{


    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public bool IsTestMode;
        public GameObject TestCanvas;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            if (TestCanvas != null)
            {
                TestCanvas.SetActive(IsTestMode);
            }
        }
    }
}