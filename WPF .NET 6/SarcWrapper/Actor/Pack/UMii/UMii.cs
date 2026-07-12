using Nintendo.Aamp;
using SarcWrapper.SarcTypes;

namespace SarcWrapper.Actor.Pack.UMii
{
    [SarcName("UMii")]
    [SarcExtension(".bumii")]
    [SarcInsideFolder(true)]
    public class UMii : SarcFile<UMii, AampFile>
    {
        public UMii(string path) : base(path) { }
        public UMii(string path, ArraySegment<byte> data) : base(path, data) { }
    }
}
