using Ethereal.Packets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ethereal
{
    public class ENPC : GlobalNPC
    {
        /* Handles npc loot, specifically getting experience from npcs */
        public override void NPCLoot(NPC npc)
        {
            ENPC eNPC = npc.GetGlobalNPC<ENPC>();
            Player player = Array.Find(Main.player, p => p.active);

            if (npc.lifeMax < 10)
                return;
            if (npc.friendly)
                return;
            if (npc.townNPC)
                return;

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                player = Main.LocalPlayer;
            }
            else if (Main.player[npc.target].active)
            {
                player = Main.player[npc.target];
            }
            else
            {
                ECharacter c = player.GetModPlayer<ECharacter>();
                foreach (Player p in Main.player)
                    if (p != null)
                        if (p.active)
                            if (p.GetModPlayer<ECharacter>() != null)
                                if (p.GetModPlayer<ECharacter>().Level > c.Level)
                                    player = p;
            }

            ECharacter character = player.GetModPlayer<ECharacter>();

            /* Base Character Experience w/out Modifiers */
            long baseExp = character.Level * npc.lifeMax;

            /* Experience Modifers w/ Boss Kills */
            long kingSlime = NPC.downedSlimeKing ? (long)(baseExp * .25) : baseExp;
            long eventGoblins = NPC.downedGoblins ? (long)(baseExp * .10) : baseExp;
            long queenBee = NPC.downedQueenBee ? (long)(baseExp * .25) : baseExp;
            long eventPirates = NPC.downedPirates ? (long)(baseExp * .10) : baseExp;
            long mechBosses = NPC.downedMechBossAny ? (long)(baseExp * .30) : baseExp;
            long plantera = NPC.downedPlantBoss ? (long)(baseExp * .25) : baseExp;
            long golem = NPC.downedGolemBoss ? (long)(baseExp * .25) : baseExp;
            long fishron = NPC.downedFishron ? (long)(baseExp * .25) : baseExp;
            long eventMartians = NPC.downedMartians ? (long)(baseExp * .10) : baseExp;
            long eventFrost = NPC.downedChristmasIceQueen ? (long)(baseExp * .05) : baseExp;
            long eventPumpkin = NPC.downedHalloweenKing ? (long)(baseExp * .05) : baseExp;
            long ancientCultist = NPC.downedAncientCultist ? (long)(baseExp * .10) : baseExp;
            long allTowers = NPC.downedTowers ? (long)(baseExp * .10) : baseExp;
            long moonLord = NPC.downedMoonlord ? (long)(baseExp * .50) : baseExp;

            /* Experience Modifiers w/ Mode */
            long expert = Main.expertMode ? (long)(baseExp * 2) : baseExp;
            long hardmode = Main.hardMode ? (long)(baseExp * .50) : baseExp;

            if (!AddEXPPacket.Write(baseExp, npc.target))
                character.AddExperience(baseExp);
        }
    }
}
