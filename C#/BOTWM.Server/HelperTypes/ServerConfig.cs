namespace BOTWM.Server.HelperTypes
{
    public class ServerConfig
    {

        public class ConnectionData
        {
            public string IP;
            public int Port;
            public string Password;
        }

        public class ServerInformationData
        {
            public string Description;
        }

        public class GamemodeData
        {
            public bool DefaultGamemode;
        }

        public class DefaultGamemodeData
        {
            public string Name;
            public bool EnemySync;
            public bool QuestSync;
            public bool KorokSync;
            public bool TowerSync;
            public bool ShrineSync;
            public bool LocationSync;
            public bool DungeonSync;
            public int Special;
        }

        public ConnectionData Connection;
        public ServerInformationData ServerInformation;
        public GamemodeData Gamemode;
        public DefaultGamemodeData DefaultGamemode;

        public ServerConfig(string? configPath = null)
        {
            Dictionary<string, Dictionary<string, string>> ini = ParseIni(
                configPath ?? Path.Combine(AppContext.BaseDirectory, "ServerConfig.ini"));

            foreach(var section in this.GetType().GetFields())
            {
                var field = Activator.CreateInstance(section.FieldType);

                foreach (var key in section.FieldType.GetFields())
                    key.SetValue(field, Convert.ChangeType(ini[section.Name][key.Name], key.FieldType));

                section.SetValue(this, field);
            }
        }

        private static Dictionary<string, Dictionary<string, string>> ParseIni(string path)
        {
            var result = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, string>? currentSection = null;
            foreach (string rawLine in File.ReadLines(path))
            {
                string line = rawLine.Trim().TrimStart('\uFEFF');
                if (line.Length == 0 || line.StartsWith('#') || line.StartsWith(';'))
                    continue;
                if (line.StartsWith('[') && line.EndsWith(']'))
                {
                    string sectionName = line[1..^1].Trim();
                    currentSection = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    result[sectionName] = currentSection;
                    continue;
                }

                int separator = line.IndexOf('=');
                if (separator < 0 || currentSection == null)
                    continue;
                currentSection[line[..separator].Trim()] = line[(separator + 1)..].Trim();
            }
            return result;
        }
    }
}
