namespace BOTWM.Server.DataTypes
{
    public class ProjectileData
    {
        public int Id;
        public byte Type;
        public bool Active;
        public Vec3f Position = new Vec3f();
        public Quaternion Rotation = new Quaternion();
    }
}
