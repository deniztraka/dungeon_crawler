using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Models
{
    [Serializable]
    public struct LastPosition
    {
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public class PlayerAreaStackModel
    {
        public LastPosition LastPosition;
        public string AreaName;
        public string UniqueObjectName;

        public PlayerAreaStackModel(Vector3 lp, string areaName, string uniqueObjectName)
        {
            LastPosition = new LastPosition();
            LastPosition.x = lp.x;
            LastPosition.y = lp.y;
            LastPosition.z = lp.z;
            AreaName = areaName;
            UniqueObjectName = uniqueObjectName;
        }
    }
}