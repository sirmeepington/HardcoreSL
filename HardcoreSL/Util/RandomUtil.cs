using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HardcoreSL.Util
{
    public static class RandomUtil
    {
        private static readonly System.Random _random = new System.Random(Mathf.RoundToInt(Time.realtimeSinceStartup));

        /// <summary>
        /// Returns True if the given successRate is true in a random roll.
        /// </summary>
        /// <param name="successRate">The % chance to roll.</param>
        /// <returns>True if the roll is less than or equal to the rate, false otherwise.</returns>
        public static bool Chance(float successRate) => (float)(_random.NextDouble() * 100) <= successRate;


    }
}
