using BOTWM.Server.DataTypes;
using BOTWM.Server.DTO;

namespace BOTWM.Server.ServerClasses
{
    public class Teleport
    {
        public Mutex TMutex = new Mutex();
        public List<Vec3f> Queue = new List<Vec3f>();

        public Teleport(int playerLimit)
        {
            for (int i = 0; i < playerLimit; i++)
                Queue.Add(new Vec3f());
        }

        public void AddTp(List<int> players, Vec3f destination)
        {
            TMutex.WaitOne();
            players.ForEach(p => Queue[p] = destination);
            TMutex.ReleaseMutex();
        }

        public void ClearTp(int player)
        {
            TMutex.WaitOne();
            Queue[player] = new Vec3f();
            TMutex.ReleaseMutex();
        }

        public TeleportDTO GetTp(int player)
        {
            TeleportDTO result;

            TMutex.WaitOne();
            result = new TeleportDTO() { Destination = Queue[player] };
            Queue[player] = new Vec3f();
            TMutex.ReleaseMutex();

            return result;
        }
    }
}