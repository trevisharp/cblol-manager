using System;
using System.Linq;
using System.Collections.Generic;

namespace CBLoLManager.GameRule;

using Model;
using Util;

public class ProposeSystem
{
    public Team TestTransference(Contract contract)
    {
        if (!Game.Current.SeeingProposes.Exists(p => p == contract.Player))
            return null;
        
        var split = Game.Current.Week / 26;
        
        var contractQuery =
            from c in Game.Current.Contracts
            where c.Closed
            where c.End > split
            where c.Team != contract.Team
            select c;
        
        var oldContract = contractQuery
            .LastOrDefault();
        
        if (oldContract == null)
            return null;

        var fee = oldContract.RescissionFee;
        contract.Team.Money -= fee;
        oldContract.Team.Money += fee;
        Game.Current.Contracts.Remove(oldContract);

        return oldContract.Team;
    }

    public Contract TryExtendContract(Contract contract)
    {
        Propose propose = new Propose();
        propose.Player = contract.Player;
        propose.RescissionFee = contract.RescissionFee;
        propose.Team = contract.Team;
        propose.Wage = contract.Wage;
        propose.Round = 1;
        propose.Time = 2;
        
        var newContract = MakeContract(propose);
        return newContract;
    }

    public float AcceptChance(Propose propose)
    {
        if (propose.Time < 1f || propose.Wage < 1f)
            return 0f;

        var sillyPower = sigmoid(propose.Team.SillyPower(), .6f, .9f);

        var avaliation = (
            propose.Player.LanePhase + propose.Player.Leadership +
            propose.Player.MechanicSkill + propose.Player.Mentality +
            propose.Player.TeamFigth + propose.Player.GameVision
        ) / 600f;
        var deviation = (avaliation - .70f) / .05f;
        var deviationPower = deviation * deviation;
        var expectedValue = deviationPower * 1000f + 1000f;
        var diff = propose.Wage - expectedValue;
        var diffPower = diff > 0 ? 1f : sigmoid(diff / expectedValue, -.2f, -.1f);

        var roundPower = sigmoid(propose.Round, -2, 4);

        var rescissionFeePound = 1 - (propose.RescissionFee - 6 * expectedValue) / (6 * expectedValue);
        var condition1Power = sigmoid(rescissionFeePound, -1, 0);

        var timePound = 2 - propose.Time;
        var condition2Power = sigmoid(timePound, -4, 0);

        return diffPower * condition1Power * condition2Power * sillyPower * roundPower;
    }

    public bool TryAccept(Propose propose)
    {
        var prob = AcceptChance(propose);
        return Random.Shared.NextSingle() < prob;
    }

    public Contract MakeContract(Propose propose)
    {
        if (propose == null)
            return null;

        if (propose.Time < 1f || 
            propose.Wage < 1f ||
            propose.RescissionFee < 1f)
            return null;
        
        bool first = true;

        while (!TryAccept(propose))
        {
            first = false;
            propose.Wage *= 1f + Random.Shared.NextSingle() / 2;
            propose.RescissionFee *= 0.9f - Random.Shared.NextSingle() / 10;
        }

        Contract contract = new Contract();
        contract.Player = propose.Player;
        contract.Team = propose.Team;
        contract.Wage = propose.Wage;
        contract.RescissionFee = propose.RescissionFee;
        contract.End = propose.Time + Game.Current.Week / 26;
        contract.Accepted = first;

        Game.Current.Contracts.Add(contract);

        return contract;
    }

    public Propose MakeRandomPropose(Team team, Position role, int round)
    {
        var players = Game.Current.PlayersInMarket
            .Where(p => p.Role == role)
            .OrderByDescending(p => 
                p.LanePhase + p.Leadership +
                p.MechanicSkill + p.Mentality +
                p.TeamFigth + p.GameVision
            );
        
        if (players.Count() == 0)
            return null;

        var player = players
            .Skip(Random.Shared.Next(players.Count() / 5))
            .FirstOrDefault();
        
        return MakeRandomPropose(team, player, round);
    }

    public Propose MakeRandomPropose(Team team, Player player, int round)
    {
        var budget = team.Money / 100 //~(2 splits + 1 future) * (5 players) * (6 months)
            * (3 + 4 * Random.Shared.NextSingle()) / 5; // random weigth [0.6 - 1.4]
        budget = sigmoid(budget, 40000, 10000, -2);

        Propose propose = new Propose();

        propose.Player = player;
        propose.Team = team;
        propose.Round = round;
        propose.Time = Random.Shared.Next(4) + 1;
        propose.Wage = budget;
        propose.RescissionFee = propose.Wage * 2 * (2 * Random.Shared.NextSingle() + 1f);

        return propose;
    }

