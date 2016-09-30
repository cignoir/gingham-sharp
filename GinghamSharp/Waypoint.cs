using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GinghamSharp
{
    public class Waypoint
    {
        public Cell Cell { get; set; }
        public Direction Direction { get; set; }
        public Waypoint Parent { get; set; }
        public int Cost { get; set; }
        public int SumCost { get; set; }
        public List<Waypoint> Chains { get; set; }

        // 移動経路上をクリックした際、クリックした地点までをロック扱いにする
        public bool IsLocked { get; set; }

        public bool IsTurning { get { return this.Parent != null ? this.Parent.Cell == this.Cell : false; } }
        public bool IsMoving { get { return !IsTurning; } }

        public Waypoint(Cell cell = null, Direction direction = Direction.D8, Waypoint parent = null)
        {
            this.Cell = cell == null ? new Cell() : cell;
            this.Direction = direction;
            this.Parent = parent;

            Update();
        }

        public static Direction DetectDirection(Waypoint from, Cell targetCell)
        {
            var diffX = targetCell.X - from.Cell.X;
            var diffY = targetCell.Y - from.Cell.Y;

            var direction = from.Direction;
            direction = diffX > 0 ? Direction.D6 : direction;
            direction = diffX < 0 ? Direction.D4 : direction;
            direction = diffY > 0 ? Direction.D8 : direction;
            direction = diffY < 0 ? Direction.D2 : direction;
            return direction;
        }

        public int CalcCost()
        {
            if (this.Parent == null)
            {
                return 0;
            }

            var isSameCell = this.Parent.Cell == this.Cell;
            var turnCost = 0;
            switch (this.Parent.Direction)
            {
                case Direction.D8:
                    switch (this.Direction)
                    {
                        case Direction.D8:
                            turnCost = isSameCell ? 5 : 0;
                            break;
                        case Direction.D2:
                            turnCost = 10;
                            break;
                        default:
                            turnCost = 5;
                            break;
                    }
                    break;
                case Direction.D2:
                    switch (this.Direction)
                    {
                        case Direction.D2:
                            turnCost = isSameCell ? 5 : 0;
                            break;
                        case Direction.D8:
                            turnCost = 10;
                            break;
                        default:
                            turnCost = 5;
                            break;
                    }
                    break;
                case Direction.D6:
                    switch (this.Direction)
                    {
                        case Direction.D6:
                            turnCost = isSameCell ? 5 : 0;
                            break;
                        case Direction.D4:
                            turnCost = 10;
                            break;
                        default:
                            turnCost = 5;
                            break;
                    }
                    break;
                case Direction.D4:
                    switch (this.Direction)
                    {
                        case Direction.D4:
                            turnCost = isSameCell ? 5 : 0;
                            break;
                        case Direction.D6:
                            turnCost = 10;
                            break;
                        default:
                            turnCost = 5;
                            break;
                    }
                    break;
                default:
                    turnCost = 0;
                    break;
            }
            return isSameCell ? turnCost : 10 + turnCost;
        }

        public List<Waypoint> PickParents()
        {
            var result = new List<Waypoint>();
            if (this.Parent != null)
            {
                result.AddRange(this.Parent.Chains);
            }
            result.Add(this);
            return result;
        }

        public Waypoint Update()
        {
            if (this.Parent == null)
            {
                this.Cost = 0;
                this.SumCost = 0;
                this.Chains = new List<Waypoint> { this };
            }
            else
            {
                this.Cost = this.CalcCost();
                this.Chains = this.PickParents();
                this.SumCost = this.Chains.Sum(wp => wp.Cost);
            }
            return this;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Waypoint;
            return this.Cell == other.Cell && this.Direction == other.Direction;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            var selfString = string.Format("{0}/{1}:{2}:{3}", this.Cell, this.Direction.ToString(), this.Cost, this.SumCost);
            var parentString = this.Parent != null ? string.Format("{0}/{1}->", this.Parent.Cell, this.Parent.Direction.ToString()) : "";
            return this.Parent != null ? parentString + selfString : selfString;
        }

        public string Inspect()
        {
            return this.ToString();
        }

        public void Lock()
        {
            this.IsLocked = true;
            this.Cell.Lock();
        }

        public void Unlock()
        {
            this.IsLocked = false;
            this.Cell.Unlock();
        }
    }
}
