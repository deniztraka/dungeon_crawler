using System.Collections;
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
        var minDamageAddition = 0;
        var maxDamageAddition = 0;

        // set min-max damage values
        switch (StatQuality)
        {
            case StatQuality.Poor:
                minDamageAddition = 0;
                maxDamageAddition = 0;
                break;
            case StatQuality.Regular:
                minDamageAddition = 2;
                maxDamageAddition = 6;
                break;
            case StatQuality.Exceptional:
                minDamageAddition = 6;
                maxDamageAddition = 10;
                break;
            case StatQuality.Rare:
                minDamageAddition = 10;
                maxDamageAddition = 15;
                break;
            case StatQuality.Legendary:
                minDamageAddition = 15;
                maxDamageAddition = 20;
                break;
        }

        if (minDamageAddition != 0)
        {
            MinDamage += (int)Mathf.Floor(Random.Range(MinDamage, MinDamage + minDamageAddition));
        }
        if (maxDamageAddition != 0)
        {
            MaxDamage += (int)Mathf.Floor(Random.Range(MaxDamage, MaxDamage + maxDamageAddition));
        }

    }
}
