using System;
using System.Linq;

namespace CBLoLManager.GameRule;

using Model;

public static class TournamentExtension
{
    public static Team SimulateRound(this Tournament tournament)
    {
        tournament.Round++;
        MatchSystem sys = new MatchSystem();
        Team mainOponent = null;

        var indexes = Enumerable.Range(0, 10).ToList();
        int r = tournament.Round - 1;
        int lastIndex = indexes.Count - 1;
        for (int i = 0; i < r; i++)
        {
            int last = indexes[lastIndex];
            indexes.RemoveAt(lastIndex);
            indexes.Insert(1, last);
        }

        for (int i = 0; i < tournament.Teams.Length / 2; i++)
        {
            var teamIndex = indexes[i];
            var opoenentIndex = indexes[tournament.Teams.Length - 1 - i];
            var team = tournament.Teams[teamIndex];
            var oponent = tournament.Teams[opoenentIndex];

            if (team == Game.Current.Team)
            {
                mainOponent = oponent;
                continue;
            }
            else if (oponent == Game.Current.Team)
            {
                mainOponent = team;
                continue;
            }
            
            if (sys.Getwinner(team, oponent))
                tournament.Wins[teamIndex]++;
            else tournament.Wins[opoenentIndex]++;
        }

        return mainOponent;
    }
}