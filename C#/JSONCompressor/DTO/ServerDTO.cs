using BOTWM.Server.HelperTypes;

namespace BOTWM.Server.DTO
{
    public class ServerDTO
    {
        public ServerDTO()
        {
            WorldData = new WorldDTO();
            NameData = new NamesDTO();
            ClosePlayers = new List<ClosePlayerDTO>();
            FarPlayers = new List<FarPlayerDTO>();
            EnemyData = new EnemyDTO();
            QuestData = new QuestsDTO();
            NetworkData = new NetworkDTO();
            DeathSwapData = new DeathSwapDTO();
        }

        public WorldDTO WorldData;
        public NamesDTO NameData;
        public List<ClosePlayerDTO> ClosePlayers;
        public List<FarPlayerDTO> FarPlayers;
        public EnemyDTO EnemyData;
        public QuestsDTO QuestData;
        public NetworkDTO NetworkData;
        public DeathSwapDTO DeathSwapData;
        public TeleportDTO TeleportData;
    }
}
