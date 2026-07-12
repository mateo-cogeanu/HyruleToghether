using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;

namespace GUIApp
{

    public class ServerClass
    {

        public string IP;
        int PORT;
        string PASSWORD;
        bool[] SETTINGS;
        string SERVERNAME;
        string enemySyncList;
        public string NetworkInterfaceName;
        bool serverOpen = false;
        int CLEARMINUTES = 5;

        public int SerializationRate { get; set; }
        public int TargetFPS { get; set; }
        public int SleepMultiplier { get; set; }
        public int isLocalTest { get; set; } 
        public int ischaracterSpawn { get; set; }

        Form1 mainWindow;
        Socket listen;
        Thread listenThread;
        Thread EnemyClearThread;
        List<Thread> clientThreads = new List<Thread>();

        public List<Dictionary<string, object>> serverData = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> extraData = new List<Dictionary<string, object>>();

        public Dictionary<string, int> serverEnemyData = new Dictionary<string, int>();
        List<Dictionary<string, int>> enemyDataQueue = new List<Dictionary<string, int>>();
        List<bool> AccessingEnemyData = new List<bool>() { false, false, false, false };
        List<List<bool>> Updated = new List<List<bool>>();
        bool ClearingData = false;
        DateTime lastClear;

        public List<string> serverQuestData = new List<string>();
        List<List<string>> questDataQueue = new List<List<string>>();

        public Dictionary<string, Dictionary<string, string>> animationData = new Dictionary<string, Dictionary<string, string>>();

        public ServerClass(Form1 mW)
        {

            if(mW != null) this.mainWindow = mW;
            this.animationData = readXML.readAnimationFile();

            for (int i = 0; i < 4; i++)
            {
                Dictionary<string, object> player = new Dictionary<string, object>();
                player.Add("Con", false);

                serverData.Add(player);

                Dictionary<string, int> dataQueue = new Dictionary<string, int>();
                enemyDataQueue.Add(dataQueue);

                List<string> qDataQueue = new List<string>();
                questDataQueue.Add(qDataQueue);

                Dictionary<string, object> extra = new Dictionary<string, object>();
                extraData.Add(extra);

                List<bool> updated = new List<bool>() { false, false, false, false };
                Updated.Add(updated);

            }

            this.SerializationRate = 60;
            this.TargetFPS = 60;
            this.SleepMultiplier = 1;
            this.isLocalTest = 0;
            this.ischaracterSpawn = 1;

            lastClear = DateTime.Now;

        }

