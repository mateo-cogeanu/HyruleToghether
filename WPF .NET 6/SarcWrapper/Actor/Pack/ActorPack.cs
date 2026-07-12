using Nintendo.Aamp;
using SarcLibrary;
using SarcWrapper.Actor.Pack.ActorLink;
using SarcWrapper.Actor.Pack.ASList;
using SarcWrapper.Actor.Pack.ModelList;
using SarcWrapper.Actor.Pack.UMii;
using SarcWrapper.SarcTypes;
using System.Net.Http.Headers;

namespace SarcWrapper
{
    [SarcName("ActorPack")]
    [SarcExtension(".sbactorpack")]
    public class ActorPack : SarcFile<ActorPack, Sarc>
    {
        public SarcFileList<ModelList, AampFile> ModelList = new SarcFileList<ModelList, AampFile>();
        public SarcFileList<ASList, AampFile> ASList = new SarcFileList<ASList, AampFile>();
        public SarcFileList<UMii, AampFile> UMii = new SarcFileList<UMii, AampFile>();
        public SarcFileList<ActorLink, AampFile> ActorLink = new SarcFileList<ActorLink, AampFile>();

        public ActorPack(string path) : base(path) { }
        public ActorPack(string path, ArraySegment<byte> data) : base(path, data) { }
    }
}
