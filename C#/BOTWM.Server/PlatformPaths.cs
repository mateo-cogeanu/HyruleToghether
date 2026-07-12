namespace BOTWM.Server
{
    public static class PlatformPaths
    {
        public static string DataDirectory
        {
            get
            {
                string? configured = Environment.GetEnvironmentVariable("MILKBAR_DATA_DIR");
                if (!string.IsNullOrWhiteSpace(configured))
                    return Path.GetFullPath(Environment.ExpandEnvironmentVariables(configured));

                if (OperatingSystem.IsMacOS())
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                        "Library", "Application Support", "MilkBarLauncher");

                string? xdgData = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
                if (!string.IsNullOrWhiteSpace(xdgData))
                    return Path.Combine(xdgData, "MilkBarLauncher");

                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                    ".local", "share", "MilkBarLauncher");
            }
        }

        public static string DataFile(string fileName) => Path.Combine(DataDirectory, fileName);
    }
}
