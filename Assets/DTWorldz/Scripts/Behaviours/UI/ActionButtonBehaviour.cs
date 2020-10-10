using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{
    public class ActionButtonBehaviour : MonoBehaviour
    {
        public bool IsCooldown = false;
        private float cooldownTime = 0f;

        public UnityEvent ActionEvent;
        public Button Button;
        public Animator Animator;
        public Image FillImage;
        public Image ActionImage;

        // Start is called before the first frame update
        public virtual void Start()
        {
            if (Button == null)
            {
                Button = gameObject.GetComponent<Button>();
            }
            if (FillImage == null)
            {
                FillImage = gameObject.GetComponentInChildren<Image>();
            }
            if (Animator == null)
            {
                Animator = gameObject.GetComponent<Animator>();
            }

            Button.onClick.AddListener(Action);
        }

        // Update is called once per frame
        public virtual void Update()
        {
            if (!IsCooldown)
            {
                Button.enabled = true;
                FillImage.fillAmount = 1;
            }
            else
            {
                
                FillImage.fillAmount += 1 / cooldownTime * Time.deltaTime;
                if (FillImage.fillAmount >= 1)
                {
                    FillImage.fillAmount = 1;
                    IsCooldown = false;
                }
            }
        }

        public void Action()
        {
            if (ActionEvent != null)
            {
                FillImage.fillAmount = 0;
                Animator.Play("OnClick");
                IsCooldown = true;
                Button.enabled = false;
                ActionEvent.Invoke();
            }
        }

        public void SetAction(UnityAction action, float cooldown)
        {
            ActionEvent.RemoveAllListeners();
            ActionEvent.AddListener(action);
            cooldownTime = cooldown;
        }

        public void SetAction(UnityAction action, float cooldown, Sprite actionImage)
        {
           SetAction(action,cooldown);
           ActionImage.sprite = actionImage;

        }

        public void SetCoolDown(float cooldown)
        {
            cooldownTime = cooldown;
        }

        public void SetActionImage(Sprite actionImage)
        {
            ActionImage.sprite = actionImage;
        }
    }
}