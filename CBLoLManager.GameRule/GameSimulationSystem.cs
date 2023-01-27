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

    public float TeamAGold
        => draft.TeamA.GetAll().Sum(x => gold[x]);
    public float TeamBGold
        => draft.TeamB.GetAll().Sum(x => gold[x]);
    public int TeamAKills
        => draft.TeamA.GetAll().Sum(x => kills[x]);
    public int TeamBKills
        => draft.TeamB.GetAll().Sum(x => kills[x]);
    
    public int Time { get; set; } = 0;
    public int TeamATowers { get; set; } = 0;
    public int TeamBTowers { get; set; } = 0;

    Dictionary<Player, int> life = new Dictionary<Player, int>();
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
        {
            nextFlash.Add(x, 0);
            nextTp.Add(x, int.MaxValue);
            life.Add(x, 100);
            kills.Add(x, 0);
            assits.Add(x, 0);
            deaths.Add(x, 0);
        }
        
        nextTp[draft.TeamA.TopLaner] = 0;
        nextTp[draft.TeamA.MidLaner] = 0;
        nextTp[draft.TeamB.TopLaner] = 0;
        nextTp[draft.TeamB.MidLaner] = 0;
    }

    public string GetFrag(Player p)
        => $"{kills[p]}/{deaths[p]}/{assits[p]}";
    
    public float GetLife(Player p)
        => life[p] / 100f;
    
    public float GetGold(Player p)
        => gold[p];

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
        var players = draft.TeamA.GetAll()
            .Concat(draft.TeamB.GetAll());
        
        foreach (var x in players)
            gold[x] += 4f * calc(x.LanePhase) * duration / 60 / 1000;
        
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
        int diff = 0,
        double defVantage = 1.0)
    {
        var members = teamA.Concat(teamB).Aggregate("", (s, p) => s + p.Nickname + ", ");
        messages.Add($"{members.Substring(members.Length - 2)} estão lutando:");

        // Quanto mais participantes da luta, mais valerá a teamFigth e menos a MechanicSkill
        int count = teamA.Count() + teamB.Count();
        float teamParam = count / 10f;

        // As vantagens iniciais podem ser reduzidas pelo time com boa visão de jogo
        if (diff > 0)
        {
            diff -= teamB.Max(x => x.GameVision) / 2 - 
                teamA.Max(x => x.GameVision) / 8;
        }
        else if (diff < 0)
        {
            diff += teamA.Max(x => x.GameVision) / 2 -
                teamB.Max(x => x.GameVision) / 8;
        }

        double goldDiff = 
            + teamA.Sum(x => gold[x])
            - teamB.Sum(x => gold[x]);
        
        // Resultado da batalha em caso de Commit Total
        // Range (-400 - 400)
        double commit = 
            + Math.Max(Math.Min(diff, 50), -50) 
            + Math.Max(Math.Min(20 * goldDiff, 200), -200)
            + (1f - teamParam) * teamA.Sum(x => x.MechanicSkill) / 5
            + teamParam * teamA.Sum(x => x.TeamFigth) / 5
            + 10 * teamA.Count(x => nextFlash[x] < Time)
            - (1f - teamParam) * teamB.Sum(x => x.MechanicSkill)
            - teamParam * teamB.Sum(x => x.TeamFigth)
            - 20 * teamB.Count(x => nextFlash[x] < Time);
        
        // Considerando vantagem defensiva
        commit = 0.75 * defVantage;
        
        // Representa a percepção do time que está perdendo a luta do seu estado
        double intent = 0;
        if (commit > 0)
        {
            var teamControl = 0.25 + 0.25 * Random.Shared.NextSingle() +
                teamB.MaxBy(x => x.Leadership).GameVision / 100.0 + 
                teamB.MaxBy(x => x.Leadership).Leadership / 200.0 + 
                teamB.Average(x => x.GameVision) / 100.0;
            intent = .4 * commit + .6 * teamControl * commit;
        }
        else
        {
            var teamControl = 0.25 + 0.25 * Random.Shared.NextSingle() +
                teamA.MaxBy(x => x.Leadership).GameVision / 100.0 + 
                teamA.MaxBy(x => x.Leadership).Leadership / 200.0 + 
                teamA.Average(x => x.GameVision) / 100.0;
            intent = .4 * commit + .6 * teamControl * commit;
        }

        // Resultado da luta considera a percepção do time defensor
        // reduzindo os danos de acordo com a intenção defensiva
        double result = intent / 2 + (commit - intent);
        result = Math.Abs(result);

        var winTeam = commit > 0 ? teamA : teamB;
        var loseTeam = commit > 0 ? teamB : teamA;

        while (result > 0)
        {
            result -= 5;
            var rot = loseTeam.Sum(x => life[x]);
            var ran = Random.Shared.NextSingle() * rot;
            foreach (var x in loseTeam)
            {
                ran -= life[x];
                if (ran < 0)
                {
                    life[x] -= 2 + (int)(6 * Random.Shared.NextSingle());
                    break;
                }
            }
            
            rot = winTeam.Sum(x => life[x]);
            ran = Random.Shared.NextSingle() * rot;
            foreach (var x in winTeam)
            {
                ran -= life[x];
                if (ran < 0)
                {
                    life[x] -= 1 + (int)(2 * Random.Shared.NextSingle());
                    break;
                }
            }
        }

        foreach (var x in loseTeam)
        {
            if (life[x] < 1)
            {
                addDeath(x);

                var rot = loseTeam.Sum(x => gold[x]);
                var ran = Random.Shared.NextSingle() * rot;
                foreach (var y in winTeam)
                {
                    ran -= gold[y];
                    if (ran < 0)
                    {
                        addKill(x);
                        break;
                    }
                }
            }
        }
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