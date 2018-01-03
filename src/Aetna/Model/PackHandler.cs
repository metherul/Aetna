using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Aetna.Model
{
    class PackHandler
    {
        public static ModPack ModPack { get; set; }
        public static List<Mod> ModsList { get; set; }

        public static string ModPackLocation { get; set; }

        /// <summary>
        /// Reads the targeted file for valid JSON and converts it to a ModPack object. This is saved within the PackHandler
        /// </summary>
        public static ModPack ReadPack(string modPackLocation)
        {
            ModPackLocation = modPackLocation;

            if (!File.Exists(ModPackLocation))
            {
                throw new Exception($"Modpack location not found: {ModPackLocation}");
            }

            if (Path.GetExtension(modPackLocation) == ".json")
            {
                var modPackContents = File.ReadAllText(modPackLocation);

                try
                {
                    ModPack = JsonConvert.DeserializeObject<ModPack>(modPackContents);
                    ModsList = ModPack.Mods;

                    return ModPack;
                }

                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "PARSE ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

                    return null;
                }
            }

            // Unzip the file into the temporary directory
            using (var sevenZipHandler = new SevenZipHandler())
            {
                var tempDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
                var extractedModPackName = Path.GetFileNameWithoutExtension(ModPackLocation);
                var extractedModPackPath = Path.Combine(tempDirectory, extractedModPackName);

                sevenZipHandler.ExtractArchive(modPackLocation);

                var packFileLocation = Path.Combine(extractedModPackPath, "modpack.json");

                if (!File.Exists(packFileLocation))
                {
                    return null;
                }

                var modPackContents = File.ReadAllText(packFileLocation);

                try
                {
                    ModPack = JsonConvert.DeserializeObject<ModPack>(modPackContents);
                    ModsList = ModPack.Mods;

                    return ModPack;
                }

                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        /// <summary>
        /// Writes the ModPack to a location on the drive.
        /// </summary>
        public static void WritePack(string writeLocation)
        {

        }
    }
}
