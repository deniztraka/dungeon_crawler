﻿using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Player;
using DTWorldz.Interfaces;
using DTWorldz.Models.MobileStats;
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

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

        if (collider.tag == "Player")
        {

        }
    }

    public override void SetModifiers(int minStatCount, int maxStatCount, StatQuality statQuality)
    {
        base.SetModifiers(minStatCount, maxStatCount, statQuality);

        // set min-max damage values
        switch (StatQuality)
        {
            case StatQuality.Poor:
                MinDamage -= 3;
                break;
            case StatQuality.Exceptional:
                MaxDamage = (int)Mathf.Floor(Random.Range(MinDamage + 1, MinDamage + 5));
                break;
            case StatQuality.Rare:
                MinDamage += 3;
                MaxDamage = (int)Mathf.Floor(Random.Range(MinDamage + 1, MinDamage + 9));
                break;
            case StatQuality.Legendary:
                MinDamage = (int)Mathf.Floor(Random.Range(MinDamage + 1, MinDamage + 9));
                MaxDamage = (int)Mathf.Floor(Random.Range(MinDamage + 1, MinDamage + 13));
                break;
        }


    }
}