        public bool serverStart(string serverName, string IP, int PORT, string PASSWORD, bool[] SETTINGS, string ESList)
        {

            this.SERVERNAME = serverName;
            this.PORT = PORT;
            this.PASSWORD = PASSWORD;
            this.SETTINGS = SETTINGS;
            this.enemySyncList = ESList;
            Dictionary<string, string> ipAddresses = new Dictionary<string, string>();

            if (IP == "localhost")
            {

                var host = Dns.GetHostEntry(Dns.GetHostName());

                foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (item.OperationalStatus == OperationalStatus.Up)
                    {
                        foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork && host.AddressList.Contains(ip.Address))
                            {
                                ipAddresses.Add(item.Name.ToString(), ip.Address.ToString());
                            }
                        }
                    }
                }

            }else
            {
                ipAddresses.Add("custom IP", IP);
            }

            foreach(var key in ipAddresses.Keys)
            {
                try
                {
                    listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.IP = ipAddresses[key].ToString();

                    IPEndPoint connect = new IPEndPoint(IPAddress.Parse(this.IP), PORT);
                    listen.Bind(connect);

                    this.NetworkInterfaceName = key;

                    if(mainWindow != null)
                    {
                        mainWindow.changeServerStatus("Server opened on " + this.NetworkInterfaceName + ".");
                    }
                    else
                    {
                        Console.WriteLine("Server opened on " + this.NetworkInterfaceName + ".");
                    }
                    serverOpen = true;

                    return true;

                }catch
                {
                    continue;
                }
            }

            return false;

        }

        public void stopServer()
        {

            serverOpen = false;
            listen.Close();

            if(listenThread != null)
                listenThread.Abort();

            foreach(Thread clientThread in clientThreads)
            {
                clientThread.Abort();
            }
        }

        public void startListen()
        {
            listenThread = new Thread(serverListen);
            listenThread.IsBackground = true;
            listenThread.Start();


        }

        public void serverListen()
        {

            while(true)
            {
                Socket connection;

                listen.Listen(4);

                connection = listen.Accept();

                var thread2 = new Thread(() => handleClient(connection));
                thread2.Start();
                clientThreads.Add(thread2);
            }

        }

        public void handleClient(Socket connection)
        {

            int SIZE = 2048;

            byte[] info = new byte[SIZE];
            string data = "";
            int array_size = 0;
            bool clientConnected = true;
            int playerNumber = 0;

            string temporaryMessage = "";
            while (serverOpen && clientConnected)
            {
                try
                {
                    array_size = connection.Receive(info, 0, info.Length, 0);
                
                    Array.Resize(ref info, array_size);
                    data = Encoding.UTF8.GetString(info);

                    if(data.Length < 18)
                    {
                        connection.Send(Encoding.UTF8.GetBytes("FW-01"));
                        continue;
                    }

                    int data_size;
                    string command;
                    
                    if(!Int32.TryParse(data.Substring(0,5), out data_size))
                    {
                        connection.Send(Encoding.UTF8.GetBytes("FW-01"));
                        continue;
                    }else
                    {

                        if (data.Length < data_size)
                        {
                            connection.Send(Encoding.UTF8.GetBytes("FW-01"));
                            continue;
                        }

                        command = data.Substring(5, 11);

                        List<string> possibleCommands = new List<string> { "!ping", "!host", "!connect", "!update", "!disconnect" };

                        bool correct = false;

                        for (int i = 0; i < possibleCommands.Count; i++)
                        {
                            if (command.Contains(possibleCommands[i]))
                            {
                                correct = true;
                            }
                        }

                        if(!correct)
                        {
                            connection.Send(Encoding.UTF8.GetBytes("FW-01"));
                            continue;
                        }

                    }

                    Dictionary<string, object> playerExtraData = new Dictionary<string, object>();

                    //playerExtraData.Add("Animation", animationName);
                    //playerExtraData.Add("Hash", animationHash);
                    playerExtraData.Add("Message", data);


                    string message;

                    try 
                    {
                        message = data.Substring(17, data_size);
                    }
                    catch (Exception e)
                    {
                        connection.Send(Encoding.UTF8.GetBytes("FW-01"));
                        continue;
                    }

                    temporaryMessage = message;

                    if (command.Contains("!ping"))
                    {

                        connection.Send(Encoding.UTF8.GetBytes(SERVERNAME + ";" + mainWindow.serverInterfaceForm.serverDescription));
                        connection.Close();
                        clientConnected = false;

                    }else if(command.Contains("!host"))
                    {
                        playerNumber = 0;
                        connection.Send(Encoding.UTF8.GetBytes("Connection successful"));
                    }
                    else if (command.Contains("!connect"))
                    {
                        int counter = 0;
                        foreach (Dictionary<string, object> item in serverData)
                        {
                            if (!(bool)item["Con"])
                            {
                                playerNumber = counter;
                                break;
                            }
                            counter++;
                        }

                        Dictionary<string, object> sendData = new Dictionary<string, object>();

                        if (playerNumber == -1)
                        {
                            sendData["Response"] = 2;
                            connection.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sendData)));
                            connection.Close();
                            clientConnected = false;
                            break;
                        }

                        string[] splittedMessage = message.Split(';');

                        if(splittedMessage[1] == this.PASSWORD || this.PASSWORD == "")
                        {
                            serverData[playerNumber]["Name"] = splittedMessage[0];
                            //serverData[playerNumber]["Con"] = true;
                        }else
                        {
                            sendData["Response"] = 3;
                            connection.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sendData)));
                            connection.Close();
                            clientConnected = false;
                            break;
                        }

                        int enemySync = SETTINGS[0] ? 1 : 0;
                        int glyphSync = SETTINGS[1] ? 1 : 0;

                        sendData["Response"] = 1;
                        sendData["playerNumber"] = playerNumber;
                        sendData["isEnemySync"] = SETTINGS[0];
                        sendData["isGlyphSync"] = SETTINGS[1];
                        sendData["isQuestSync"] = SETTINGS[2];
                        sendData["enemySyncList"] = enemySyncList;
                        sendData["serverName"] = SERVERNAME;

                        foreach(KeyValuePair<string, int> kvp in serverEnemyData)
                        {

                            enemyDataQueue[playerNumber].Add(kvp.Key, kvp.Value);

                        }

                        foreach(string item in serverQuestData)
                        {

                            questDataQueue[playerNumber].Add(item);

                        }

                        connection.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sendData)));

                    }
                    else if (command.Contains("!update"))
                    {

                        Dictionary<string, object> playerData = new Dictionary<string, object>();
                        playerData.Add("Con", true);
                        playerData.Add("Name", serverData[playerNumber]["Name"]);

                        if(message.Contains("Initial connection"))
                        {
                            serverData[playerNumber] = playerData;
                            connection.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize("Added to server")));
                            continue;

                        }

                        Dictionary<string, JsonElement> svDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(message);

                        // Player data

                        Dictionary<string, JsonElement> playerDataDict = svDict["PD"].Deserialize<Dictionary<string, JsonElement>>();

                        string animationName = "";
                        string animationHash = "";

                        foreach(KeyValuePair<string, JsonElement> pair in playerDataDict)
                        {
                            if(pair.Key == "A")
                            {

                                int[] animData = readAnimation(pair.Value.GetInt32());
                                playerData.Add("Schd", animData[0]);
                                playerData.Add("Anim", animData[1]);

                                if (animationData.ContainsKey(pair.Value.GetInt32().ToString()))
                                {
                                    animationName = animationData[pair.Value.GetInt32().ToString()]["Name"];
                                    animationHash = pair.Value.GetInt32().ToString();
                                    playerExtraData.Add("animationName", animationName);
                                    playerExtraData.Add("animationHash", animationHash);
                                }

                            }
                            else
                            {

                                if (pair.Value.ValueKind == JsonValueKind.Object)
                                {

                                    if(pair.Key != "E")
                                    {

                                        playerData.Add(pair.Key, playerDataReadMap(playerDataDict[pair.Key].Deserialize<Dictionary<string, JsonElement>>()));

                                    }else
                                    {

                                        playerData.Add(pair.Key, playerDataReadEq(playerDataDict[pair.Key].Deserialize<Dictionary<string, JsonElement>>(), playerNumber));

                                    }

                                }
                                else
                                {

                                    if (pair.Value.ValueKind == JsonValueKind.Number)
                                    {

                                        float value = (float)Math.Round(pair.Value.GetDouble(), 4);

                                        if (value % 1 == 0)
                                        {

                                            playerData.Add(pair.Key, Convert.ToInt32(value));

                                        }
                                        else
                                        {

                                            playerData.Add(pair.Key, value);

                                        }

                                    }
                                    else
                                    {

                                        playerData.Add(pair.Key, pair.Value.GetString());

                                    }

                                }

                            }

                        }

                        serverData[playerNumber] = playerData;

                        // Enemy data

                        if (playerNumber == 0)
                        {

                            double currentTime = DateTime.Now.Subtract(lastClear).TotalMinutes;

                            if (DateTime.Now.Subtract(lastClear).TotalMinutes > CLEARMINUTES)
                            {

                                ClearingData = true;

                                while (AccessingEnemyData[0] || AccessingEnemyData[1] || AccessingEnemyData[2] || AccessingEnemyData[3]) { }

                                serverEnemyData.Clear();

                                ClearingData = false;

                                lastClear = DateTime.Now;

                            }

                        }

                        if (!ClearingData)
                        {

                            AccessingEnemyData[playerNumber] = true;

                            Dictionary<string, JsonElement> enemyData = svDict["ED"].Deserialize<Dictionary<string, JsonElement>>()["H"].Deserialize<Dictionary<string, JsonElement>>();

                            foreach (KeyValuePair<string, JsonElement> kvp in enemyData)
                            {

                                if (serverEnemyData.ContainsKey(kvp.Key))
                                {

                                    if (serverEnemyData[kvp.Key] > kvp.Value.GetInt32())
                                    {

                                        serverEnemyData[kvp.Key] = kvp.Value.GetInt32();

                                        for (int i = 0; i < 4; i++)
                                        {

                                            if ((bool)serverData[i]["Con"])
                                            {

                                                if (enemyDataQueue[i].ContainsKey(kvp.Key))
                                                {
                                                    enemyDataQueue[i][kvp.Key] = kvp.Value.GetInt32();
                                                }
                                                else
                                                {
                                                    enemyDataQueue[i].Add(kvp.Key, kvp.Value.GetInt32());
                                                }

                                            }

                                        }

                                    }

                                }
                                else
                                {
                                    serverEnemyData[kvp.Key] = kvp.Value.GetInt32();

                                    for (int i = 0; i < 4; i++)
                                    {

                                        if ((bool)serverData[i]["Con"])
                                        {

                                            enemyDataQueue[i].Add(kvp.Key, kvp.Value.GetInt32());

                                        }

                                    }
                                }

                            }

                            AccessingEnemyData[playerNumber] = false;

                        }

                        // Quest data

                        JsonElement quests = svDict["QD"].Deserialize<Dictionary<string, JsonElement>>()["C"];
                        
                        for(int i = 0; i < quests.GetArrayLength(); i++)
                        {

                            if(!serverQuestData.Contains(quests[i].GetString()))
                            {

                                serverQuestData.Add(quests[i].GetString());

                                for(int j = 0; j < 4; j++)
                                {

                                    if((bool)serverData[j]["Con"])
                                    {

                                        questDataQueue[j].Add(quests[i].GetString());

                                    }

                                }

                            }

                        }

                        List<string> playerQuestQueue = new List<string>();

                        for(int i = 0; i < 100; i++)
                        {

                            if(questDataQueue[playerNumber].Count > 0)
                            {

                                playerQuestQueue.Add(questDataQueue[playerNumber][0]);
                                questDataQueue[playerNumber].RemoveAt(0);

                            }

                        }

                        for(int i = 0; i < 4; i++)
                        {

                            Updated[i][playerNumber] = true;

                        }

                        Dictionary<string, object> sendData = new Dictionary<string, object>();

                        //serverDataAnim = new List<Dictionary<string, object>>(serverData);
                        //serverDataAnim[playerNumber]["Animation"] = animationName;
                        //serverDataAnim[playerNumber]["Hash"] = animationHash;

                        Dictionary<string, int> NetworkData = new Dictionary<string, int>();
                        List<int> UpdatedData = new List<int>();

                        for (int i = 0; i < 4; i++)
                        {

                            UpdatedData.Add(Updated[playerNumber][i] ? 1 : 0);
                            Updated[playerNumber][i] = false;

                        }

                        NetworkData["SR"] = this.SerializationRate;
                        NetworkData["TFPS"] = this.TargetFPS;
                        NetworkData["SM"] = this.SleepMultiplier;
                        NetworkData["LT"] = this.isLocalTest;
                        NetworkData["CS"] = this.ischaracterSpawn;

                        extraData[playerNumber] = playerExtraData;

                        sendData.Add("PD", serverData);
                        sendData.Add("ED", enemyDataQueue[playerNumber]);
                        sendData.Add("QD", playerQuestQueue);
                        sendData.Add("ND", NetworkData);
                        sendData.Add("UD", UpdatedData);

                        connection.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sendData)));

                        enemyDataQueue[playerNumber].Clear();

                    }
                    else if(command.Contains("!disconnect"))
                    {
                        connection.Close();
                        clientConnected = false;
                        serverData[playerNumber]["Con"] = false;
                    }
                }
                catch (Exception e)
                {
                    if(mainWindow != null)
                    {
                        mainWindow.serverInterfaceForm.AppendLog(e.ToString(), Color.Black);
                    }
                    else
                    {
                        Console.WriteLine(e.ToString());
                    }
                    connection.Close();
                    clientConnected = false;
                    serverData[playerNumber]["Name"] = "";
                    serverData[playerNumber]["Con"] = false;
                    enemyDataQueue[playerNumber].Clear();
                    questDataQueue[playerNumber].Clear();
                }

            }

            Thread.CurrentThread.Join();

        }

        private List<object> playerDataReadMap(Dictionary<string, JsonElement> data)
        {

            //object[] result = new object[3];
            List<object> result = new List<object>();

            foreach(KeyValuePair<string, JsonElement> kvp in data)
            {

                if (kvp.Value.ValueKind == JsonValueKind.Number)
                {

                    float value = (float)Math.Round(kvp.Value.GetDouble(), 4);

                    if (value % 1 == 0)
                    {
                        result.Add(Convert.ToInt32(value));
                    }
                    else
                    {
                        result.Add(value);
                    }

                }
                else if (kvp.Value.ValueKind == JsonValueKind.String)
                {
                    result.Add(kvp.Value.GetString());
                }

            }

            return result;

        }

        private Dictionary<string, string> playerDataReadEq(Dictionary<string, JsonElement> data, int playerNumber)
        {

            //object[] result = new object[3];
            Dictionary<string, string> result = new Dictionary<string, string>();
            int i = 0;

            foreach (KeyValuePair<string, JsonElement> kvp in data)
            {

                //if (kvp.Value.ValueKind == JsonValueKind.Number)
                //{

                //    float value = (float)Math.Round(kvp.Value.GetDouble(), 4);

                //    if (value % 1 == 0)
                //    {
                //        result.Add(Convert.ToInt32(value));
                //    }
                //    else
                //    {
                //        result.Add(value);
                //    }

                //}
                //else if (kvp.Value.ValueKind == JsonValueKind.String)
                //{
                //    result.Add(kvp.Value.GetString());
                //}

                if(kvp.Value.ToString() != ".")
                {
                    result[kvp.Key] = kvp.Value.ToString();
                }else
                {
                    Dictionary<string, string> serverEqData = (Dictionary<string, string>)serverData[playerNumber]["E"];
                    result[kvp.Key] = serverEqData[kvp.Key];
                }

                //i++;

            }

            return result;

        }

        private int[] readAnimation(int animationHash)
        {

            string animationHashMsg = animationHash.ToString();

            int[] result = { 0, 0 };

            if (animationData.ContainsKey(animationHashMsg))
            {
                result[0] = Convert.ToInt32(animationData[animationHashMsg]["Schedule"]);
                result[1] = Convert.ToInt32(animationData[animationHashMsg]["Animation"]);
            }

            return result;
        }

    }
}
