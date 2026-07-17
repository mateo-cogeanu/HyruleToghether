using BOTW.Logging;
using BOTWM.Server.DataTypes;
using BOTWM.Server.DTO;
using BOTWM.Server.HelperTypes;

namespace BOTWM.Server.ServerClasses
{
    public class Player
    {
        public bool Connected;
        public byte PlayerNumber;
        public string Name;
        public Vec3f Position;
        public Quaternion Rotation1;
        public Quaternion Rotation2;
        public Quaternion Rotation3;
        public Quaternion Rotation4;
        public int Animation;
        public int Health;
        public float AtkUp;
        public byte EquipmentState;
        public CharacterEquipment Equipment;
        public CharacterLocation Location;
        public Vec3f Bomb;
        public Vec3f Bomb2;
        public Vec3f BombCube;
        public Vec3f BombCube2;
        public ProjectileData Arrow = new ProjectileData();
        public byte ModelType;
        public string Model;
        public BumiiDTO MiiData;
        private byte LastLoggedEquipmentState = byte.MaxValue;
        private int LastLoggedArrowId;
        private bool LastLoggedArrowActive;

        public Player(byte playerNumber)
        {
            Connected = false;
            this.PlayerNumber = playerNumber;
        }

        public void AssignPlayer(string name)
        {
            this.Connected = true;
            this.Name = name;
        }

        public void Update(ClientPlayerDTO userData)
        {
            this.Map(userData);
            if (LastLoggedEquipmentState != userData.EquipmentState)
            {
                LastLoggedEquipmentState = userData.EquipmentState;
                Logger.LogDebug(
                    $"Equipment relay player {PlayerNumber}: mode={EquipmentState}, " +
                    $"normalized={DescribeEquipmentMode(EquipmentState)}.");
            }
            if (userData.Arrow != null &&
                (LastLoggedArrowId != userData.Arrow.Id ||
                 LastLoggedArrowActive != userData.Arrow.Active))
            {
                LastLoggedArrowId = userData.Arrow.Id;
                LastLoggedArrowActive = userData.Arrow.Active;
                Logger.LogDebug(
                    $"Arrow relay player {PlayerNumber}: id={userData.Arrow.Id}, " +
                    $"type={userData.Arrow.Type}, active={userData.Arrow.Active}.");
            }
        }

        private static string DescribeEquipmentMode(byte mode) => mode switch
        {
            0 => "sheathed",
            1 => "legacy-held",
            2 => "melee",
            3 => "bow",
            4 => "shield",
            _ => "unknown-held"
        };
        
    }
}
