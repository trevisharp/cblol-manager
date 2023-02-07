using System;
using System.Linq;
using System.Collections.Generic;

namespace CBLoLManager.GameRule;

using Model;
using Util;

[Serializable]
public class Game
{
    private static Game crr = new Game();
    public static Game Current => crr;
    public Game() { }

    public Team Team { get; set;}
    public List<Team> Others { get; set; } = new List<Team>();

    public Tournament Tournament { get; set; }

    public void EndSeason()
    {
        Week = 25;
    }
    
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

    public int Week { get; set; } = 1;

    public WeekEvent CurrentWeekEvent
    {
        get
        {
            int week = (Week - 1) % 24;

            if (week == 0)
            {
                return WeekEvent.Sponsorship;
            }
            else if (week == 1)
            {
                return WeekEvent.MarketWeek;
            }
            else if (week < 24)
            {
                return WeekEvent.GameEvent;
            }
            else
            {
                return WeekEvent.GameEvent;
            }
        }
    }

    public static void New()
        => crr = new Game();
    
    public static void Save()
        => Serializer.Save("save.cblol", crr);

    public static void Load()
        => crr = Serializer.Load<Game>("save.cblol");
}