using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GinghamSharp
{
    public class Actor
    {
        public Waypoint Waypoint { get; set; }
        public int Weight { get; set; }
        public List<Waypoint> MoveSteps { get; set; }
        public int TeamId { get; set; }
        public MoveStatus MoveStatus { get; set; }
        public bool IsMoveEnd { get { return this.MoveStatus == MoveStatus.FINISHED || this.MoveStatus == MoveStatus.STOPPED; } }
        public int MovePower { get; set; }
        public int JumpPower { get; set; }

        public Actor(Waypoint waypoint, int weight = 100, int teamId = 0, int movePower = 999, int jumpPower = 999)
        {
            this.Waypoint = waypoint;
            this.Weight = weight;
            this.TeamId = teamId;
            this.MoveStatus = MoveStatus.DEFAULT;
            this.MovePower = movePower;
            this.JumpPower = jumpPower;

            this.MoveSteps = new List<Waypoint>() { waypoint };
        }

        public void ClearMoveSteps(bool force = false)
        {
            if (force)
            {
                this.MoveSteps = new List<Waypoint>() { this.Waypoint };
                this.Waypoint.Cell.IsMovePath = true;
            }
            else
            {
                if (this.MoveSteps.Count == 0)
                {
                    this.MoveSteps = new List<Waypoint>() { this.Waypoint };
                } else
                {
                    this.MoveSteps = this.MoveSteps.Where(wp => wp.IsLocked).ToList();
                }
            }
        }
    }
}
