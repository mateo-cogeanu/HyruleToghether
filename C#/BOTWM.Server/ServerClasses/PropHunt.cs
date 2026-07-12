using BOTW.Logging;
using BOTWM.Server.DataTypes;
using BOTWM.Server.DTO;
using Newtonsoft.Json;
using System.Numerics;

namespace BOTWM.Server.ServerClasses
{
    public class PlayerData
    {
        public bool Hunter { get; set; }
        public bool Alive { get; set; }
        public TimeSpan TimeToFind { get; set; } = TimeSpan.Zero;
    }

    public class PropHunt
    {
        public Mutex PHMutex = new Mutex();
        public DateTime PropHuntStartTime { get; set; }
        public bool InitialCountdown { get; set; } = false;
        public DateTime InitialCountdownFinishTime { get; set; }
        public Dictionary<byte, ModelDataDTO> OriginalModels { get; set; }
        public Dictionary<byte, PlayerData> Players { get; set; }
        public PropHuntPhaseEnum CurrentPhase { get; set; }

        private ProphuntLocation Settings { get; set; }

        public DateTime HidingTime { get; set; }

        public PropHunt()
        {
            this.PropHuntStartTime = DateTime.MinValue;
            this.OriginalModels = new Dictionary<byte, ModelDataDTO>();
            this.CurrentPhase = PropHuntPhaseEnum.Stopped;
            this.Players = new Dictionary<byte, PlayerData>();
            this.HidingTime = DateTime.Now;
        }

        public void Start(ProphuntLocation location, int hidingTime)
        {
            PHMutex.WaitOne(100);

            if(this.CurrentPhase == PropHuntPhaseEnum.Stopped)
            {
                this.Settings = location;

                //this.OriginalModels = new Dictionary<byte, ModelDataDTO>(ServerData.ModelData.GetAllPlayers().Models);
                this.OriginalModels = JsonConvert.DeserializeObject<Dictionary<byte, ModelDataDTO>>(JsonConvert.SerializeObject(ServerData.ModelData.GetAllPlayers().Models));
                this.CurrentPhase = PropHuntPhaseEnum.Countdown;
                this.InitialCountdownFinishTime = DateTime.Now.AddSeconds(3);
                this.InitialCountdown = true;
                this.HidingTime = DateTime.Now.AddSeconds(hidingTime + 3);
                this.Players.Clear();
                Random rng = new Random();

                foreach(Player player in ServerData.PlayerList.Where(p => p.Connected))
                {
                    this.Players[player.PlayerNumber] = new PlayerData() { Hunter = false, Alive = true };
                }

                int numberOfHunters = (int)Math.Floor(ServerData.GetPlayers().Names.Count() * 0.25);
                if (numberOfHunters == 0) numberOfHunters = 1;

                while(numberOfHunters > 0)
                {
                    int randomPlayer = rng.Next(this.Players.Count);
                    //int randomPlayer = 1;
                    if (this.Players.ContainsKey((byte)randomPlayer) && !this.Players[(byte)randomPlayer].Hunter)
                    {
                        this.Players[(byte)randomPlayer].Hunter = true;
                        this.Players[(byte)randomPlayer].Alive = false;
                        numberOfHunters--;
                    }
                }

                foreach(KeyValuePair<byte, PlayerData> player in this.Players.Where(kvp => !kvp.Value.Hunter))
                {
                    ModelDataDTO playerModel = ServerData.ModelData.PlayerModels[player.Key];
                    playerModel.ModelType = 1;
                    playerModel.Model = this.Settings.PossibleModels[rng.Next(this.Settings.PossibleModels.Count())];
                    ServerData.ModelData.AddModel(player.Key, playerModel);
                }

                Logger.LogInformation($"Started prophunt with {this.Players.Count} players");

                IEnumerable<string> hunterNames = ServerData.NameData.GetAllPlayers().Names
                                                                                     .Where(name => this.Players.Where(kvp => kvp.Value.Hunter)
                                                                                     .Select(kvp => kvp.Key)
                                                                                     .Contains(name.Key))
                                                                                     .Select(name => name.Value);

                string endOfMessage = hunterNames.Count() == 1 ? "is the hunter" : "are the hunters";

                Logger.LogInformation($"Hiding phase started! {String.Join(" ,", hunterNames)} {endOfMessage}");
            }

            PHMutex.ReleaseMutex();
        }

