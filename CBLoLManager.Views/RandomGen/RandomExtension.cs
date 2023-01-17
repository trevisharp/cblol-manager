using System.Linq;
using System.Collections.Generic;

namespace CBLoLManager.RandomGen;

using Model;

public static class RandExtension
{
    public static Player Rand(
        this IEnumerable<Player> players,
        Position role,
        int seed = 0
    ) => players
        .Where(p => p.Role == role)
        .Skip(seed)
        .FirstOrDefault();
    
    public static Organization Rand(
        this IEnumerable<Organization> orgs,
        int seed
    ) => orgs
        .Skip(seed)
        .FirstOrDefault();

    public static Team Rand(
        this IEnumerable<Organization> orgs,
        IEnumerable<Player> players,
        int seed
    ) => new Team()
    {
        AdCarry = players.Rand(Position.AdCarry, seed),
        TopLaner = players.Rand(Position.TopLaner, seed),
        MidLaner = players.Rand(Position.MidLaner, seed),
        Support = players.Rand(Position.Support, seed),
        Jungler = players.Rand(Position.Jungler, seed),
        Organization = orgs.Rand(seed),
    };

    public static Champion Rand(
        this IEnumerable<Champion> champions,
        int seed
    ) => champions
        .Skip(seed)
        .FirstOrDefault();

    public static List<Champion> Rand(
        this IEnumerable<Champion> champions,
        int seed,
        int count
    ) => champions
        .Skip(seed)
        .Take(count)
        .ToList();

    public static DraftResult Rand(
        this IEnumerable<Organization> orgs,
        IEnumerable<Player> players,
        IEnumerable<Champion> champions,
        int seed1,
        int seed2
    )
    {
        var result = new DraftResult()
        {
            TeamA = orgs.Rand(players, seed1),
            TeamB = orgs.Rand(players, seed2),
        };
        result.TeamADraft.AddRange(champions.Rand(seed1, 5));
        result.TeamBDraft.AddRange(champions.Rand(seed2 + 5, 5));

        return result;
    }
}