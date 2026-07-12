using BOTWM.Server.DTO;

namespace BOTWM.Server.ServerClasses
{
    public class Enemy
    {
        const int CLEARMINUTES = 60;

        public Mutex EMutex = new Mutex();
        public bool isEnemySync;
        private DateTime LastClear;

        public Dictionary<int, int> EnemyList;
        public List<Dictionary<int, int>> Queue;

        public Enemy(int playerLimit, bool enemySync)
        {
            EnemyList = new Dictionary<int, int>();
            Queue = new List<Dictionary<int, int>>();
            for (int i = 0; i < playerLimit; i++)
                Queue.Add(new Dictionary<int, int>());
            UpdateServiceStatus(enemySync);
            LastClear = DateTime.Now;
        }

        public void UpdateServiceStatus(bool newStatus)
        {
            isEnemySync = newStatus;
        }

        public void Update(EnemyDTO userData)
        {
            double TimeSinceLastClear = DateTime.Now.Subtract(LastClear).TotalMinutes;

            if (!isEnemySync || TimeSinceLastClear > CLEARMINUTES)
            {
                ClearEnemyData();
                return;
            }

            EMutex.WaitOne(100);

            foreach (EnemyData Enemy in userData.Health)
                UpdateEnemyHealth(Enemy.Hash, Enemy.Health);

            EMutex.ReleaseMutex();
        }

        public void ClearEnemyData()
        {
            EMutex.WaitOne(100);

            EnemyList.Clear();
            for (int i = 0; i < Queue.Count; i++)
                Queue[i].Clear();

            LastClear = DateTime.Now;

            EMutex.ReleaseMutex();

            return;
        }

        public void FillQueue(int playerNumber)
        {
            EMutex.WaitOne(100);

            Queue[playerNumber].Clear();

            foreach (KeyValuePair<int, int> kvp in EnemyList)
                Queue[playerNumber].Add(kvp.Key, kvp.Value);

            EMutex.ReleaseMutex();
        }

        public List<EnemyData> GetQueue(int playerNumber)
        {
            List<EnemyData> Data = new List<EnemyData>();

            EMutex.WaitOne(100);

            foreach(KeyValuePair<int, int> kvp in Queue[playerNumber])
                Data.Add(new EnemyData(kvp.Key, kvp.Value));

            Queue[playerNumber].Clear();

            EMutex.ReleaseMutex();

            return Data;
        }

        private void UpdateEnemyHealth(int hash, int health)
        {
            if (!EnemyList.ContainsKey(hash) || (EnemyList.ContainsKey(hash) && EnemyList[hash] > health))
            {
                EnemyList[hash] = health; // TODO: Make sure that we can add new entries to dictionaries through this method

                for (int i = 0; i < Queue.Count; i++)
                    Queue[i][hash] = health;
            }
        }
    }
}
