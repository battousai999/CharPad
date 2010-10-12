using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    public class BasicAdjustment
    {
        public int Modifier { get; set; }
        public string Note { get; set; }

        public BasicAdjustment(int modifier, string note)
        {
            this.Modifier = modifier;
            this.Note = note;
        }
    }
}
