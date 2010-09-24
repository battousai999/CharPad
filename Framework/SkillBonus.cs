using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    public class SkillBonus
    {
        public int Bonus { get; set; }
        public Skill Skill { get; set; }

        public SkillBonus(int bonus, Skill skill)
        {
            this.Bonus = bonus;
            this.Skill = skill;
        }
    }
}
