namespace Breath_of_the_Wild_Multiplayer.MVVM.Model.DTO
{
    public class LocalServerDTO
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public bool Favorite { get; set; }
        public string Password { get; set; }
    }

    public class ServerDataDTO
    {
        public bool CorrectPassword { get; set; }
        public string Description { get; set; }
        public NamesDTO PlayerList { get; set; }
        public string Gamemode { get; set; }
        public int PlayerLimit { get; set; }
    }
}
