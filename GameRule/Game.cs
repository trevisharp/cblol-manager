using System;
using System.Linq;
using System.Collections.Generic;

namespace CBLoLManager.GameRule;

using Model;

[Serializable]
public class Game
{
    private static Game crr = new Game();
    public static Game Current => crr;
    private Game() { }

    public Team Team { get; set;}
    public List<Team> Others { get; set; } = new List<Team>();
    
    public List<Player> FreeAgent { get; set; } = new List<Player>();
    public List<Player> EndContract { get; set; } = new List<Player>();
    public List<Player> SeeingProposes { get; set; } = new List<Player>();

    public IEnumerable<Player> PlayersInMarket =>
        FreeAgent.Union(EndContract).Union(SeeingProposes);

    public List<Contract> Contracts { get; set; } = new List<Contract>();

    public bool EndContractStep
    {
        get
        {
            if (!EndContractStepForPlayer)
                return false;
            
            foreach (var x in Others)
            {
                if (x.GetAll().Count < 5)
                    return false;
            }

            return true;
        }
    }
    
    public bool EndContractStepForPlayer
    {
        get
        {
            if (Team.GetAll().Count < 5)
                return false;

            return true;
        }
    }

    public int Round { get; set; } = 1;

    public static void New()
        => crr = new Game();
    
    public static void Save()
    {

    }

    public static void Load()
    {

    }
}