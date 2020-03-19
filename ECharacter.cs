using Ethereal.Enums;
using Ethereal.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Ethereal
{
    public class ECharacter : ModPlayer
    {
        /* Character Level */
        public int Level { get; set; } = 1;
        /* Character Experience To Next Level */
        public long Experience { get; set; }
        /* Character Total Experience */
        public long TotalExperience { get; set; }
        /* Dictionary of Player Stats */
        public Dictionary<PlayerStats, int> BaseStats { get; set; } = new Dictionary<PlayerStats, int>();

        /* Initialization */
        public bool Initialized { get; set; }

        /* Gets Points Allocated to Skills */
        public int PointsAllocated => Enum.GetValues(typeof(PlayerStats)).Cast<PlayerStats>().Sum(stat => BaseStats[stat]);

        /* Handles Character Level-Up */
        public void LevelUp()
        {
            Level += 1;
            SyncLevelPacket.Write(player.whoAmI, Level);
            Main.NewText("Congratulations! You are now level: " + Level, 43, 135, 255);
        }

        /* Handles Character Experience Gain and Leveling */
        public void AddExperience(long xp)
        {
            if (Main.gameMenu)
                return;
            if (xp == 0)
                return;
            if (Level > 274)
                return;

            Experience += xp;
            TotalExperience += xp;
            Main.NewText("You have earned +" + xp + " experience!", 255, 71, 188);

        Check:
            if (Experience >= GetExperienceNeededForLevel(Level))
            {
                Experience -= GetExperienceNeededForLevel(Level);
                LevelUp();
                goto Check;
            }
        }

        /* Handles Experience to Next Level */
        public long GetExperienceNeededForLevel(int level)
        {
            if (level < 0 || level >= EConstants.levelEXP.Length)
            {
                return long.MaxValue;
            }

            return EConstants.levelEXP[level];
        }

        /* Initalizes Base Statistics, GUI, etc. */
        public override void Initialize()
        {
            BaseStats = new Dictionary<PlayerStats, int>();

            Level = 1;
            Experience = 0;

            foreach (PlayerStats stat in Enum.GetValues(typeof(PlayerStats)))
            {
                BaseStats[stat] = 0;
            }
        }

        /* Base Initialization of gui elements */
        public void InitializeGUI()
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            EGUI.GUIElements.Clear();
        }

        /* Modifies what happens when a player enters the world */
        public override void OnEnterWorld(Player player)
        {
            Ethereal.PlayerEnteredWorld = true;
            InitializeGUI();
        }

        private void ModifyDamage(ref int damage, ref bool crit, NPC target, Item item = null, Projectile proj = null)
        {
            float strDamage = 1f + BaseStats[PlayerStats.STR] * 0.04f;
            damage = (int)(damage * strDamage);
        }

        /* Modifies damage done to npcs */
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            ModifyDamage(ref damage, ref crit, target, item);
        }

        /* Handles Player Connecting */
        public override void PlayerConnect(Player playerObj)
        {
            SyncLevelPacket.Write(playerObj.whoAmI, Level, true);
        }

        /* Saves Character Statistics */
        public override TagCompound Save()
        {
            TagCompound tagCompound = new TagCompound
            {
                { "level", Level},
                { "experience", Experience},
                { "totalExperience", TotalExperience },
                { "baseSTR", BaseStats[PlayerStats.STR] },
                { "baseDEX", BaseStats[PlayerStats.DEX] },
                { "baseLUK", BaseStats[PlayerStats.LUK] },
                { "baseINT", BaseStats[PlayerStats.INT] }
            };

            return tagCompound;
        }

        /* Loads Character Statistics */
        public override void Load(TagCompound tag)
        {
            try
            {
                Level = tag.GetInt("level");
                Experience = tag.GetLong("experience");
                TotalExperience = tag.GetLong("totalExperience");
            }
            catch (SystemException e)
            {
                ModLoader.GetMod(EConstants.ModName).Logger.InfoFormat("@Level and Experience :: " + e);
            }

            try
            {
                foreach (PlayerStats stat in Enum.GetValues(typeof(PlayerStats)))
                    BaseStats[stat] = tag.GetInt("base" + stat.ToString().ToUpper());
            }
            catch (SystemException e)
            {
                ModLoader.GetMod(EConstants.ModName).Logger.InfoFormat("@Stats :: " + e);
            }
        }
    }
}
