//using BOTWM.Server.HelperTypes;
//using System.Net;
//using System.Net.NetworkInformation;
//using System.Net.Sockets;
//using System.Text;
//using System.Text.Json;
//using static BOTWM.Server.JSONBuilder.JSONBuilder;

//namespace BOTWM.Server
//{
//    public class ServerOld
//    {

//        public string IP;
//        int PORT;
//        string PASSWORD;
//        ServerSettings SETTINGS;
//        string SERVERNAME;
//        string enemySyncList;
//        public string NetworkInterfaceName;
//        bool serverOpen = false;
//        int CLEARMINUTES = 60;

//        public string Version = "0.18.1";

//        public Mutex TimeMutex = new Mutex();
//        public Mutex EnemyMutex = new Mutex();
//        public Mutex QuestMutex = new Mutex();

//        public bool forcedWeather = false;

//        public int SerializationRate { get; set; }
//        public int TargetFPS { get; set; }
//        public int SleepMultiplier { get; set; }
//        public int isLocalTest { get; set; }
//        public int ischaracterSpawn { get; set; }
//        public int DisplayNames { get; set; }
//        public int GlyphDistance { get; set; }
//        public int GlyphTime { get; set; }
//        public bool isQuestSync { get; set; }
//        public bool isEnemySync { get; set; }

//        public bool EnemyLog { get; set; }

//        public int ClientLog { get; set; }
//        public bool ServerLog { get; set; }

//        Socket listen;
//        Thread listenThread;
//        Thread EnemyClearThread;
//        List<Thread> clientThreads = new List<Thread>();

//        public List<Dictionary<string, object>> serverData = new List<Dictionary<string, object>>();
//        public List<Dictionary<string, object>> extraData = new List<Dictionary<string, object>>();

//        public Dictionary<string, object> WorldData = new Dictionary<string, object>();
//        public Dictionary<string, int> serverEnemyData = new Dictionary<string, int>();
//        List<Dictionary<string, int>> enemyDataQueue = new List<Dictionary<string, int>>();
//        List<bool> AccessingEnemyData = new List<bool>() { false, false, false, false };
//        List<List<bool>> Updated = new List<List<bool>>();
//        bool ClearingData = false;
//        DateTime lastClear;

//        public List<string> serverQuestData = new List<string>();
//        public List<Dictionary<string, List<object>>> serverBombData = new List<Dictionary<string, List<object>>>();
//        List<List<string>> questDataQueue = new List<List<string>>();

//        public Dictionary<string, Dictionary<string, string>> animationData = new Dictionary<string, Dictionary<string, string>>();

//        private Dictionary<string, string> ArmorMappings = new Dictionary<string, string>();

//        public DeathSwapSettings DeathSwap = new DeathSwapSettings();
//        public Mutex DeathSwapMutex = new Mutex();

//        public List<DeathSwapDTO> DeathSwapQueue = new List<DeathSwapDTO>();

//        Mutex PlayerDataMutex = new Mutex();
//        Mutex EnemyDataMutex = new Mutex();
//        Mutex WorldDataMutex = new Mutex();
//        Mutex BombDataMutex = new Mutex();

//        public ServerOld()
//        {

//            this.animationData = readXML.readAnimationFile();
//            this.ArmorMappings = ReadArmorMappingJson();

//            for (int i = 0; i < 4; i++)
//            {
//                Dictionary<string, object> player = new Dictionary<string, object>();
//                player.Add("Con", false);

//                serverData.Add(player);

//                Dictionary<string, int> dataQueue = new Dictionary<string, int>();
//                enemyDataQueue.Add(dataQueue);

//                List<string> qDataQueue = new List<string>();
//                questDataQueue.Add(qDataQueue);

//                Dictionary<string, object> extra = new Dictionary<string, object>();
//                extraData.Add(extra);

//                List<bool> updated = new List<bool>() { false, false, false, false };
//                Updated.Add(updated);

//                Dictionary<string, List<object>> bomb = new Dictionary<string, List<object>>();
//                serverBombData.Add(bomb);

//                DeathSwapQueue.Add(new DeathSwapDTO() { Phase = 0, Position = new List<float>() { 0, 0, 0 } });

//            }

//            WorldData.Add("T", (float)-1);
//            WorldData.Add("D", -1);
//            WorldData.Add("W", 0);

