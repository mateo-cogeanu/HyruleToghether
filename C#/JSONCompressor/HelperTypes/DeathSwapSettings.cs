using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTWM.Server.HelperTypes
{
    public class DeathSwapSettings
    {

        public class Limit
        {
            public int Lower;
            public int Upper;
            public int Value;
            public bool random = true;
        }

        public bool Enabled = false;
        public bool Running = false;
        public DateTime LastTime;
        public Limit TimerLimit;

        public DeathSwapSettings()
        {

            TimerLimit = new Limit()
            {
                Lower = 5,
                Upper = 10,
                Value = -1
            };

        }

        public bool ShouldSwap()
        {

            if (TimerLimit.Value == -1 || Running == false)
            {
                LastTime = DateTime.Now;

                if (TimerLimit.random)
                    CalculateNewLimit();

                Running = true;

                return false;
            }

            if (DateTime.Now.Subtract(LastTime).TotalMinutes > TimerLimit.Value)
            {
                LastTime = DateTime.Now;

                if (TimerLimit.random)
                    CalculateNewLimit();

                return true;
            }

            return false;

        }

        public byte GetSwapPhase()
        {

            if (TimerLimit.Value == -1 || Running == false)
            {
                LastTime = DateTime.Now;

                if (TimerLimit.random)
                    CalculateNewLimit();

                Running = true;

                return 0;
            }

            if (DateTime.Now.Subtract(LastTime).TotalMinutes > TimerLimit.Value)
            {
                Console.WriteLine("Swapping...");
                LastTime = DateTime.Now;

                if (TimerLimit.random)
                    CalculateNewLimit();

                return 2;
            } 
            else
            {
                if (DateTime.Now.Subtract(LastTime).TotalMinutes > Convert.ToDouble(TimerLimit.Value) - 0.08)
                    return 1;
            }
            return 0;

        }

        public double TimeLeft()
        {

            if (!Running)
                return 0;

            return TimerLimit.Value - DateTime.Now.Subtract(LastTime).TotalMinutes;

        }

        public int CalculateNewLimit()
        {

            TimerLimit.Value = new Random().Next(TimerLimit.Lower, TimerLimit.Upper);

            return TimerLimit.Value;

        }

        public void ChangeLimits(int lower = -1, int upper = -1, int value = -1, int random = -1)
        {

            if(lower != -1)
                TimerLimit.Lower = lower;
            if (upper != -1)
                TimerLimit.Upper = upper;
            if (value != -1)
            {
                TimerLimit.Value = value;
                LastTime = DateTime.Now;
            }
            if (random != -1)
                TimerLimit.random = random == 0 ? false : true;

        }

        public void RestartTimer()
        {

            LastTime = DateTime.Now;

        }

    }
}
