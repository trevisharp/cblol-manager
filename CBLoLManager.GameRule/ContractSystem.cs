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
        
    }

    public Contract TryKeepContract(Contract oldContract)
    {
        var newContract = new Contract();

        newContract.Team = oldContract.Team;
        newContract.Player = oldContract.Player;
        newContract.RescissionFee = oldContract.RescissionFee;
        newContract.Wage = oldContract.Wage;
        newContract.End = oldContract.End + 2;
        newContract.Accepted = false;

        return newContract;
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
}