//            this.SerializationRate = 60;
//            this.TargetFPS = 60;
//            this.SleepMultiplier = 1;
//            this.isLocalTest = 0;
//            this.ischaracterSpawn = 1;
//            this.DisplayNames = 1;
//            this.GlyphDistance = 250;
//            this.GlyphTime = 60;
//            this.isQuestSync = false;
//            this.isEnemySync = false;

//            lastClear = DateTime.Now;
//            ClientLog = 0;

//        }

//        //public bool serverStart(string serverName, string IP, int PORT, string PASSWORD, bool[] SETTINGS, string ESList)
//        public bool serverStart(string serverName, string IP, int PORT, string PASSWORD, ServerSettings SETTINGS, string ESList)
//        {

//            this.SERVERNAME = serverName;
//            this.PORT = PORT;
//            this.PASSWORD = PASSWORD;
//            this.SETTINGS = SETTINGS;
//            this.enemySyncList = ESList;

//            //this.isEnemySync = SETTINGS[0];

//            //for(int i = 2; i < 9; i++)
//            //{
//            //    if (SETTINGS[i])
//            //    {
//            //        isQuestSync = true;
//            //        break;
//            //    }
//            //}

//            isQuestSync = SETTINGS.QuestSyncSettings.AnyTrue;

//            Dictionary<string, string> ipAddresses = new Dictionary<string, string>();

//            if (IP == "localhost")
//            {

//                var host = Dns.GetHostEntry(Dns.GetHostName());

//                foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
//                {
//                    if (item.OperationalStatus == OperationalStatus.Up)
//                    {
//                        foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
//                        {
//                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork && host.AddressList.Contains(ip.Address))
//                            {
//                                ipAddresses.Add(item.Name.ToString(), ip.Address.ToString());
//                            }
//                        }
//                    }
//                }

//            }
//            else
//            {
//                ipAddresses.Add("custom IP", IP);
//            }

//            foreach (var key in ipAddresses.Keys)
//            {
//                try
//                {
//                    listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//                    this.IP = ipAddresses[key].ToString();

//                    IPEndPoint connect = new IPEndPoint(IPAddress.Parse(this.IP), PORT);
//                    listen.Bind(connect);

//                    this.NetworkInterfaceName = key;

//                    LogInfo("Server opened on " + this.NetworkInterfaceName + ".");
//                    serverOpen = true;

//                    return true;

//                }
//                catch
//                {
//                    continue;
//                }
//            }

//            return false;

//        }

//        public void stopServer()
//        {

//            serverOpen = false;
//            listen.Close();

//            if (listenThread != null)
//                listenThread.Abort();

//            foreach (Thread clientThread in clientThreads)
//            {
//                clientThread.Abort();
//            }
//        }

//        public void startListen()
//        {
//            listenThread = new Thread(serverListen);
//            listenThread.IsBackground = true;
//            listenThread.Start();
//        }

//        public void serverListen()
//        {
//            while (true)
//            {
//                Socket connection;

//                listen.Listen(10);

//                connection = listen.Accept();

//                var thread2 = new Thread(() => handleClient(connection));
//                thread2.Start();
//                clientThreads.Add(thread2);
//            }

//        }

//        public void handleClient(Socket connection)
//        {

//            int SIZE = 2048;

//            byte[] info = new byte[SIZE];
//            string data = "";
//            //string appendable = "";
//            List<byte> appendable = new List<byte>();
//            short messageSize = 0;
//            int array_size = 0;
//            bool clientConnected = true;
//            int playerNumber = -1;

//            string temporaryMessage = "";
//            string lastMessage = "";
//            while (serverOpen && clientConnected)
//            {

//                try
//                {

//                    array_size = connection.Receive(info, 0, info.Length, 0);

//                    Array.Resize(ref info, array_size);

//                    //appendable += Encoding.UTF8.GetString(info).Replace("\0", "");
//                    appendable.AddRange(info);

//                    //if(appendable.Contains("END"))
//                    //{
//                    //    data = appendable.Substring(0, appendable.Length - 3);
//                    //    appendable = "";
//                    //}else
//                    //{
//                    //    continue;
//                    //}

//                    if(appendable.Count != SIZE)
//                    {
//                        continue;
//                    }

//                    foreach(byte b in appendable)
//                    {
//                        LogInfo($"{b:X2}");
//                    }

//                    while (true)
//                    {

//                    }

