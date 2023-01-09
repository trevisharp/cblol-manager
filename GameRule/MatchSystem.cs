using System;

namespace CBLoLManager.GameRule;

using Model;

public class MatchSystem
{
    private float winChance(float a, float b)
    {
        if (a > b)
          return winHigherChance(a, b);
        return 100f - winHigherChance(a, b);
    }
    
    private float winHigherChance(float a, float b)
    {
        float diff = a - b;
        float prob = 101 - 100 / (1 + MathF.Pow(1.25f, a);
        return prob > 100 ? 100 : prob;
    }
}
