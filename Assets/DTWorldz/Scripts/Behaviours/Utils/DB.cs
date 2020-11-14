using System.Collections;
using System.Collections.Generic;
using DTWorldz.ScriptableObjects.DB;
using UnityEngine;

public class DB : MonoBehaviour
{
    public ItemDatabase ItemDatabase;

    public static DB Instance { get; private set; }

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
}
