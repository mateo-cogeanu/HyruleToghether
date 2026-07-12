using SarcWrapper.SarcTypes;

namespace SarcWrapper.Actor
{
    [SarcName("Actor")]
    public class ActorFolder : SarcFolder<ActorFolder>
    {
        public PackFolder Pack = new PackFolder();
    }
}
