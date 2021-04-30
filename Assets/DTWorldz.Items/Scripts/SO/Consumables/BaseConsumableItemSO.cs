using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Items.SO
{
    public abstract class BaseConsumableItemSO : BaseItemSO
    {
        public GameObject EffectPrefab;
        public float RegenAmount;
        internal virtual void Use()
        {
            Debug.Log(Name + " is consumed.");
            if (EffectPrefab != null)
            {
                var playerGameObject = GameObject.FindGameObjectWithTag("Player");
                var effectObj = Instantiate(EffectPrefab, playerGameObject.transform.position, Quaternion.identity, playerGameObject.transform);
                Destroy(effectObj, 2f);
            }
        }
    }
}