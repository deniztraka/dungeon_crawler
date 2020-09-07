using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Interfaces;
using UnityEngine;
namespace DTWorldz.Behaviours.Items.Utils
{
    public class GoldItemBehaviour : BaseItemBehaviour, ILootItem
    {
        [SerializeField]
        private int count;
        public int Count
        {
            get { return count; }
        }
        
        public bool isStackable = true;
        public bool IsStackable
        {
            get
            {
                return isStackable;
            }
            set
            {
                isStackable = value;
            }
        }
        public void SetCount(int count)
        {
            if(isStackable){
                this.count += count;
            } else{
                throw new NotImplementedException("Could not set count on unstackable item behaviour ");
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnAfterDrop()
        {
            
        }
    }
}