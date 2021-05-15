using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.Behaviours.UI;
using DTWorldz.Items.Models;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{
    public class StackingMenu : MonoBehaviour
    {
        private Canvas canvas;
        private Slider slider;
        public Image Icon;
        public Text CurrentValueText;
        public Text MaxValueText;
        public Text MinValueText;

        ItemSlotBehaviour fromSlot;
        ItemSlotBehaviour toSlot;
        // Start is called before the first frame update
        void Start()
        {
            slider = GetComponentInChildren<Slider>();
            canvas = GetComponent<Canvas>();
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float sliderValue)
        {
            CurrentValueText.text = sliderValue.ToString();
        }

        public void OpenStackingMenu(ItemSlotBehaviour fromSlot, ItemSlotBehaviour toSlot)
        {
            canvas.enabled = true;
            this.fromSlot = fromSlot;
            this.toSlot = toSlot;
            this.Icon.sprite = fromSlot.GetItem().Icon;
            var maxValue = 0;
            var minValue = 1;
            var maxStackQuantity = fromSlot.GetItem().MaxStackQuantity;

            var targetHasItem = toSlot.HasItem;
            if (!toSlot.HasItem)
            {
                maxValue = fromSlot.GetQuantity();
            }
            else
            {
                maxValue = maxStackQuantity - toSlot.GetQuantity();
            }

            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.value = maxValue;

            MaxValueText.text = maxValue.ToString();
            MinValueText.text = minValue.ToString();
        }

        public void Accept()
        {
            var item = this.fromSlot.GetItem();
            this.toSlot.SetItem(new ItemContainerSlot(item, (int)slider.value));
            if ((int)slider.value == this.fromSlot.GetQuantity())
            {
                this.fromSlot.RemoveItem();
            }
            else
            {
                this.fromSlot.SetQuantity(((int)(slider.maxValue - slider.value)));
            }

            Close();
        }

        public void Cancel()
        {
            Close();
        }

        public void Close()
        {
            canvas.enabled = false;
            toSlot = null;
            fromSlot = null;
            Icon.sprite = null;
        }
    }
}