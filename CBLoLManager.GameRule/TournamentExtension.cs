using System;

namespace CBLoLManager.GameRule;

using Model;

public static class TournamentExtension
{
    public static Team SimulateRound(this Tournament tournament)
    {
        tournament.Round++;
        MatchSystem sys = new MatchSystem();
        Team mainOponent = null;

        bool[] hasGame = new bool[tournament.Teams.Length];
        int crrTeam = 0;
        int crrOponent = 0;

        for (int i = 0; i < tournament.Teams.Length / 2; i++)
        {
            if (hasGame[crrTeam])
            {
                crrTeam = (crrTeam + 1) % tournament.Teams.Length;
                continue;
            }
            
            crrOponent = (crrTeam + tournament.Round) % tournament.Teams.Length;

            hasGame[crrTeam] = true;
            hasGame[crrOponent] = true;

            var team = tournament.Teams[crrTeam];
            var oponent = tournament.Teams[crrOponent];

            if (team == Game.Current.Team)
            {
                mainOponent = oponent;
                crrTeam = (crrOponent + 1) % tournament.Teams.Length;
                continue;
            }
            else if (oponent == Game.Current.Team)
            {
                mainOponent = team;
                crrTeam = (crrOponent + 1) % tournament.Teams.Length;
                continue;
            }
            
            if (sys.Getwinner(team, oponent))
                tournament.Wins[crrTeam]++;
            else tournament.Wins[crrOponent]++;

            crrTeam = (crrOponent + 1) % tournament.Teams.Length;
        }

        if (mainOponent == null)
            throw new Exception("Não foi possível encontrar um oponente");

        return mainOponent;
    }
}