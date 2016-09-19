using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GinghamSharp
{
    public class Space
    {
        public int Width { get; set; }
        public int Depth { get; set; }
        public int Height { get; set; }
        public Cell[][][] Cells { get; set; }

        public Space(int width = 0, int depth = 0, int height = 0)
        {
            this.Width = width;
            this.Depth = depth;
            this.Height = height;

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; x < this.Depth; y++)
                {
                    for (int z = 0; x < this.Height; z++)
                    {
                        this.Cells[x][y][z] = new Cell(x, y, z);
                    }
                }
            }
        }

        public int? HeightAt(int x, int y)
        {
            if (this.Cells[x][y] == null)
            {
                return null;
            }

            var result = 0;
            var groundCells = this.Cells[x][y].Where(cell => cell.IsGround);
            if (groundCells.Count() > 0)
            {
                var zListMax = groundCells.Select(cell => cell.Z).Max();
                result = zListMax < 0 ? 0 : zListMax;
            }
            else
            {
                result = 0;
            }

            return result;
        }

        public Cell GroundAt(int x, int y)
        {
            var z = HeightAt(x, y);
            var isIllegal = z == null || x < 0 || y < 0 || z < 0 || x >= this.Width || y >= this.Depth || z >= this.Height;
            return isIllegal ? null : this.Cells[x][y][(int)z];
        }

        public Cell RotateRight(Cell center, Cell target)
        {
            return target != null ? this.GroundAt(center.X + (target.Y - center.Y), center.Y - (target.X - center.X)) : null;
        }

        public Cell RotateLeft(Cell center, Cell target)
        {
            return target != null ? this.GroundAt(center.X - (target.Y - center.Y), center.Y + (target.X - center.X)) : null;
        }

        public Cell RotateReverse(Cell center, Cell target)
        {
            return target != null ? this.GroundAt(center.X - (target.X - center.X), center.Y - (target.Y - center.Y)) : null;
        }

        public Cell BuildRangeCell(Waypoint waypoint, int query)
        {
            var x = waypoint.Cell.X;
            var y = waypoint.Cell.Y;
            var queryString = query.ToString();
            for (int n = 0; n < queryString.Length; n++)
            {
                switch ((int)queryString[n])
                {
                    case 8:
                        y += 1;
                        break;
                    case 2:
                        y -= 1;
                        break;
                    case 6:
                        x += 1;
                        break;
                    case 4:
                        x -= 1;
                        break;
                    default:
                        break;
                }
            }

            var target = this.GroundAt(x, y);
            Cell result = null;
            switch (waypoint.Direction)
            {
                case Direction.D2:
                    result = this.RotateReverse(waypoint.Cell, target);
                    break;
                case Direction.D6:
                    result = this.RotateRight(waypoint.Cell, target);
                    break;
                case Direction.D4:
                    result = this.RotateLeft(waypoint.Cell, target);
                    break;
                default:
                    result = target;
                    break;
            }

            return (result == null || result.IsOccupied) ? null : result;
        }

        public List<Cell> BuildAllRangeCells(Waypoint waypoint, List<int> queryList)
        {
            var result = new List<Cell>();
            foreach (var query in queryList)
            {
                var rangeCell = this.BuildRangeCell(waypoint, query);
                if (rangeCell != null)
                {
                    result.Add(rangeCell);
                }
            }
            return result.Distinct().ToList();
        }
    }
}