//                    Tuple<MessageType, object> test =  BuildFromBytes(appendable.ToArray());

//                    LogInfo(test.Item1.ToString());

//                    //lastMessage = appendable;

//                    if (ClientLog == playerNumber + 1 && playerNumber > 0)
//                    {
//                        LogInfo($"Client: {data}");
//                        ClientLog = 0;
//                    }

//                    if (data.Length < 18)
//                    {
//                        connection.Send(Encoding.UTF8.GetBytes("FW-01"));
//                        //LogInfo($"Jugador {playerNumber + 1} got package lost. {data}");
//                        continue;
//                    }

//                    int data_size;
//                    string command;

//                    if (!Int32.TryParse(data.Substring(0, 5), out data_size))
//                    {
//                        connection.Send(Encoding.UTF8.GetBytes("FW-02"));
//                        //LogInfo($"Jugador {playerNumber + 1} got package lost. {data}");
                        
//                        continue;
//                    }
//                    else
//                    {

//                        if (data.Length < data_size)
//                        {
//                            connection.Send(Encoding.UTF8.GetBytes("FW-03"));
//                            //LogInfo($"Jugador {playerNumber + 1} got package lost. {data}");
                            
//                            continue;
//                        }

//                        command = data.Substring(5, 11);

//                        List<string> possibleCommands = new List<string> { "!ping", "!host", "!connect", "!update", "!disconnect" };

//                        bool correct = false;

//                        for (int i = 0; i < possibleCommands.Count; i++)
//                        {
//                            if (command.Contains(possibleCommands[i]))
//                            {
//                                correct = true;
//                            }
//                        }
                        
//                        if (!correct)
//                        {
//                            connection.Send(Encoding.UTF8.GetBytes("FW-04"));
//                            //LogInfo($"Jugador {playerNumber + 1} got package lost. {data}");
                            
//                            continue;
//                        }

//                    }

//                    Dictionary<string, object> playerExtraData = new Dictionary<string, object>();

//                    //playerExtraData.Add("Animation", animationName);
//                    //playerExtraData.Add("Hash", animationHash);
//                    playerExtraData.Add("Message", data);

//                    string message;

//                    try
//                    {
//                        message = data.Substring(17, data_size);
//                    }
//                    catch (Exception e)
//                    {
//                        connection.Send(Encoding.UTF8.GetBytes("FW-05"));
//                        //LogInfo($"Jugador {playerNumber + 1} got package lost. {data}");
                        
//                        continue;
//                    }

//                    temporaryMessage = message;

//                    if (command.Contains("!ping"))
//                    {

//                        connection.Send(Encoding.UTF8.GetBytes(""));
//                        connection.Close();
//                        clientConnected = false;

//                    }
//                    else if (command.Contains("!host"))
//                    {
//                        playerNumber = 0;
//                        connection.Send(Encoding.UTF8.GetBytes("Connection successful"));
//                    }
//                    else if (command.Contains("!connect"))
//                    {
//                        int counter = 0;
//                        foreach (Dictionary<string, object> item in serverData)
//                        {
//                            if (!(bool)item["Con"])
//                            {
//                                playerNumber = counter;
//                                break;
//                            }
//                            counter++;
//                        }

//                        Dictionary<string, object> sendData = new Dictionary<string, object>();

//                        if (playerNumber == -1)
//                        {
//                            sendData["Response"] = 2;
//                            connection.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sendData)));
//                            connection.Close();
//                            clientConnected = false;
//                            break;
//                        }
                        
//                        string[] splittedMessage = message.Split(';');

//                        if (splittedMessage[1] == this.PASSWORD || this.PASSWORD == "")
//                        {
//                            serverData[playerNumber]["Name"] = splittedMessage[0];
//                            //serverData[playerNumber]["Con"] = true;
//                        }
//                        else
//                        {
//                            sendData["Response"] = 3;
//                            connection.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sendData)));
//                            connection.Close();
//                            clientConnected = false;
//                            break;
//                        }

//                        //V | K | T | O | C | L | D

