using SarcLibrary;
using SarcWrapper.SarcTypes;

namespace SarcWrapper.Actor
{
    [SarcName("Pack")]
    public class PackFolder : SarcFolder<PackFolder>
    {
        public SarcFileList<ActorPack, Sarc> Actors = new SarcFileList<ActorPack, Sarc>();
    }
}
