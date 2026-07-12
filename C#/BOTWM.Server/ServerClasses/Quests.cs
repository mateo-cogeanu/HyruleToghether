using BOTWM.Server.DTO;

namespace BOTWM.Server.ServerClasses
{
    public class Quests
    {
        public Mutex QMutex = new Mutex();

        public bool isQuestSync;

        public List<string> ServerQuests;
        public List<List<string>> Queue = new List<List<string>>();

        public Quests(int playerLimit, bool questSync)
        {
            ServerQuests = new List<string>();

            for (int i = 0; i < playerLimit; i++)
                Queue.Add(new List<string>());

            UpdateServiceStatus(questSync);
        }

        public void UpdateServiceStatus(bool newStatus)
        {
            isQuestSync = newStatus;
        }

        public void Update(QuestsDTO userData)
        {

            if (!isQuestSync)
            {
                ClearQuests();
                return;
            }

            QMutex.WaitOne(100);

            ProcessQuests(userData);

            QMutex.ReleaseMutex();

        }

        public void ProcessQuests(QuestsDTO userData)
        {
            foreach(string Quest in userData.Completed)
            {
                if (!ServerQuests.Contains(Quest))
                {
                    ServerQuests.Add(Quest);

                    for (int i = 0; i < Queue.Count; i++)
                        Queue[i].Add(Quest);
                }

            }
        }

        public void ProcessQuests(List<string> Quests)
        {
            foreach (string Quest in Quests)
            {
                if (!ServerQuests.Contains(Quest))
                {
                    ServerQuests.Add(Quest);

                    for(int i = 0; i < Queue.Count; i++)
                        Queue[i].Add(Quest);
                }

            }
        }

        public void ClearQuests()
        {
            QMutex.WaitOne(100);

            ServerQuests.Clear();

            for (int i = 0; i < Queue.Count; i++)
            {
                Queue[i].Clear();
            }

            QMutex.ReleaseMutex();
        }

        public void FillQueue(int playerNumber)
        {
            QMutex.WaitOne(100);

            Queue[playerNumber].Clear();

            foreach(string Quest in ServerQuests)
                Queue[playerNumber].Add(Quest);

            QMutex.ReleaseMutex();
        }

        public List<string> GetQuests(int playerNumber)
        {
            QMutex.WaitOne(100);

            List<string> QuestData = new List<string>(Queue[playerNumber]);

            Queue[playerNumber].Clear();

            QMutex.ReleaseMutex();

            return QuestData;
        }

        public List<string> GetPlayerQuests(int playerNumber)
        {
            List<string> PlayerQuests = new List<string>();

            QMutex.WaitOne(100);

            for(int i = 0; i < 100; i++)
            {
                if (Queue[playerNumber].Count == 0)
                    break;

                PlayerQuests.Add(Queue[playerNumber][0]);
                Queue[playerNumber].RemoveAt(0);
            }

            QMutex.ReleaseMutex();

            return PlayerQuests;
        }
    }
}