//                        sendData["Response"] = 1;
//                        sendData["playerNumber"] = playerNumber;
//                        //sendData["isEnemySync"] = SETTINGS[0];
//                        //sendData["isGlyphSync"] = SETTINGS[1];
//                        //sendData["V"] = SETTINGS[2];
//                        //sendData["K"] = SETTINGS[3];
//                        //sendData["T"] = SETTINGS[4];
//                        //sendData["S"] = SETTINGS[5];
//                        //sendData["L"] = SETTINGS[6];
//                        //sendData["D"] = SETTINGS[7];
//                        //sendData["H"] = SETTINGS[8];
//                        sendData["SETTINGS"] = SETTINGS;
//                        sendData["QS"] = isQuestSync;
//                        sendData["enemySyncList"] = enemySyncList;
//                        sendData["serverName"] = SERVERNAME;

//                        foreach (KeyValuePair<string, int> kvp in serverEnemyData)
//                        {

//                            enemyDataQueue[playerNumber].Add(kvp.Key, kvp.Value);

//                        }

//                        foreach (string item in serverQuestData)
//                        {

//                            questDataQueue[playerNumber].Add(item);

//                        }

//                        connection.Send(Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(sendData)));

//                        if(ServerLog)
//                        {
//                            LogInfo($"Server: {Newtonsoft.Json.JsonConvert.SerializeObject(sendData)}");
//                            ServerLog = false;
//                        }

//                    }
//                    else if (command.Contains("!update"))
//                    {

//                        Dictionary<string, object> playerData = new Dictionary<string, object>();
//                        playerData.Add("Con", true);
//                        playerData.Add("Name", serverData[playerNumber]["Name"]);

//                        if (message.Contains("Initial connection"))
//                        {
//                            serverData[playerNumber] = playerData;
//                            connection.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize("Added to server")));
//                            ConsoleColor color = ConsoleColor.White;

//                            switch(playerNumber)
//                            {
//                                case 0:
//                                    color = ConsoleColor.Green;
//                                    break;
//                                case 1:
//                                    color = ConsoleColor.Red;
//                                    break;
//                                case 2:
//                                    color = ConsoleColor.Blue;
//                                    break;
//                                case 3:
//                                    color = ConsoleColor.Magenta;
//                                    break;
//                            }

//                            LogInfo($"{serverData[playerNumber]["Name"]} connected", color);
                            
//                            continue;
//                        }

//                        Dictionary<string, JsonElement> svDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(message);

//                        //World data

//                        Dictionary<string, JsonElement> WorldDataDict = svDict["WD"].Deserialize<Dictionary<string, JsonElement>>();

//                        TimeMutex.WaitOne(100);

//                        updateWorldTime((float)Math.Round(WorldDataDict["T"].GetDouble(), 4), WorldDataDict["D"].GetInt32(), WorldDataDict["W"].GetInt32(), playerNumber);

//                        TimeMutex.ReleaseMutex();

//                        // Bomb data

//                        Dictionary<string, JsonElement> BombDataDict = svDict["BD"].Deserialize<Dictionary<string, JsonElement>>();

//                        Dictionary<string, List<object>> BombData = new Dictionary<string, List<object>>();

//                        foreach (KeyValuePair<string, JsonElement> kv in BombDataDict)
//                        {

//                            if (kv.Key == "NA") continue;

//                            BombData.Add(kv.Key, playerDataReadMap(kv.Value.Deserialize<Dictionary<string, JsonElement>>()));

//                        }

//                        serverBombData[playerNumber] = BombData;

//                        // Player data

//                        Dictionary<string, JsonElement> playerDataDict = svDict["PD"].Deserialize<Dictionary<string, JsonElement>>();

//                        string animationName = "";
//                        string animationHash = "";

//                        foreach (KeyValuePair<string, JsonElement> pair in playerDataDict)
//                        {
//                            if (pair.Key == "A")
//                            {

//                                int[] animData = readAnimation(pair.Value.GetInt32());
//                                playerData.Add("Schd", animData[0]);
//                                playerData.Add("Anim", animData[1]);

//                                if (animationData.ContainsKey(pair.Value.GetInt32().ToString()))
//                                {
//                                    animationName = animationData[pair.Value.GetInt32().ToString()]["Name"];
//                                    animationHash = pair.Value.GetInt32().ToString();
//                                    playerExtraData.Add("animationName", animationName);
//                                    playerExtraData.Add("animationHash", animationHash);
//                                }

//                            }
//                            else
//                            {

//                                if (pair.Value.ValueKind == JsonValueKind.Object)
//                                {

//                                    if (pair.Key != "E")
//                                    {

//                                        playerData.Add(pair.Key, playerDataReadMap(playerDataDict[pair.Key].Deserialize<Dictionary<string, JsonElement>>()));

