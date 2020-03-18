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
            long baseExp = character.Level * npc.lifeMax;
            long scaled = Main.expertMode ? baseExp * 2 : baseExp;

            if (!AddEXPPacket.Write(scaled, npc.target))
                character.AddExperience(scaled);
        }
    }
}
