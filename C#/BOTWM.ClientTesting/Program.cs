using BOTWM.ClientTesting;
using BOTWM.Server.DataTypes;
using BOTWM.Server.DTO;
using BOTWM.Server.JSONBuilder;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

int SIZE = 7168;

byte[] info = new byte[SIZE];

Console.Write("Enter your IP: ");

string userIP = Console.ReadLine();

Console.Write("Write simulated ping in ms: ");
string sPing = Console.ReadLine();
int ping = 50;

if (sPing != "")
    Int16.Parse(sPing);

Console.Write("Copy data: ");
string sCOPY = Console.ReadLine();
bool COPY = false;

if(sCOPY == "")
    COPY = true;

Console.Write("ModelType (0 for armorSync, 1 for custom model, 2 for bumii): ");
string sModelType = Console.ReadLine();
int modelType = 1;

if (sModelType != "")
    modelType = Int16.Parse(sModelType);

BumiiDTO bumii = new BumiiDTO();
string customModel = "";

if(modelType == 1)
{
    Console.Write("Set model (empty for sidon): ");
    customModel = Console.ReadLine();

    if (customModel == "")
    {
        customModel = "Npc_Zora_Hero:Npc_Zora_Hero";
    }
    else
    {
        if(!customModel.Contains(":"))
        {
            customModel = $"{customModel}:{customModel}";
        }
    }
}
else if(modelType == 2)
{
    Console.Write("Bumii path: ");
    string bumiiPath = Console.ReadLine();

    if (bumiiPath == "")
        bumiiPath = @"D:\Mods\mpbumii\dummy.bumii";

    Console.Write("BumiiIO path: ");
    string bumiiIOPath = Console.ReadLine();

    if (bumiiIOPath == "")
        bumiiIOPath = "D:\\Mods\\mpbumii\\bumii_IO.exe";

    bumii = BumiiLoader.readBumii(bumiiPath, bumiiIOPath).Item2;
}

//Console.Write("Actor distance: ");
//string sDistance = Console.ReadLine();
//float distance = 1;

    //if(sDistance != "")
    //    distance = float.Parse(sDistance);

float distance = 0;

Console.WriteLine($"Connecting to {userIP}...");

List<Socket> Sockets = new List<Socket>();

int PlayerNumber = 1;

for (int i = 0; i < PlayerNumber; i++)
{
    IPEndPoint ip = new IPEndPoint(IPAddress.Parse(userIP), 5050);
    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    s.Connect(ip);

    Sockets.Add(s);

    ModelDataDTO model = new ModelDataDTO();

    model.ModelType = (byte)modelType;
    model.Model = customModel;
    model.Mii = bumii;

    List<byte> message = new List<byte>() { 2 };
    message.AddRange(new JSONBuilder().BuildArrayOfBytes(new ConnectDTO() { Name = $"Taca_A_Xereca_Pra_Mim{i}", Password = "NoPassword", ModelData = model }, true));
    sendServerMessage(s, message);

    string test = Encoding.UTF8.GetString(info);

    var response = JsonConvert.DeserializeObject<ConnectResponseDTO>(test);
    distance = response.PlayerNumber + 1;
}

Thread.Sleep(2000);

int count = 0;

List<EnemyData> enemyData = new List<EnemyData>();
List<string> questList = new List<string>();

Dictionary<byte, string> models = new Dictionary<byte, string>();

for (int i = 0; i < 100; i++)
{
    enemyData.Add(new EnemyData(i, i));
    questList.Add($"C{i}00");
}

ClientDTO client = new ClientDTO()
{
    WorldData = new WorldDTO() { Time = 5, Day = 2, Weather = 0 },
    PlayerData = new ClientPlayerDTO()
    {
        Position = new Vec3f((float)1743.11, (float)115.56, (float)1937.63),
        Rotation1 = new Quaternion((float)-0.21, (float)-0.98, (float)0.98, (float)-0.21),
        Rotation2 = new Quaternion((float)0.78, (float)0.63, (float)0.78, (float)0.63),
        Rotation3 = new Quaternion((float)0.5, (float)0.5, (float)0.5, (float)0.5),
        Rotation4 = new Quaternion((float)0.5, (float)0.5, (float)0.5, (float)0.5),
        Animation = 3,
        Health = 30,
        AtkUp = 5,
        IsEquipped = true,
        Equipment = new CharacterEquipment()
        {
            WType = 1,
            Sword = 70,
            Shield = 2,
            Bow = 0,
            Head = 48,
            Upper = 48,
            Lower = 48
        },
        Location = new CharacterLocation() { Map = 1, Section = 29 },
        Bomb = new Vec3f(1000, 200, 30),
        Bomb2 = new Vec3f(50, 3, 2),
        BombCube = new Vec3f(1, 2, 3),
        BombCube2 = new Vec3f(2, 3, 1)
    },
    EnemyData = new EnemyDTO() { Health = enemyData },
    QuestData = new QuestsDTO() { Completed = questList }
};

