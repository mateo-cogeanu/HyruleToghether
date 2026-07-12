using BOTWM.Server.DTO;

namespace BOTWM.Server.ServerClasses
{
    public class Names
    {
        public Mutex NMutex = new Mutex();

        public Dictionary<byte, string> PlayerNames;
        public List<Dictionary<byte, string>> Queue;

        public Names(int playerLimit)
        {
            PlayerNames = new Dictionary<byte, string>();
            Queue = new List<Dictionary<byte, string>>();

            for (int i = 0; i < playerLimit; i++)
                Queue.Add(new Dictionary<byte, string>());
        }

        public void AddName(byte playerNumber, string name)
        {
            NMutex.WaitOne(100);
            PlayerNames[playerNumber] = name;

            for(int i = 0; i < Queue.Count; i++)
                Queue[i][playerNumber] = name;

            NMutex.ReleaseMutex();
        }

        public void RemoveName(byte playerNumber)
        {
            NMutex.WaitOne(100);
            PlayerNames.Remove(playerNumber);

            for (int i = 0; i < Queue.Count; i++)
                Queue[i].Remove(playerNumber);

            NMutex.ReleaseMutex();
        }

        public void FillQueue(int playerNumber)
        {
            NMutex.WaitOne(100);

            foreach (KeyValuePair<byte, string> kvp in PlayerNames)
                Queue[playerNumber][kvp.Key] = kvp.Value;

            NMutex.ReleaseMutex();
        }

        public Dictionary<byte, string> GetQueue(int playerNumber)
        {
            Dictionary<byte, string> Data = new Dictionary<byte, string>();

            NMutex.WaitOne(100);

            foreach (KeyValuePair<byte, string> kvp in Queue[playerNumber])
                Data.Add(kvp.Key, kvp.Value);

            Queue[playerNumber].Clear();

            NMutex.ReleaseMutex();

            return Data;
        }

        public NamesDTO GetAllPlayers()
        {
            NMutex.WaitOne(100);

            var result = new NamesDTO()
            {
                Names = this.PlayerNames
            };

            NMutex.ReleaseMutex();

            return result;
        }

    }
}