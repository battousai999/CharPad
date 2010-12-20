using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    public static class ArmorDefinitions
    {
        private static List<Armor> normalArmors;
        private static List<Armor> specialArmors;
        private static List<Shield> shields;

        public static List<Armor> NormalArmors
        {
            get { return normalArmors; }
        }

        public static List<Armor> SpecialArmors
        {
            get { return specialArmors; }
        }

        public static List<Shield> Shields
        {
            get { return shields; }
        }

        static ArmorDefinitions()
        {
            normalArmors = new List<Armor>();

            normalArmors.Add(new Armor("Cloth", ArmorType.Cloth, 0, 0, 0, 1, false, "", 0));
            normalArmors.Add(new Armor("Leather", ArmorType.Leather, 2, 0, 0, 25, false, "", 0));
            normalArmors.Add(new Armor("Hide", ArmorType.Hide, 3, -1, 0, 30, false, "", 0));
            normalArmors.Add(new Armor("Chainmail", ArmorType.Chainmail, 6, -1, -1, 40, true, "", 0));
            normalArmors.Add(new Armor("Scale", ArmorType.Scale, 7, 0, -1, 45, true, "", 0));
            normalArmors.Add(new Armor("Plate", ArmorType.Plate, 8, -2, -1, 50, true, "", 0));


            shields = new List<Shield>();

            shields.Add(new Shield("Light shield", ArmorType.LightShield, 1, 0, 5));
            shields.Add(new Shield("Heavy shield", ArmorType.HeavyShield, 2, -2, 10));


            specialArmors = new List<Armor>();

            specialArmors.Add(new Armor("Githweave", ArmorType.Cloth, 0, 0, 0, 0, false, "+1 Will", 3));
            specialArmors.Add(new Armor("Mindweave", ArmorType.Cloth, 0, 0, 0, 0, false, "+2 Will", 4));
            specialArmors.Add(new Armor("Feyweave", ArmorType.Cloth, 1, 0, 0, 0, false, "", 4));
            specialArmors.Add(new Armor("Efreetweave", ArmorType.Cloth, 1, 0, 0, 0, false, "+1 Will", 5));
            specialArmors.Add(new Armor("Mindpatterned", ArmorType.Cloth, 1, 0, 0, 0, false, "+2 Will", 6));
            specialArmors.Add(new Armor("Starweave", ArmorType.Cloth, 2, 0, 0, 0, false, "", 6));

            specialArmors.Add(new Armor("Drowmesh", ArmorType.Leather, 2, 0, 0, 0, false, "+1 Reflex", 3));
            specialArmors.Add(new Armor("Snakeskin", ArmorType.Leather, 2, 0, 0, 0, false, "+2 Reflex", 4));
            specialArmors.Add(new Armor("Feyleather", ArmorType.Leather, 3, 0, 0, 0, false, "", 4));
            specialArmors.Add(new Armor("Anathema", ArmorType.Leather, 3, 0, 0, 0, false, "+1 Reflex", 5));
            specialArmors.Add(new Armor("Swordwing", ArmorType.Leather, 3, 0, 0, 0, false, "+2 Reflex", 6));
            specialArmors.Add(new Armor("Starleather", ArmorType.Leather, 4, 0, 0, 0, false, "", 6));

            specialArmors.Add(new Armor("Earthhide", ArmorType.Hide, 3, -1, 0, 0, false, "+1 Fortitude", 3));
            specialArmors.Add(new Armor("Feyhide", ArmorType.Hide, 3, -1, 0, 0, false, "+2 Fortitude", 4));
            specialArmors.Add(new Armor("Darkhide", ArmorType.Hide, 4, -1, 0, 0, false, "", 4));
            specialArmors.Add(new Armor("Stalkerhide", ArmorType.Hide, 4, -1, 0, 0, false, "+1 Fortitude", 5));
            specialArmors.Add(new Armor("Voidhide", ArmorType.Hide, 4, -1, 0, 0, false, "+2 Fortitude", 6));
            specialArmors.Add(new Armor("Elderhide", ArmorType.Hide, 5, -1, 0, 0, false, "", 6));

            specialArmors.Add(new Armor("Finemail", ArmorType.Chainmail, 7, -1, -1, 0, true, "", 2));
            specialArmors.Add(new Armor("Braidmail", ArmorType.Chainmail, 8, -1, -1, 0, true, "", 3));
            specialArmors.Add(new Armor("Crysteel", ArmorType.Chainmail, 8, -1, -1, 0, true, "+2 Will", 4));
            specialArmors.Add(new Armor("Forgemail", ArmorType.Chainmail, 9, -1, -1, 0, true, "", 4));
            specialArmors.Add(new Armor("Weavemail", ArmorType.Chainmail, 10, -1, -1, 0, true, "+1 Will", 5));
            specialArmors.Add(new Armor("Pitmail", ArmorType.Chainmail, 11, -1, -1, 0, true, "+2 Will", 6));
            specialArmors.Add(new Armor("Spiritmail", ArmorType.Chainmail, 12, -1, -1, 0, true, "", 6));

            specialArmors.Add(new Armor("Drakescale", ArmorType.Scale, 8, 0, -1, 0, true, "", 2));
            specialArmors.Add(new Armor("Wyvernscale", ArmorType.Scale, 9, 0, -1, 0, true, "", 3));
            specialArmors.Add(new Armor("Stormscale", ArmorType.Scale, 9, 0, -1, 0, true, "+2 Fortitude", 4));
            specialArmors.Add(new Armor("Wyrmscale", ArmorType.Scale, 10, 0, -1, 0, true, "", 4));
            specialArmors.Add(new Armor("Nagascale", ArmorType.Scale, 11, 0, -1, 0, true, "+1 Fortitude", 5));
            specialArmors.Add(new Armor("Titanscale", ArmorType.Scale, 12, 0, -1, 0, true, "+2 Fortitude", 6));
            specialArmors.Add(new Armor("Elderscale", ArmorType.Scale, 13, 0, -1, 0, true, "", 6));

            specialArmors.Add(new Armor("Rimefire plate", ArmorType.Plate, 8, -2, -1, 0, true, "Resist 1 all", 2));
            specialArmors.Add(new Armor("Layered plate", ArmorType.Plate, 9, -2, -1, 0, true, "", 2));
            specialArmors.Add(new Armor("Gith plate", ArmorType.Plate, 10, -2, -1, 0, true, "", 3));
            specialArmors.Add(new Armor("Warplate", ArmorType.Plate, 11, -2, -1, 0, true, "", 4));
            specialArmors.Add(new Armor("Specter plate", ArmorType.Plate, 10, -2, -1, 0, true, "Resist 2 all", 4));
            specialArmors.Add(new Armor("Legion plate", ArmorType.Plate, 12, -2, -1, 0, true, "", 5));
            specialArmors.Add(new Armor("Tarrasque plate", ArmorType.Plate, 12, -2, -1, 0, true, "Resist 5 all", 6));
            specialArmors.Add(new Armor("Godplate", ArmorType.Plate, 14, -2, -1, 0, true, "", 6));
        }
    }
}
