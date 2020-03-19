using System.Collections.Generic;
using System.IO;
using Ethereal.Enums;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ethereal.Packets
{
    /* Read/Write Stats Packet */
    public static class SyncStatsPacket
    {
        public static void Read(BinaryReader reader)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                ECharacter character = Main.player[reader.ReadInt32()].GetModPlayer<ECharacter>();
                character.Level = reader.ReadInt32();
                character.BaseStats[PlayerStats.STR] = reader.ReadInt32();
                character.BaseStats[PlayerStats.DEX] = reader.ReadInt32();
                character.BaseStats[PlayerStats.LUK] = reader.ReadInt32();
                character.BaseStats[PlayerStats.INT] = reader.ReadInt32();
            }
        }

        public static void Write(int whoAmI, int level, int STR, int DEX, int LUK, int INT)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = Ethereal.Instance.GetPacket();
                packet.Write((byte)Message.SyncStats);
                packet.Write(whoAmI);
                packet.Write(level);
                packet.Write(STR);
                packet.Write(DEX);
                packet.Write(LUK);
                packet.Write(INT);
                packet.Send();
            }
        }
    }
}