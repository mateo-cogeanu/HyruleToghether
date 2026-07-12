using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTWM.Server
{

    public class QuestSettings
    {
        public bool Vanilla;
        public bool Koroks;
        public bool Towers;
        public bool Shrines;
        public bool Locations;
        public bool DivineBeast;

        public bool AnyTrue { 
            get
            {
                return Vanilla || Koroks || Towers || Shrines || Locations || DivineBeast;
            } 
        }
    }

    public enum Gamemode
    {
        NoGamemode,
        HunterVsSpeedrunner,
        DeathSwap
    }

    public class ServerSettings
    {

        public string SettingsName;

        public bool EnemySync = false;
        public Gamemode GameMode;
        public QuestSettings QuestSyncSettings;

        public ServerSettings(string settingsName, bool enemySync = false, bool vanillaQuests = false, bool korokSync = false, bool towerSync = false, bool shrineSync = false, bool locationSync = false, bool divineBeasts = false, Gamemode gamemode = Gamemode.NoGamemode)
        {
            SettingsName = settingsName;
            EnemySync = enemySync;
            QuestSyncSettings = new QuestSettings()
            {
                Vanilla = vanillaQuests,
                Koroks = korokSync,
                Towers = towerSync,
                Shrines = shrineSync,
                Locations = locationSync,
                DivineBeast = divineBeasts
            };

            GameMode = gamemode;
        }

        public bool CompareSettings(ServerSettings settingsToCompare)
        {

            if (settingsToCompare.EnemySync == this.EnemySync &&
                settingsToCompare.QuestSyncSettings.Vanilla == this.QuestSyncSettings.Vanilla &&
                settingsToCompare.QuestSyncSettings.Koroks == this.QuestSyncSettings.Koroks &&
                settingsToCompare.QuestSyncSettings.Towers == this.QuestSyncSettings.Towers &&
                settingsToCompare.QuestSyncSettings.Shrines == this.QuestSyncSettings.Shrines &&
                settingsToCompare.QuestSyncSettings.Locations == this.QuestSyncSettings.Locations &&
                settingsToCompare.QuestSyncSettings.DivineBeast == this.QuestSyncSettings.DivineBeast &&
                settingsToCompare.GameMode == this.GameMode)
                return true;

            return false;

        }
    }
}
