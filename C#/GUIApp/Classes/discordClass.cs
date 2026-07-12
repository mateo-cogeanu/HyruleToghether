using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIApp
{
    public class discordClass
    {

        Discord.Discord discord = new Discord.Discord(946199926847705088, (UInt64)Discord.CreateFlags.Default);

        public discordClass()
        {
            var activityManager = discord.GetActivityManager();

            var activity = new Discord.Activity
            {
                State = "Exclusive version :eyes: ",
                Assets =
                {
                    LargeImage = "logo",
                }
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                
            });

        }
        
        public void update()
        {
            discord.RunCallbacks();
        }

    }
}
