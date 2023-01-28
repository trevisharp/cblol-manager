using System;
using System.Linq;

namespace CBLoLManager.GameRule;

using System.Collections.Generic;
using Model;

public class GameSimulationSystem
{
    private DraftResult draft;
    private GameEventSystem evSys;

    private bool firstBlood = true;
    
    private int aMagicDamage = 0;
    private int aPhysicalDamage = 0;
    private int aDefense = 0;

    private Player pA = null;
    private Player pB = null;

    public Queue<string> MessageQueue { get; private set; } = new Queue<string>();
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

    Dictionary<Player, int> respawn = new Dictionary<Player, int>();
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
            respawn.Add(x, int.MaxValue);
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

        if (Time < 400)
            lanePhase(timeStep);
        else if (Time < 1000)
            midPhase(timeStep);
        else endPhase(timeStep);

        Time += timeStep;

        var players = draft.TeamA.GetAll()
            .Concat(draft.TeamB.GetAll());
        foreach (var x in players)
        {
            if (Time > respawn[x])
            {
                respawn[x] = int.MaxValue;
                life[x] = 100;
            }
        }
    }

    private void lanePhase(int timeStep)
    {
        var players = draft.TeamA.GetAll()
            .Concat(draft.TeamB.GetAll());
        
        foreach (var x in players)
        {
            var goldGen = x.LanePhase + 20 * Random.Shared.NextSingle() - 10;
            gold[x] += 4f * goldGen * timeStep / 60 / 1000;

            life[x] -= (int)((110 - x.LanePhase) * Random.Shared.NextSingle() / 2);
            if (life[x] < 30)
            {
                MessageQueue.Enqueue($"{x.Nickname} voltou para base.");
                life[x] = 100;
                gold[x] -= 0.5f * goldGen * timeStep / 60 / 1000;
            }
        }
        
        lanePhaseJgEvent();
    }

    private void midPhase(int timeStep)
    {
        var players = draft.TeamA.GetAll()
            .Concat(draft.TeamB.GetAll());
        
        foreach (var x in players)
        {
            var goldGen = x.LanePhase / 2 + 5 * gold[x] + 30 * Random.Shared.NextSingle();
            gold[x] += 4f * goldGen * timeStep / 60 / 1000;

            life[x] -= (int)((110 - x.LanePhase) * Random.Shared.NextSingle() / 4);

            if (life[x] < 80)
                life[x] = 100;
        }
        
        addFigth(
            draft.TeamA.GetAll(), draft.TeamB.GetAll(), 0, 1
        );
    }

    private void endPhase(int timeStep)
    {
        var players = draft.TeamA.GetAll()
            .Concat(draft.TeamB.GetAll());
        
        foreach (var x in players)
        {
            var goldGen = 10 * gold[x] + 40 * Random.Shared.NextSingle();
            gold[x] += 4f * goldGen * timeStep / 60 / 1000;

            life[x] -= (int)((110 - x.LanePhase) * Random.Shared.NextSingle() / 4);

            if (life[x] < 80)
                life[x] = 100;
        }
        
        addFigth(
            draft.TeamA.GetAll(), draft.TeamB.GetAll(), 0, 1
        );
    }

    private void lanePhaseJgEvent()
    {
        Position gankA = (Position)Random.Shared.Next(5);
        Position gankB = (Position)Random.Shared.Next(5);

        if (gankA == gankB)
        {
            switch (gankA)
            {
                case Position.TopLaner:
                    addFigth(new Player[] {
                        draft.TeamA.TopLaner,
                        draft.TeamA.Jungler
                    }, new Player[] {
                        draft.TeamB.TopLaner,
                        draft.TeamB.Jungler
                    }, 0, 0.75);
                    break;
                case Position.MidLaner:
                    addFigth(new Player[] {
                        draft.TeamA.MidLaner,
                        draft.TeamA.Jungler
                    }, new Player[] {
                        draft.TeamB.MidLaner,
                        draft.TeamB.Jungler
                    }, 0, 0.75);
                    break;
                case Position.Support:
                case Position.AdCarry:
                    addFigth(new Player[] {
                        draft.TeamA.AdCarry,
                        draft.TeamA.Support,
                        draft.TeamA.Jungler
                    }, new Player[] {
                        draft.TeamB.AdCarry,
                        draft.TeamB.Support,
                        draft.TeamB.Jungler
                    }, 0, 0.75);
                    break;
                
                default:
                    // Sem Gank
                    break;
            }
        }
        else
        {
            switch (gankA)
            {
                case Position.TopLaner:
                    addFigth(new Player[] {
                        draft.TeamA.TopLaner,
                        draft.TeamA.Jungler
                    }, new Player[] {
                        draft.TeamB.TopLaner
                    }, 50, 0.5);
                    break;
                case Position.MidLaner:
                    addFigth(new Player[] {
                        draft.TeamA.MidLaner,
                        draft.TeamA.Jungler
                    }, new Player[] {
                        draft.TeamB.MidLaner
                    }, 50, 0.5);
                    break;
                case Position.Support:
                case Position.AdCarry:
                    addFigth(new Player[] {
                        draft.TeamA.AdCarry,
                        draft.TeamA.Support,
                        draft.TeamA.Jungler
                    }, new Player[] {
                        draft.TeamB.AdCarry,
                        draft.TeamB.Support
                    }, 50, 0.5);
                    break;
                
                default:
                    // Sem Gank
                    break;
            }
            
            switch (gankB)
            {
                case Position.TopLaner:
                    addFigth(new Player[] {
                        draft.TeamB.TopLaner,
                        draft.TeamB.Jungler
                    }, new Player[] {
                        draft.TeamA.TopLaner
                    }, 50, 0.5);
                    break;
                case Position.MidLaner:
                    addFigth(new Player[] {
                        draft.TeamB.MidLaner,
                        draft.TeamB.Jungler
                    }, new Player[] {
                        draft.TeamA.MidLaner
                    }, 50, 0.5);
                    break;
                case Position.Support:
                case Position.AdCarry:
                    addFigth(new Player[] {
                        draft.TeamB.AdCarry,
                        draft.TeamB.Support,
                        draft.TeamB.Jungler
                    }, new Player[] {
                        draft.TeamA.AdCarry,
                        draft.TeamA.Support
                    }, 50, 0.5);
                    break;
                
                default:
                    // Sem Gank
                    break;
            }
        }

    }

    private void addFigth(
        IEnumerable<Player> teamA,
        IEnumerable<Player> teamB,
        int diff = 0,
        double defVantage = 1.0)
    {
        var members = teamA.Concat(teamB).Aggregate("", (s, p) => s + p.Nickname + ", ");
        MessageQueue.Enqueue($"{members.Substring(0, members.Length - 2)} estão lutando.");

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
        // Range (-2000 - 2000)
        double commit = 
            + Math.Max(Math.Min(diff, 50), -50) 
            + Math.Max(Math.Min(20 * goldDiff, 200), -200)
            + (1f - teamParam) * teamA.Average(x => x.MechanicSkill)
            + teamParam * teamA.Average(x => x.TeamFigth)
            + 10 * teamA.Count(x => nextFlash[x] < Time)
            - (1f - teamParam) * teamB.Average(x => x.MechanicSkill)
            - teamParam * teamB.Average(x => x.TeamFigth)
            - 10 * teamB.Count(x => nextFlash[x] < Time);
        // Considerando vantagem defensiva
        commit *= 5 * defVantage;
        
        // Representa a percepção do time que está perdendo a luta do seu estado
        double intent = 0;
        if (commit > 0)
        {
            var teamControl = 0.25 + 0.25 * Random.Shared.NextSingle() +
                teamB.MaxBy(x => x.Leadership).GameVision / 100.0 / 5 + 
                teamB.MaxBy(x => x.Leadership).Leadership / 100.0 / 10 + 
                teamB.Average(x => x.GameVision) / 100.0 / 5;
            intent = .4 * commit + .6 * teamControl * commit;
        }
        else
        {
            var teamControl = 0.25 + 0.25 * Random.Shared.NextSingle() +
                teamA.MaxBy(x => x.Leadership).GameVision / 100.0 / 5 + 
                teamA.MaxBy(x => x.Leadership).Leadership / 100.0 / 10 + 
                teamA.Average(x => x.GameVision) / 100.0 / 5;
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
                        addKill(y);
                        MessageQueue.Enqueue(
                            MessageSystem.KillMessage(y, champs[y], x, champs[x])
                        );
                        ran = float.MaxValue;
                    }
                    else
                    {
                        addAssist(y);
                    }
                }
            }
        }

        foreach (var x in winTeam)
        {
            if (life[x] < 1)
            {
                addDeath(x);

                var rot = winTeam.Sum(x => gold[x]);
                var ran = Random.Shared.NextSingle() * rot;
                foreach (var y in loseTeam)
                {
                    ran -= gold[y];
                    if (ran < 0)
                    {
                        addKill(y);
                        MessageQueue.Enqueue(
                            MessageSystem.KillMessage(y, champs[y], x, champs[x])
                        );
                        ran = float.MaxValue;
                    }
                    else
                    {
                        addAssist(y);
                    }
                }
            }
        }
    }

    private void addDeath(Player player)
    {
        deaths[player]++;
        respawn[player] = Time + 4 + Time / 60;
    }

    private void addKill(Player player)
    {
        kills[player]++;
        gold[player] += 0.3f;
    }

    private void addAssist(Player player)
    {
        assits[player]++;
        gold[player] += 0.05f;
    }
}