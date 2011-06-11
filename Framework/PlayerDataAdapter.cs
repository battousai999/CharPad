using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;

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
            // If file exists, rename (as a backup)

            // Create database file...
            using (SqlCeEngine engine = new SqlCeEngine("DataSource=\"" + filename + "\"; Password=\"charpad\""))
            {
                engine.CreateDatabase();

                // Build schema
                // Save each party member
            }
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
