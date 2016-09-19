using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GinghamSharp
{
    public class MoveSimulator
    {
        public List<Actor> NextStep(int currentStepIndex, List<Actor> actors)
        {
            foreach (var actor in actors)
            {
                if (currentStepIndex + 1 >= actor.MoveSteps.Count)
                {
                    actor.MoveStatus = MoveStatus.FINISHED;
                }

                var grouped = actors.Where(a => !a.IsMoveEnd).GroupBy(a => a.MoveSteps[currentStepIndex + 1].Cell);
                foreach (var group in grouped)
                {
                    var goal = group.Key;
                    var allInGoal = new List<Actor>();
                    Actor winner = null;
                    var losers = new List<Actor>();
                    if (actors.Select(a => a.Waypoint.Cell).Contains(goal))
                    {
                        winner = actors.Where(a => a.Waypoint.Cell == goal).First();
                        losers = group.Where(a => a.GetHashCode() != winner.GetHashCode()).ToList();
                        allInGoal.Add(winner);
                        allInGoal.AddRange(losers);
                    }
                    else
                    {
                        var maxWeight = group.Select(a => a.Weight).Max();
                        var winners = group.Where(a => a.Weight == maxWeight);
                        winner = winners.ElementAt(new Random().Next(winners.Count()));
                        winner.Waypoint = winner.MoveSteps[currentStepIndex + 1];
                        winner.MoveStatus = MoveStatus.DEFAULT;
                        losers = group.Where(a => a.GetHashCode() != winner.GetHashCode()).ToList();
                        allInGoal.Add(winner);
                        allInGoal.AddRange(losers);
                    }

                    if (allInGoal.Select(a => a.TeamId).Distinct().Count() == 1)
                    {
                        foreach (var loser in losers)
                        {
                            loser.MoveStatus = MoveStatus.STAY;
                            loser.MoveSteps.Insert(currentStepIndex, loser.MoveSteps[currentStepIndex]);
                        }
                    }
                    else
                    {
                        winner.MoveSteps = winner.MoveSteps.Take((currentStepIndex + 1) + 1).ToList();
                        winner.MoveStatus = MoveStatus.STOPPED;
                        foreach (var loser in losers)
                        {
                            loser.MoveStatus = MoveStatus.STOPPED;
                            loser.MoveSteps = loser.MoveSteps.Take(currentStepIndex + 1).ToList();
                        }
                    }
                }
            }
            return actors;
        }

        public List<MoveFrame> Record(List<Actor> actors)
        {
            var isAllMoved = actors.Where(a => a.IsMoveEnd).Count() == actors.Count();
            var isAllStayed = actors.Where(a => !a.IsMoveEnd).Select(a => a.MoveStatus).Distinct().First() == MoveStatus.STAY;

            var index = 0;
            var records = new List<MoveFrame> { new MoveFrame(index, actors) };

            while (!isAllMoved && !isAllStayed)
            {
                actors = NextStep(index, actors);
                index++;
                records.Add(new MoveFrame(index, actors));

                isAllMoved = actors.Where(a => a.IsMoveEnd).Count() == actors.Count();
                isAllStayed = actors.Where(a => !a.IsMoveEnd).Select(a => a.MoveStatus).Distinct().First() == MoveStatus.STAY;
            }

            return records;
        }
    }
}