    public (List<string>, Contract) RunRound(Propose yourPorpose)
    {
        List<string> events = new List<string>();

        Contract yourContract = null;
        if (yourPorpose != null)
            yourContract = MakeContract(yourPorpose);

        int round = yourPorpose?.Round ?? 15;

        List<Position> list = new List<Position>()
        {
            Position.MidLaner, Position.MidLaner, Position.MidLaner,
            Position.TopLaner, Position.TopLaner, Position.TopLaner,
            Position.Jungler, Position.Jungler, Position.Jungler,
            Position.AdCarry, Position.AdCarry, Position.AdCarry,
            Position.Support, Position.Support, Position.Support,
        }
        .OrderBy(x => Random.Shared.Next())
        .ToList();

        foreach (var team in Game.Current.Others
            .OrderByDescending(t => t.Money))
        {
            int i = 0;
            var players = team.GetAll();
            Propose propose = null;
            while (i < list.Count)
            {
                if (players.Select(p => p.Role).Contains(list[i]))
                {
                    i++;
                    continue;
                }

                var contractQuery = 
                    from con in Game.Current.Contracts
                    where !con.Closed
                    where con.Team == team
                    where con.Player.Role == list[i]
                    select con.Player;
                
                var player = contractQuery
                    .FirstOrDefault();
                
                propose = player is null ?
                    MakeRandomPropose(team, list[i], round) :
                    MakeRandomPropose(team, player, round);
                
                list.RemoveAt(i);
                break;
            }

            if (propose == null)
            {
                events.Add($"{team.Organization.Name} não fez movimentações no mercado.");
                continue;
            }
            
            var c = MakeContract(propose);

            string formatedPlayer = Formatter.FormatPlayer(c.Player.Nickname, c.Player.Name);

            if (c.Accepted)
            { 
                var bestQuery = 
                    from con in Game.Current.Contracts
                    where !con.Closed
                    where con.Player == c.Player
                    where con.Wage > c.Wage
                    select con;
                
                if (bestQuery.Count() > 0)
                {
                    var bestContract = bestQuery
                        .OrderByDescending(c => c.Wage)
                        .FirstOrDefault();
                    
                    c = bestContract;
                    events.Add($"{team.Organization.Name} tentou contratar {formatedPlayer}," +
                        $" mas {c.Team.Organization.Name} teve uma proposta melhor e ficou com o jogador!.");
                }
                else events.Add($"{c.Team.Organization.Name} contratou {formatedPlayer}.");

                c.Closed = true;
                c.Team.Add(c.Player);

                // Player Seeing Proposes
                var oldTeam = TestTransference(c);
                if (oldTeam != null)
                    events.Add($"{formatedPlayer} saiu da {oldTeam.Organization.Name}.");

                Game.Current.FreeAgent.Remove(c.Player);
                Game.Current.SeeingProposes.Remove(c.Player);
                Game.Current.EndContract.Remove(c.Player);
            }
            else
            {
                events.Add($"{team.Organization.Name} está em negociação com {formatedPlayer}.");
            }
        }

        return (events, yourContract);
    }

    public void Complete()
    {   
        while (!Game.Current.EndContractStep)
            foreach (var team in Game.Current.Others
                .OrderByDescending(t => t.Money))
            {
                if (team.AdCarry == null)
                {
                    var contract = MakeContract(MakeRandomPropose(team, Position.AdCarry, 15));
                    
                    if (contract == null)
                        return;
                    
                    contract.Accepted = true;
                    contract.Closed = true;
                    team.Add(contract.Player);
                    Game.Current.FreeAgent.Remove(contract.Player);
                    Game.Current.SeeingProposes.Remove(contract.Player);
                    Game.Current.EndContract.Remove(contract.Player);
                }
                else if (team.TopLaner == null)
                {
                    var contract = MakeContract(MakeRandomPropose(team, Position.TopLaner, 15));

                    if (contract == null)
                        return;
                    
                    contract.Accepted = true;
                    contract.Closed = true;
                    team.Add(contract.Player);
                    Game.Current.FreeAgent.Remove(contract.Player);
                    Game.Current.SeeingProposes.Remove(contract.Player);
                    Game.Current.EndContract.Remove(contract.Player);
                }
                else if (team.MidLaner == null)
                {
                    var contract = MakeContract(MakeRandomPropose(team, Position.MidLaner, 15));
                    
                    if (contract == null)
                        return;
                    
                    contract.Accepted = true;
                    contract.Closed = true;
                    team.Add(contract.Player);
                    Game.Current.FreeAgent.Remove(contract.Player);
                    Game.Current.SeeingProposes.Remove(contract.Player);
                    Game.Current.EndContract.Remove(contract.Player);
                }
                else if (team.Jungler == null)
                {
                    var contract = MakeContract(MakeRandomPropose(team, Position.Jungler, 15));
                    
                    if (contract == null)
                        return; 
                    
                    contract.Accepted = true;
                    contract.Closed = true;
                    team.Add(contract.Player);
                    Game.Current.FreeAgent.Remove(contract.Player);
                    Game.Current.SeeingProposes.Remove(contract.Player);
                    Game.Current.EndContract.Remove(contract.Player);
                }
                else if (team.Support == null)
                {
                    var contract = MakeContract(MakeRandomPropose(team, Position.Support, 15));
                    
                    if (contract == null)
                        return;
                    
                    contract.Accepted = true;
                    contract.Closed = true;
                    team.Add(contract.Player);
                    Game.Current.FreeAgent.Remove(contract.Player);
                    Game.Current.SeeingProposes.Remove(contract.Player);
                    Game.Current.EndContract.Remove(contract.Player);
                }
            }
    }

    private float sigmoid(float x, float zp, float mp)
    {
        const float maxPoint = 4.5f;
        // simoid(f(zp)) = 0.5 -> a zp + b = 0
        // sigmoid(f(mp)) = ~0.99 -> a mp + b = maxPoint
        // b = - a zp
        // a mp - a zp = maxPoint
        // a = 5 / (mp - zp)
        float a = maxPoint / (mp - zp);
        float b = -a * zp;
        return sigmoid(x, 1f, 1 / a, b);
    }

    private float sigmoid(float x, float K, float a, float b)
    {
        return K / (1f + MathF.Exp(-(x / a + b)));
    }
}