using Nintendo.Aamp;
using SarcWrapper.SarcTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SarcWrapper.Actor.Pack.ActorLink
{
    [SarcName("ActorLink")]
    [SarcExtension(".bxml")]
    [SarcInsideFolder(true)]
    public class ActorLink : SarcFile<ActorLink, AampFile>
    {
        public ActorLink(string path, ArraySegment<byte> data) : base(path, data) { }
    }
}
