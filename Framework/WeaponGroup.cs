using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    [Flags]
    public enum WeaponGroup
    {
        LightBlade = 1,
        HeavyBlade = 2,
        Mace = 4,
        Spear = 8,
        Staff = 16,
        Axe = 32,
        Flail = 64,
        Hammer = 128,
        Pick = 256,
        Polearm = 512,
        Crossbow = 1024,
        Sling = 2048,
        Bow = 4096,
    }
}
