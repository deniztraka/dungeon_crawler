using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.UI;
using DTWorldz.Models.MobileStats;
using DTWorldz.ScriptableObjects.Items;
using DTWorldz.Utils;
using UnityEngine;

public class BaseItemBehaviour : MonoBehaviour
{
    private ItemLabelCanvas labelCanvas;
    public BaseItem ItemTemplate;
    public StatQuality StatQuality;
    public StrengthModifier StrengthModifier;
    public DexterityModifier DexterityModifier;

    public int MinDamage;
    public int MaxDamage;
    // Start is called before the first frame update
    void Start()
    {
        labelCanvas = gameObject.GetComponentInChildren<ItemLabelCanvas>();
        if(labelCanvas != null){
            labelCanvas.SetLabelColor(StatQuality);            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {


    }

    public virtual void SetModifiers(int minStatCount, int maxStatCount, StatQuality statQuality)
    {
        var randomStatCount = Random.Range(minStatCount, maxStatCount);
                
        StatQuality = LootingUtils.GetRandomStatQuality(statQuality);

        var randomStats = LootingUtils.GetRandomStats(randomStatCount, StatQuality);
        foreach (var stat in randomStats)
        {
            if (stat.GetType() == typeof(StrengthModifier))
            {
                StrengthModifier = (StrengthModifier)stat;
            }
            else if (stat.GetType() == typeof(DexterityModifier))
            {
                DexterityModifier = (DexterityModifier)stat;
            }
        }
    }    
}
