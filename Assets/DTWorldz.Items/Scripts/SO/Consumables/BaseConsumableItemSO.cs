using System.Collections;
using System.Collections.Generic;
using DTWorldz.Scripts.Managers;
using UnityEngine;
namespace DTWorldz.Items.SO
{
    public abstract class BaseConsumableItemSO : BaseItemSO
    {
        public GameObject EffectPrefab;
        public float RegenAmount;
        internal virtual void Use()
        {
            if (EffectPrefab != null)
            {
                var player = GameManager.Instance.PlayerBehaviour;
                var effectObj = Instantiate(EffectPrefab, player.transform.position, Quaternion.identity, player.transform);
                Destroy(effectObj, 2f);
            }
        }
    }
}