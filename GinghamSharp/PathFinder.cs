using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GinghamSharp
{
    class PathFinder
    {
        public static List<Waypoint> FindMovePath(Space space, Waypoint from, Waypoint to, int costLimit = 999)
        {
            var openList = new List<Waypoint> { from };
            if (from.Cell == to.Cell)
            {
                return openList;
            }

            var closeList = new List<Waypoint>();
            int loopLimit = 0;

            while (openList.Count() > 0 && loopLimit < 1000)
            {
                var currentWp = openList.First();
                closeList.Add(currentWp);
                openList.RemoveAt(0);

                var adjacentWaypoints = FindAdjacentWaypoints(space, currentWp);
                foreach (var wp in adjacentWaypoints)
                {
                    if (wp.SumCost < costLimit)
                    {
                        if (!closeList.Contains(wp))
                        {
                            openList.Add(wp);
                        }
                    }
                }
                loopLimit++;
            }

            var shortestChains = new List<Waypoint> { from };
            var endPoints = closeList.Where(closed => closed.Cell == to.Cell);

            if (endPoints.Count() != 0)
            {
                var shortestCost = 999;
                foreach (var endWp in endPoints)
                {
                    if (endWp.SumCost < shortestCost)
                    {
                        shortestCost = endWp.SumCost;
                        shortestChains = endWp.Chains;
                    }
                }
            }

            return shortestChains;
        }

        public static List<Waypoint> FindSkillPath(Space space, Waypoint from, Waypoint to, int maxHeight = 10)
        {
            var path = new List<Waypoint> { from };
            var lastWp = path.Last();
            var shouldMoveY = from.Direction == Direction.D8 || from.Direction == Direction.D2;

            var loopLimit = 0;

            while (lastWp.Cell.X != to.Cell.X || lastWp.Cell.Y != to.Cell.Y)
            {
                loopLimit++;
                if (loopLimit > 30)
                {
                    break;
                }

                if (shouldMoveY && lastWp.Cell.Y != to.Cell.Y)
                {
                    if (lastWp.Cell.Y < to.Cell.Y)
                    {
                        if (lastWp.Cell.Y + 1 != space.Depth)
                        {
                            var height = space.HeightAt(lastWp.Cell.X, lastWp.Cell.Y + 1);
                            if (height == null)
                            {
                                height = 0;
                            }
                            var cell = space.Cells[lastWp.Cell.X][lastWp.Cell.Y + 1][(int)height];
                            if (!cell.IsPassable)
                            {
                                break;
                            }

                            if (lastWp.Direction == Direction.D8)
                            {
                                if (cell.Z > maxHeight)
                                {
                                    break;
                                }

                                var wp = new Waypoint(cell, Direction.D8, lastWp);
                                path.Add(wp);
                                lastWp = wp;
                            }
                            else
                            {
                                var tmp = new Waypoint(lastWp.Cell, Direction.D8, lastWp);
                                path.Add(tmp);

                                if (cell.Z > maxHeight)
                                {
                                    break;
                                }

                                tmp = new Waypoint(cell, Direction.D8, tmp);
                                path.Add(tmp);
                                lastWp = tmp;
                            }
                        }
                    }
                    else if(lastWp.Cell.Y > to.Cell.Y)
                    {
                        if (lastWp.Cell.Y - 1 >= 0)
                        {
                            var height = space.HeightAt(lastWp.Cell.X, lastWp.Cell.Y - 1);
                            if (height == null)
                            {
                                height = 0;
                            }
                            var cell = space.Cells[lastWp.Cell.X][lastWp.Cell.Y - 1][(int)height];
                            if (!cell.IsPassable)
                            {
                                break;
                            }

                            if (lastWp.Direction == Direction.D2)
                            {
                                if (cell.Z > maxHeight)
                                {
                                    break;
                                }
                                var wp = new Waypoint(lastWp.Cell, Direction.D2, lastWp);
                                path.Add(wp);
                                lastWp = wp;
                            }
                            else
                            {
                                var tmp = new Waypoint(lastWp.Cell, Direction.D2, lastWp);
                                path.Add(tmp);
                                if (cell.Z > maxHeight)
                                {
                                    break;
                                }

                                tmp = new Waypoint(cell, Direction.D2, tmp);
                                path.Add(tmp);
                                lastWp = tmp;
                            }
                        }
                    }

                    shouldMoveY = false;
                    continue;
                }
                else
                {
                    shouldMoveY = false;
                }

                if (!shouldMoveY && lastWp.Cell.X != to.Cell.X)
                {
                    if (lastWp.Cell.X < to.Cell.X)
                    {
                        if (lastWp.Cell.X + 1 != space.Width)
                        {
                            var height = space.HeightAt(lastWp.Cell.X + 1, lastWp.Cell.Y);
                            if (height == null)
                            {
                                height = 0;
                            }
                            var cell = space.Cells[lastWp.Cell.X + 1][lastWp.Cell.Y][(int)height];
                            if (!cell.IsPassable)
                            {
                                break;
                            }

                            if (lastWp.Direction == Direction.D6)
                            {
                                if (cell.Z > maxHeight)
                                {
                                    break;
                                }
                                var wp = new Waypoint(cell, Direction.D6, lastWp);
                                path.Add(wp);
                                lastWp = wp;
                            }
                            else
                            {
                                var tmp = new Waypoint(lastWp.Cell, Direction.D6, lastWp);
                                path.Add(tmp);
                                if (cell.Z > maxHeight)
                                {
                                    break;
                                }
                                tmp = new Waypoint(cell, Direction.D6, tmp);
                                path.Add(tmp);
                                lastWp = tmp;
                            }
                        }
                    }
                    else if (lastWp.Cell.X > to.Cell.X)
                    {
                        if (lastWp.Cell.X - 1 >= 0)
                        {
                            var height = space.HeightAt(lastWp.Cell.X - 1, lastWp.Cell.Y);
                            if (height == null)
                            {
                                height = 0;
                            }
                            var cell = space.Cells[lastWp.Cell.X - 1][lastWp.Cell.Y][(int)height];
                            if (!cell.IsPassable)
                            {
                                break;
                            }

                            if (lastWp.Direction == Direction.D4)
                            {
                                if (cell.Z > maxHeight)
                                {
                                    break;
                                }
                                var wp = new Waypoint(cell, Direction.D4, lastWp);
                                path.Add(wp);
                                lastWp = wp;
                            }
                            else
                            {
                                var tmp = new Waypoint(lastWp.Cell, Direction.D4, lastWp);
                                path.Add(tmp);
                                if (cell.Z > maxHeight)
                                {
                                    break;
                                }
                                tmp = new Waypoint(cell, Direction.D4, tmp);
                                path.Add(tmp);
                                lastWp = tmp;
                            }
                        }
                    }
                }
                shouldMoveY = true;
            } // end of while

            return path.Where(wp => wp != null).ToList();
        }

        public static List<Waypoint> FindAdjacentWaypoints(Space space, Waypoint wp)
        {
            var adjacentList = new List<Waypoint>();
            var adjacentCells = FindAdjacentCells(space, wp.Cell);
            foreach (var cell in adjacentCells)
            {
                var moveDirection = Waypoint.DetectDirection(wp, cell);
                var parent = wp;
                if (moveDirection != wp.Direction)
                {
                    var turnWp = new Waypoint(wp.Cell, moveDirection, wp);
                    parent = turnWp;
                }
                var moveTo = new Waypoint(cell, moveDirection, parent);
                adjacentList.Add(moveTo);
            }
            return adjacentList;
        }

        public static List<Cell> FindAdjacentCells(Space space, Cell cell)
        {
            var w = space.Width;
            var d = space.Depth;
            var h = space.Height;
            var x = cell.X;
            var y = cell.Y;
            var z = cell.Z;

            var adjacentList = new List<Cell>();
            if (x + 1 < w)
            {
                var targetCell = space.Cells[x + 1][y][z];
                if (targetCell != null && !targetCell.IsOccupied)
                {
                    adjacentList.Add(targetCell);
                }
            }

            if (x - 1 >= 0)
            {
                var targetCell = space.Cells[x - 1][y][z];
                if (targetCell != null && !targetCell.IsOccupied)
                {
                    adjacentList.Add(targetCell);
                }
            }

            if (y + 1 < d)
            {
                var targetCell = space.Cells[x][y + 1][z];
                if (targetCell != null && !targetCell.IsOccupied)
                {
                    adjacentList.Add(targetCell);
                }
            }

            if (y - 1 >= 0)
            {
                var targetCell = space.Cells[x][y - 1][z];
                if (targetCell != null && !targetCell.IsOccupied)
                {
                    adjacentList.Add(targetCell);
                }
            }

            return adjacentList;
        }
    }
}
