using BOTWM.Server.DataTypes;

namespace BOTWM.Server.DTO
{
    public enum PlayerStatus : byte
    {
        Close,
        Far
    }

    public class ClosePlayerDTO
    {
        public byte PlayerNumber;
        public PlayerStatus Status { get { return PlayerStatus.Close; } }
        public bool Updated;
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
    }

    public class FarPlayerDTO
    {
        public byte PlayerNumber;
        public PlayerStatus Status { get { return PlayerStatus.Far; } }
        public bool Updated;
        public Vec3f Position;
        public CharacterLocation Location;
    }

    public class ClientPlayerDTO
    {
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
    }
}