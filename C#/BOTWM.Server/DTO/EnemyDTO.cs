namespace BOTWM.Server.DTO
{

    public class EnemyData
    {

        public int Hash;
        public int Health;

        public EnemyData(int hash, int health)
        {
            Hash = hash;
            Health = health;
        }

    }

    public class EnemyDTO
    {
        public List<EnemyData> Health;
    }
}
