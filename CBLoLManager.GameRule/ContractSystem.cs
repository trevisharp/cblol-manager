using System;
using System.Linq;

namespace CBLoLManager.GameRule;

using Model;

public class ContractSystem
{
    public void Run()
    {
        var mainOrg = Game.Current.Team.Organization.Name;
        foreach (var team in Game.Current.Teams)
        {
            if (team.Organization.Name == mainOrg)
                continue;
            
            MakeContractStep(team);
        }
    }

    public void MakeContractStep(Team team)
    {
        var rand = Random.Shared;
        var players = team.GetAll();

        var contracts = 
            from c in Game.Current.Contracts
            where players.Exists(p => p == c.Player)
            select c;

        int split = Game.Current.Week / 26;
        foreach (var contract in contracts.ToList())
        {
            bool finished = contract.End <= split;

            if (!finished)
            {
                // Keep Player
                if (rand.NextSingle() > .3f)
                    continue;
                
                Game.Current.SeeingProposes
                    .Add(contract.Player);
                team.Remove(contract.Player);

                continue;
            }
            
            var newContract = UpdateContract(contract);

            bool accepted = acceptChance(team, newContract) > rand.NextSingle();
            if (accepted)
            {
                newContract.Closed = true;
                Game.Current.Contracts.Add(newContract);
                return;
            }

            // Give Up
            if (rand.NextSingle() > .7f)
            {
                Game.Current.FreeAgent
                    .Add(contract.Player);
                team.Remove(contract.Player);
                continue;
            }
            
            ProposeSystem psys = new ProposeSystem();
            var extendedContract = psys.TryExtendContract(contract);
            if (extendedContract.Accepted)
            {
                extendedContract.Closed = true;
                continue;
            }
            
            Game.Current.EndContract
                .Add(contract.Player);
            team.Remove(contract.Player);
        }
    }
    
    public void ClearUncloseContracts()
    {
        Game.Current.Contracts.RemoveAll(
            c => !c.Closed
        );
    }

    public Contract UpdateContract(Contract oldContract)
    {
        var team = oldContract.Team;
        var hist = team.History.Last();
        var avgWage = hist.PlayerWage / 5 / 6;
        var sales = hist.ShirtSale;

        var newContract = new Contract();
        newContract.Team = team;
        newContract.Player = oldContract.Player;
        newContract.Accepted = true;

        var rand = Random.Shared;
        var newDuration = rand.Next(1, 5);
        newContract.End = oldContract.End + newDuration;

        var newWage = oldContract.Wage * (1.1f + rand.NextSingle() / 5);
        if (newWage < avgWage)
            newWage += (avgWage - newWage) / 5;
        newWage += MathF.Min(200, sales);
        
        newContract.Wage = newWage;
        newContract.RescissionFee = oldContract.RescissionFee + 2 * newContract.Wage * newDuration;

        return newContract;
    }

    private float acceptChance(Team team, Contract contract)
    {
        var teamMoney = team.Money;
        var paymentParam = 60 * contract.Wage;

        var paymentWeigth = paymentParam / teamMoney;
        var playerWeigth = 1f - (
            contract.Player.LanePhase + contract.Player.Leadership +
            contract.Player.MechanicSkill + contract.Player.Mentality +
            contract.Player.TeamFigth + contract.Player.GameVision
        ) / 600f;

        return sigmoid(playerWeigth - paymentWeigth);
    }

    private float sigmoid(float x)
        => 1 / (1 + MathF.Exp(-x));
}