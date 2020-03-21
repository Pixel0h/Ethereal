using Ethereal.Enums;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Ethereal.Commands
{
    public class LevelCommand : ModCommand
    {
        /* Defines Command, Description, Command Type, and Usage */
        public override string Command => "level";
        public override string Description => "Sets your character level to the chosen value";
        public override CommandType Type => CommandType.Chat;
        public override string Usage => "/level <level>";

        /* Handles Command */
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            try
            {
                ECharacter character = caller.Player.GetModPlayer<ECharacter>();
                long xp = character.Experience / character.GetExperienceNeededForLevel(character.Level);
                character.Level = int.Parse(args[0]);
                character.Experience = character.GetExperienceNeededForLevel(character.Level) * xp;

                foreach (PlayerStats stat in Enum.GetValues(typeof(PlayerStats)))
                {
                    character.BaseStats[stat] += 1;
                }

                Main.NewText("Level Increased To: " + character.Level, 43, 255, 128);
            }
            catch (Exception e)
            {
                ECharacter character = caller.Player.GetModPlayer<ECharacter>();

                Main.NewText("Please use: " + Usage, 255, 43, 43);
                Main.NewText("Your Current Level Is: " + character.Level, 255, 223, 63);
                Main.NewText("Your Total Experience Is: " + character.TotalExperience, 255, 223, 63);
                Main.NewText("Experience To Next Level: " + character.Experience, 255, 223, 63);
                ModLoader.GetMod(EConstants.ModName).Logger.InfoFormat("@Command :: " + e);
            }
        }
    }
}
