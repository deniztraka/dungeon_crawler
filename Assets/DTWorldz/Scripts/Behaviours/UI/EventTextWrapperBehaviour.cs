using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{
    public class EventTextWrapperBehaviour : MonoBehaviour
    {
        public float HideInSeconds = 3f;
        private Text textComponent;

        // Start is called before the first frame update
        void Start()
        {
            textComponent = GetComponentInChildren<Text>();

            StartCoroutine(FadeTo(0.0f, HideInSeconds));
            Destroy(this.gameObject, 3.5f);
        }

        IEnumerator FadeTo(float aValue, float aTime)
        {
            float alpha = textComponent.color.a;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
                textComponent.color = newColor;
                yield return null;
            }
        }

        public void SetText(string text)
        {
            if (textComponent == null)
            {
                textComponent = GetComponentInChildren<Text>();
                textComponent.text = text;
            }
        }
    }
}
