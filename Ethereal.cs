using Ethereal.Enums;
using Ethereal.Packets;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Terraria;
using Terraria.ID;
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
            Properties = new ModProperties
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
            Instance = this;
        }

        /* Handles mod loading */
        public override void Load()
        {
            EConfig.Initialize();

            Main.player[Main.myPlayer].hbLocked = false;
        }

        /* Handles message logging */
        public static void WriteLog(string msg)
        {
            Debug.WriteLine("LOG: " + msg);
            ModLoader.GetMod(EConstants.ModName).Logger.InfoFormat(msg);
        }

        /* Handles interface drawing and GUIs */
        public bool DrawInterface()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            if (Main.netMode == NetmodeID.Server || Main.gameMenu) return true;
            try
            {
                for (int i = 0; i < EGUI.GUIElements.Count; i += 1)
                {
                    EGUI gui = EGUI.GUIElements[i];
                    if (gui.PreDraw())
                        gui.Draw(Main.spriteBatch, Main.LocalPlayer);
                }
            }
            catch (SystemException e)
            {
                WriteLog(e.ToString());

            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            return true;
        }

        /* Handles Network Packets */
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            Message msg = (Message)reader.ReadByte();

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
                case Message.SyncStats:
                    SyncStatsPacket.Read(reader);
                    break;
            }
        }

        /* Creates variables for summary and version */
        public class VersionInfo
        {
            public string summary;
            public string version;
        }

        /* Checks for updates via github */
        public static void CheckForUpdates()
        {
            try
            {
                string url = @"http://raw.githubusercontent.com/Pixel0h/Ethereal/master/Ethereal_VersionInfo.json";
                WebClient client = new WebClient();
                Version currentVersion = Instance.Version;
                client.DownloadStringCompleted += (sender, e) =>
                {
                    try
                    {
                        string text = e.Result;
                        VersionInfo versionInfo = JsonConvert.DeserializeObject<VersionInfo>(text);
                        Version latestVersion = new Version(versionInfo.version);
                        if (latestVersion > currentVersion)
                        {
                            Main.NewText("[c/cccccc:New version of] [c/ffdb00:Ethereal] [c/cccccc:available]");
                            Main.NewTextMultiline("[c/cccccc:Summary:] " + versionInfo.summary, WidthLimit: 725);
                            Main.NewText("[c/cccccc:Get the update from Mod Browser]");
                        }
                        else if (latestVersion == currentVersion && new Version(EConfig.Stats.LastStartVersion) < currentVersion)
                        {
                            Main.NewText("[c/cccccc:Ethereal is now up to date!]");
                            Main.NewTextMultiline("[c/cccccc:Summary changes:] " + versionInfo.summary, WidthLimit: 725);
                        }

                        EConfig.Stats.LastStartVersion = currentVersion.ToString();
                        EConfig.SaveStats();
                    }
                    catch
                    {

                    }
                };
                client.DownloadStringAsync(new Uri(url), url);
            }
            catch
            {

            }
        }
    }
}