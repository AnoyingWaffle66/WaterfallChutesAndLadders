﻿using System;

namespace waterfallChutesAndLadders
{
    internal static class Dice
    {
        private static Random rand = new Random();

        public static int roll()
        {
            //Assumes rolling one six sided die.
            return rand.Next(1, 7);
        }

        public static int roll(int low, int high)
        {
            //Inputting roll(1, 6) will act as a six sided die.
            return rand.Next(low, high + 1);
        }
    }
}