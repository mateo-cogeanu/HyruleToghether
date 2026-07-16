using BOTWM.Server.JSONBuilder;
using BOTWM.Server.DTO;
using BOTWM.Server.DataTypes;
using BOTWM.Server.HelperTypes;
using BOTWM.Server.ServerClasses;
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

        [Fact]
        public void PreserveAnimationHashAcrossServerPlayerMapping()
        {
            const int animationHash = -2101720748;
            ClientPlayerDTO update = new ClientPlayerDTO { Animation = animationHash };
            Player player = new Player(1);
            player.Update(update);

            ClosePlayerDTO response = new ClosePlayerDTO();
            response.Map(player);

            Assert.Equal(animationHash, player.Animation);
            Assert.Equal(animationHash, response.Animation);
        }

        [Fact]
        public void PreserveRawEquipmentStateAcrossServerPlayerMapping()
        {
            const byte rawEquipmentState = 0x57;
            ClientPlayerDTO update = new ClientPlayerDTO
            {
                EquipmentState = rawEquipmentState
            };
            Player player = new Player(1);
            player.Update(update);

            ClosePlayerDTO response = new ClosePlayerDTO();
            response.Map(player);

            Assert.Equal(rawEquipmentState, player.EquipmentState);
            Assert.Equal(rawEquipmentState, response.EquipmentState);
        }

        [Fact]
        public void PreserveRawEquipmentStateAcrossBinaryServerProtocol()
        {
            const byte rawEquipmentState = 0x57;
            ClientDTO update = new ClientDTO
            {
                WorldData = new WorldDTO(),
                PlayerData = new ClientPlayerDTO
                {
                    Position = new Vec3f(),
                    Rotation1 = new Quaternion(),
                    Rotation2 = new Quaternion(),
                    Rotation3 = new Quaternion(),
                    Rotation4 = new Quaternion(),
                    EquipmentState = rawEquipmentState,
                    Equipment = new CharacterEquipment(),
                    Location = new CharacterLocation(),
                    Bomb = new Vec3f(),
                    Bomb2 = new Vec3f(),
                    BombCube = new Vec3f(),
                    BombCube2 = new Vec3f()
                },
                EnemyData = new EnemyDTO { Health = new List<EnemyData>() },
                QuestData = new QuestsDTO { Completed = new List<string>() }
            };
            JSONBuilder builder = new JSONBuilder();
            byte[] payload = builder.BuildArrayOfBytes(update, debug: true);
            byte[] frame = new byte[7168];
            frame[0] = (byte)MessageType.update;
            payload.CopyTo(frame, 1);

            Tuple<MessageType, object> decoded = builder.BuildFromBytes(frame);
            ClientDTO decodedUpdate = Assert.IsType<ClientDTO>(decoded.Item2);

            Assert.Equal(MessageType.update, decoded.Item1);
            Assert.Equal(rawEquipmentState, decodedUpdate.PlayerData.EquipmentState);
        }
    }
}
