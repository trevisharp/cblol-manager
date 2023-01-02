using System.Linq;

namespace CBLoLManager.GameRule;

using Model;

public static class TeamAnalytics
{
    public static float SillyPower(this Team team)
    {
        var players = team.GetAll();
        return players.Sum(p => 
            (p.GameVision +
            p.LanePhase +
            p.MechanicSkill +
            p.TeamFigth) / 400f - .8f);
    }

    public static Player AveragePlayer(this Team team)
    {
        Player player = new Player();

        var players = team.GetAll();

        if (players.Count < 5)
            return player;

        player.Leadership = (int)players
            .OrderByDescending(p => p.Leadership)
            .Take(2)
            .Select((p, i) => i == 0 ?
                p.Leadership * 70f / 100f :
                p.Leadership * 30f / 100f)
            .Sum();
        
        player.LanePhase = (
            2 * team.TopLaner.LanePhase +
            2 * team.MidLaner.LanePhase +
            team.Support.LanePhase + 
            team.AdCarry.LanePhase +
            team.Jungler.LanePhase
        ) / 7;

        player.MechanicSkill = (int)players
            .OrderBy(p => p.MechanicSkill)
            .Select((p, i) => p.MechanicSkill * i)
            .Sum() / 10;
        
        player.GameVision = (int)players
            .OrderByDescending(p => p.Leadership * p.GameVision)
            .Take(3)
            .Append(team.Jungler)
            .Append(team.Support)
            .Select(p => p.GameVision)
            .Average();
        
        player.Mentality = (int)(
            players
            .OrderBy(p => p.Mentality)
            .Take(2)
            .Select(p => (double)p.Mentality)
            .Append(players.Sum(p => p.Mentality))
            .Append(
                players
                    .OrderByDescending(p => p.Leadership)
                    .FirstOrDefault().Mentality
            ).Sum() / 8);

        player.TeamFigth = (int)players
            .Average(p => p.TeamFigth);

        return player;
    }
}