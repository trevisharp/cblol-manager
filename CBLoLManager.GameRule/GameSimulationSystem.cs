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

    private bool[] towersA = new bool[]
    {
        true, true, true, 
        true, true, true, 
        true, true, true,
        true, true, true,
        true, true, true
    };
    private bool[] towersB = new bool[]
    {
        true, true, true, 
        true, true, true, 
        true, true, true,
        true, true, true,
        true, true, true
    };
    
    public IEnumerable<bool> TowersUpA => towersA;
    public IEnumerable<bool> TowersUpB => towersB;

    private int aMagicDamage = 0;
    private int aPhysicalDamage = 0;
    private int aDefense = 0;

    private Player pA = null;
    private Player pB = null;

    private int towerPointA = 0;
    private int towerPointB = 0;

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
    Dictionary<Player, int> nextGank = new Dictionary<Player, int>();

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
            respawn.Add(x, int.MinValue);
        }
        
        nextTp[draft.TeamA.TopLaner] = 0;
        nextTp[draft.TeamA.MidLaner] = 0;
        nextTp[draft.TeamB.TopLaner] = 0;
        nextTp[draft.TeamB.MidLaner] = 0;

        nextGank[draft.TeamA.Jungler] = 0;
        nextGank[draft.TeamB.Jungler] = 0;
    }

    public string GetFrag(Player p)
        => $"{kills[p]}/{deaths[p]}/{assits[p]}";
    
    public float GetLife(Player p)
    {
        if (respawn[p] > Time)
            return 0f;
        return life[p] / 100f;
    }
    
    public float GetGold(Player p)
        => gold[p];

    public bool IsAlive(Player p)
        => respawn[p] == int.MinValue || respawn[p] < Time;

    public void NextStep()
    {
        if (Time < 90)
        {
            Time += 10;
            return;
        }

        var timeStep = 10;

        if (Time < 500)
            lanePhase(timeStep);
        else if (Time < 1200)
            midPhase(timeStep);
        else endPhase(timeStep);

        Time += timeStep;

        testRespawn();
        towerDestruction();
    }

    private void testRespawn()
    {
        var players = draft.TeamA.GetAll()
            .Concat(draft.TeamB.GetAll());
        foreach (var x in players)
        {
            if (respawn[x] == int.MinValue)
                continue;
            
            if (Time > respawn[x])
            {
                MessageQueue.Enqueue($"{x.Nickname} renasceu.");
                respawn[x] = int.MinValue;
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
        }
        

        if (Random.Shared.Next(90) < timeStep)
            lanePhaseJgEvent();
        else if (Random.Shared.Next(60) < timeStep)
            laneFigthEvent(timeStep);
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

        if (Random.Shared.Next(90) < timeStep)
            laneFigthEvent(timeStep);
        else if (Random.Shared.Next(120) < timeStep)
            teamFigth(2f);
    }

    private void endPhase(int timeStep)
    {
        var players = draft.TeamA.GetAll()
            .Concat(draft.TeamB.GetAll());
        
        foreach (var x in players)
        {
            var goldGen = 10 * gold[x] + 40 * Random.Shared.NextSingle();
            gold[x] += 4f * goldGen * timeStep / 60 / 1000;
        }

        if (Random.Shared.Next(120) < timeStep)
            teamFigth(5f);
    }

    private void teamFigth(float intensity)
    {
        var alive = draft.TeamA.GetAll()
            .Concat(draft.TeamB.GetAll())
            .Where(p => IsAlive(p));
        if (alive.Count() < 10)
            return;
        MessageQueue.Enqueue("Uma Team Figth começou");
        addFigth(draft.TeamA.GetAll(),
            draft.TeamB.GetAll(), 0, intensity);
    }

    private void laneFigthEvent(int timeStep)
    {
        laneFigthEvent(draft.TeamA.TopLaner, draft.TeamB.TopLaner, timeStep);
        laneFigthEvent(draft.TeamA.MidLaner, draft.TeamB.MidLaner, timeStep);
        laneFigthEvent(
        new Player[] 
        {
            draft.TeamA.AdCarry,
            draft.TeamA.Support,
        },
        new Player[] 
        {
            draft.TeamB.AdCarry,
            draft.TeamB.Support,
        }, timeStep);
    }

    private void towerDestruction()
    {
        towerPointA += 5;
        towerPointB += 5;

        if (TeamAGold > TeamBGold)
        {
            var diff = TeamAGold - TeamBGold;
            towerPointA += (int)(10 * diff);
        }
        else if (TeamBGold > TeamAGold)
        {
            var diff = TeamBGold - TeamAGold;
            towerPointB += (int)(10 * diff);
        }

        if (towerPointA > 400)
        {
            TeamATowers++;
            towerPointA = 0;
            MessageQueue.Enqueue($"{draft.TeamA.Organization.Name} derrubou uma torre");

            int index = Random.Shared.Next(3);
            while (index < 12 && !towersB[index])
                index += 3;
            
            while (!towersB[index])
                index++;

            towersB[index] = false;

            if (index == 14)
                finalizeGame(true);
        }

        if (towerPointB > 400)
        {
            TeamBTowers++;
            towerPointB = 0;
            MessageQueue.Enqueue($"{draft.TeamB.Organization.Name} derrubou uma torre");

            int index = Random.Shared.Next(3);
            while (index < 12 && !towersA[index])
                index += 3;
                
            while (!towersA[index])
                index++;

            towersA[index] = false;

            if (index == 14)
                finalizeGame(false);
        }
    }

    private void finalizeGame(bool teamAWin)
    {
        if (teamAWin)
        {
            MessageQueue.Enqueue($"{draft.TeamA.Organization.Name} venceu");
        }
        else
        {
            MessageQueue.Enqueue($"{draft.TeamB.Organization.Name} venceu");
        }
    }

    private void laneFigthEvent(
        Player p, Player q, int timeStep
    ) 
    => laneFigthEvent(new Player[] { p }, new Player[] { q }, timeStep);

    private void laneFigthEvent(
        IEnumerable<Player> pa,
        IEnumerable<Player> pb,
        int timeStep)
    {
        addFigth(pa, pb, 0);

        foreach (var x in pa.Concat(pb))
        {
            if (respawn[x] > Time)
                continue;
            if (life[x] < 25 + 10 * Random.Shared.NextSingle())
            {
                var goldGen = x.LanePhase + 20 * Random.Shared.NextSingle() - 10;

                MessageQueue.Enqueue($"{x.Nickname} voltou para base.");
                life[x] = 100;
                gold[x] -= 0.5f * goldGen * timeStep / 60 / 1000;
            }
        }
    }

    private void lanePhaseJgEvent()
    {
        Position gankA = (Position)Random.Shared.Next(5);
        Position gankB = (Position)Random.Shared.Next(5);
        if (nextGank[draft.TeamA.Jungler] > Time)
            gankA = Position.Jungler;
        if (nextGank[draft.TeamB.Jungler] > Time)
            gankB = Position.Jungler;
        
        if (gankA != Position.Jungler)
            nextGank[draft.TeamA.Jungler] = Time + 40 + Random.Shared.Next(20);
        if (gankB != Position.Jungler)
            nextGank[draft.TeamB.Jungler] = Time + 40 + Random.Shared.Next(20);

        if (gankA == gankB)
        {
            switch (gankA)
            {
                case Position.TopLaner:
                    MessageQueue.Enqueue("Ambos os junglers gankaram o Top");
                    addFigth(new Player[] {
                        draft.TeamA.TopLaner,
                        draft.TeamA.Jungler
                    }, new Player[] {
                        draft.TeamB.TopLaner,
                        draft.TeamB.Jungler
                    }, 0, 0.75);
                    break;
                case Position.MidLaner:
                    MessageQueue.Enqueue("Ambos os junglers gankaram o Mid");
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
                    MessageQueue.Enqueue("Ambos os junglers gankaram o Bot");
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
                    MessageQueue.Enqueue(draft.TeamA.Jungler.Nickname +
                        " iniciou um gank no Top");
                    addFigth(new Player[] {
                        draft.TeamA.TopLaner,
                        draft.TeamA.Jungler
                    }, new Player[] {
                        draft.TeamB.TopLaner
                    }, 50, 0.5);
                    break;
                case Position.MidLaner:
                    MessageQueue.Enqueue(draft.TeamA.Jungler.Nickname +
                        " iniciou um gank no Mid");
                    addFigth(new Player[] {
                        draft.TeamA.MidLaner,
                        draft.TeamA.Jungler
                    }, new Player[] {
                        draft.TeamB.MidLaner
                    }, 50, 0.5);
                    break;
                case Position.Support:
                case Position.AdCarry:
                    MessageQueue.Enqueue(draft.TeamA.Jungler.Nickname +
                        " iniciou um gank no Bot");
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
                    MessageQueue.Enqueue(draft.TeamB.Jungler.Nickname +
                        " iniciou um gank no Top");
                    addFigth(new Player[] {
                        draft.TeamB.TopLaner,
                        draft.TeamB.Jungler
                    }, new Player[] {
                        draft.TeamA.TopLaner
                    }, 50, 0.5);
                    break;
                case Position.MidLaner:
                    MessageQueue.Enqueue(draft.TeamB.Jungler.Nickname +
                        " iniciou um gank no Mid");
                    addFigth(new Player[] {
                        draft.TeamB.MidLaner,
                        draft.TeamB.Jungler
                    }, new Player[] {
                        draft.TeamA.MidLaner
                    }, 50, 0.5);
                    break;
                case Position.Support:
                case Position.AdCarry:
                    MessageQueue.Enqueue(draft.TeamB.Jungler.Nickname +
                        " iniciou um gank no Bot");
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
        double defVantage = 1.0,
        int restartCount = 0)
    {
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
            result -= 3;
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
    
        var aliveA = teamA.Where(x => IsAlive(x));
        var aliveB = teamB.Where(x => IsAlive(x));
        if (aliveA.Count() > 2 && aliveB.Count() > 2 && restartCount < 5)
            addFigth(aliveA, aliveB, 0, defVantage, restartCount + 1);
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
        if (firstBlood)
        {
            MessageQueue.Enqueue("First Blood");
            firstBlood = false;
            gold[player] += 0.2f;
        }
    }

    private void addAssist(Player player)
    {
        assits[player]++;
        gold[player] += 0.05f;
    }
}