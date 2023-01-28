using static System.Random;

namespace CBLoLManager.GameRule;

using Model;

public static class MessageSystem
{
    public static string KillMessage(
        Player killer, Champion champKiller,
        Player killed, Champion champKilled)
    {
        const int N = 3;
        switch (Shared.Next() % N)
        {
            default:
            case 0:
                return $"{killed.Nickname} morreu nas mãos de {killer.Nickname}";
            
            case 1:
                return $"{killed.Nickname} não tankou o {champKiller.Name} do {killer.Nickname}";

            case 2:
                return $"{champKiller.Name} finalizou o {champKilled.Name}";
        }
    }
}