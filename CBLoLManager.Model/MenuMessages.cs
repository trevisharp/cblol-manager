using System;
using System.Linq;

namespace CBLoLManager.Model;

[Serializable]
public class MenuMessages : DataCollection<MenuMessages, MenuMessage>
{
    public static MenuMessage GetRandom()
    {
        var all = MenuMessages.All;
        var wei = all.Sum(m => m.Weigth);
        var rand = Random.Shared.Next(wei);

        foreach (var message in all)
        {
            if (rand < message.Weigth)
                return message;
            rand -= message.Weigth;
        }
        return null;
    }
}