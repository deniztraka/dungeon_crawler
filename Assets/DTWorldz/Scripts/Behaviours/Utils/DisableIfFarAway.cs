using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.Utils
{
    public class DisableIfFarAway : MonoBehaviour
    {
        // --------------------------------------------------
        // Variables:

        private GameObject itemActivatorObject;
        private ObjectCullingManager activationScript;

        // --------------------------------------------------

        void Start()
        {
            itemActivatorObject = GameObject.Find("ObjectCullingManager");
            if (itemActivatorObject != null)
            {
                activationScript = itemActivatorObject.GetComponent<ObjectCullingManager>();
                StartCoroutine("AddToList");
            }



        }

        IEnumerator AddToList()
        {
            yield return new WaitForSeconds(0.1f);

            activationScript.addList.Add(new ActivatorItem { item = this.gameObject });
        }
    }
}