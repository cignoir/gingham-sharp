using NClone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GinghamSharp
{
    class MoveFrame
    {
        public int Index { get; set; }
        public List<Actor> Actors { get; set; }

        public MoveFrame(int index, List<Actor> actors)
        {
            this.Index = index;
            this.Actors = Clone.ObjectGraph(actors);
        }
    }

}
