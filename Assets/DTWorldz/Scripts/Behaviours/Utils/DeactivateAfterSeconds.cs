using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.Utils
{
    public class DeactivateAfterSeconds : MonoBehaviour
    {
        // Start is called before the first frame update
        public float AfterSeconds = 3f;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(SetStatusAfterTime());
        }

        // Update is called once per frame
        private IEnumerator SetStatusAfterTime()
        {

            yield return new WaitForSeconds(AfterSeconds);

            gameObject.SetActive(false);
        }
    }
}