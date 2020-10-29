using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours
{


    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private string currentAct;

        public bool IsTestMode;
        public GameObject TestCanvas;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);            
        }
        // Start is called before the first frame update
        void Start()
        {
            if (TestCanvas != null)
            {
                TestCanvas.SetActive(IsTestMode);
            }
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void SetCurrentAct(string actName){
            currentAct = actName;
        }

        public string GetCurrentAct(){
            return currentAct;;
        }
    }
}