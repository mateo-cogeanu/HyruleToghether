using BOTW.Logging;
using BOTWM.Server.DTO;

namespace BOTWM.Server.ServerClasses
{
    public class Models
    {
        public Mutex MMutex = new Mutex();

        public Dictionary<byte, ModelDataDTO> PlayerModels;
        public List<Dictionary<byte, ModelDataDTO>> Queue;

        public Models(int playerLimit)
        {
            PlayerModels = new Dictionary<byte, ModelDataDTO>();
            Queue = new List<Dictionary<byte, ModelDataDTO>>();

            for (int i = 0; i < playerLimit; i++)
                Queue.Add(new Dictionary<byte, ModelDataDTO>());
        }

        public void AddModel(byte playerNumber, ModelDataDTO model)
        {
            MMutex.WaitOne(100);
            PlayerModels[playerNumber] = model;

            for (int i = 0; i < Queue.Count; i++)
                Queue[i][playerNumber] = model;

            MMutex.ReleaseMutex();
        }

        public void RemoveModel(byte playerNumber)
        {
            MMutex.WaitOne(100);
            PlayerModels.Remove(playerNumber);

            for (int i = 0; i < Queue.Count; i++)
                Queue[i].Remove(playerNumber);

            MMutex.ReleaseMutex();
        }

        public void FillQueue(int playerNumber)
        {
            MMutex.WaitOne(100);

            foreach (KeyValuePair<byte, ModelDataDTO> kvp in PlayerModels)
                Queue[playerNumber][kvp.Key] = kvp.Value;

            MMutex.ReleaseMutex();
        }

        public Dictionary<byte, ModelDataDTO> GetQueue(int playerNumber)
        {
            Dictionary<byte, ModelDataDTO> Data = new Dictionary<byte, ModelDataDTO>();

            MMutex.WaitOne(100);

            int queueCount = Queue[playerNumber].Count;

            for(int i = 0; i < queueCount && i < 5; i++)
            {
                KeyValuePair<byte, ModelDataDTO> kvp = Queue[playerNumber].First();
                Data.Add(kvp.Key, kvp.Value);
                Queue[playerNumber].Remove(kvp.Key);
            }

            //foreach (KeyValuePair<byte, ModelDataDTO> kvp in Queue[playerNumber])
            //    Data.Add(kvp.Key, kvp.Value);

            //Queue[playerNumber].Clear();

            MMutex.ReleaseMutex();

            return Data;
        }

        public ModelsDTO GetAllPlayers()
        {
            MMutex.WaitOne(100);

            var result = new ModelsDTO()
            {
                Models = this.PlayerModels
            };

            MMutex.ReleaseMutex();

            return result;
        }

    }
}