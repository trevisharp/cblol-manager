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

    public IEnumerable<Pick> Simulate()
    {
        var opts = Champions.All.ToList();

        var blueToPick = Blue.GetAll().ToList();
        var redToPick = Red.GetAll().ToList();
        
        var pickOpt = bluePick();
        yield return pickOpt;
        opts.Remove(pickOpt.Selected);

        pickOpt = redPick();
        yield return pickOpt;
        opts.Remove(pickOpt.Selected);

        pickOpt = redPick();
        yield return pickOpt;
        opts.Remove(pickOpt.Selected);

        pickOpt = bluePick();
        yield return pickOpt;
        opts.Remove(pickOpt.Selected);

        pickOpt = bluePick();
        yield return pickOpt;
        opts.Remove(pickOpt.Selected);

        pickOpt = redPick();
        yield return pickOpt;
        opts.Remove(pickOpt.Selected);

        pickOpt = redPick();
        yield return pickOpt;
        opts.Remove(pickOpt.Selected);

        pickOpt = bluePick();
        yield return pickOpt;
        opts.Remove(pickOpt.Selected);

        pickOpt = bluePick();
        yield return pickOpt;
        opts.Remove(pickOpt.Selected);

        pickOpt = redPick();
        yield return pickOpt;
        opts.Remove(pickOpt.Selected);

        Pick bluePick()
        {
            return pick(blueToPick, Blue);
        }

        Pick redPick()
        {
            return pick(redToPick, Red);
        }

        Pick pick(List<Player> toPick, Team team)
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

            var pickList = opts.Where(c => c.Role == player.Role)
                .OrderBy(c => Random.Shared.Next())
                .ToArray();

            var pick1 = pickList.FirstOrDefault();
            var pick2 = pickList.Skip(1).FirstOrDefault();
            
            toPick.Remove(player);

            Pick pick = new Pick();
            pick.OptionA = pick1;
            pick.OptionB = pick2;

            return pick;
        }   
    }
}