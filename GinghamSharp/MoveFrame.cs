using NClone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GinghamSharp
{
    public class MoveFrame
    {
        public int Index { get; set; }
        public List<Actor> Actors { get; set; }

        public MoveFrame(int index, List<Actor> actors)
        {
            this.Index = index;
            var list = new List<Actor>();
            foreach (var actor in actors)
            {
                list.Add(new Actor(actor.Code, actor.Waypoint));
            }
            this.Actors = list;
        }
    }

}
