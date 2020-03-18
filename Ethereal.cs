using Ethereal.Enums;
using Ethereal.Packets;
using System.Diagnostics;
using System.IO;
using Terraria.ModLoader;

namespace Ethereal
{
	public class Ethereal : Mod
	{
		/* Defines class as mod instance */
		public static Ethereal Instance { get; set; }
		/* Defines true/false if player entered world*/
		public static bool PlayerEnteredWorld { get; set; } = false;

		/* Handles class method */
		public Ethereal()
		{
			Instance = this;
		}

		/* Handles message logging */
		public static void WriteLog(string msg)
		{
			Debug.WriteLine("LOG: " + msg);
			ModLoader.GetMod(EConstants.ModName).Logger.InfoFormat(msg);
		}

		/* Handles Network Packets */
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			Message msg = (Message)reader.ReadByte();

			//Improper formatting...I refuse to have it proper
			#if DEBUG
			WriteLog($"Handling {msg}");
			#endif

			switch (msg)
			{
				case Message.AddExp:
					AddEXPPacket.Read(reader);
					break;
				case Message.SyncLevel:
					SyncLevelPacket.Read(reader);
					PlayerEnteredWorld = true;
					break;
			}
		}
	}
}