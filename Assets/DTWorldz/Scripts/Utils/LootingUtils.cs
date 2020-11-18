using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DTWorldz.Models.MobileStats;
using UnityEngine;
namespace DTWorldz.Utils
{

    public static class LootingUtils
    {
        public static List<BaseStatModifier> GetRandomStats(int count, StatQuality statQuality)
        {
            string[] stats = { "STR", "DEX" };
            stats = stats.OrderBy(x => Random.value).ToArray<string>();

            if (count > stats.Length)
            {
                count = stats.Length;
            }

            var chosenStats = stats.Take(count);

            var resultedStats = new List<BaseStatModifier>();
            foreach (var chosenStat in chosenStats)
            {
                switch (chosenStat)
                {
                    case "STR":
                        resultedStats.Add(new StrengthModifier(statQuality));
                        break;
                    case "DEX":
                        resultedStats.Add(new DexterityModifier(statQuality));
                        break;
                }
            }

            return resultedStats;
        }

        public static StatQuality GetRandomStatQuality(StatQuality maxStatQuality)
        {
            var randomStatQuality = Random.Range(0, (int)maxStatQuality + 1);
            switch (randomStatQuality)
            {
                case 0:
                    return StatQuality.Poor;
                case 1:
                    return StatQuality.Regular;
                case 2:
                    return StatQuality.Exceptional;
                case 3:
                    return StatQuality.Rare;
                case 4:
                    return StatQuality.Legendary;
                default:
                    return StatQuality.Poor;
            }
        }
    }
}