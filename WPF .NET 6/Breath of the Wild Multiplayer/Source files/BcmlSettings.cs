using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Breath_of_the_Wild_Multiplayer.Source_files
{
    public static class BcmlSettings
    {
        public static string GameDir { get => getSetting("game_dir"); }
        public static string UpdateDir { get => getSetting("update_dir"); }
        public static string CemuDir { get => getSetting("cemu_dir"); }
        public static string MergedDir { get => @$"{getSetting("store_dir")}\merged\content"; }

        private static string getSetting(string setting)
        {
            if (!File.Exists(Properties.Settings.Default.bcmlLocation))
                return null;

            string CemuSettings = File.ReadAllText(Properties.Settings.Default.bcmlLocation);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(CemuSettings)![setting];
        }
    }
}
