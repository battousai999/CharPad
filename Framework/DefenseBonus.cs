using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    public class DefenseBonus
    {
        public int Bonus { get; set; }
        public DefenseType Defense { get; set; }

        public DefenseBonus(int bonus, DefenseType defense)
        {
            this.Bonus = bonus;
            this.Defense = defense;
        }
    }
}
