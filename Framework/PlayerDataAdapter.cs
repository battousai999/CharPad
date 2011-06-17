using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;

namespace CharPad.Framework
{
    public static class PlayerDataAdapter
    {
        public static Party LoadParty(string filename)
        {
            using (SqlCeConnection conn = new SqlCeConnection("DataSource=\"" + filename + "\"; Password=\"charpad\""))
            {
                string sqlText = "select * from Player";
                DataTable data = new DataTable();

                using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(sqlText, conn))
                {
                    adapter.Fill(data);
                }

                Party party = new Party();

                foreach (DataRow row in data.Rows)
                {
                    party.Members.Add(LoadPlayer(conn, row));
                }

                return party;
            }
        }

        private static Player LoadPlayer(SqlCeConnection conn, DataRow row)
        {
            int playerId = Convert.ToInt32(row["Id"]);
            Player player = new Player();

            player.CharacterName = Convert.ToString(row["PlayerName"]);
            player.PlayerName = Convert.ToString(row["PersonName"]);
            player.IsMale = Convert.ToBoolean(row["IsMale"]);
            player.Deity = Convert.ToString(row["Deity"]);
            player.Level = Convert.ToInt32(row["Level"]);
            player.Str = Convert.ToInt32(row["Str"]);
            player.Con = Convert.ToInt32(row["Con"]);
            player.Dex = Convert.ToInt32(row["Dex"]);
            player.Int = Convert.ToInt32(row["Int"]);
            player.Wis = Convert.ToInt32(row["Wis"]);
            player.Cha = Convert.ToInt32(row["Cha"]);
            player.Picture = BuildPictureFromByteArray(row.IsNull("Picture") ? null : (byte[])row["Picture"]);
            player.Notes = (row.IsNull("Notes") ? "" : Convert.ToString(row["Notes"]));

            LoadPlayerClass(conn, player, playerId);
            LoadPlayerRace(conn, player, playerId);

            LoadBasicAdjustmentList(conn, player.HitPoints.MiscAdjustments, row["HitPoints_AdjustListId"]);
            LoadBasicAdjustmentList(conn, player.SurgeValue.MiscAdjustments, row["Surge_AdjustListId"]);
            LoadBasicAdjustmentList(conn, player.SurgesPerDay.MiscAdjustments, row["SurgesPerDay_AdjustListId"]);

            LoadSkill(conn, player.Acrobatics, row);
            LoadSkill(conn, player.Arcana, row);
            LoadSkill(conn, player.Athletics, row);
            LoadSkill(conn, player.Bluff, row);
            LoadSkill(conn, player.Diplomacy, row);
            LoadSkill(conn, player.Dungeoneering, row);
            LoadSkill(conn, player.Endurance, row);
            LoadSkill(conn, player.Heal, row);
            LoadSkill(conn, player.History, row);
            LoadSkill(conn, player.Insight, row);
            LoadSkill(conn, player.Intimidate, row);
            LoadSkill(conn, player.Nature, row);
            LoadSkill(conn, player.Perception, row);
            LoadSkill(conn, player.Religion, row);
            LoadSkill(conn, player.Stealth, row);
            LoadSkill(conn, player.Streetwise, row);
            LoadSkill(conn, player.Thievery, row);

            LoadBasicAdjustmentList(conn, player.Initiative.MiscAdjustments, row["Initiative_AdjustListId"]);
            LoadBasicAdjustmentList(conn, player.AcDefense.MiscAdjustments, row["AcDefense_AdjustListId"]);
            LoadBasicAdjustmentList(conn, player.FortDefense.MiscAdjustments, row["FortDefense_AdjustListId"]);
            LoadBasicAdjustmentList(conn, player.ReflexDefense.MiscAdjustments, row["ReflexDefense_AdjustListId"]);
            LoadBasicAdjustmentList(conn, player.WillDefense.MiscAdjustments, row["WillDefense_AdjustListId"]);
            LoadBasicAdjustmentList(conn, player.Speed.MiscAdjustments, row["Speed_AdjustListId"]);

            Dictionary<int, IInventoryItem> itemMap = BuildLoadingItemMap(conn);

            foreach (int key in itemMap.Keys)
            {
                player.Inventory.Add(itemMap[key]);
            }

            LoadPlayerWeaponBonuses(conn, player, playerId, itemMap);

            player.Armor = (row.IsNull("ArmorId") ? null : (Armor)itemMap[Convert.ToInt32(row["ArmorId"])]);
            player.Shield = (row.IsNull("ShieldId") ? null : (Shield)itemMap[Convert.ToInt32(row["ShieldId"])]);
            player.Weapon = (row.IsNull("WeaponId") ? null : (Weapon)itemMap[Convert.ToInt32(row["WeaponId"])]);
            player.WeaponOffhand = (row.IsNull("OffhandWeaponId") ? null : (Weapon)itemMap[Convert.ToInt32(row["OffhandWeaponId"])]);
            player.RangedWeapon = (row.IsNull("RangedWeaponId") ? null : (Weapon)itemMap[Convert.ToInt32(row["RangedWeaponId"])]);
            player.Implement = (row.IsNull("ImplementId") ? null : (Weapon)itemMap[Convert.ToInt32(row["ImplementId"])]);

            LoadWeaponSpec(conn, player.WeaponSpec, Convert.ToInt32(row["WeaponSpecId"]));
            LoadWeaponSpec(conn, player.WeaponOffhandSpec, Convert.ToInt32(row["OffhandWeaponSpecId"]));
            LoadWeaponSpec(conn, player.RangedWeaponSpec, Convert.ToInt32(row["OffhandWeaponSpecId"]));
            LoadWeaponSpec(conn, player.ImplementSpec, Convert.ToInt32(row["ImplementSpecId"]));

            LoadFeatures(conn, playerId, player.Feats, 1);
            LoadFeatures(conn, playerId, player.RaceFeatures, 2);
            LoadFeatures(conn, playerId, player.ClassFeatures, 3);
            LoadFeatures(conn, playerId, player.ParagonFeatures, 4);
            LoadFeatures(conn, playerId, player.DestinyFeatures, 5);

            LoadPowers(conn, playerId, player, player.Powers);
            LoadResistanceValues(conn, playerId, player.Resistances);

            return player;
        }

        private static void LoadResistanceValues(SqlCeConnection conn, int playerId, ObservableCollectionEx<ResistanceValue> list)
        {
            string sqlText = "select * from ResistanceValue where PlayerId = " + playerId.ToString() + " order by sequence";
            DataTable data = new DataTable();

            using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(sqlText, conn))
            {
                adapter.Fill(data);
            }

