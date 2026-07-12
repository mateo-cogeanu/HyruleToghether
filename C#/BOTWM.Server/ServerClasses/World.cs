using BOTWM.Server.DTO;

namespace BOTWM.Server.ServerClasses
{
    public class World
    {
        public float Time;
        public int Day;
        public int Weather;

        public bool isForcedWeather;

        private Mutex WMutex = new Mutex();

        public World()
        {
            Day = -1;
            Time = -1;
            Weather = 0;
        }

        public void UpdateTime(WorldDTO userData)
        {
            WMutex.WaitOne(100);
            if(this.Day == -1 || this.Time == -1)
            {
                this.Day = 0;
                this.Time = userData.Time;
            }
            else
            {
                if(userData.Day - this.Day == 1)
                {
                    this.Day = userData.Day;
                    this.Time = userData.Time;
                }
                else if(userData.Day == this.Day && userData.Time - this.Time > 0)
                {
                    this.Day = userData.Day;
                    this.Time = userData.Time;
                }
            }

            WMutex.ReleaseMutex();
        }

        public void UpdateWeather(WorldDTO userData)
        {
            this.Weather = userData.Weather;
        }
    }
}
