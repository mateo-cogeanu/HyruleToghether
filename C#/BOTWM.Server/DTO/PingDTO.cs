namespace BOTWM.Server.DTO
{
    public class PingDTO
    {
        public bool CorrectPassword { get; set; }
        public string Description { get; set; }
        public NamesDTO PlayerList { get; set; }
        public string GameMode { get; set; }
        public int PlayerLimit { get; set; }
    }
}
