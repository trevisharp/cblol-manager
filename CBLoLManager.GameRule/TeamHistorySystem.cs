using System.Linq;

namespace CBLoLManager.GameRule;

using System;
using Model;

// TODO: Inicializar os golds dos times que não o do jogador corretamente 
public class TeamHistorySystem
{
    public TeamHistory Create(Team team, bool first)
    {
        TeamHistory history = new TeamHistory();

        var game = Game.Current;
        int pos = first ? -1 : getPosition(team, game.Tournament);
        int rpos = first ? -1 : getRegularPoisition(team, game.Tournament);
        var hist = first ? null : team.History.MaxBy(x => 3 * x.Year + x.Split);
        double pop = team.Popularity / 100.0;
        const double f = 4;
        double hype = 10 < pos + rpos ? 0 : (20 - 2 * pos - rpos) / (20.0 - 3.0);
        var players = team.GetAll().Select(x => x.Name);

        history.Year = game.StartYear + game.Week / 52;
        history.Split = (game.Week % 52) / 26;
        history.CBLoLAward = first ? 0 : pos switch
        {
            1 => 200_000,
            2 => 150_000,
            3 => 100_000,
            4 => 80_000,
            5 => 70_000,
            6 => 60_000,
            7 => 50_000,
            8 => 40_000,
            9 => 30_000,
            _ => 20_000
        };
        history.Titles = first ? 0 : hist.Titles + (pos == 1 ? 1 : 0);
        history.Vices = first ? 0 : hist.Vices + (pos == 2 ? 1 : 0);
        history.RegularPhaseTitles = first ? 0 : hist.RegularPhaseTitles + (rpos == 1 ? 1 : 0);
        history.PlayOffsPlayed = first ? 0 : hist.PlayOffsPlayed + (pos < 7 ? 1 : 0);
        history.Followers = (int)Math.Pow(10, 3 + f * pop * pop);
        history.ShirtSale = (int)(0.2 * history.Followers * (0.1 + hype));
        history.ShirtSaleTotal = (hist?.ShirtSaleTotal ?? 0) + history.ShirtSale;
        history.InitialGold = first ? Game.Current.BaseGold : 
            hist.InitialGold + hist.CBLoLAward + 200 * hist.ShirtSale - hist.PlayerWage;
        history.PlayerWage = game.Contracts
            .Where(c => players.Contains(c.Player.Name))
            .Select(c => 6 * c.Wage)
            .Sum();

        return history;
    }

    private int getRegularPoisition(Team team, Tournament tournament)
    {        
        var pontuations = tournament.Teams
            .Zip(tournament.Wins)
            .OrderBy(x => x.Second)
            .Select(x => x.First)
            .ToArray();
        
        int pos = 10;
        foreach (var p in pontuations)
        {
            if (p == team)
                return pos;
            
            pos--;
        }

        return 1;
    }

    private int getPosition(Team team, Tournament tournament)
    {
        var playOffs = tournament.PlayOffs;
        if (playOffs.Champion == team)
            return 1;
        
        if (playOffs.FinalA == team || playOffs.FinalB == team)
            return 2;
        
        if (playOffs.LoserBracketFinalA == team || playOffs.LoserBracketFinalB == team)
            return 3;
        
        if (playOffs.LoserBracketThirdPhaseA == team || playOffs.LoserBracketThirdPhaseB == team)
            return 4;
        
        if (playOffs.LoserBracketSecondPhaseA == team || playOffs.LoserBracketSecondPhaseB == team)
            return 5;

        if (playOffs.LoserBracketA == team || playOffs.LoserBracketB == team)
            return 6;
        
        var pontuations = tournament.Teams
            .Zip(tournament.Wins)
            .OrderBy(x => x.Second)
            .Select(x => x.First)
            .Take(3)
            .ToArray();
        
        int pos = 10;
        foreach (var p in pontuations)
        {
            if (p == team)
                return pos;
            
            pos--;
        }

        return 7;
    }
}