namespace BOTW.Logging
{
    public static class Logger
    {
        public enum LogLevelEnum : int
        {
            INFO = 2,
            WARNING = 3,
            DEBUG = 4
        }

        public enum LogWriteLevelEnum : int
        {
            CRT = 0,
            ERR = 1,
            INF = 2,
            WRN = 3,
            DBG = 4
        }

        private static LogLevelEnum CMDLevel;
        private static LogLevelEnum LogLevel;

        private static string CurrentDirectory = ResolveLogDirectory();
        private static string LogPath = Path.Combine(CurrentDirectory, "LatestLog.txt");
        private static string LogsFolder = Path.Combine(CurrentDirectory, "Logs");

        private static string ResolveLogDirectory()
        {
            string? configured = Environment.GetEnvironmentVariable("MILKBAR_SERVER_LOG_DIR");
            return string.IsNullOrWhiteSpace(configured)
                ? AppContext.BaseDirectory
                : Path.GetFullPath(Environment.ExpandEnvironmentVariables(configured));
        }

        private static Mutex logMutex = new Mutex();

        public static void Start(LogLevelEnum logLevel, LogLevelEnum cmdLevel)
        {
            LogLevel = logLevel;
            CMDLevel = cmdLevel;

            SetupLogFile();
        }

        public static void LogCritical(string message, string details = "", ConsoleColor color = ConsoleColor.DarkRed) => WriteToLog(LogWriteLevelEnum.CRT, message, details, color);
        public static void LogError(string message, string details = "", ConsoleColor color = ConsoleColor.DarkRed) => WriteToLog(LogWriteLevelEnum.ERR, message, details, color);
        public static void LogInformation(string message, string details = "", ConsoleColor color = ConsoleColor.White) => WriteToLog(LogWriteLevelEnum.INF, message, details, color);
        public static void LogWarning(string message, string details = "", ConsoleColor color = ConsoleColor.DarkYellow) => WriteToLog(LogWriteLevelEnum.WRN, message, details, color);
        public static void LogDebug(string message, string details = "", ConsoleColor color = ConsoleColor.Magenta) => WriteToLog(LogWriteLevelEnum.DBG, message, details, color);

        public static string? LogInput(string message)
        {
            if(!string.IsNullOrEmpty(message))
                WriteToLog(LogWriteLevelEnum.INF, message, "", ConsoleColor.White, false);

            string userInput = Console.ReadLine();

            using (StreamWriter LogFile = new StreamWriter(LogPath, true))
            {
                if (userInput != null)
                    LogFile.WriteLine(userInput);
            }

            return userInput;
        }

        private static void SetupLogFile()
        {
            Directory.CreateDirectory(CurrentDirectory);
            if(File.Exists(LogPath))
            {
                string creationTime = File.GetCreationTime(LogPath).ToString("yyyy-MM-dd, HH-mm-ss");

                if (!Directory.Exists(LogsFolder))
                    Directory.CreateDirectory(LogsFolder);

                string postName = "";

                // The correct way to manage a log file already existing is by creating a differently named log file
                if(File.Exists(Path.Combine(LogsFolder, $"{creationTime}.txt")))
                    postName = $"({Directory.GetFiles(LogsFolder).Where(file => Path.GetFileNameWithoutExtension(file).StartsWith(creationTime)).Count()})";

                string archivedLog = Path.Combine(LogsFolder, $"{creationTime}{postName}.txt");
                if (!File.Exists(archivedLog))
                {
                    File.Move(LogPath, archivedLog);
                }
                else
                {
                    // If for some reason, even after naming the file differently, it fails. Here we can delete it.
                    Console.WriteLine($"Failed to create file at: {archivedLog}. Deleting it...");
                    File.Delete(LogPath);
                }
            }

            File.CreateText(LogPath).Dispose();
        }

        private static void WriteToLog(LogWriteLevelEnum writeLevel, string message, string details, ConsoleColor color, bool newLine = true, bool writeToCMD = true)
        {
            string MessageTime = DateTime.Now.ToString("HH:mm:ss");

            if(writeToCMD)
                WriteToConsole(writeLevel, message, MessageTime, color, newLine);

            if ((int)writeLevel > (int)LogLevel)
                return;

            logMutex.WaitOne(100);

            try
            {
                using (StreamWriter LogFile = new StreamWriter(LogPath, true))
                {
                    LogFile.Write($"[{MessageTime}] {message}");

                    if (!string.IsNullOrEmpty(details))
                        LogFile.Write($" - Details: {details}");

                    if (newLine)
                        LogFile.WriteLine();
                }
            }
            catch(Exception ex)
            {
                WriteToConsole(LogWriteLevelEnum.ERR, "\nCould not write to log file. Please make sure that you don't have another server process and restart your server.", MessageTime, ConsoleColor.DarkRed, true);
            }

            logMutex.ReleaseMutex();
        }

        private static void WriteToConsole(LogWriteLevelEnum writeLevel, string message, string datetime, ConsoleColor color, bool newLine)
        {
            if ((int)writeLevel > (int)CMDLevel)
                return;

            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write($"[{datetime}] ");

            Console.ForegroundColor = color;

            if (newLine)
                Console.WriteLine(message);
            else
                Console.Write(message);


            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
