using System;
using System.Linq;
using System.Collections.Generic;

namespace CBLoLManager.GameRule;

using Model;

// TODO: Build a Ban System
public class DraftSystem
{
    public Team Blue { get; set; }
    public Team Red { get; set; }

    public IEnumerable<Champion> Simulate()
    {
        var opts = Champions.All.ToList();

        var blueToPick = Blue.GetAll().ToList();
        var redToPick = Red.GetAll().ToList();
        
        yield return bluePick();
        yield return redPick();
        yield return redPick();
        yield return bluePick();
        yield return bluePick();
        yield return redPick();
        
        yield return redPick();
        yield return bluePick();
        yield return bluePick();
        yield return redPick();

        Champion bluePick()
        {
            return pick(blueToPick, Blue);
        }

        Champion redPick()
        {
            return pick(redToPick, Red);
        }

        Champion pick(List<Player> toPick, Team team)
        {
            List<Player> poss = new List<Player>();

            var leader = toPick
                .OrderByDescending(p => p.Leadership + p.Mentality)
                .FirstOrDefault();
            poss.Add(leader);

            var best = toPick
                .OrderByDescending(p => p.LanePhase + p.MechanicSkill)
                .FirstOrDefault();
            poss.Add(best);
            
            if (toPick.Contains(team.Support))
                poss.Add(team.Support);
            
            if (toPick.Contains(team.Jungler))
                poss.Add(team.Jungler);
                
            var player = poss[Random.Shared.Next(poss.Count)];

            var pick = opts.Where(c => c.Role == player.Role)
                .OrderBy(c => Random.Shared.Next())
                .FirstOrDefault();
            
            opts.Remove(pick);
            toPick.Remove(player);

            return pick;
        }   
    }
}