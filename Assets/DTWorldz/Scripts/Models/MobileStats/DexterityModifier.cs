using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Models.MobileStats
{
    [Serializable]
    public class DexterityModifier : BaseStatModifier
    {
        public DexterityModifier(int val) : base("Dexterity", val)
        {
        }

        public DexterityModifier(StatQuality quality) : base("Dexterity")
        {
            switch (quality)
            {
                case StatQuality.Poor:
                    Value = UnityEngine.Random.Range(0, 4);
                    break;

                case StatQuality.Regular:
                    Value = UnityEngine.Random.Range(0, 6);
                    break;

                case StatQuality.Exceptional:
                    Value = UnityEngine.Random.Range(5, 9);
                    break;

                case StatQuality.Rare:
                    Value = UnityEngine.Random.Range(8, 11);
                    break;

                case StatQuality.Legendary:
                    Value = UnityEngine.Random.Range(10, 13);
                    break;
            }
        }
    }
}