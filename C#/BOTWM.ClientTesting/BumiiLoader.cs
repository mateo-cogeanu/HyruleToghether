using BOTWM.Server.DTO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace BOTWM.ClientTesting
{
    public static class BumiiLoader
    {
        public static Tuple<string, BumiiDTO?> readBumii(string filePath, string bumiiIOPath)
        {
            BumiiDTO? bumii = null;
            string path = "";

            string cmdOutput = runCMD(filePath, bumiiIOPath);

            try
            {
                bumii = JsonConvert.DeserializeObject<BumiiDTO>(cmdOutput);
                path = filePath;
            }
            catch(Exception ex)
            {
                throw new Exception("Error reading bumii file. Make sure the file is properly formatted.");
            }

            return new Tuple<string, BumiiDTO?>(path, bumii);
        }

        private static string runCMD(string bumiiPath, string bumiiIOPath)
        {
            ProcessStartInfo start = new ProcessStartInfo()
            {
                FileName = bumiiIOPath,
                Arguments = bumiiPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            var cmd = Process.Start(start);
            string output = cmd.StandardOutput.ReadToEnd();

            cmd.WaitForExit();

            return output;
        }
    }
}
