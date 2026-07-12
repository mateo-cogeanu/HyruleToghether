using Nintendo.Aamp;
using SarcWrapper.SarcTypes;

namespace SarcWrapper.Actor.Pack.ASList
{
    [SarcName("ASList")]
    [SarcExtension(".baslist")]
    [SarcInsideFolder(true)]
    public class ASList : SarcFile<ASList, AampFile>
    {
        public ASList(string path, ArraySegment<byte> data) : base(path, data) { }
    }
}
