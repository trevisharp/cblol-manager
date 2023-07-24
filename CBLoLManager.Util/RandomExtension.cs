using System.Linq;
using System.Collections.Generic;

namespace CBLoLManager.Util;

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
        Position role,
        int seed
    ) => champions
        .Where(c => c.Role == role)
        .Skip(seed)
        .FirstOrDefault();

    public static IEnumerable<Champion> Rand(
        this IEnumerable<Champion> champions,
        int seed
    )
    {
        yield return champions
            .Where(c => c.Role == Position.TopLaner)
            .Skip(seed)
            .FirstOrDefault();
        yield return champions
            .Where(c => c.Role == Position.Jungler)
            .Skip(seed)
            .FirstOrDefault();
        yield return champions
            .Where(c => c.Role == Position.MidLaner)
            .Skip(seed)
            .FirstOrDefault();
        yield return champions
            .Where(c => c.Role == Position.AdCarry)
            .Skip(seed)
            .FirstOrDefault();
        yield return champions
            .Where(c => c.Role == Position.Support)
            .Skip(seed)
            .FirstOrDefault();
    }

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
        result.TeamADraft.AddRange(champions.Rand(seed1));
        result.TeamBDraft.AddRange(champions.Rand(seed2));

        return result;
    }
}