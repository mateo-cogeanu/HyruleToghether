using BOTWM.Server.JSONBuilder;
using System.Text;

namespace BOTWM.Tests
{
    public class ServerShould
    {
        [Fact]
        public void DecodePingPasswordWithoutTheMessageTypeByte()
        {
            byte[] frame = new byte[6144];
            frame[0] = (byte)MessageType.ping;
            Encoding.UTF8.GetBytes("hyrule-secret").CopyTo(frame, 1);

            Tuple<MessageType, object> result = new JSONBuilder().BuildFromBytes(frame);

            Assert.Equal(MessageType.ping, result.Item1);
            Assert.Equal("hyrule-secret", result.Item2);
        }

        [Fact]
        public void ResolveAPlatformDataDirectory()
        {
            Assert.False(string.IsNullOrWhiteSpace(BOTWM.Server.PlatformPaths.DataDirectory));
            Assert.Equal("ArmorMapping.txt", Path.GetFileName(BOTWM.Server.PlatformPaths.DataFile("ArmorMapping.txt")));
        }
    }
}
