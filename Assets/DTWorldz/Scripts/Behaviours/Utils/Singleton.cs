﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.Utils
{
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// e.g. public class MyClassName : Singleton<MyClassName> {}
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        // create a private reference to T instance
        private static T instance;

        public static T Instance
        {
            get
            {
                // if instance is null
                if (instance == null)
                {
                    // find the generic instance
                    instance = FindObjectOfType<T>();

                    // if it's null again create a new object
                    // and attach the generic instance
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        instance = obj.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        public virtual void Awake()
        {
            // create the instance
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}