//                                    }
//                                    else
//                                    {

//                                        playerData.Add(pair.Key, playerDataReadEq(playerDataDict[pair.Key].Deserialize<Dictionary<string, JsonElement>>(), playerNumber));

//                                    }

//                                }
//                                else
//                                {

//                                    if (pair.Value.ValueKind == JsonValueKind.Number)
//                                    {

//                                        float value = (float)Math.Round(pair.Value.GetDouble(), 4);

//                                        if (value % 1 == 0)
//                                        {

//                                            playerData.Add(pair.Key, Convert.ToInt32(value));

//                                        }
//                                        else
//                                        {

//                                            playerData.Add(pair.Key, value);

//                                        }

//                                    }
//                                    else
//                                    {

//                                        playerData.Add(pair.Key, pair.Value.GetString());

//                                    }

//                                }

//                            }

//                        }

//                        serverData[playerNumber] = playerData;

//                        // Enemy data

//                        if(!isEnemySync)
//                        {

//                            EnemyMutex.WaitOne(100);

//                            serverEnemyData.Clear();

//                            for(int i = 0; i < 4; i++)
//                            {
//                                enemyDataQueue[i].Clear();
//                            }

//                            EnemyMutex.ReleaseMutex();

//                            lastClear = DateTime.Now;
//                        }
//                        else
//                        {

//                            if (playerNumber == 0)
//                            {

//                                double currentTime = DateTime.Now.Subtract(lastClear).TotalMinutes;

//                                if (DateTime.Now.Subtract(lastClear).TotalMinutes > CLEARMINUTES)
//                                {

//                                    //ClearingData = true;

//                                    //while (AccessingEnemyData[0] || AccessingEnemyData[1] || AccessingEnemyData[2] || AccessingEnemyData[3]) { }

//                                    EnemyMutex.WaitOne(100);

//                                    serverEnemyData.Clear();

//                                    EnemyMutex.ReleaseMutex();

//                                    LogInfo($"[Enemy Log] Cleared enemy data", ConsoleColor.Cyan);

//                                    //ClearingData = false;

//                                    lastClear = DateTime.Now;

//                                }

//                            }

//                            EnemyMutex.WaitOne(100);

//                            Dictionary<string, JsonElement> enemyData = svDict["ED"].Deserialize<Dictionary<string, JsonElement>>()["H"].Deserialize<Dictionary<string, JsonElement>>();

//                            foreach (KeyValuePair<string, JsonElement> kvp in enemyData)
//                            {

//                                if (serverEnemyData.ContainsKey(kvp.Key))
//                                {

//                                    if (serverEnemyData[kvp.Key] > kvp.Value.GetInt32())
//                                    {

//                                        serverEnemyData[kvp.Key] = kvp.Value.GetInt32();

//                                        if (EnemyLog) LogInfo($"[Enemy Log] {kvp.Key} lost health. New health is {serverEnemyData[kvp.Key]}", ConsoleColor.DarkRed);

//                                        for (int i = 0; i < 4; i++)
//                                        {

//                                            if ((bool)serverData[i]["Con"])
//                                            {

//                                                if (enemyDataQueue[i].ContainsKey(kvp.Key))
//                                                {
//                                                    enemyDataQueue[i][kvp.Key] = kvp.Value.GetInt32();
//                                                }
//                                                else
//                                                {
//                                                    enemyDataQueue[i].Add(kvp.Key, kvp.Value.GetInt32());
//                                                }

//                                            }

//                                        }

//                                    }

//                                }
//                                else
//                                {
//                                    serverEnemyData[kvp.Key] = kvp.Value.GetInt32();

//                                    for (int i = 0; i < 4; i++)
//                                    {

//                                        if ((bool)serverData[i]["Con"])
//                                        {

//                                            if (enemyDataQueue[i].ContainsKey(kvp.Key))
//                                            {
//                                                enemyDataQueue[i][kvp.Key] = kvp.Value.GetInt32();
//                                            }
//                                            else
//                                            {
//                                                enemyDataQueue[i].Add(kvp.Key, kvp.Value.GetInt32());
//                                            }

//                                        }

//                                    }
//                                }

//                            }

//                            EnemyMutex.ReleaseMutex();

//                        }

//                        // Quest data

//                        List<string> playerQuestQueue = new List<string>();

//                        if (!isQuestSync)
//                        {

