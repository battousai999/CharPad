using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    [Flags]
    public enum WeaponProperties
    {
        None = 0,
        OffHand = 1,
        LightThrown = 2,
        HeavyThrown = 4, 
        Versatile = 8,
        HighCrit = 16,
        Reach = 32,
        LoadFree = 64,
        LoadMinor = 128,
        Small = 256, 
        Defensive = 512,
        Brutal_1 = 1024,
        Brutal_2 = 2048,
    }
}
