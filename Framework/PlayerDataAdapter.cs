using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    public static class PlayerDataAdapter
    {
        public static Party LoadParty(string filename)
        {
            return null;
        }

        public static void SaveParty(string filename, Party party)
        {
        }

        public static void SaveParty(string filename, List<Player> players)
        {
            Party party = new Party();

            players.ForEach(x => party.Members.Add(x));

            SaveParty(filename, party);
        }

        public static void SaveParty(string filename, Player player)
        {
            SaveParty(filename, new List<Player> { player });
        }
    }
}
