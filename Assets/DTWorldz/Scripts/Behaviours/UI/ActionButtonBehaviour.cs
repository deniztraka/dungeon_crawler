using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
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

        private Sprite tempImage;

        // Start is called before the first frame update
        public virtual void Start()
        {
            if (Button == null)
            {
                Button = gameObject.GetComponent<Button>();
            }
            if (FillImage == null)
            {
                FillImage = transform.Find("FillImage").GetComponent<Image>();
            }
            if (Animator == null)
            {
                Animator = gameObject.GetComponent<Animator>();
            }

            if(ActionImage == null){
                ActionImage = transform.Find("ActionImage").GetComponent<Image>();
            }

            tempImage = ActionImage.sprite;

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
                if(Animator != null){
                    Animator.Play("OnClick");
                }
                IsCooldown = true;
                Button.enabled = false;
                ActionEvent.Invoke();
            }
        }

        public void SetFillAmount(float amount)
        {
            if (FillImage != null)
            {
                FillImage.fillAmount = amount;                
            }
        }

        public void SetAction(UnityAction action, float cooldown)
        {
            ActionEvent.RemoveAllListeners();

            if(action == null){
                cooldownTime = 0;
                ActionImage.sprite = tempImage;
                return;
            }
            ActionEvent.AddListener(action);
            cooldownTime = cooldown + 0.1f;
        }

        public void SetAction(UnityAction action, float cooldown, Sprite actionImage)
        {
            if (action == null)
            {
                ActionImage.sprite = tempImage;
            } else {
                ActionImage.sprite = actionImage;
            }

            SetAction(action, cooldown);
            
            

        }

        public void SetCoolDown(float cooldown)
        {
            cooldownTime = cooldown + 0.1f;
        }

        public void SetActionImage(Sprite actionImage)
        {
            ActionImage.sprite = actionImage;
        }
    }
}