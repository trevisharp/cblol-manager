using System;
using System.Linq;

namespace CBLoLManager.GameRule;

using Model;

public static class TournamentExtension
{
    public static void SetWinner(this Tournament tournament, Team winner, Team looser)
    {
        var playoff = tournament.PlayOffs;
        var team = Game.Current.Team;

        if (playoff.FinalA != null && playoff.FinalB != null) // Week 5
        {
            playoff.Champion = winner;
        }
        else if (playoff.LoserBracketFinalA != null && playoff.LoserBracketFinalB != null) // Week 4
        {
            playoff.FinalB = winner;
        }
        else if (playoff.LoserBracketThirdPhaseA != null && playoff.LoserBracketThirdPhaseB != null) // Week 3
        {
            playoff.LoserBracketFinalB = winner;
        }
        else if (playoff.WinnerBracketFinalA != null && playoff.WinnerBracketFinalB != null && 
                playoff.LoserBracketSecondPhaseA != null && playoff.LoserBracketSecondPhaseB != null) // Week 2
        {
            if (winner.Organization.Name == playoff.WinnerBracketFinalA.Organization.Name || 
                winner.Organization.Name == playoff.WinnerBracketFinalB.Organization.Name)
            {
                playoff.FinalA = winner;
                playoff.LoserBracketFinalA = looser;
            }
            else
            {
                playoff.LoserBracketThirdPhaseB = winner;
            }
        }
        else // Week1
        {
            if (winner == playoff.WinnerBracketA || winner == playoff.WinnerBracketB)
            {
                playoff.WinnerBracketFinalA = winner;
                playoff.LoserBracketThirdPhaseA = looser;
            }
            else if (winner == playoff.WinnerBracketC || winner == playoff.WinnerBracketD)
            {
                playoff.WinnerBracketB = winner;
                playoff.LoserBracketSecondPhaseA = looser;
            }
            else
            {
                playoff.LoserBracketSecondPhaseB = winner;
            }
        }
    }

    public static bool StartPlayOffs(this Tournament tournament)
    {   
        var teams = tournament.Teams
            .Zip(tournament.Wins)
            .OrderByDescending(x => x.Second)
            .Take(6)
            .Select(x => x.First)
            .ToArray();
        
        if (tournament.PlayOffs != null)
            return teams.Contains(Game.Current.Team);
        
        PlayOffs playOffs = new PlayOffs();

        playOffs.WinnerBracketA = teams[0];
        playOffs.WinnerBracketB = teams[3];
        playOffs.WinnerBracketC = teams[1];
        playOffs.WinnerBracketD = teams[2];
        playOffs.LoserBracketA = teams[4];
        playOffs.LoserBracketB = teams[5];
        
        tournament.PlayOffs = playOffs;

        return teams.Contains(Game.Current.Team);
    }

    public static Team SimulatePlayOffRound(this Tournament tournament)
    {
        MatchSystem sys = new MatchSystem();
        Team oponent = null;
        (Team, Team)? result = null;

        var playoff = tournament.PlayOffs;
        var team = Game.Current.Team;

        if (playoff.FinalA != null && playoff.FinalB != null) // Week 5
        {
            result = getWinner(playoff.FinalA, playoff.FinalB);
            if (result.HasValue)
            {
                var winner = result.Value.Item1;
                var looser = result.Value.Item2;

                playoff.Champion = winner;
            }
        }
        else if (playoff.LoserBracketFinalA != null && playoff.LoserBracketFinalB != null) // Week 4
        {
            result = getWinner(playoff.LoserBracketFinalA, playoff.LoserBracketFinalB);
            if (result.HasValue)
            {
                var winner = result.Value.Item1;
                var looser = result.Value.Item2;

                playoff.FinalB = winner;
            }
        }
        else if (playoff.LoserBracketThirdPhaseA != null && playoff.LoserBracketThirdPhaseB != null) // Week 3
        {
            result = getWinner(playoff.LoserBracketThirdPhaseA, playoff.LoserBracketThirdPhaseB);
            if (result.HasValue)
            {
                var winner = result.Value.Item1;
                var looser = result.Value.Item2;

                playoff.LoserBracketFinalB = winner;
            }
        }
        else if (playoff.WinnerBracketFinalA != null && playoff.WinnerBracketFinalB != null && 
                playoff.LoserBracketSecondPhaseA != null && playoff.LoserBracketSecondPhaseB != null) // Week 2
        {
            result = getWinner(playoff.WinnerBracketFinalA, playoff.WinnerBracketFinalB);
            if (result.HasValue)
            {
                var winner = result.Value.Item1;
                var looser = result.Value.Item2;

                playoff.FinalA = winner;
                playoff.LoserBracketFinalA = looser;
            }

            result = getWinner(playoff.LoserBracketSecondPhaseA, playoff.LoserBracketSecondPhaseB);
            if (result.HasValue)
            {
                var winner = result.Value.Item1;
                var looser = result.Value.Item2;

                playoff.LoserBracketThirdPhaseB = winner;
            }
        }
        else // Week1
        {
            result = getWinner(playoff.WinnerBracketA, playoff.WinnerBracketB);
            if (result.HasValue)
            {
                var winner = result.Value.Item1;
                var looser = result.Value.Item2;

                playoff.WinnerBracketFinalA = winner;
                playoff.LoserBracketThirdPhaseA = looser;
            }

            result = getWinner(playoff.WinnerBracketC, playoff.WinnerBracketD);
            if (result.HasValue)
            {
                var winner = result.Value.Item1;
                var looser = result.Value.Item2;

                playoff.WinnerBracketFinalB = winner;
                playoff.LoserBracketSecondPhaseA = looser;
            }

            result = getWinner(playoff.LoserBracketA, playoff.LoserBracketB);
            if (result.HasValue)
            {
                var winner = result.Value.Item1;
                var looser = result.Value.Item2;

                playoff.LoserBracketSecondPhaseB = winner;
            }
        }
        
        return oponent;

        (Team, Team)? getWinner(Team a, Team b)
        {
            if (a == team)
            {
                oponent = b;
                return null;
            }

            if (b == team)
            {
                oponent = a;
                return null;
            }

            if (sys.GetWinner(a, b))
                return (a, b);
            
            return (b, a);
        }
    }

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
            
            if (sys.GetWinner(team, oponent))
                tournament.Wins[teamIndex]++;
            else tournament.Wins[opoenentIndex]++;
        }

        return mainOponent;
    }
}