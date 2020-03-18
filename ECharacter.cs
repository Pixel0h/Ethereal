using Ethereal.Packets;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Ethereal
{
    public class ECharacter : ModPlayer
    {
        /* Character Level */
        public int Level { get; set; } = 1;
        /* Character Experience */
        public long Experience { get; set; }

        /* Handles Character Level-Up */
        public void LevelUp()
        {
            Level += 1;
            SyncLevelPacket.Write(player.whoAmI, Level);
            Main.NewText("Congratulations! You are now level: " + Level, 255, 223, 63);
        }

        /* Handles Character Experience Gain and Leveling */
        public void AddExperience(long xp)
        {
            if (Main.gameMenu)
                return;
            if (xp == 0)
                return;
            Experience += xp;
            Main.NewText("You have earned +" + xp + " experience!", 255, 223, 63);

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

        /* Initalizes Base Statistics */
        public override void Initialize()
        {
            Level = 1;
            Experience = 0;
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
                {"level", Level},
                {"experience", Experience}
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
            }
            catch (Exception e)
            {
                ModLoader.GetMod(EConstants.ModName).Logger.InfoFormat("@Level and Experience :: " + e);
            }
        }
    }
}
