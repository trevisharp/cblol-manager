using System;
using System.Linq;

namespace CBLoLManager.GameRule;

using System.Collections.Generic;
using Model;

public class GameSimulationSystem
{
    private DraftResult draft;
    private GameEventSystem evSys;

    private List<string> messages = new List<string>();
    private bool firstBlood = true;
    
    private int aMagicDamage = 0;
    private int aPhysicalDamage = 0;
    private int aDefense = 0;

    private Player pA = null;
    private Player pB = null;

    public float TeamAGold { get; set; } = 2.5f;
    public float TeamBGold { get; set; } = 2.5f;
    public int TeamAKills { get; set; } = 0;
    public int TeamBKills { get; set; } = 0;
    public int Time { get; set; } = 0;
    public int TeamATowers { get; set; } = 0;
    public int TeamBTowers { get; set; } = 0;

    Dictionary<Player, int> kills = new Dictionary<Player, int>();
    Dictionary<Player, int> deaths = new Dictionary<Player, int>();
    Dictionary<Player, int> assits = new Dictionary<Player, int>();
    Dictionary<Player, float> gold = new Dictionary<Player, float>();
    Dictionary<Player, Champion> champs = new Dictionary<Player, Champion>();
    Dictionary<Player, int> nextFlash = new Dictionary<Player, int>();
    Dictionary<Player, int> nextTp = new Dictionary<Player, int>();

    public GameSimulationSystem(DraftResult draft)
    {
        this.draft = draft;
        this.evSys = new GameEventSystem(draft);

        foreach (var pick in draft.TeamADraft)
        {
            if (pick.AD)
                aPhysicalDamage += 1 + 3 * pick.Damage;
            else aMagicDamage += 1 + 3 * pick.Damage;
            aDefense += 1 + 3 * pick.Defence;
        }

        pA = draft.TeamA.AveragePlayer();
        pB = draft.TeamB.AveragePlayer();

        var players = draft.TeamA.GetAll()
            .Concat(draft.TeamB.GetAll());

        foreach (var x in players)
            gold.Add(x, .5f);
        
        foreach (var x in draft.TeamA.GetAll())
            champs.Add(x, draft.TeamADraft.FirstOrDefault(c => c.Role == x.Role));

        foreach (var x in draft.TeamB.GetAll())
            champs.Add(x, draft.TeamBDraft.FirstOrDefault(c => c.Role == x.Role));

        foreach (var x in players)
            nextFlash.Add(x, 0);
        
        nextTp.Add(draft.TeamA.TopLaner, 0);
        nextTp.Add(draft.TeamA.MidLaner, 0);
        nextTp.Add(draft.TeamB.TopLaner, 0);
        nextTp.Add(draft.TeamB.MidLaner, 0);
    }

    public void NextStep()
    {
        if (Time == 0)
        {
            Time = 90;
            return;
        }

        var timeStep = Random.Shared.Next(40, 80);

        if (Time < 300)
            lanePhase(timeStep);


        Time += timeStep;
    }

    private void lanePhase(int timeStep)
    {
        var duration = Time + timeStep > 300 ? 300 - Time : timeStep;
        TeamAGold += 20f * calc(pA.LanePhase) * duration / 60 / 1000;
        TeamBGold += 20f * calc(pB.LanePhase) * duration / 60 / 1000;
        lanePhaseJgEvent();

    }

    private void lanePhaseJgEvent()
    {
        Position gankA = (Position)Random.Shared.Next(5);
        Position gankB = (Position)Random.Shared.Next(5);



    }

    private void simulateGank(
        Player jg, Player[] allies, Player[] enemies, 
        ref float jgdiff, ref float lanediff, Champion jgChamp,
        Champion[] alliesChamps, Champion[] enemiesChamps)
    {
        var gameVision = enemies.Average(x => x.GameVision);
        if (Random.Shared.Next(100) < gameVision / 2)
        {
            messages.Add(
                $"{jg.Nickname} tentou gankar a lane de {enemies[0].Nickname}, mas " +
                "o lance foi antecipado e o lance acabou não ocorrendo"
            );
            jgdiff -= 0.1f;
            lanediff += 0.1f;

            return;
        }
        
        addFigth(
            allies.Append(jg),
            enemies
        );
    }

    private void addFigth(
        IEnumerable<Player> teamA,
        IEnumerable<Player> teamB,
        int diff = 0)
    {
        var members = teamA.Concat(teamB).Aggregate("", (s, p) => s + p.Nickname + ", ");
        messages.Add(@$"Uma luta entre {members.Substring(members.Length - 2)} começou.");

        int count = teamA.Count() + teamB.Count();
        float team = count / 10f;

        if (diff > 0)
        {
            diff -= teamB.Max(x => x.GameVision) / 2;
            if (diff < 0)
                diff = 0;
        }
        else if (diff < 0)
        {
            diff += teamA.Max(x => x.GameVision) / 2;
            if (diff > 0)
                diff = 0;
        }

        double commit = diff
            + (1f - team) * teamA.Sum(x => x.MechanicSkill)
            + team * teamA.Sum(x => x.TeamFigth)
            + 20 * teamA.Sum(x => gold[x])
            + 20 * teamA.Count(x => nextFlash[x] < Time)
            - (1f - team) * teamB.Sum(x => x.MechanicSkill)
            - team * teamB.Sum(x => x.TeamFigth)
            - 20 * teamB.Sum(x => gold[x])
            - 20 * teamB.Count(x => nextFlash[x] < Time);
        
        float ip = Random.Shared.NextSingle();
        double intent = commit > 0
            ? commit - ip * (200 - teamA.Average(x => x.GameVision) - teamA.Max(x => x.Leadership))
            : commit + ip * (200 - teamB.Average(x => x.GameVision) - teamB.Max(x => x.Leadership));


    }

    private void addDeath(Player player)
    {
        if (deaths.ContainsKey(player))
            deaths.Add(player, 1);
        else deaths[player]++;
    }

    private void addKill(Player player)
    {
        if (kills.ContainsKey(player))
            kills.Add(player, 1);
        else kills[player]++;
    }

    private void addAssist(Player player)
    {
        if (assits.ContainsKey(player))
            assits.Add(player, 1);
        else assits[player]++;
    }

    private float calc(int value)
    {
        float force = value + 20 * Random.Shared.NextSingle() - 10;
        return force;
    }
}