using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GinghamSharp
{
    public class PathFinder
    {
        public static List<Waypoint> FindMovePath(Space space, Waypoint from, Waypoint to, int movePower = 999, int jumpPower = 999, int margin = 0)
        {
            space.ResetMovePathInfo(false);

            var openList = new List<Waypoint> { from };
            var closeList = new List<Waypoint>();
            int loopLimit = 0;

            while (openList.Count() > 0 && loopLimit < 1000)
            {
                var currentWp = openList.First();
                closeList.Add(currentWp);
                openList.RemoveAt(0);

                var adjacentWaypoints = FindAdjacentWaypoints(space, currentWp, jumpPower);
                foreach (var wp in adjacentWaypoints)
                {
                    if ((wp.SumCost + margin) <= movePower)
                    {
                        if (!closeList.Contains(wp))
                        {
                            wp.Cell.IsPassable = true;
                            openList.Add(wp);
                        }
                    }
                }
                loopLimit++;
            }

            var shortestChains = from.Chains;
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

            foreach (Waypoint wp in shortestChains)
            {
                wp.Cell.IsMovePath = true;
            }

            return shortestChains;
        }

        public static List<Waypoint> FindSkillPath(Space space, Waypoint from, Waypoint to, int maxHeight = 999)
        {
            var path = new List<Waypoint> { from };
            var shouldMoveY = from.Direction == Direction.D8 || from.Direction == Direction.D2;

            var loopLimit = 0;

            while (path.Last().Cell.X != to.Cell.X || path.Last().Cell.Y != to.Cell.Y)
            {
                loopLimit++;
                if (loopLimit > 30)
                {
                    break;
                }

                if (shouldMoveY && path.Last().Cell.Y != to.Cell.Y)
                {
                    if (path.Last().Cell.Y < to.Cell.Y)
                    {
                        if (path.Last().Cell.Y + 1 != space.Depth)
                        {
                            var height = space.HeightAt(path.Last().Cell.X, path.Last().Cell.Y + 1);
                            if (height == null)
                            {
                                height = 0;
                            }
                            var cell = space.Cells[path.Last().Cell.X][path.Last().Cell.Y + 1][(int)height];
                            if (cell.IsOccupied)
                            {
                                break;
                            }

                            if (path.Last().Direction != Direction.D8)
                            {
                                path.Add(new Waypoint(path.Last().Cell, Direction.D8, path.Last()));
                            }

                            if (cell.Z > maxHeight)
                            {
                                break;
                            }
                            
                            path.Add(new Waypoint(cell, Direction.D8, path.Last()));
                        }
                    }
                    else if(path.Last().Cell.Y > to.Cell.Y)
                    {
                        if (path.Last().Cell.Y - 1 >= 0)
                        {
                            var height = space.HeightAt(path.Last().Cell.X, path.Last().Cell.Y - 1);
                            if (height == null)
                            {
                                height = 0;
                            }
                            var cell = space.Cells[path.Last().Cell.X][path.Last().Cell.Y - 1][(int)height];
                            if (cell.IsOccupied)
                            {
                                break;
                            }

                            if (path.Last().Direction != Direction.D2)
                            {
                                path.Add(new Waypoint(path.Last().Cell, Direction.D2, path.Last()));
                            }

                            if (cell.Z > maxHeight)
                            {
                                break;
                            }

                            path.Add(new Waypoint(cell, Direction.D2, path.Last()));
                        }
                    }

                    shouldMoveY = false;
                    continue;
                }
                else
                {
                    shouldMoveY = false;
                }

                if (!shouldMoveY && path.Last().Cell.X != to.Cell.X)
                {
                    if (path.Last().Cell.X < to.Cell.X)
                    {
                        if (path.Last().Cell.X + 1 != space.Width)
                        {
                            var height = space.HeightAt(path.Last().Cell.X + 1, path.Last().Cell.Y);
                            if (height == null)
                            {
                                height = 0;
                            }
                            var cell = space.Cells[path.Last().Cell.X + 1][path.Last().Cell.Y][(int)height];
                            if (cell.IsOccupied)
                            {
                                break;
                            }

                            if (path.Last().Direction != Direction.D6)
                            {
                                path.Add(new Waypoint(path.Last().Cell, Direction.D6, path.Last()));
                            }

                            if (cell.Z > maxHeight)
                            {
                                break;
                            }

                            path.Add(new Waypoint(cell, Direction.D6, path.Last()));
                        }
                    }
                    else if (path.Last().Cell.X > to.Cell.X)
                    {
                        if (path.Last().Cell.X - 1 >= 0)
                        {
                            var height = space.HeightAt(path.Last().Cell.X - 1, path.Last().Cell.Y);
                            if (height == null)
                            {
                                height = 0;
                            }
                            var cell = space.Cells[path.Last().Cell.X - 1][path.Last().Cell.Y][(int)height];
                            if (cell.IsOccupied)
                            {
                                break;
                            }

                            if (path.Last().Direction != Direction.D4)
                            {
                                path.Add(new Waypoint(path.Last().Cell, Direction.D4, path.Last()));
                            }

                            if (cell.Z > maxHeight)
                            {
                                break;
                            }

                            path.Add(new Waypoint(cell, Direction.D4, path.Last()));
                        }
                    }
                }
                shouldMoveY = true;
            } // end of while

            return path.Where(wp => wp != null).ToList();
        }

        public static List<Waypoint> FindAdjacentWaypoints(Space space, Waypoint wp, int jumpPower = 999)
        {
            var adjacentList = new List<Waypoint>();
            var adjacentCells = FindAdjacentCells(space, wp.Cell, jumpPower);
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

        public static List<Cell> FindAdjacentCells(Space space, Cell cell, int jumpPower = 999)
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
                if (targetCell != null)
                {
                    if (!targetCell.IsOccupied && Math.Abs(z - targetCell.Z) <= jumpPower)
                    {
                        adjacentList.Add(targetCell);
                    }
                }
            }

            if (x - 1 >= 0)
            {
                var targetCell = space.Cells[x - 1][y][z];
                if (targetCell != null)
                {
                    if (!targetCell.IsOccupied && Math.Abs(z - targetCell.Z) <= jumpPower)
                    {
                        adjacentList.Add(targetCell);
                    }
                }
            }

            if (y + 1 < d)
            {
                var targetCell = space.Cells[x][y + 1][z];
                if (targetCell != null)
                {
                    if (!targetCell.IsOccupied && Math.Abs(z - targetCell.Z) <= jumpPower)
                    {
                        adjacentList.Add(targetCell);
                    }
                }
            }

            if (y - 1 >= 0)
            {
                var targetCell = space.Cells[x][y - 1][z];
                if (targetCell != null)
                {
                    if (!targetCell.IsOccupied && Math.Abs(z - targetCell.Z) <= jumpPower)
                    {
                        adjacentList.Add(targetCell);
                    }
                }
            }

            return adjacentList;
        }
    }
}
