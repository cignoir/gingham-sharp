using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GinghamSharp
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public bool IsOccupied { get; set; }
        public bool IsPassable { get { return !IsOccupied; } }
        public bool IsGround { get; set; }
        public bool IsSky { get { return !IsGround; } }
        public bool IsMovePath { get; set; }
        public bool IsLocked { get; set; }
        
        public Cell(int x = 0, int y = 0, int z = 0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;

            this.IsOccupied = false;
            this.IsGround = false;
            this.IsMovePath = false;
            this.IsLocked = false;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                var other = obj as Cell;
                return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", this.X, this.Y, this.Z);
        }

        public string Inspect()
        {
            return this.ToString();
        }

        public Cell SetGround()
        {
            this.IsGround = true;
            return this;
        }

        public void ClearPath()
        {
            if (!this.IsLocked)
            {
                this.IsMovePath = false;
            }
        }

        public void Lock()
        {
            this.IsLocked = true;
        }

        public void Unlock()
        {
            this.IsLocked = false;
        }
    }
}
