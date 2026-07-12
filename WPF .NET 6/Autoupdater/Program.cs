using Autoupdater;

Console.WriteLine("**********************************************************");
Console.WriteLine("*       Breath of the Wild Multiplayer Autoupdater       *");
Console.WriteLine("* This updater will prepare the mod installation for you *");
Console.WriteLine("*   Please don't close this installer until it is done   *");
Console.WriteLine("**********************************************************\n");

DirectoryInfo directoryInfo = new DirectoryInfo(".");

List<string> FilesToDelete = new List<string>() { "Version.txt", "Breath of the Wild Multiplayer.exe", "Breath of the Wild Multiplayer.dll", "Newtonsoft.Json.dll", "Breath of the Wild Multiplayer.deps.json", "Breath of the Wild Multiplayer.runtimeconfig.json" };
List<string> FoldersToDelete = new List<string>() { "Resources", "DedicatedServer", "BNPs" };

List<FileInfo> foundFiles = new List<FileInfo>();
List<DirectoryInfo> foundDirectories = new List<DirectoryInfo>();

int fileCount = 0;

foreach (FileInfo file in directoryInfo.GetFiles())
{
    fileCount++;

    if (file.Name.ToLower().Contains("autoupdater") || !FilesToDelete.Contains(file.Name))
        continue;

    foundFiles.Add(file);
}

foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
{
    fileCount++;

    if (FoldersToDelete.Contains(directory.Name))
        foundDirectories.Add(directory);
}

string FullServerConfig = "";
string ServerConfigFile = "";

if (foundFiles.Count + foundDirectories.Count > 0)
{
    Console.Write("Old installation found.\nAll files but the backgrounds and server configuration from your old installation will be cleared.\nBackup any file you may want to keep before continuing.\nPress enter to continue...");
    Console.Read();

    Console.Write("\nClearing old installation... ");

    foreach (FileInfo file in foundFiles)
        file.Delete();

    foreach (DirectoryInfo dir in foundDirectories)
    {
        if(dir.Name == "DedicatedServer")
        {
            FullServerConfig = dir.FullName;
            ServerConfigFile = File.ReadAllText(dir.GetFiles("ServerConfig.ini").First().FullName);
        }

        dir.Delete(true);
    }

    Console.WriteLine("Cleared.");
}
else
{
    if(fileCount > 1)
    {
        Console.Write("This autoupdater will generate files in the current folder.\nSeems like your current folder is not empty.\nTo avoid cluttering the current folder, please move the autoupdater to an empty folder.\nPress Enter to close the autoupdater...");
        Console.Read();
        Environment.Exit(1);
    }
}

Console.Write("Locating latest version... ");

(string LatestVersion, string Url) = await GithubIntegration.GetLatestVersion();

Console.WriteLine($"Version {LatestVersion} found to be the latest.");

Console.Write("Downloading latest version... ");

string downloadName = await GithubIntegration.DownloadZip(Url);

Console.WriteLine("Downloaded.");

Console.Write("Unzipping... ");

System.IO.Compression.ZipFile.ExtractToDirectory($"{downloadName}.zip", downloadName);

Console.WriteLine("Unzipped.");

Console.Write("Copying files... ");

foreach(FileInfo file in new DirectoryInfo($"{downloadName}").GetFiles())
    file.CopyTo(file.Name);

foreach (DirectoryInfo directory in new DirectoryInfo($"{downloadName}").GetDirectories())
{
    if(directory.Name != "Backgrounds")
    {
        directory.MoveTo(directory.Name);
        continue;
    }

    if(!Directory.Exists("Backgrounds"))
    {
        directory.MoveTo(directory.Name);
        continue;
    }
    
    foreach(FileInfo background in directory.GetFiles())
        if (!File.Exists($"Backgrounds\\{background.Name}"))
            background.MoveTo($"Backgrounds\\{background.Name}");
}

if(!string.IsNullOrEmpty(FullServerConfig))
    File.WriteAllText($"{FullServerConfig}\\ServerConfig.ini", ServerConfigFile);

using (StreamWriter sw = File.CreateText("Version.txt"))
    sw.Write(LatestVersion);

new DirectoryInfo(downloadName).Delete(true);
new FileInfo($"{downloadName}.zip").Delete();

Console.WriteLine("Files copied.\n");

Console.WriteLine("Installation succeded! You may now close this updater.");
Console.WriteLine("Have fun!");

Thread.Sleep(5000);