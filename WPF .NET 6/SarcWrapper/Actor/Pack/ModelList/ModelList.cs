using Nintendo.Aamp;
using SarcWrapper.SarcTypes;

namespace SarcWrapper.Actor.Pack.ModelList
{
    [SarcName("ModelList")]
    [SarcExtension(".bmodellist")]
    [SarcInsideFolder(true)]
    public class ModelList : SarcFile<ModelList, AampFile>
    {
        public ModelList(string path) : base(path) { }
        public ModelList(string path, ArraySegment<byte> data) : base(path, data) { }
    }
}
