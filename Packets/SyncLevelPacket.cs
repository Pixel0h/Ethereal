using Ethereal.Enums;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ethereal.Packets
{
    public static class SyncLevelPacket
    {
        /* Read/Write Syncing Level Packet */
        public static void Read(BinaryReader reader)
        {
            if (Main.netMode == NetmodeID.Server)
                Main.player[reader.ReadInt32()].GetModPlayer<ECharacter>().Level = reader.ReadInt32();
        }

        public static void Write(int whoAmI, int level, bool force = false)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = Ethereal.Instance.GetPacket();
                packet.Write((byte)Message.SyncLevel);
                packet.Write(whoAmI);
                packet.Write(level);
                packet.Send();
            }
            else if (force)
            {
                ModPacket packet = Ethereal.Instance.GetPacket();
                packet.Write((byte)Message.SyncLevel);
                packet.Write(whoAmI);
                packet.Write(level);
                packet.Send();
            }
        }
    }
}
