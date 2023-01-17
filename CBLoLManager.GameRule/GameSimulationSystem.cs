using System;

namespace CBLoLManager.GameRule;

using Model;

public class GameSimulationSystem
{
    private DraftResult draft;
    private GameEventSystem evSys;
    
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
    }

    public void NextStep()
    {
        if (Time == 0)
        {
            Time = 90;
            return;
        }

        var timeStep = Random.Shared.Next(60, 121);

        // Lane Phase
        if (Time < 300)
        {
            var duration = Time + timeStep > 300 ? 300 - Time : timeStep;
            TeamAGold += 20f * calc(pA.LanePhase) * duration / 60 / 1000;
            TeamBGold += 20f * calc(pB.LanePhase) * duration / 60 / 1000;
        }


        Time += timeStep;
    }

    private float calc(int value)
    {
        float force = value + 20 * Random.Shared.NextSingle() - 10;
        return force;
    }
}