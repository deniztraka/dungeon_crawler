using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Models.MobileStats
{
    [Serializable]
    public class StrengthModifier : BaseStatModifier
    {
        public StrengthModifier(int val) : base("Strength", val)
        {
        }

        public StrengthModifier(StatQuality quality) : base("Strength")
        {
            switch (quality)
            {
                case StatQuality.Poor:
                    Value = UnityEngine.Random.Range(0, 2);
                    break;

                case StatQuality.Regular:
                    Value = UnityEngine.Random.Range(2, 4);
                    break;

                case StatQuality.Exceptional:
                    Value = UnityEngine.Random.Range(4, 8);
                    break;

                case StatQuality.Rare:
                    Value = UnityEngine.Random.Range(8, 12);
                    break;

                case StatQuality.Legendary:
                    Value = UnityEngine.Random.Range(12, 16);
                    break;
            }
        }
    }
}