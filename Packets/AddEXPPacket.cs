using Ethereal.Enums;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ethereal.Packets
{
    public static class AddEXPPacket
    {
        /* Read/Write Experience Packet */
        public static void Read(BinaryReader reader)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ECharacter character = Main.LocalPlayer.GetModPlayer<ECharacter>();
                character.AddExperience((long)reader.ReadInt64());
            }
        }

        public static bool Write(long scaled, int target)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = Ethereal.Instance.GetPacket();
                packet.Write((byte)Message.AddExp);
                packet.Write(scaled);
                packet.Write(target);
                packet.Send();

                return true;
            }

            return false;
        }
    }
}
