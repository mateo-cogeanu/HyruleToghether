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
        public bool IsEquipped;
        public CharacterEquipment Equipment;
        public CharacterLocation Location;
        public Vec3f Bomb;
        public Vec3f Bomb2;
        public Vec3f BombCube;
        public Vec3f BombCube2;
        public byte ModelType;
        public string Model;
        public BumiiDTO MiiData;

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
        }
        
    }
}
