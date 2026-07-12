namespace SarcWrapper.Helper
{
    public static class DataExtensions
    {
        public static Dictionary<string, ArraySegment<byte>> filterByFolder(this Dictionary<string, ArraySegment<byte>> data, string filter)
        {
            return data.Where(kvp => kvp.Key.StartsWith(filter)).ToDictionary();
        }

        public static Dictionary<string, ArraySegment<byte>> filterByExtension(this Dictionary<string, ArraySegment<byte>> data, string filter)
        {
            return data.Where(kvp => Path.GetExtension(kvp.Key) == filter).ToDictionary();
        }
    }
}
