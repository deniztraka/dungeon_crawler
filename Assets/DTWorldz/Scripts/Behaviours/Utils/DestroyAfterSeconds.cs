using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.Utils
{
    public class DestroyAfterSeconds : MonoBehaviour
    {
         // Start is called before the first frame update
        public float AfterSeconds = 3f;
        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject,AfterSeconds);
        }
    }
}