        public void UpdateStatus()
        {
            PHMutex.WaitOne(100);

            if(this.CurrentPhase == PropHuntPhaseEnum.Countdown && DateTime.Now > this.InitialCountdownFinishTime && this.InitialCountdown)
            {
                this.CurrentPhase = PropHuntPhaseEnum.Hiding;
                this.InitialCountdown = false;
            }

            if(this.CurrentPhase == PropHuntPhaseEnum.Hiding && DateTime.Now > this.HidingTime.AddSeconds(-3))
            {
                this.CurrentPhase = PropHuntPhaseEnum.Countdown;
            }

            if(this.CurrentPhase == PropHuntPhaseEnum.Countdown && DateTime.Now > this.HidingTime)
            {
                this.CurrentPhase = PropHuntPhaseEnum.Running;
                this.PropHuntStartTime = DateTime.Now;
                Logger.LogInformation($"Hunting phase started!");
            }
            else if(this.CurrentPhase == PropHuntPhaseEnum.Running)
            {
                foreach(Player player in ServerData.PlayerList.Where(p => p.Connected))
                {
                    if(player.Health <= 0 && this.Players.ContainsKey(player.PlayerNumber))
                    {
                        this.Players[player.PlayerNumber].Alive = false;
                        this.Players[player.PlayerNumber].TimeToFind = DateTime.Now - this.PropHuntStartTime;
                        ServerData.ModelData.AddModel(player.PlayerNumber, this.OriginalModels[player.PlayerNumber]);
                    }
                }

                if(!this.Players.Any(p => p.Value.Alive))
                {
                    Logger.LogInformation("Hunters found all of the props! Results:");
                    int positions = 1;

                    Logger.LogDebug(JsonConvert.SerializeObject(this.Players, Formatting.Indented));
                    foreach(KeyValuePair<byte, PlayerData> player in this.Players.Where(p => !p.Value.Hunter).OrderByDescending(p => p.Value.TimeToFind))
                    {
                        Logger.LogInformation($"({positions}°) {ServerData.NameData.GetAllPlayers().Names[player.Key]}: {player.Value.TimeToFind.TotalSeconds} seconds");
                        positions++;
                    }

                    this.StopGameMode();
                }

                if(!this.Players.Where(p => p.Value.Alive && !p.Value.Hunter).Any(p => ServerData.GetPlayer((int)p.Key).Connected))
                {
                    Logger.LogInformation("Prop hunt match finished because of player disconnection");
                    this.StopGameMode();
                }
            }

            PHMutex.ReleaseMutex();
        }

        public void Stop()
        {
            PHMutex.WaitOne(100);

            this.StopGameMode();

            PHMutex.ReleaseMutex();
        }

        private void StopGameMode()
        {
            this.CurrentPhase = PropHuntPhaseEnum.Stopped;

            foreach(KeyValuePair<byte, PlayerData> player in this.Players)
            {
                ServerData.ModelData.AddModel(player.Key, this.OriginalModels[player.Key]);
            }

            this.OriginalModels.Clear();
            this.Players.Clear();
            this.Settings = null;
        }

        public PropHuntDTO GetData(byte playerNumber)
        {
            if (this.Players.ContainsKey(playerNumber))
            {
                PHMutex.WaitOne(100);

                PropHuntPhaseEnum phase = this.CurrentPhase;
                bool isHunter = this.Players[playerNumber].Hunter;
                Vec3f startingPosition = this.Players[playerNumber].Hunter ? this.Settings.HunterStartingPosition : this.Settings.HidersStartingPosition;

                PHMutex.ReleaseMutex();

                return new PropHuntDTO()
                {
                    IsPlaying = true,
                    Phase = (byte)phase,
                    IsHunter = isHunter,
                    StartingPosition = startingPosition
                };
            }

            return new PropHuntDTO()
            {
                IsPlaying = false,
                Phase = 0,
                IsHunter = false,
                StartingPosition = new Vec3f(0, 0, 0)
            };
        }
    }
}