//                            QuestMutex.WaitOne(100);

//                            serverQuestData.Clear();

//                            for(int i = 0; i < 4; i++)
//                            {
//                                questDataQueue[i].Clear();
//                            }

//                            QuestMutex.ReleaseMutex();

//                        }
//                        else
//                        {

//                            QuestMutex.WaitOne(100);

//                            JsonElement quests = svDict["QD"].Deserialize<Dictionary<string, JsonElement>>()["C"];

//                            //for (int i = 0; i < quests.GetArrayLength(); i++)
//                            //{

//                            //    if (!serverQuestData.Contains(quests[i].GetString()))
//                            //    {

//                            //        serverQuestData.Add(quests[i].GetString());

//                            //        for (int j = 0; j < 4; j++)
//                            //        {

//                            //            if ((bool)serverData[j]["Con"])
//                            //            {

//                            //                questDataQueue[j].Add(quests[i].GetString());

//                            //            }

//                            //        }

//                            //    }

//                            //}

//                            ProcessQuests(quests);

//                            for (int i = 0; i < 100; i++)
//                            {

//                                if (questDataQueue[playerNumber].Count > 0)
//                                {

//                                    playerQuestQueue.Add(questDataQueue[playerNumber][0]);
//                                    questDataQueue[playerNumber].RemoveAt(0);

//                                }

//                            }

//                            QuestMutex.ReleaseMutex();

//                        }

//                        for (int i = 0; i < 4; i++)
//                        {

//                            Updated[i][playerNumber] = true;

//                        }

//                        Dictionary<string, object> sendData = new Dictionary<string, object>();

//                        Dictionary<string, int> NetworkData = new Dictionary<string, int>();
//                        List<int> UpdatedData = new List<int>();

//                        for (int i = 0; i < 4; i++)
//                        {

//                            UpdatedData.Add(Updated[playerNumber][i] ? 1 : 0);
//                            Updated[playerNumber][i] = false;

//                        }

//                        NetworkData["SR"] = this.SerializationRate;
//                        NetworkData["TFPS"] = this.TargetFPS;
//                        NetworkData["SM"] = this.SleepMultiplier;
//                        NetworkData["LT"] = this.isLocalTest;
//                        NetworkData["CS"] = this.ischaracterSpawn;
//                        NetworkData["DN"] = this.DisplayNames;
//                        NetworkData["GT"] = this.GlyphTime;
//                        NetworkData["GD"] = this.GlyphDistance;
//                        NetworkData["QS"] = this.isQuestSync ? 1 : 0;
//                        NetworkData["ES"] = this.isEnemySync ? 1 : 0;

//                        extraData[playerNumber] = playerExtraData;

//                        //sendData.Add("PD", serverData);
//                        //sendData.Add("ED", enemyDataQueue[playerNumber]);
//                        //sendData.Add("QD", playerQuestQueue);
//                        //sendData.Add("ND", NetworkData);
//                        //sendData.Add("UD", UpdatedData);
//                        //sendData.Add("WD", WorldData);
//                        //sendData.Add("BD", serverBombData);

//                        sendData.Add("PD", new List<Dictionary<string, object>>(serverData));
//                        sendData.Add("ED", new Dictionary<string, int>(enemyDataQueue[playerNumber]));
//                        sendData.Add("QD", new List<string>(playerQuestQueue));
//                        sendData.Add("ND", new Dictionary<string, int>(NetworkData));
//                        sendData.Add("UD", new List<int>(UpdatedData));
//                        sendData.Add("WD", new Dictionary<string, object>(WorldData));
//                        sendData.Add("BD", new List<Dictionary<string, List<object>>>(serverBombData));

//                        if (SETTINGS.GameMode == Gamemode.DeathSwap && playerNumber == 0)
//                        {

//                            DeathSwapMutex.WaitOne(100);

//                            if(DeathSwap.Enabled && (bool)serverData[1]["Con"])
//                            {

//                                byte NewDeathSwapPhase = DeathSwap.GetSwapPhase();
                                
//                                for(int i = 0; i < 4; i++)
//                                {

//                                    if (DeathSwapQueue[i].Phase != 2)
//                                        DeathSwapQueue[i].Phase = NewDeathSwapPhase;

//                                }

//                                if (NewDeathSwapPhase == 2)
//                                {

