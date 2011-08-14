using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
    public abstract class DiceBag
    {
        public static Random Roll = new Random();
        public static int RollDice;


        public static void Shake(int Sides)
        {
            Random Seed = new Random();
            RollDice = Roll.Next(1, Sides);
        }

            
            
    }
}
