using System;
using System.Collections.Generic;
using System.Text;
using Godot;

namespace Gerbil
{
    public static class RandomnessManager
    {
        public static RandomNumberGenerator RandomNumberGenerator { get; private set; }

        static RandomnessManager()
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            rng.Seed = (ulong)GD.Hash(DateTime.Now.ToString());
            RandomNumberGenerator = rng;
        }
    }
}