//                                    List<float> NewPos1 = new List<float>() { Convert.ToSingle(((List<object>)serverData[1]["P"])[0]), Convert.ToSingle(((List<object>)serverData[1]["P"])[1]), Convert.ToSingle((float)((List<object>)serverData[1]["P"])[2]) };
//                                    List<float> NewPos2 = new List<float>() { Convert.ToSingle(((List<object>)serverData[0]["P"])[0]), Convert.ToSingle(((List<object>)serverData[0]["P"])[1]), Convert.ToSingle((float)((List<object>)serverData[0]["P"])[2]) };
                                    
//                                    DeathSwapQueue[0].Position = NewPos1;

//                                    DeathSwapQueue[1].Position = NewPos2;

//                                }

//                            }
//                            else
//                            {
//                                DeathSwap.Running = false;
//                                DeathSwap.RestartTimer();
//                            }

//                            DeathSwapMutex.ReleaseMutex();

//                        }

//                        DeathSwapMutex.WaitOne(100);

//                        //sendData.Add("SWAP", DeathSwapPhaseQueue[playerNumber]);
//                        sendData.Add("SWAP", DeathSwapQueue[playerNumber]);

//                        //DeathSwapPhaseQueue[playerNumber] = 0;

//                        DeathSwapMutex.ReleaseMutex();

//                        //connection.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sendData)));

//                        connection.Send(Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(sendData)));

//                        if (DeathSwapQueue[playerNumber].Phase == 2)
//                            DeathSwapQueue[playerNumber].Phase = 0;

//                        if (ServerLog)
//                        {
//                            LogInfo($"Server: {JsonSerializer.Serialize(sendData)}");
//                            ServerLog = false;
//                        }

//                        enemyDataQueue[playerNumber].Clear();

//                    }
//                    else if (command.Contains("!disconnect"))
//                    {
//                        connection.Close();
//                        clientConnected = false;
//                        serverData[playerNumber]["Con"] = false;
//                    }
//                }
//                catch (Exception e)
//                {

//                    ConsoleColor color = ConsoleColor.White;

//                    switch (playerNumber)
//                    {
//                        case 0:
//                            color = ConsoleColor.Green;
//                            break;
//                        case 1:
//                            color = ConsoleColor.Red;
//                            break;
//                        case 2:
//                            color = ConsoleColor.Blue;
//                            break;
//                        case 3:
//                            color = ConsoleColor.Magenta;
//                            break;
//                    }

//                    LogInfo($"{serverData[playerNumber]["Name"]} disconnected because {e}", color);
//                    connection.Close();
//                    clientConnected = false;
//                    serverData[playerNumber]["Name"] = "";
//                    serverData[playerNumber]["Con"] = false;
//                    enemyDataQueue[playerNumber].Clear();
//                    questDataQueue[playerNumber].Clear();
//                }

//            }

//            Thread.CurrentThread.Join();

//        }

//        private List<object> playerDataReadMap(Dictionary<string, JsonElement> data)
//        {

//            //object[] result = new object[3];
//            List<object> result = new List<object>();

//            foreach (KeyValuePair<string, JsonElement> kvp in data)
//            {

//                if (kvp.Value.ValueKind == JsonValueKind.Number)
//                {

//                    float value = (float)Math.Round(kvp.Value.GetDouble(), 4);

//                    if (value % 1 == 0)
//                    {
//                        result.Add(Convert.ToInt32(value));
//                    }
//                    else
//                    {
//                        result.Add(value);
//                    }

//                }
//                else if (kvp.Value.ValueKind == JsonValueKind.String)
//                {
//                    result.Add(kvp.Value.GetString());
//                }

//            }

//            return result;

//        }

//        public void ProcessQuests(JsonElement quests)
//        {

//            for (int i = 0; i < quests.GetArrayLength(); i++)
//            {

//                if (!serverQuestData.Contains(quests[i].GetString()))
//                {

//                    serverQuestData.Add(quests[i].GetString());

//                    for (int j = 0; j < 4; j++)
//                    {

//                        if ((bool)serverData[j]["Con"])
//                        {

//                            questDataQueue[j].Add(quests[i].GetString());

//                        }

//                    }

//                }

//            }

//        }

//        private Dictionary<string, string> playerDataReadEq(Dictionary<string, JsonElement> data, int playerNumber)
//        {

//            //object[] result = new object[3];
//            Dictionary<string, string> result = new Dictionary<string, string>();
//            int i = 0;

