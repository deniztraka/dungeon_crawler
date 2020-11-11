using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Player;
using DTWorldz.Interfaces;
using UnityEngine;

public class WeaponBehaviour : BaseItemBehaviour, ILootItem
{
    public int count = 1;
    public int Count
    {
        get
        {
            return count;
        }
    }

    public bool isStackable = false;
    public bool IsStackable
    {
        get
        {
            return isStackable;
        }
    }

    public void OnAfterDrop()
    {
        
    }

    public void SetCount(int count)
    {
       count = 1;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnTriggerEnter2D(Collider2D collider)
        {
            base.OnTriggerEnter2D(collider);

            if (collider.tag == "Player")
            {
               
            }
        }
}
