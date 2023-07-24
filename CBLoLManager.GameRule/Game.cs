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
    public Game() { }

    public int StartYear { get; private set; } = DateTime.Now.Year;
    public int BaseGold { get; set; } = 300_000;
    public Team Team { get; set;}
    public int Week { get; set; }
    public List<Team> Others { get; private set; } = new List<Team>();
    public IEnumerable<Team> Teams => Others.Prepend(Team);

    public Tournament Tournament { get; private set; }
    
    public List<Player> FreeAgent { get; private set; } = new List<Player>();
    public List<Player> EndContract { get; private set; } = new List<Player>();
    public List<Player> SeeingProposes { get; private set; } = new List<Player>();

    public IEnumerable<Player> PlayersInMarket =>
        FreeAgent.Union(EndContract).Union(SeeingProposes);

    public List<Contract> Contracts { get; private set; } = new List<Contract>();

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

    public WeekEvent CurrentWeekEvent
    {
        get
        {
            int week = Week % 26;

            if (week == 0)
                return WeekEvent.Sponsorship;
            else if (week == 1)
                return WeekEvent.MarketWeek;
            else if ((week > 1 && week < 12) || (week > 12 && week < 18))
                return WeekEvent.GameEvent;
            else if (week == 12)
                return WeekEvent.None;
            else if (week == 24)
                return WeekEvent.AdministrativeWeek;
            else if (week == 25)
                return WeekEvent.ContractWeek;
            else
                return WeekEvent.None;
        }
    }

    public void EndSeason()
    {
        Week += 26 - (Week % 26);
        Week -= 2; // Go To Administrative Week
    }

    public void Init(Organization org)
    {
        Team team = new Team();
        team.Organization = org;

        this.Team = team;
        team.Money = BaseGold;
        
        List<float> moneys = new List<float>()
        {
            BaseGold, 
            2 * BaseGold, 2 * BaseGold,
            3 * BaseGold, 3 * BaseGold, 
            5 * BaseGold, 5 * BaseGold,
            8 * BaseGold, 8 * BaseGold
        }
        .OrderBy(x => Random.Shared.Next())
        .ToList();
        
        foreach (var x in Organizations.All
            .Where(o => o.Name != org.Name))
        {
            this.Others.Add(new Team()
            {
                Organization = x,
                Money = moneys[0]
            });
            moneys.RemoveAt(0);
        }

        this.NewTournament();
        this.FreeAgent.AddRange(Players.All);

        MakeAdministrativeWeek(true);
    }

    public void NewTournament()
    {
        this.Tournament = new Tournament(
            this.Others.Append(
                this.Team
            ).ToArray()
        );
    }

    public void MakeAdministrativeWeek(bool firstTime = false)
    {
        TeamHistorySystem sys = new TeamHistorySystem();
        foreach (var team in Teams)
        {
            var hist = sys.Create(team, firstTime);
            team.History.Add(hist);

            if (firstTime)
                continue;
            
            team.Money += hist.CBLoLAward + 200 * hist.ShirtSale;
        }
    }

    public static void New()
        => crr = new Game();
    
    public static void Save()
        => Serializer.Save("save.cblol", crr);

    public static void Load()
        => crr = Serializer.Load<Game>("save.cblol");
}