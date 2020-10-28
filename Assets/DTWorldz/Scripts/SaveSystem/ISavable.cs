using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.SaveSystem
{
    interface ISavable
    {
        string DataObjName { get; set; }
        void OnSave<T>(T obj);
        T OnLoad<T>();
    }
}