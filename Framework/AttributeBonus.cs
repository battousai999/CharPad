using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    public class AttributeBonus
    {
        public int Bonus { get; set; }
        public AttributeType Attribute { get; set; }
        public AttributeType? OptionalAttribute { get; set; }

        public bool CanChooseAttribute { get { return (OptionalAttribute != null); } }

        public AttributeBonus(int bonus, AttributeType attribute)
        {
            this.Bonus = bonus;
            this.Attribute = attribute;
        }

        public AttributeBonus(int bonus, AttributeType attribute1, AttributeType attribute2)
        {
            this.Bonus = bonus;
            this.Attribute = attribute1;
            this.OptionalAttribute = attribute2;
        }
    }
}
