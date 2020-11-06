using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Utils;
using UnityEngine;
namespace DTWorldz.Behaviours.UI
{
    public class EventTextManager : MonoBehaviour
    {
        public static EventTextManager Instance { get; private set; }
        public GameObject EventTextPrefab;
        public Transform TextParent;

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
        
        public void Add(string text)
        {
            var eventTextObject = Instantiate(EventTextPrefab, Vector3.zero, Quaternion.identity, TextParent);
            var eventText = eventTextObject.GetComponent<EventTextWrapperBehaviour>();
            eventText.SetText(text);

        }
    }
}