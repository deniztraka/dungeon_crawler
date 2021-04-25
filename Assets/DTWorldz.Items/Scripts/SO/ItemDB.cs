using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTWorldz.Items.SO
{
    [CreateAssetMenu(fileName = "ItemDB", menuName = "DTWorldz.Items/ItemDB", order = 0)]
    public class ItemDB : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<int> ids;
        [SerializeField]
        private List<BaseItemSO> items;

        //Unity doesn't know how to serialize a Dictionary
        public Dictionary<int, BaseItemSO> ItemDictionary = new Dictionary<int, BaseItemSO>();

        internal BaseItemSO GetItemById(int id)
        {
            BaseItemSO outItem = null;
            ItemDictionary.TryGetValue(id, out outItem);
            return outItem;
        }

        public void OnBeforeSerialize()
        {
            ids = new List<int>();
            items = new List<BaseItemSO>();

            foreach (var kvp in ItemDictionary)
            {
                ids.Add(kvp.Key);
                items.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            ItemDictionary = new Dictionary<int, BaseItemSO>();
            ids = new List<int>();
            for (int i = 0; i < items.Count; i++)
            {
                ids.Add(i);
                if (items[i] != null)
                {
                    items[i].Id = i;
                }

                ItemDictionary.Add(ids[i], items[i]);
            }
        }
    }
}