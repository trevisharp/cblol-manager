using static System.MathF;

namespace CBLoLManager.GameRule;

using Model;

public static class PopularitySystem
{
    public static void OnWin(Team team)
    {
        var pop = team.Popularity;

        pop += 2 * sigmoid(-pop / 10);

        team.Popularity = pop > 100 ? 100 : pop;
    }

    public static void OnPlayOffWin(Team team)
    {
        var pop = team.Popularity;

        pop += 4 * sigmoid(-pop / 20);

        team.Popularity = pop > 100 ? 100 : pop;
    }

    public static void OnSecondPlace(Team team)
    {
        var pop = team.Popularity;

        pop += 6 * sigmoid(-pop / 25);

        team.Popularity = pop > 100 ? 100 : pop;
    }

    public static void OnChampion(Team team)
    {
        var pop = team.Popularity;

        pop += 8 * sigmoid(-pop / 50);

        team.Popularity = pop > 100 ? 100 : pop;
    }

    private static float sigmoid(float x)
        => 1 / (1 + Exp(-x));
}