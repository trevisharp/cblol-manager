using System;
using System.Linq;
using System.Collections.Generic;

namespace CBLoLManager.GameRule;

using Model;

public class SponsorshipSystem
{
    public Sponsorship[] GetMain(Team team)
    {
        var sponsors = Sponsors.All
            .Where(s => s.isMain)
            .Zip(new int[] { 2, 4, 6, 8, }
                .OrderBy(n => Random.Shared.Next()));
        return build(team, sponsors)
            .OrderBy(s => s.Value)
            .ToArray();
    } 

    public Sponsorship[] GetSecond(Team team)
    {
        var sponsors = Sponsors.All
            .Where(s => !s.isMain)
            .Zip(new int[] { 1, 2, 3, 4, }
                .OrderBy(n => Random.Shared.Next()));
        return build(team, sponsors)
            .OrderBy(s => s.Value)
            .ToArray();
    }

    private Sponsorship[] build(Team team, IEnumerable<(Sponsor, int)> sponsors)
    {
        return sponsors
            .Select(x =>
            {
                var sponsor = x.Item1;
                var duration = x.Item2;
                float rand = Random.Shared.NextSingle();

                float durationForce = 0;
                for (int i = 0; i < duration; i++)
                    durationForce += (10 - i);

                float baseValue = 
                    50 +
                    50 * rand + 
                    6 * team.Popularity + 
                    (sponsor.isMain ? 300 : 0);

                float value = 25 * durationForce * baseValue;

                Sponsorship sponsorship = new Sponsorship();

                sponsorship.Sponsor = sponsor;
                sponsorship.Start = Game.Current.Week;
                sponsorship.Value = value;
                sponsorship.Duration = 26 * duration - 1;

                return sponsorship;
            }).ToArray();
    }
}