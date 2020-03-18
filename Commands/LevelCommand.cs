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

                Main.NewText("Current Level: " + character.Level, 255, 223, 63);
                Main.NewText("Current Experience: " + character.Experience, 255, 223, 63);
                Main.NewText("Current Needed Experience: " + character.GetExperienceNeededForLevel(character.Level), 255, 223, 63);
            }
            catch (Exception e)
            {
                Main.NewText("Please use: " + Usage, 255, 223, 63);
                ModLoader.GetMod(EConstants.ModName).Logger.InfoFormat("@Command :: " + e);
            }
        }
    }
}
