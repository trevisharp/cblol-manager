using System;
using System.Linq;

namespace CBLoLManager.Model;

using GameRule;

[Serializable]
public class Tournament
{
    public int Round { get; set; } = 0;
    public Team[] Teams { get; private set; }
    public int[] Wins { get; private set; }

    public Tournament(Team[] teams)
    {
        this.Teams = teams
            .OrderBy(x => Random.Shared.Next())
            .ToArray();
        
        this.Wins = new int[teams.Length];
    }

    public Team SimulateRound()
    {
        Round++;
        MatchSystem sys = new MatchSystem();
        Team mainOponent = null;

        bool[] hasGame = new bool[Teams.Length];
        int crrTeam = 0;
        int crrOponent = 0;

        for (int i = 0; i < Teams.Length / 2; i++)
        {
            if (hasGame[crrTeam])
            {
                crrTeam = (crrTeam + 1) % Teams.Length;
                continue;
            }
            
            crrOponent = (crrTeam + Round) % Teams.Length;

            hasGame[crrTeam] = true;
            hasGame[crrOponent] = true;

            var team = Teams[crrTeam];
            var oponent = Teams[crrOponent];

            if (team == Game.Current.Team)
            {
                mainOponent = oponent;
                crrTeam = (crrOponent + 1) % Teams.Length;
                continue;
            }
            else if (oponent == Game.Current.Team)
            {
                mainOponent = team;
                crrTeam = (crrOponent + 1) % Teams.Length;
                continue;
            }
            
            if (sys.Getwinner(team, oponent))
                Wins[crrTeam]++;
            else Wins[crrOponent]++;

            crrTeam = (crrOponent + 1) % Teams.Length;
        }

        if (mainOponent == null)
            throw new Exception("Não foi possível encontrar um oponente");

        return mainOponent;
    }
}