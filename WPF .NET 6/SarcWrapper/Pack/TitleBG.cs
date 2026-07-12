using SarcLibrary;
using SarcWrapper.Actor;
using SarcWrapper.SarcTypes;

namespace SarcWrapper.Pack
{
    [SarcName("TitleBG")]
    [SarcExtension(".pack")]
    public class TitleBG : SarcFile<TitleBG, Sarc>
    {
        public ActorFolder Actor = new ActorFolder();

        public TitleBG(string path) : base(path) { }
        public TitleBG(string path, ArraySegment<byte> data) : base(path) {}

        //public TitleBG(string path)
        //{
        //    if (Path.GetFileName(path) != "TitleBG.pack")
        //    {
        //        throw new FileLoadException("File provided is not TitleBG");
        //    }

        //    sarcFile = Sarc.FromBinary(File.ReadAllBytes(path));

        //    Actor.LoadChildren(sarcFile);
        //}
    }
}
