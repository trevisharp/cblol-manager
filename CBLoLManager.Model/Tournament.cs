using System;
using System.Linq;

namespace CBLoLManager.Model;

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

    public void AddWin(Team team)
    {
        for (int i = 0; i < Teams.Length; i++)
        {
            if (Teams[i] == team)
            {
                Wins[i]++;
                return;
            }
        }
    }
}