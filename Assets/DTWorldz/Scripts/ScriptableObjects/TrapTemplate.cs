using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TrapTemplate", menuName = "ScriptableObjects/TrapTemplate", order = 3)]
    public class TrapTemplate : ScriptableObject
    {
        public List<GameObject> FloorTrapPrefabs;
        public List<GameObject> TopWallTrapPrefabs;
        public List<GameObject> LeftWallTrapPrefabs;
        public List<GameObject> RightWallTrapPrefabs;
    }
}