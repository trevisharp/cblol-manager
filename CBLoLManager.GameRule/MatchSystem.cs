using System;

namespace CBLoLManager.GameRule;

using Model;

public class MatchSystem
{
    public bool GetWinner(Team a, Team b)
    {
        var avgA = TeamAverage(a);
        var avgB = TeamAverage(b);
        var aWinChance = winChance(avgA, avgB);

        var value = 100 * Random.Shared.NextSingle();
        return value < aWinChance;
    }

    private float TeamAverage(Team team)
    {
        var avgplayer = team.AveragePlayer();
        var avg = (
            avgplayer.LanePhase +
            avgplayer.Mentality +
            avgplayer.GameVision +
            avgplayer.MechanicSkill +
            avgplayer.Leadership +
            avgplayer.TeamFigth) / 6f;
        return avg;
    }

    private float winChance(float a, float b)
    {
        if (a > b)
          return winHigherChance(a, b);
        return 100f - winHigherChance(a, b);
    }
    
    private float winHigherChance(float a, float b)
    {
        float diff = a - b;
        float prob = 101 - 100 / (1 + MathF.Pow(1.25f, diff));
        return prob > 100 ? 100 : prob;
    }
}
