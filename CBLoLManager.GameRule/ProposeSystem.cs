using System;
using System.Linq;
using System.Collections.Generic;

namespace CBLoLManager.GameRule;

using Model;
using Util;

// TODO: Implementar saída de um time
// TODO: Abater custo da multa recisória
public class ProposeSystem
{
    public float AcceptChance(Propose propose)
    {
        if (propose.Time < 1f || propose.Wage < 1f ||
            propose.RescissionFee < 1f)
            return 0f;

        var sillyPower = propose.Team.SillyPower();

        var avaliation = (
            propose.Player.LanePhase + propose.Player.Leadership +
            propose.Player.MechanicSkill + propose.Player.Mentality +
            propose.Player.TeamFigth + propose.Player.GameVision
        ) / 600f;
        var deviation = (avaliation - .70f) / .05f;
        var deviationPower = deviation * deviation;
        var expectedValue = deviationPower * 1000f + 1000f;
        var diff = propose.Wage - expectedValue;
        var diffPower = .25f + diff / 5000f;

        var roundPower = propose.Round / 15f;

        var rescissionFeePound = expectedValue * 12 - propose.RescissionFee;
        var condition1Power = .5f + rescissionFeePound / 120000f;

        var timePound = 2f - propose.Time;
        var condition2Power = .5f + propose.Time / 4f;

        return 
            .5f * diffPower + 
            .1f * roundPower + 
            .1f * condition1Power + 
            .1f * condition2Power + 
            .2f * sillyPower;
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
            propose.Wage *= 1f + Random.Shared.NextSingle();
            propose.RescissionFee *= 1.1f;
        }

        Contract contract = new Contract();
        contract.Player = propose.Player;
        contract.Team = propose.Team;
        contract.Wage = propose.Wage;
        contract.RescissionFee = propose.RescissionFee;
        contract.End = propose.Time + Game.Current.Week / 24;
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
        {
            return null;
        }

        var player = players
            .Skip(Random.Shared.Next(players.Count() / 2))
            .FirstOrDefault();
        
        var budget = team.Money
            / 3 // 2 splits + 1 future
            / 5 // 5 players
            / 6
            * (4 + 2 * Random.Shared.NextSingle()) / 5; // random weigth [0.8 - 1.2]

        Propose propose = new Propose();

        propose.Player = player;
        propose.Team = team;
        propose.Round = round;
        propose.Time = Random.Shared.Next(4) + 1;
        propose.Wage = budget;
        propose.RescissionFee = propose.Wage *
            ((2 * Random.Shared.NextSingle() + 1f) * 6 * propose.Time);

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
            Propose propose = null;
            while (i < list.Count)
            {
                if (team.GetAll().Select(p => p.Role).Contains(list[i]))
                {
                    i++;
                    continue;
                }

                propose = MakeRandomPropose(team,
                    list[i], round);
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
                c.Closed = true;
                team.Add(c.Player);
                Game.Current.FreeAgent.Remove(c.Player);
                Game.Current.SeeingProposes.Remove(c.Player);
                Game.Current.EndContract.Remove(c.Player);
                events.Add($"{team.Organization.Name} contratou {formatedPlayer}.");
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
}