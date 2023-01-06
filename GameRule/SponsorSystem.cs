using System;
using System.Linq;
using System.Collections.Generic;

namespace CBLoLManager.GameRule;

using Model;

public class SponsorshipSystem
{
    public Sponsorship[] GetMain(Team team)
    {
        return Sponsors.All
            .Where(s => s.isMain)
            .Select(s =>
            {
                int risk = Random.Shared.Next(4) + 1;
                float baseValue = 50 +
                    7.5f * team.Popularity + 
                    50 * s.Strong;
                
                int riskPower = 0;
                for (int i = risk, j = 4; i > 0; i--, j--)
                    riskPower += j;

                float value = 20 * baseValue * riskPower;

                Sponsorship sponsorship = new Sponsorship();

                sponsorship.Sponsor = s;
                sponsorship.Start = Game.Current.Week;
                sponsorship.Value = value;
                sponsorship.Duration = 24 * risk;

                return sponsorship;
            }).ToArray();
    } 

    public Sponsorship[] GetSecond(Team team)
    {
        return Sponsors.All
            .Where(s => !s.isMain)
            .Select(s =>
            {
                int risk = Random.Shared.Next(2) + 1;
                float baseValue = 50 +
                    7.5f * team.Popularity + 
                    50 * s.Strong;
                
                int riskPower = 0;
                for (int i = risk, j = 2; i > 0; i--, j--)
                    riskPower += j;

                float value = 20 * baseValue * riskPower;

                Sponsorship sponsorship = new Sponsorship();

                sponsorship.Sponsor = s;
                sponsorship.Start = Game.Current.Week;
                sponsorship.Value = value;
                sponsorship.Duration = 24 * risk;

                return sponsorship;
            }).ToArray();
    }
}