string first = "";
bool firstCopy = true;

while (true)
{

    for (int i = 0; i < PlayerNumber; i++)
    {

        List<byte> mes = new List<byte>() { 3 };

        mes.AddRange(new JSONBuilder().BuildArrayOfBytes(client, true));

        var watch = new System.Diagnostics.Stopwatch();

        watch.Start();

        if(i == 31)
        {
            var t = "a";
        }

        int meslength = sendServerMessage(Sockets[i], mes, true);

        watch.Stop();

        ServerDTO serverData = new JSONBuilder().BuildFromBytesTest(info);

        if (COPY && serverData.ClosePlayers.Count > 0)
        {
            if(firstCopy)
            {
                client.PlayerData.Position = serverData.ClosePlayers[0].Position;
                client.PlayerData.Position.z = client.PlayerData.Position.z + distance;
                client.PlayerData.Rotation1 = serverData.ClosePlayers[0].Rotation1;
                client.PlayerData.Rotation2 = serverData.ClosePlayers[0].Rotation2;
                client.PlayerData.Rotation3 = serverData.ClosePlayers[0].Rotation3;
                client.PlayerData.Rotation4 = serverData.ClosePlayers[0].Rotation4;
            }
            client.PlayerData.Animation = serverData.ClosePlayers[0].Animation;
            client.PlayerData.Health = serverData.ClosePlayers[0].Health;
            client.PlayerData.AtkUp = serverData.ClosePlayers[0].AtkUp;
            client.PlayerData.IsEquipped = serverData.ClosePlayers[0].IsEquipped;
            client.PlayerData.Equipment.WType = serverData.ClosePlayers[0].Equipment.WType;
            client.PlayerData.Equipment.Sword = serverData.ClosePlayers[0].Equipment.Sword;
            client.PlayerData.Equipment.Shield = serverData.ClosePlayers[0].Equipment.Shield;
            client.PlayerData.Equipment.Head = serverData.ClosePlayers[0].Equipment.Head;
            client.PlayerData.Equipment.Upper = serverData.ClosePlayers[0].Equipment.Upper;
            client.PlayerData.Equipment.Lower = serverData.ClosePlayers[0].Equipment.Lower;
            client.PlayerData.Bomb = CopyBombWithOffset(serverData.ClosePlayers[0].Bomb, distance);
            client.PlayerData.Bomb2 = CopyBombWithOffset(serverData.ClosePlayers[0].Bomb2, distance);
            client.PlayerData.BombCube = CopyBombWithOffset(serverData.ClosePlayers[0].BombCube, distance);
            client.PlayerData.BombCube2 = CopyBombWithOffset(serverData.ClosePlayers[0].BombCube2, distance);

            firstCopy = true;
        }
        else if(COPY && serverData.ClosePlayers.Count == 0 && serverData.FarPlayers.Count > 0)
        {
            client.PlayerData.Position = serverData.FarPlayers[0].Position;
            client.PlayerData.Position.z = client.PlayerData.Position.z + distance;
        }

        if (i == PlayerNumber - 1)
        {
            foreach(KeyValuePair<byte, ModelDataDTO> pair in serverData.ModelData.Models)
            {
                models[pair.Key] = pair.Value.Model;
            }

            Console.Clear();
            //Console.WriteLine("\n\n\n");
            Console.WriteLine($"Message length: {meslength} bytes");

            string json = JsonConvert.SerializeObject(serverData);

            if (first == "")
                first = json;

            //for (int j = 0; j < meslength; j++)
            //    Console.Write($"0x{info[j]:X2}, ");

            Console.WriteLine();

            Console.WriteLine($"Old message length: {json.Length} bytes");
            Console.WriteLine($"Time elapsed: {watch.ElapsedMilliseconds} ms");

            Console.WriteLine($"Information: {json}");

            Console.WriteLine();

            Console.WriteLine(JsonConvert.SerializeObject(models, Formatting.Indented));
        }

    }

    count++;

    //Thread.Sleep(ping);

    //Console.Write("Enter to continue... ");
    //string dummy = Console.ReadLine();

}

Vec3f CopyBombWithOffset(Vec3f original, float offset)
{
    Vec3f response = new Vec3f();

    if (response.x == 0 && response.y == 0 && response.z == 0)
        return original;

    if (response.x == -1 && response.y == -1 && response.z == -1)
        return original;

    response.x = original.x;
    response.y = original.y;
    response.z = original.z + offset;
    return response;
}

int sendServerMessage(Socket s, List<byte> data, bool header = false)
{
    while (data.Count < SIZE)
        data.Add(0);

    s.Send(data.ToArray());

    int length;

    length = s.Receive(info, 0, info.Length, 0);

    return length;
}