using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DTWorldz.SaveSystem;
using UnityEngine;
namespace DTWorldz.DataModel
{
    public abstract class BaseDataModel : ISavable
    {
        private SaveSystemManager saveSystemManager;
        private string dataObjName;
        public string DataObjName
        {
            get { return dataObjName; }
            set { dataObjName = value; }
        }

        protected BaseDataModel(SaveSystemManager saveSystemManager, string dataObjName)
        {
            this.saveSystemManager = saveSystemManager;
            this.dataObjName = dataObjName;
        }

        public virtual T OnLoad<T>()
        {
            var persistentPath = Application.persistentDataPath;
            string filePath = Path.Combine(persistentPath, saveSystemManager.SavePath);
            filePath = Path.Combine(filePath, DataObjName + ".dnz");
            var obj = default(T);
            if (File.Exists(filePath))
            {
                var bf = new BinaryFormatter();
                using (var file = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    var deserializedObjString = (System.String)bf.Deserialize(file);
                    //JsonUtility.FromJsonOverwrite(deserializedObjString, deserializedObj);
                    obj = JsonUtility.FromJson<T>(deserializedObjString);
                }
            }

            return obj;
        }

        public virtual void OnSave<T>(T obj)
        {

            var persistentPath = Application.persistentDataPath;
            string filePath = Path.Combine(persistentPath, saveSystemManager.SavePath);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);                
            }

            filePath = Path.Combine(filePath, DataObjName + ".dnz");
            var jsonData = JsonUtility.ToJson(obj);

            var bf = new BinaryFormatter();
            using (var file = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                bf.Serialize(file, jsonData);
            }

        }
    }

}