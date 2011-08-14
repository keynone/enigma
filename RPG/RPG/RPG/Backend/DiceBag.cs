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


        public static int RollDiceD(int Sides)
        {
            Roll = new Random();
            RollDice = Roll.Next(0, Sides);
            return RollDice;
        }
            
    }
}