            foreach (DataRow row in data.Rows)
            {
                list.Add(new ResistanceValue(Convert.ToInt32(row["Modifier"]),
                    Convert.ToString(row["DamageType"]),
                    (row.IsNull("Description") ? "" : Convert.ToString(row["Description"]))));
            }
        }

        private static void LoadPowers(SqlCeConnection conn, int playerId, Player player, PowerCollection powers)
        {
            string sqlText = "select * from PlayerPower where PlayerId = " + playerId.ToString() + " order by Sequence";
            DataTable data = new DataTable();

            using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(sqlText, conn))
            {
                adapter.Fill(data);
            }

            foreach (DataRow row in data.Rows)
            {
                Power power = new Power(player);

                power.Name = Convert.ToString(row["Name"]);
                power.PowerType = (PowerType)Convert.ToInt32(row["PowerType"]);
                power.ActionType = (PowerActionType)Convert.ToInt32(row["ActionType"]);
                power.Level = Convert.ToInt32(row["Level"]);
                power.Description = Convert.ToString(row["Description"]);
                power.Notes = (row.IsNull("Notes") ? "" : Convert.ToString(row["Notes"]));
                power.AttackType = (PowerAttackType)Convert.ToInt32(row["AttackType"]);
                power.AttackWeapon = (WeaponSlot)Convert.ToInt32(row["AttackWeapon"]);
                power.AttackAttribute = (AttributeType)Convert.ToInt32(row["AttackAttribute"]);
                power.DefenseType = (DefenseType)Convert.ToInt32(row["DefenseType"]);
                power.Damage = (row.IsNull("Damage") ? null : Dice.GetFromString(Convert.ToString(row["Damage"])));
                power.WeaponDamamgeMultiplier = Convert.ToInt32(row["WeaponDamageMultiplier"]);
                power.DamageType = Convert.ToString(row["DamageType"]);
                power.BonusDamageAttribute = (row.IsNull("BonusDamageAttribute") ? null : (AttributeType?)Convert.ToInt32(row["BonusDamageAttribute"]));
                power.Picture = BuildPictureFromByteArray(row.IsNull("Picture") ? null : (byte[])row["Picture"]);

                LoadBasicAdjustmentList(conn, power.AttackModifiers, row["AttackModifiers_AdjustListId"]);
                LoadBasicAdjustmentList(conn, power.DamageModifiers, row["DamageModifiers_AdjustListId"]);

                powers.Add(power);
            }
        }

        private static void LoadFeatures(SqlCeConnection conn, int playerId, ObservableCollectionEx<FeatureValue> list, int featureType)
        {
            string sqlText = "select * from FeatureValue where PlayerId = " + playerId.ToString() + 
                " and FeatureType = " + featureType.ToString() + " order by sequence";

            DataTable data = new DataTable();

            using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(sqlText, conn))
            {
                adapter.Fill(data);
            }

            foreach (DataRow row in data.Rows)
            {
                list.Add(new FeatureValue(Convert.ToString(row["Name"]),
                    (row.IsNull("ShortDescription") ? "" : Convert.ToString(row["ShortDescription"])),
                    (row.IsNull("LongDescription") ? "" : Convert.ToString(row["LongDescription"]))));
            }
        }

        private static void LoadWeaponSpec(SqlCeConnection conn, WeaponSpecValue spec, int specId)
        {
            string sqlText = "select * from WeaponSpec where id = " + specId.ToString();
            DataTable data = new DataTable();

            using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(sqlText, conn))
            {
                adapter.Fill(data);
            }

            if ((data == null) || (data.Rows.Count == 0))
                throw new InvalidOperationException("No weapon spec found for id: " + specId.ToString());

            LoadBasicAdjustmentList(conn, spec.ToHitAdjustments, data.Rows[0]["ToHit_AdjustListId"]);
            LoadBasicAdjustmentList(conn, spec.DamageAdjustments, data.Rows[0]["Damage_AdjustListId"]);
        }

        private static void LoadPlayerWeaponBonuses(SqlCeConnection conn, Player player, int playerId, Dictionary<int, IInventoryItem> itemMap)
        {
            string sqlText = "select * from WeaponBonus where PlayerId = " + playerId.ToString();
            DataTable data = new DataTable();

            using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(sqlText, conn))
            {
                adapter.Fill(data);
            }

            foreach (DataRow row in data.Rows)
            {
                WeaponBonus bonus = new WeaponBonus();
                bonus.Bonus = new WeaponBonusValue();

                bonus.Weapon = (Weapon)itemMap[Convert.ToInt32(row["WeaponId"])];
                LoadBasicAdjustmentList(conn, bonus.Bonus.ToHitAdjustments, row["ToHitAdjustListId"]);
                LoadBasicAdjustmentList(conn, bonus.Bonus.DamageAdjustments, row["DamageAdjustListId"]);

                player.WeaponBonuses.Add(bonus);
            }
        }

        private static Dictionary<int, IInventoryItem> BuildLoadingItemMap(SqlCeConnection conn)
        {
            Dictionary<int, IInventoryItem> map = new Dictionary<int, IInventoryItem>();
            string sqlText = "select * from {0}";
            DataTable data = new DataTable();

            using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(String.Format(sqlText, "Armor"), conn))
            {
                adapter.Fill(data);
            }

            foreach (DataRow row in data.Rows)
            {
                map.Add(Convert.ToInt32(row["Id"]), LoadArmor(row));
            }

            data = new DataTable();

            using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(String.Format(sqlText, "Shield"), conn))
            {
                adapter.Fill(data);
            }

            foreach (DataRow row in data.Rows)
            {
                map.Add(Convert.ToInt32(row["Id"]), LoadShield(row));
            }

            data = new DataTable();

            using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(String.Format(sqlText, "Weapon"), conn))
            {
                adapter.Fill(data);
            }

            foreach (DataRow row in data.Rows)
            {
                map.Add(Convert.ToInt32(row["Id"]), LoadWeapon(row));
            }

            return map; 
        }

        private static Weapon LoadWeapon(DataRow row)
        {
            Weapon weapon = new Weapon();

            weapon.Name = Convert.ToString(row["Name"]);
            weapon.ProficiencyBonus = Convert.ToInt32(row["ProficiencyBonus"]);
            weapon.EnhancementBonus = Convert.ToInt32(row["EnhancementBonus"]);
            weapon.Damage = (row.IsNull("Damage") ? null : Dice.GetFromString(Convert.ToString(row["Damage"])));
            weapon.Range = Convert.ToString(row["Range"]);
            weapon.Group = (WeaponGroup)Convert.ToInt32(row["Group"]);
            weapon.Properties = (WeaponProperties)Convert.ToInt32(row["Properties"]);
            weapon.BasePrice = Convert.ToInt32(row["BasePrice"]);
            weapon.Category = (WeaponCategory)Convert.ToInt32(row["Category"]);
            weapon.IsTwoHanded = Convert.ToBoolean(row["IsTwoHanded"]);
            weapon.IsImplement = Convert.ToBoolean(row["IsImplement"]);
            weapon.Picture = BuildPictureFromByteArray(row.IsNull("Picture") ? null : (byte[])row["Picture"]);
            weapon.Notes = (row.IsNull("Notes") ? "" : Convert.ToString(row["Notes"]));

            return weapon;
        }

        private static Shield LoadShield(DataRow row)
        {
            Shield shield = new Shield();

            shield.Name = Convert.ToString(row["Name"]);
            shield.ArmorType = (ArmorType)Convert.ToInt32(row["ArmorType"]);
            shield.EnhancementBonus = Convert.ToInt32(row["EnhancementBonus"]);
            shield.ArmorBonus = Convert.ToInt32(row["ArmorBonus"]);
            shield.SkillModifier = Convert.ToInt32(row["SkillModifier"]);
            shield.BasePrice = Convert.ToInt32(row["BasePrice"]);
            shield.Picture = BuildPictureFromByteArray(row.IsNull("Picture") ? null : (byte[])row["Picture"]);
            shield.Notes = (row.IsNull("Notes") ? "" : Convert.ToString(row["Notes"]));

            return shield;
        }

        private static Armor LoadArmor(DataRow row)
        {
            Armor armor = new Armor();

            armor.Name = Convert.ToString(row["Name"]);
            armor.ArmorType = (ArmorType)Convert.ToInt32(row["ArmorType"]);
            armor.EnhancementBonus = Convert.ToInt32(row["EnhancementBonus"]);
            armor.ArmorBonus = Convert.ToInt32(row["ArmorBonus"]);
            armor.SkillModifier = Convert.ToInt32(row["SkillModifier"]);
            armor.SpeedModifier = Convert.ToInt32(row["SpeedModifier"]);
            armor.BasePrice = Convert.ToInt32(row["BasePrice"]);
            armor.IsHeavy = Convert.ToBoolean(row["IsHeavy"]);
            armor.SpecialProperty = Convert.ToString(row["SpecialProperty"]);
            armor.MinEnhancementBonus = Convert.ToInt32(row["MinEnhancementBonus"]);
            armor.Picture = BuildPictureFromByteArray(row.IsNull("Picture") ? null : (byte[])row["Picture"]);
            armor.Notes = (row.IsNull("Notes") ? "" : Convert.ToString(row["Notes"]));

            return armor;
        }

        private static void LoadSkill(SqlCeConnection conn, SkillValue skill, DataRow row)
        {
            string skillName = Enum.Format(typeof(Skill), skill.Skill, "G");

            skill.IsTrained = Convert.ToBoolean(row[skillName + "_IsTrained"]);
            LoadBasicAdjustmentList(conn, skill.MiscAdjustments, row[skillName + "_AdjustListId"]);
        }

        private static void LoadBasicAdjustmentList(SqlCeConnection conn, BasicAdjustmentList list, object adjustListId)
        {
            if ((adjustListId == null) || (adjustListId == DBNull.Value))
                return;

            int id = Convert.ToInt32(adjustListId);
            string sqlText = "select * from BasicAdjustmentList where ListId = " + id.ToString() + " order by sequence";
            DataTable data = new DataTable();

            using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(sqlText, conn))
            {
                adapter.Fill(data);
            }

            list.Clear();

            if ((data == null) || (data.Rows.Count == 0))
                return;

            foreach (DataRow row in data.Rows)
            {
                list.Add(new BasicAdjustment(Convert.ToInt32(row["Modifier"]),
                    (row.IsNull("Note") ? "" : Convert.ToString(row["Note"]))));
            }
        }

        private static void LoadPlayerClass(SqlCeConnection conn, Player player, int playerId)
        {
            string sqlText = "select * from PlayerClass where Id = " + playerId.ToString();
            DataTable data = new DataTable();

            using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(sqlText, conn))
            {
                adapter.Fill(data);
            }

            if ((data == null) || (data.Rows.Count == 0))
                throw new InvalidOperationException("No player class found for player: " + playerId.ToString());

            DataRow row = data.Rows[0];
            PlayerClass playerClass = new PlayerClass();

            playerClass.Name = Convert.ToString(row["Name"]);
            playerClass.BaseHealth = Convert.ToInt32(row["BaseHealth"]);
            playerClass.HealthPerLevel = Convert.ToInt32(row["HealthPerLevel"]);
            playerClass.BaseHealingSurges = Convert.ToInt32(row["BaseHealingSurges"]);
            playerClass.FortitudeBonus = Convert.ToInt32(row["FortitudeBonus"]);
            playerClass.ReflexBonus = Convert.ToInt32(row["ReflexBonus"]);
            playerClass.WillBonus = Convert.ToInt32(row["WillBonus"]);

            player.Class = playerClass;
        }

        private static void LoadPlayerRace(SqlCeConnection conn, Player player, int playerId)
        {
            string sqlText = "select * from PlayerRace where Id = " + playerId.ToString();
            DataTable data = new DataTable();

            using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(sqlText, conn))
            {
                adapter.Fill(data);
            }

            if ((data == null) || (data.Rows.Count == 0))
                throw new InvalidOperationException("No player race found for player: " + playerId.ToString());

            DataRow row = data.Rows[0];
            PlayerRace playerRace = new PlayerRace();

            playerRace.Name = Convert.ToString(row["Name"]);
            playerRace.Size = (CreatureSize)Convert.ToInt32(row["Size"]);
            playerRace.BaseSpeed = Convert.ToInt32(row["BaseSpeed"]);

            player.Race = playerRace;
        }

        public static void SaveParty(string filename, Party party)
        {
            string backupFilename = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileName(filename) + ".bak");

            // If file exists, rename (as a backup)
            if (File.Exists(filename))
            {
                File.Delete(backupFilename);
                File.Move(filename, Path.Combine(Path.GetDirectoryName(filename), Path.GetFileName(filename) + ".bak"));
            }

            // Create database file...
            try
            {
                using (SqlCeEngine engine = new SqlCeEngine("DataSource=\"" + filename + "\"; Password=\"charpad\""))
                {
                    engine.CreateDatabase();

                    List<string> scripts = GetBuildSchemaScripts();

                    if ((scripts == null) || (scripts.Count == 0))
                        throw new InvalidOperationException("Invalid create script.");

                    // Build schema
                    using (SqlCeConnection conn = new SqlCeConnection(engine.LocalConnectionString))
                    {
                        conn.Open();

                        foreach (string script in scripts)
                        {
                            if (String.IsNullOrWhiteSpace(script))
                                continue;

                            using (SqlCeCommand command = new SqlCeCommand(script, conn))
                            {
                                command.ExecuteNonQuery();
                            }
                        }

                        // Save each party member
                        foreach (Player player in party.Members)
                        {
                            SavePlayer(conn, player);
                        }
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                    File.Delete(filename);
                    File.Move(backupFilename, filename);
                }
                catch (Exception)
                {
                    // Consume exception...
                }

                throw;
            }
        }

        private static void SavePlayer(SqlCeConnection conn, Player player)
        {
            int playerId = GetNextId(conn, "Player");

            int playerClassId = SavePlayerClass(conn, player);
            int playerRaceId = SavePlayerRace(conn, player);
            int? hitPoints_AdjustListId = SaveBasicAdjustmentList(conn, player.HitPoints.MiscAdjustments);
            int? surge_AdjustListId = SaveBasicAdjustmentList(conn, player.SurgeValue.MiscAdjustments);
            int? surgesPerDay_AdjustListId = SaveBasicAdjustmentList(conn, player.SurgesPerDay.MiscAdjustments);
            int? acrobatics_AdjustListId = SaveBasicAdjustmentList(conn, player.Acrobatics.MiscAdjustments);
            int? arcana_AdjustListId = SaveBasicAdjustmentList(conn, player.Arcana.MiscAdjustments);
            int? athletics_AdjustListId = SaveBasicAdjustmentList(conn, player.Athletics.MiscAdjustments);
            int? bluff_AdjustListId = SaveBasicAdjustmentList(conn, player.Bluff.MiscAdjustments);
            int? diplomacy_AdjustListId = SaveBasicAdjustmentList(conn, player.Diplomacy.MiscAdjustments);
            int? dungeoneering_AdjustListId = SaveBasicAdjustmentList(conn, player.Dungeoneering.MiscAdjustments);
            int? endurance_AdjustListId = SaveBasicAdjustmentList(conn, player.Endurance.MiscAdjustments);
            int? heal_AdjustListId = SaveBasicAdjustmentList(conn, player.Heal.MiscAdjustments);
            int? history_AdjustListId = SaveBasicAdjustmentList(conn, player.History.MiscAdjustments);
            int? insight_AdjustListId = SaveBasicAdjustmentList(conn, player.Insight.MiscAdjustments);
            int? intimidate_AdjustListId = SaveBasicAdjustmentList(conn, player.Intimidate.MiscAdjustments);
            int? nature_AdjustListId = SaveBasicAdjustmentList(conn, player.Nature.MiscAdjustments);
            int? perception_AdjustListId = SaveBasicAdjustmentList(conn, player.Perception.MiscAdjustments);
            int? religion_AdjustListId = SaveBasicAdjustmentList(conn, player.Religion.MiscAdjustments);
            int? stealth_AdjustListId = SaveBasicAdjustmentList(conn, player.Stealth.MiscAdjustments);
            int? streetwise_AdjustListId = SaveBasicAdjustmentList(conn, player.Streetwise.MiscAdjustments);
            int? thieverty_AdjustListId = SaveBasicAdjustmentList(conn, player.Thievery.MiscAdjustments);
            int? initiative_AdjustListId = SaveBasicAdjustmentList(conn, player.Initiative.MiscAdjustments);
            int? acDefense_AdjustListId = SaveBasicAdjustmentList(conn, player.AcDefense.MiscAdjustments);
            int? fortDefense_AdjustListId = SaveBasicAdjustmentList(conn, player.FortDefense.MiscAdjustments);
            int? reflexDefense_AdjustListId = SaveBasicAdjustmentList(conn, player.ReflexDefense.MiscAdjustments);
            int? willDefense_AdjustListId = SaveBasicAdjustmentList(conn, player.WillDefense.MiscAdjustments);
            int? speed_AdjustListId = SaveBasicAdjustmentList(conn, player.Speed.MiscAdjustments);

            Dictionary<IInventoryItem, int> itemMap = BuildItemMap(conn, player.Inventory);

            int? armorId = (player.Armor == null ? null : (int?)itemMap[player.Armor]);
            int? shieldId = (player.Shield == null ? null : (int?)itemMap[player.Shield]);
            int? weaponId = (player.Weapon == null ? null : (int?)itemMap[player.Weapon]);
            int? offhandWeaponId = (player.WeaponOffhand == null ? null : (int?)itemMap[player.WeaponOffhand]);
            int? rangedWeaponId = (player.RangedWeapon == null ? null : (int?)itemMap[player.RangedWeapon]);
            int? implementId = (player.Implement == null ? null : (int?)itemMap[player.Implement]);

            int weaponSpecId = SaveWeaponSpec(conn, player.WeaponSpec);
            int offhandWeaponSpecId = SaveWeaponSpec(conn, player.WeaponOffhandSpec);
            int rangedWeaponSpecId = SaveWeaponSpec(conn, player.RangedWeaponSpec);
            int implementSpecId = SaveWeaponSpec(conn, player.ImplementSpec);

            SaveWeaponBonuses(conn, playerId, player.WeaponBonuses, itemMap);
            SaveResistances(conn, playerId, player.Resistances);

            SaveFeatures(conn, playerId, player.Feats, 1);
            SaveFeatures(conn, playerId, player.RaceFeatures, 2);
            SaveFeatures(conn, playerId, player.ClassFeatures, 3);
            SaveFeatures(conn, playerId, player.ParagonFeatures, 4);
            SaveFeatures(conn, playerId, player.DestinyFeatures, 5);

            SavePowers(conn, playerId, player.Powers);

            string sqlText = "insert Player(Id, PlayerName, PersonName, PlayerClassId, PlayerRaceId, IsMale, Deity, Level, Str, Con, Dex, Int, Wis, Cha, " +
                "HitPoints_AdjustListId, Surge_AdjustListId, SurgesPerDay_AdjustListId, Acrobatics_IsTrained, Acrobatics_AdjustListId, " +
                "Arcana_IsTrained, Arcana_AdjustListId, Athletics_IsTrained, Athletics_AdjustListId, Bluff_IsTrained, Bluff_AdjustListId, " +
                "Diplomacy_IsTrained, Diplomacy_AdjustListId, Dungeoneering_IsTrained, Dungeoneering_AdjustListId, " +
                "Endurance_IsTrained, Endurance_AdjustListId, Heal_IsTrained, Heal_AdjustListId, History_IsTrained, History_AdjustListId, " +
                "Insight_IsTrained, Insight_AdjustListId, Intimidate_IsTrained, Intimidate_AdjustListId, Nature_IsTrained, Nature_AdjustListId, " +
                "Perception_IsTrained, Perception_AdjustListId, Religion_IsTrained, Religion_AdjustListId, Stealth_IsTrained, Stealth_AdjustListId, " +
                "Streetwise_IsTrained, Streetwise_AdjustListId, Thievery_IsTrained, Thievery_AdjustListId, Initiative_AdjustListId, " +
                "AcDefense_AdjustListId, FortDefense_AdjustListId, ReflexDefense_AdjustListId, WillDefense_AdjustListId, Speed_AdjustListId, " +
                "ArmorId, ShieldId, WeaponId, OffhandWeaponId, RangedWeaponId, ImplementId, Picture, WeaponSpecId, OffhandWeaponSpecId, " +
                "RangedWeaponSpecId, ImplementSpecId, Notes) " +
                "values(@Id, @PlayerName, @PersonName, @PlayerClassId, @PlayerRaceId, @IsMale, @Deity, @Level, @Str, @Con, @Dex, @Int, @Wis, @Cha, " +
                "@HitPoints_AdjustListId, @Surge_AdjustListId, @SurgesPerDay_AdjustListId, @Acrobatics_IsTrained, @Acrobatics_AdjustListId, " +
                "@Arcana_IsTrained, @Arcana_AdjustListId, @Athletics_IsTrained, @Athletics_AdjustListId, @Bluff_IsTrained, @Bluff_AdjustListId, " +
                "@Diplomacy_IsTrained, @Diplomacy_AdjustListId, @Dungeoneering_IsTrained, @Dungeoneering_AdjustListId, " +
                "@Endurance_IsTrained, @Endurance_AdjustListId, @Heal_IsTrained, @Heal_AdjustListId, @History_IsTrained, @History_AdjustListId, " +
                "@Insight_IsTrained, @Insight_AdjustListId, @Intimidate_IsTrained, @Intimidate_AdjustListId, @Nature_IsTrained, @Nature_AdjustListId, " +
                "@Perception_IsTrained, @Perception_AdjustListId, @Religion_IsTrained, @Religion_AdjustListId, @Stealth_IsTrained, @Stealth_AdjustListId, " +
                "@Streetwise_IsTrained, @Streetwise_AdjustListId, @Thievery_IsTrained, @Thievery_AdjustListId, @Initiative_AdjustListId, " +
                "@AcDefense_AdjustListId, @FortDefense_AdjustListId, @ReflexDefense_AdjustListId, @WillDefense_AdjustListId, @Speed_AdjustListId, " +
                "@ArmorId, @ShieldId, @WeaponId, @OffhandWeaponId, @RangedWeaponId, @ImplementId, @Picture, @WeaponSpecId, @OffhandWeaponSpecId, " +
                "@RangedWeaponSpecId, @ImplementSpecId, @Notes)";

            using (SqlCeCommand command = new SqlCeCommand(sqlText, conn))
            {
                command.Parameters.AddWithValue("@Id", playerId);
                command.Parameters.AddWithValue("@PlayerName", player.CharacterName);
                command.Parameters.AddWithValue("@PersonName", player.PlayerName);
                command.Parameters.AddWithValue("@PlayerClassId", playerClassId);
                command.Parameters.AddWithValue("@PlayerRaceId", playerRaceId);
                command.Parameters.AddWithValue("@IsMale", player.IsMale);
                command.Parameters.AddWithValue("@Deity", player.Deity);
                command.Parameters.AddWithValue("@Level", player.Level);
                command.Parameters.AddWithValue("@Str", player.Str);
                command.Parameters.AddWithValue("@Con", player.Con);
                command.Parameters.AddWithValue("@Dex", player.Dex);
                command.Parameters.AddWithValue("@Int", player.Int);
                command.Parameters.AddWithValue("@Wis", player.Wis);
                command.Parameters.AddWithValue("@Cha", player.Cha);
                command.Parameters.AddWithValue("@HitPoints_AdjustListId", ConvertNull(hitPoints_AdjustListId));
                command.Parameters.AddWithValue("@Surge_AdjustListId", ConvertNull(surge_AdjustListId));
                command.Parameters.AddWithValue("@SurgesPerDay_AdjustListId", ConvertNull(surgesPerDay_AdjustListId));
                command.Parameters.AddWithValue("@Acrobatics_IsTrained", player.Acrobatics.IsTrained);
                command.Parameters.AddWithValue("@Acrobatics_AdjustListId", ConvertNull(acrobatics_AdjustListId));
                command.Parameters.AddWithValue("@Arcana_IsTrained", player.Arcana.IsTrained);
                command.Parameters.AddWithValue("@Arcana_AdjustListId", ConvertNull(arcana_AdjustListId));
                command.Parameters.AddWithValue("@Athletics_IsTrained", player.Athletics.IsTrained);
                command.Parameters.AddWithValue("@Athletics_AdjustListId", ConvertNull(athletics_AdjustListId));
                command.Parameters.AddWithValue("@Bluff_IsTrained", player.Bluff.IsTrained);
                command.Parameters.AddWithValue("@Bluff_AdjustListId", ConvertNull(bluff_AdjustListId));
                command.Parameters.AddWithValue("@Diplomacy_IsTrained", player.Diplomacy.IsTrained);
                command.Parameters.AddWithValue("@Diplomacy_AdjustListId", ConvertNull(diplomacy_AdjustListId));
                command.Parameters.AddWithValue("@Dungeoneering_IsTrained", player.Dungeoneering.IsTrained);
                command.Parameters.AddWithValue("@Dungeoneering_AdjustListId", ConvertNull(dungeoneering_AdjustListId));
                command.Parameters.AddWithValue("@Endurance_IsTrained", player.Endurance.IsTrained);
                command.Parameters.AddWithValue("@Endurance_AdjustListId", ConvertNull(endurance_AdjustListId));
                command.Parameters.AddWithValue("@Heal_IsTrained", player.Heal.IsTrained);
                command.Parameters.AddWithValue("@Heal_AdjustListId", ConvertNull(heal_AdjustListId));
                command.Parameters.AddWithValue("@History_IsTrained", player.History.IsTrained);
                command.Parameters.AddWithValue("@History_AdjustListId", ConvertNull(history_AdjustListId));
                command.Parameters.AddWithValue("@Insight_IsTrained", player.Insight.IsTrained);
                command.Parameters.AddWithValue("@Insight_AdjustListId", ConvertNull(insight_AdjustListId));
                command.Parameters.AddWithValue("@Intimidate_IsTrained", player.Intimidate.IsTrained);
                command.Parameters.AddWithValue("@Intimidate_AdjustListId", ConvertNull(intimidate_AdjustListId));
                command.Parameters.AddWithValue("@Nature_IsTrained", player.Nature.IsTrained);
                command.Parameters.AddWithValue("@Nature_AdjustListId", ConvertNull(nature_AdjustListId));
                command.Parameters.AddWithValue("@Perception_IsTrained", player.Perception.IsTrained);
                command.Parameters.AddWithValue("@Perception_AdjustListId", ConvertNull(perception_AdjustListId));
                command.Parameters.AddWithValue("@Religion_IsTrained", player.Religion.IsTrained);
                command.Parameters.AddWithValue("@Religion_AdjustListId", ConvertNull(religion_AdjustListId));
                command.Parameters.AddWithValue("@Stealth_IsTrained", player.Stealth.IsTrained);
                command.Parameters.AddWithValue("@Stealth_AdjustListId", ConvertNull(stealth_AdjustListId));
                command.Parameters.AddWithValue("@Streetwise_IsTrained", player.Streetwise.IsTrained);
                command.Parameters.AddWithValue("@Streetwise_AdjustListId", ConvertNull(streetwise_AdjustListId));
                command.Parameters.AddWithValue("@Thievery_IsTrained", player.Thievery.IsTrained);
                command.Parameters.AddWithValue("@Thievery_AdjustListId", ConvertNull(thieverty_AdjustListId));
                command.Parameters.AddWithValue("@Initiative_AdjustListId", ConvertNull(initiative_AdjustListId));
                command.Parameters.AddWithValue("@AcDefense_AdjustListId", ConvertNull(acDefense_AdjustListId));
                command.Parameters.AddWithValue("@FortDefense_AdjustListId", ConvertNull(fortDefense_AdjustListId));
                command.Parameters.AddWithValue("@ReflexDefense_AdjustListId", ConvertNull(reflexDefense_AdjustListId));
                command.Parameters.AddWithValue("@WillDefense_AdjustListId", ConvertNull(willDefense_AdjustListId));
                command.Parameters.AddWithValue("@Speed_AdjustListId", ConvertNull(speed_AdjustListId));
                command.Parameters.AddWithValue("@ArmorId", ConvertNull(armorId));
                command.Parameters.AddWithValue("@ShieldId", ConvertNull(shieldId));
                command.Parameters.AddWithValue("@WeaponId", ConvertNull(weaponId));
                command.Parameters.AddWithValue("@OffhandWeaponId", ConvertNull(offhandWeaponId));
                command.Parameters.AddWithValue("@RangedWeaponId", ConvertNull(rangedWeaponId));
                command.Parameters.AddWithValue("@ImplementId", ConvertNull(implementId));
                command.Parameters.AddWithValue("@Picture", ConvertNull(ConvertPictureToByteArray(player.Picture)));
                command.Parameters.AddWithValue("@WeaponSpecId", weaponSpecId);
                command.Parameters.AddWithValue("@OffhandWeaponSpecId", offhandWeaponSpecId);
                command.Parameters.AddWithValue("@RangedWeaponSpecId", rangedWeaponSpecId);
                command.Parameters.AddWithValue("@ImplementSpecId", implementSpecId);
                command.Parameters.AddWithValue("@Notes", (String.IsNullOrEmpty(player.Notes) ? (object)DBNull.Value : (object)player.Notes));

                command.ExecuteNonQuery();
            }
        }

        private static void SavePowers(SqlCeConnection conn, int playerId, PowerCollection powers)
        {
            string sqlText = "insert PlayerPower(PlayerId, Sequence, Name, PowerType, ActionType, Level, Description, Notes, AttackType, AttackWeapon, " +
                "AttackAttribute, DefenseType, AttackModifiers_AdjustListId, Damage, WeaponDamageMultiplier, DamageType, BonusDamageAttribute, " +
                "DamageModifiers_AdjustListId, Picture) " +
                "values(@PlayerId, @Sequence, @Name, @PowerType, @ActionType, @Level, @Description, @Notes, @AttackType, @AttackWeapon, " +
                "@AttackAttribute, @DefenseType, @AttackModifiers_AdjustListId, @Damage, @WeaponDamageMultiplier, @DamageType, @BonusDamageAttribute, " +
                "@DamageModifiers_AdjustListId, @Picture)";

            for (int i = 0; i < powers.Count; i++)
            {
                Power power = powers[i];

                int? attackModifiers_AdjustListId = SaveBasicAdjustmentList(conn, power.AttackModifiers);
                int? damageModifiers_AdjustListId = SaveBasicAdjustmentList(conn, power.DamageModifiers);

                using (SqlCeCommand command = new SqlCeCommand(sqlText, conn))
                {
                    command.Parameters.AddWithValue("@PlayerId", playerId);
                    command.Parameters.AddWithValue("@Sequence", i + 1);
                    command.Parameters.AddWithValue("@Name", power.Name);
                    command.Parameters.AddWithValue("@PowerType", (int)power.PowerType);
                    command.Parameters.AddWithValue("@ActionType", (int)power.ActionType);
                    command.Parameters.AddWithValue("@Level", power.Level);
                    command.Parameters.AddWithValue("@Description", power.Description);
                    command.Parameters.AddWithValue("@Notes", (String.IsNullOrEmpty(power.Notes) ? (object)DBNull.Value : (object)power.Notes));
                    command.Parameters.AddWithValue("@AttackType", (int)power.AttackType);
                    command.Parameters.AddWithValue("@AttackWeapon", (int)power.AttackWeapon);
                    command.Parameters.AddWithValue("@AttackAttribute", (int)power.AttackAttribute);
                    command.Parameters.AddWithValue("@DefenseType", (int)power.DefenseType);
                    command.Parameters.AddWithValue("@AttackModifiers_AdjustListId", ConvertNull(attackModifiers_AdjustListId));
                    command.Parameters.AddWithValue("@Damage", (power.Damage == null ? (object)DBNull.Value : (object)power.Damage.DisplayString));
                    command.Parameters.AddWithValue("@WeaponDamageMultiplier", power.WeaponDamamgeMultiplier);
                    command.Parameters.AddWithValue("@DamageType", power.DamageType);
                    command.Parameters.AddWithValue("@BonusDamageAttribute", (power.BonusDamageAttribute == null ? (object)DBNull.Value : (object)power.BonusDamageAttribute.Value));
                    command.Parameters.AddWithValue("@DamageModifiers_AdjustListId", ConvertNull(damageModifiers_AdjustListId));
                    command.Parameters.AddWithValue("@Picture", ConvertNull(ConvertPictureToByteArray(power.Picture)));

                    command.ExecuteNonQuery();
                }
            }
        }

        private static void SaveFeatures(SqlCeConnection conn, int playerId, ObservableCollectionEx<FeatureValue> list, int featureType)
        {
            string sqlText = "insert FeatureValue(PlayerId, FeatureType, Sequence, Name, ShortDescription, LongDescription) " +
                "values(@PlayerId, @FeatureType, @Sequence, @Name, @ShortDescription, @LongDescription)";

            for (int i = 0; i < list.Count; i++)
            {
                FeatureValue feature = list[i];

                using (SqlCeCommand command = new SqlCeCommand(sqlText, conn))
                {
                    command.Parameters.AddWithValue("@PlayerId", playerId);
                    command.Parameters.AddWithValue("@FeatureType", featureType);
                    command.Parameters.AddWithValue("@Sequence", i + 1);
                    command.Parameters.AddWithValue("@Name", feature.Name);
                    command.Parameters.AddWithValue("@ShortDescription", (String.IsNullOrEmpty(feature.ShortDescription) ? (object)DBNull.Value : (object)feature.ShortDescription));
                    command.Parameters.AddWithValue("@LongDescription", (String.IsNullOrEmpty(feature.LongDescription) ? (object)DBNull.Value : (object)feature.LongDescription));

                    command.ExecuteNonQuery();
                }
            }
        }

        private static void SaveResistances(SqlCeConnection conn, int playerId, ObservableCollectionEx<ResistanceValue> list)
        {
            string sqlText = "insert ResistanceValue(PlayerId, Sequence, Modifier, DamageType, Description) " +
                "values(@PlayerId, @Sequence, @Modifier, @DamageType, @Description)";

            for (int i = 0; i < list.Count; i++)
            {
                ResistanceValue resistance = list[i];

                using (SqlCeCommand command = new SqlCeCommand(sqlText, conn))
                {
                    command.Parameters.AddWithValue("@PlayerId", playerId);
                    command.Parameters.AddWithValue("@Sequence", i + 1);
                    command.Parameters.AddWithValue("@Modifier", resistance.Modifier);
                    command.Parameters.AddWithValue("@DamageType", resistance.DamageType);
                    command.Parameters.AddWithValue("@Description", (resistance.Description == null ? (object)DBNull.Value : (object)resistance.Description));

                    command.ExecuteNonQuery();
                }
            }
        }

        private static void SaveWeaponBonuses(SqlCeConnection conn, int playerId, WeaponBonusList bonusList, Dictionary<IInventoryItem, int> itemMap)
        {
            string sqlText = "insert WeaponBonus(PlayerId, WeaponId, ToHitAdjustListId, DamageAdjustListId) " +
                "values(@PlayerId, @WeaponId, @ToHitAdjustListId, @DamageAdjustListId)";

            foreach (WeaponBonus bonus in bonusList)
            {
                int? toHit_AdjustListId = SaveBasicAdjustmentList(conn, bonus.Bonus.ToHitAdjustments);
                int? damage_AdjustListId = SaveBasicAdjustmentList(conn, bonus.Bonus.DamageAdjustments);

                if ((bonus.Weapon == null) || !itemMap.ContainsKey(bonus.Weapon))
                    continue;

                using (SqlCeCommand command = new SqlCeCommand(sqlText, conn))
                {
                    command.Parameters.AddWithValue("@PlayerId", playerId);
                    command.Parameters.AddWithValue("@WeaponId", itemMap[bonus.Weapon]);
                    command.Parameters.AddWithValue("@ToHitAdjustListId", ConvertNull(toHit_AdjustListId));
                    command.Parameters.AddWithValue("@DamageAdjustListId", ConvertNull(damage_AdjustListId));

                    command.ExecuteNonQuery();
                }
            }
        }

        private static int SaveWeaponSpec(SqlCeConnection conn, WeaponSpecValue spec)
        {
            int id = GetNextId(conn, "WeaponSpec");
            int? toHit_AdjustListId = SaveBasicAdjustmentList(conn, spec.ToHitAdjustments);
            int? damage_AdjustListId = SaveBasicAdjustmentList(conn, spec.DamageAdjustments);

            string sqlText = "insert WeaponSpec(Id, WeaponSlot, ToHit_AdjustListId, Damage_AdjustListId) " +
                "values(@Id, @WeaponSlot, @ToHit_AdjustListId, @Damage_AdjustListid)";

            using (SqlCeCommand command = new SqlCeCommand(sqlText, conn))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@WeaponSlot", (int)spec.Slot);
                command.Parameters.AddWithValue("@ToHit_AdjustListId", ConvertNull(toHit_AdjustListId));
                command.Parameters.AddWithValue("@Damage_AdjustListId", ConvertNull(damage_AdjustListId));

                command.ExecuteNonQuery();
            }

            return id;
        }

        private static object ConvertNull(int? value)
        {
            return (value == null ? (object)DBNull.Value : (object)value.Value);
        }

        private static object ConvertNull(byte[] value)
        {
            return (value == null ? (object)DBNull.Value : (object)value);
        }

        private static Dictionary<IInventoryItem, int> BuildItemMap(SqlCeConnection conn, ObservableCollectionEx<IInventoryItem> list)
        {
            int highItemId = GetNextId(conn, "Armor");
            highItemId = Math.Max(highItemId, GetNextId(conn, "Shield"));
            highItemId = Math.Max(highItemId, GetNextId(conn, "Weapon"));

            Dictionary<IInventoryItem, int> itemMap = new Dictionary<IInventoryItem, int>();

            for (int i = 0; i < list.Count; i++)
            {
                IInventoryItem item = list[i];
                int id = i + highItemId;

                if (itemMap.ContainsKey(item))
                    continue;

                if (item is Armor)
                    SaveArmor(conn, id, (Armor)item);
                else if (item is Shield)
                    SaveShield(conn, id, (Shield)item);
                else if (item is Weapon)
                    SaveWeapon(conn, id, (Weapon)item);
                else
                    throw new InvalidOperationException("Unexpected type of inventory item: " + item.GetType().Name);

                itemMap.Add(item, id); 
            }

            return itemMap;
        }

        private static void SaveArmor(SqlCeConnection conn, int id, Armor armor)
        {
            string sqlText = "insert Armor(Id, Name, ArmorType, EnhancementBonus, ArmorBonus, SkillModifier, SpeedModifier, BasePrice, IsHeavy, SpecialProperty, MinEnhancementBonus, Picture, Notes) " +
                "values(@Id, @Name, @ArmorType, @EnhancementBonus, @ArmorBonus, @SkillModifier, @SpeedModifier, @BasePrice, @IsHeavy, @SpecialProperty, @MinEnhancementBonus, @Picture, @Notes)";

            using (SqlCeCommand command = new SqlCeCommand(sqlText, conn))
            {
                byte[] pictureBytes = ConvertPictureToByteArray(armor.Picture);

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", armor.Name);
                command.Parameters.AddWithValue("@ArmorType", (int)armor.ArmorType);
                command.Parameters.AddWithValue("@EnhancementBonus", armor.EnhancementBonus);
                command.Parameters.AddWithValue("@ArmorBonus", armor.ArmorBonus);
                command.Parameters.AddWithValue("@SkillModifier", armor.SkillModifier);
                command.Parameters.AddWithValue("@SpeedModifier", armor.SpeedModifier);
                command.Parameters.AddWithValue("@BasePrice", armor.BasePrice);
                command.Parameters.AddWithValue("@IsHeavy", armor.IsHeavy);
                command.Parameters.AddWithValue("@SpecialProperty", armor.SpecialProperty);
                command.Parameters.AddWithValue("@MinEnhancementBonus", armor.MinEnhancementBonus);
                command.Parameters.AddWithValue("@Picture", (pictureBytes == null ? (object)DBNull.Value : (object)pictureBytes));
                command.Parameters.AddWithValue("@Notes", (String.IsNullOrEmpty(armor.Notes) ? (object)DBNull.Value : (object)armor.Notes));

                command.ExecuteNonQuery();
            }
        }

        public static System.Drawing.Image BuildPictureFromByteArray(byte[] imageData)
        {
            if ((imageData == null) || (imageData.Length == 0))
                return null;

            using (MemoryStream stream = new MemoryStream(imageData))
            {
                return new System.Drawing.Bitmap(stream);
            }
        }

        public static byte[] ConvertPictureToByteArray(System.Drawing.Image image)
        {
            if (image == null)
                return null;

            if (!(image is System.Drawing.Bitmap))
                throw new InvalidOperationException("Cannot handle non-bitmap images.");

            System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)image;

            using (MemoryStream stream = new MemoryStream())
            {
                System.Drawing.Bitmap copyBitmap = new System.Drawing.Bitmap(bitmap);

                copyBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;

                return stream.ToArray();
            }
        }

        private static void SaveShield(SqlCeConnection conn, int id, Shield shield)
        {
            string sqlText = "insert Shield(Id, Name, ArmorType, EnhancementBonus, ArmorBonus, SkillModifier, BasePrice, Picture, Notes) " +
                "values(@Id, @Name, @ArmorType, @EnhancementBonus, @ArmorBonus, @SkillModifier, @BasePrice, @Picture, @Notes)";

            using (SqlCeCommand command = new SqlCeCommand(sqlText, conn))
            {
                byte[] pictureBytes = ConvertPictureToByteArray(shield.Picture);

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", shield.Name);
                command.Parameters.AddWithValue("@ArmorType", (int)shield.ArmorType);
                command.Parameters.AddWithValue("@EnhancementBonus", shield.EnhancementBonus);
                command.Parameters.AddWithValue("@ArmorBonus", shield.ArmorBonus);
                command.Parameters.AddWithValue("@SkillModifier", shield.SkillModifier);
                command.Parameters.AddWithValue("@BasePrice", shield.BasePrice);
                command.Parameters.AddWithValue("@Picture", (pictureBytes == null ? (object)DBNull.Value : (object)pictureBytes));
                command.Parameters.AddWithValue("@Notes", (String.IsNullOrEmpty(shield.Notes) ? (object)DBNull.Value : (object)shield.Notes));

                command.ExecuteNonQuery();
            }
        }

        private static void SaveWeapon(SqlCeConnection conn, int id, Weapon weapon)
        {
            string sqlText = "insert Weapon(Id, Name, ProficiencyBonus, EnhancementBonus, Damage, Range, [Group], Properties, BasePrice, Category, IsTwoHanded, IsImplement, Picture, Notes) " +
                "values(@Id, @Name, @ProficiencyBonus, @EnhancementBonus, @Damage, @Range, @Group, @Properties, @BasePrice, @Category, @IsTwoHanded, @IsImplement, @Picture, @Notes)";

            using (SqlCeCommand command = new SqlCeCommand(sqlText, conn))
            {
                byte[] pictureBytes = ConvertPictureToByteArray(weapon.Picture);

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", weapon.Name);
                command.Parameters.AddWithValue("@ProficiencyBonus", weapon.ProficiencyBonus);
                command.Parameters.AddWithValue("@EnhancementBonus", weapon.EnhancementBonus);
                command.Parameters.AddWithValue("@Damage", (weapon.Damage == null ? (object)DBNull.Value : (object)weapon.Damage.DisplayString));
                command.Parameters.AddWithValue("@Range", weapon.Range);
                command.Parameters.AddWithValue("@Group", (int)weapon.Group);
                command.Parameters.AddWithValue("@Properties", (int)weapon.Properties);
                command.Parameters.AddWithValue("@BasePrice", weapon.BasePrice);
                command.Parameters.AddWithValue("@Category", (int)weapon.Category);
                command.Parameters.AddWithValue("@IsTwoHanded", weapon.IsTwoHanded);
                command.Parameters.AddWithValue("@IsImplement", weapon.IsImplement);
                command.Parameters.AddWithValue("@Picture", (pictureBytes == null ? (object)DBNull.Value : (object)pictureBytes));
                command.Parameters.AddWithValue("@Notes", (String.IsNullOrEmpty(weapon.Notes) ? (object)DBNull.Value : (object)weapon.Notes));

                command.ExecuteNonQuery();
            }
        }

        private static int? SaveBasicAdjustmentList(SqlCeConnection conn, BasicAdjustmentList list)
        {
            if (list.Count == 0)
                return null;

            int id = GetNextId(conn, "BasicAdjustmentList", "ListId");
            string sqlText = "insert BasicAdjustmentList(ListId, Sequence, Modifier, Note) " +
                "values(@ListId, @Sequence, @Modifier, @Note)";

            for (int i = 0; i < list.Count; i++)
            {
                using (SqlCeCommand command = new SqlCeCommand(sqlText, conn))
                {
                    BasicAdjustment adj = list[i];

                    command.Parameters.AddWithValue("@ListId", id);
                    command.Parameters.AddWithValue("@Sequence", i + 1);
                    command.Parameters.AddWithValue("@Modifier", adj.Modifier);
                    command.Parameters.AddWithValue("@Note", (String.IsNullOrEmpty(adj.Note) ? (object)DBNull.Value : (object)adj.Note));

                    command.ExecuteNonQuery();
                }
            }

            return id;
        }

        private static int SavePlayerClass(SqlCeConnection conn, Player player)
        {
            int id = GetNextId(conn, "PlayerClass");
            string sqlText = "insert PlayerClass(Id, Name, BaseHealth, HealthPerLevel, BaseHealingSurges, FortitudeBonus, ReflexBonus, WillBonus) " +
                "values(@Id, @Name, @BaseHealth, @HealthPerLevel, @BaseHealingSurges, @FortitudeBonus, @ReflexBonus, @WillBonus)";

            using (SqlCeCommand command = new SqlCeCommand(sqlText, conn))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", player.Class.Name);
                command.Parameters.AddWithValue("@BaseHealth", player.Class.BaseHealth);
                command.Parameters.AddWithValue("@HealthPerLevel", player.Class.HealthPerLevel);
                command.Parameters.AddWithValue("@BaseHealingSurges", player.Class.BaseHealingSurges);
                command.Parameters.AddWithValue("@FortitudeBonus", player.Class.FortitudeBonus);
                command.Parameters.AddWithValue("@ReflexBonus", player.Class.ReflexBonus);
                command.Parameters.AddWithValue("@WillBonus", player.Class.WillBonus);

                command.ExecuteNonQuery();
            }

            return id;
        }

        private static int SavePlayerRace(SqlCeConnection conn, Player player)
        {
            int id = GetNextId(conn, "PlayerRace");
            string sqlText = "insert PlayerRace(Id, Name, Size, BaseSpeed) " +
                "values(@Id, @Name, @Size, @BaseSpeed)";

            using (SqlCeCommand command = new SqlCeCommand(sqlText, conn))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", player.Race.Name);
                command.Parameters.AddWithValue("@Size", (int)player.Race.Size);
                command.Parameters.AddWithValue("@BaseSpeed", player.Race.BaseSpeed);

                command.ExecuteNonQuery();
            }

            return id;
        }

        private static int GetNextId(SqlCeConnection conn, string tableName)
        {
            return GetNextId(conn, tableName, "Id");
        }

        private static int GetNextId(SqlCeConnection conn, string tableName, string idName)
        {
            string sqlText = "select max(" + idName + ") from " + tableName;

            using (SqlCeCommand command = new SqlCeCommand(sqlText, conn))
            {
                object obj = command.ExecuteScalar();

                if (obj == DBNull.Value)
                    return 1;
                else
                    return (Convert.ToInt32(obj) + 1);
            }
        }

        private static List<string> GetBuildSchemaScripts()
        {
            string fullText = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Framework\\db_script2.sqlce"));

            //Regex regex = new Regex("^go$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            //return new List<string>(regex.Split(fullText));

            return new List<string>(fullText.Split(new string[] { Environment.NewLine + "GO" + Environment.NewLine, 
                Environment.NewLine + "go" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
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