//            foreach (KeyValuePair<string, JsonElement> kvp in data)
//            {

//                //if (kvp.Value.ValueKind == JsonValueKind.Number)
//                //{

//                //    float value = (float)Math.Round(kvp.Value.GetDouble(), 4);

//                //    if (value % 1 == 0)
//                //    {
//                //        result.Add(Convert.ToInt32(value));
//                //    }
//                //    else
//                //    {
//                //        result.Add(value);
//                //    }

//                //}
//                //else if (kvp.Value.ValueKind == JsonValueKind.String)
//                //{
//                //    result.Add(kvp.Value.GetString());
//                //}

//                if (kvp.Value.ToString() != ".")
//                {

//                    string ArmorToWrite = kvp.Value.ToString();

//                    Dictionary<string, string> DefaultArmor = new Dictionary<string, string>() { { "H", "Armor_Default_Head"}, { "U", "Armor_Default_Upper" }, { "L", "Armor_Default_Lower" } };

//                    List<string> ArmorParts = new List<string>() { "H", "U", "L" };
//                    if(ArmorParts.Contains(kvp.Key))
//                    {

//                        if (kvp.Value.ToString() == "")
//                        {

//                            ArmorToWrite = DefaultArmor[kvp.Key];

//                        }

//                        ArmorToWrite = ProcessArmor(ArmorToWrite);
//                    }
//                    result[kvp.Key] = ArmorToWrite;
//                }
//                else
//                {
//                    Dictionary<string, string> serverEqData = (Dictionary<string, string>)serverData[playerNumber]["E"];
//                    result[kvp.Key] = serverEqData[kvp.Key];
//                }




//                //i++;

//            }

//            return result;

//        }

//        private Dictionary<string, string> ReadArmorMappingJson()
//        {

//            string AppdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BOTWM";

//            string ArmorMappingJson = File.ReadAllText(AppdataFolder + "\\ArmorMapping.txt");

//            return JsonSerializer.Deserialize<Dictionary<string, string>>(ArmorMappingJson);

//        }

//        private string ProcessArmor(string ArmorValue)
//        {

//            string ArmorIndex = ArmorValue.Substring(6, 3);
//            string newArmorIndex = ArmorIndex;

//            if(ArmorMappings.Keys.Contains(ArmorIndex))
//            {

//                newArmorIndex = ArmorMappings[ArmorIndex];

//            }

//            return ArmorValue.Replace(ArmorIndex, newArmorIndex);

//        }

//        private int[] readAnimation(int animationHash)
//        {

//            string animationHashMsg = animationHash.ToString();

//            int[] result = { 0, 0 };

//            if (animationData.ContainsKey(animationHashMsg))
//            {
//                result[0] = Convert.ToInt32(animationData[animationHashMsg]["Schedule"]);
//                result[1] = Convert.ToInt32(animationData[animationHashMsg]["Animation"]);
//            }

//            return result;
//        }

//        public virtual void LogInfo(string message, ConsoleColor color = ConsoleColor.White)
//        {
//            Console.ForegroundColor = ConsoleColor.Gray;

//            Console.Write($"[{DateTime.Now.ToString("HH:mm:ss")}] ");

//            Console.ForegroundColor = color;

//            Console.WriteLine($"{message}");

//            Console.ForegroundColor = ConsoleColor.White;
//        }

//        public void updateWorldTime(float time, int day, int Weather, int playerNumber)
//        {

//            float WorldTime = (float)WorldData["T"];
//            int WorldDay = (int)WorldData["D"];
//            int WorldWeather = (int)WorldData["W"];

//            if(WorldDay == -1 || WorldTime == -1)
//            {
//                WorldData["D"] = 0;
//                WorldData["T"] = time;
//            }
//            else
//            {

//                if(day - WorldDay == 1)
//                {
//                    WorldData["D"] = day;
//                    WorldData["T"] = time;
//                }
//                else if(day == WorldDay && time - WorldTime > 0)
//                {
//                    WorldData["D"] = day;
//                    WorldData["T"] = time;
//                }

//            }

//            if(playerNumber != -1 && forcedWeather)
//            {
//                return;
//            }

//            for(int i = playerNumber - 1; i >= 0; i--)
//            {

//                if ((bool)serverData[i]["Con"])
//                {
//                    return;
//                }

//            }

//            WorldData["W"] = Weather;

//        }

//    }
//}