using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using UnityEditor;
using UnityEngine;

namespace DTWorldz.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerAreaStack", menuName = "ScriptableObjects/PlayerAreaStack", order = 4)]
    public class PlayerAreaStack : ScriptableObject
    {
        [SerializeField]
        private Stack<PlayerAreaStackModel> lastPlayerAreas;

        public PlayerAreaStackModel Pop()
        {
            if (lastPlayerAreas == null || lastPlayerAreas.Count == 0)
            {
                return null;
            }
            return lastPlayerAreas.Pop();
        }

        public PlayerAreaStackModel Peek()
        {
            if (lastPlayerAreas == null || lastPlayerAreas.Count == 0)
            {
                return null;
            }
            return lastPlayerAreas.Peek();
        }

        public void Push(PlayerAreaStackModel model)
        {
            if (lastPlayerAreas == null)
            {
                lastPlayerAreas = new Stack<PlayerAreaStackModel>(); 
            }
            lastPlayerAreas.Push(model);
        }

        // This class is inside the TestClass so it could access its private fields
        // this custom editor will show up on any object with TestScript attached to it
        // you don't need (and can't) attach this class to a gameobject
        [CustomEditor(typeof(PlayerAreaStack))]
        public class StackPreview : Editor
        {
            public override void OnInspectorGUI()
            {

                // get the target script as TestScript and get the stack from it
                var ts = (PlayerAreaStack)target;
                var stack = ts.lastPlayerAreas;

                if (stack != null)
                {
                    // some styling for the header, this is optional
                    var bold = new GUIStyle();
                    bold.fontStyle = FontStyle.Bold;
                    GUILayout.Label("Items in my stack", bold);

                    // add a label for each item, you can add more properties
                    // you can even access components inside each item and display them
                    // for example if every item had a sprite we could easily show it 
                    foreach (var item in stack)
                    {
                        GUILayout.Label(item.AreaName + "-" + item.UniqueObjectName);
                        GUILayout.Label(item.LastPosition.x + ", " + item.LastPosition.y);                        
                        GUILayout.Label("-------------------------------------------------");
                    }

                    if(GUILayout.Button("Remove All")){
                        stack = new Stack<PlayerAreaStackModel>();
                    }
                }
            }
        }
    }
}