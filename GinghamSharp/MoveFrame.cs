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
                var newActor = new Actor(actor.Code, actor.Waypoint);
                newActor.MoveSteps = actor.MoveSteps;
                list.Add(newActor);
            }
            this.Actors = list;
        }
    }

}
