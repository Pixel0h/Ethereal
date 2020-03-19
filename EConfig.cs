using System;
using System.IO;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;

namespace Ethereal
{
    /* Defines variables and methods used for the Ethereal config */
    public class EConfig
    {
        private static Config _configLocal = new Config();

        private static Config _configServer = new Config();

        private static ConfigStats _stats = new ConfigStats();
        public static ClientConfig ClientSide => ConfigLocal.ClientSide;

        public static string StatsPath => Main.SavePath + Path.DirectorySeparatorChar + "Ethereal_Stats.json";
        public static string ConfigPath => Main.SavePath + Path.DirectorySeparatorChar + "Ethereal_Config.json";

        public static Config ConfigLocal
        {
            get
            {
                if (_configLocal != null)
                    return _configLocal;
                _configLocal = new Config();
                LoadConfig(ConfigPath, ref _configLocal);
                return _configLocal;
            }
            private set => _configLocal = value;
        }

        public static Config ConfigServer
        {
            get => _configServer ?? (_configServer = new Config());
            private set => _configServer = value;
        }

        public static ConfigStats Stats
        {
            get
            {
                if (_stats != null)
                    return _stats;
                _stats = new ConfigStats();
                LoadConfig(StatsPath, ref _stats);

                return _stats;
            }
            private set => _stats = value;
        }

        public static void Initialize()
        {
            try
            {
                ConfigLocal = new Config();
                Stats = new ConfigStats();
                Load();
            }
            catch (SystemException e)
            {
                ModLoader.GetMod(EConstants.ModName).Logger.InfoFormat(e.ToString());
            }
        }

        public static void Load()
        {
            try
            {
                Directory.CreateDirectory(Main.SavePath);

                _configLocal = new Config();
                LoadConfig(ConfigPath, ref _configLocal);
                if (_configLocal == null) _configLocal = new Config();
                Save();

                _stats = new ConfigStats();
                LoadConfig(StatsPath, ref _stats);
                if (_stats == null) _stats = new ConfigStats();
                SaveStats();
            }
            catch (SystemException e)
            {
                ModLoader.GetMod(EConstants.ModName).Logger.InfoFormat(e.ToString());
            }
        }

        private static void LoadConfig<T>(string path, ref T config) where T : class
        {
            try
            {
                if (!File.Exists(path))
                    return;
                using (StreamReader reader = new StreamReader(path))
                {
                    config = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                }
            }
            catch (SystemException e)
            {
                ModLoader.GetMod(EConstants.ModName).Logger.InfoFormat(e.ToString());
            }
        }

        public static void Save()
        {
            try
            {
                Directory.CreateDirectory(Main.SavePath);
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(ConfigLocal, Formatting.Indented).Replace("  ", "\t"));
            }
            catch (SystemException e)
            {
                ModLoader.GetMod(EConstants.ModName).Logger.InfoFormat(e.ToString());
            }
        }

        public static void SaveStats()
        {
            Directory.CreateDirectory(Main.SavePath);
            File.WriteAllText(StatsPath, JsonConvert.SerializeObject(Stats, Formatting.Indented).Replace("  ", "\t"));
        }

        public class ClientConfig
        {
            //Client Stuff
        }

        public class Config
        {
            public ClientConfig ClientSide { get; set; } = new ClientConfig();
        }

        public class ConfigStats
        {
            public string LastStartVersion { get; set; } = "0.0.3";
        }
    }
}