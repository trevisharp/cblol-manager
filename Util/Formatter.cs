using System.Linq;

namespace CBLoLManager.Util;

using Model;

public static class Formatter
{
    public static string FormatMoney(float number)
    {
        int value = (int)number * 100;
        string result = $",{(value % 100).ToString("00")} R$";
        value /= 100;

        result = $"{(value % 1000).ToString("000")}" + result;
        value /= 1000;

        while (value > 999)
        {
            result = $"{(value % 1000).ToString("000")}'" + result;
            value /= 1000;
        }
        
        result = $"{(value % 1000).ToString("###")}'" + result;
        value /= 1000;

        return result;
    }

    public static string FormatPlayer(Player player)
    {
        var result = string.Empty;

        var parts = player.Name.Split(' ');

        return string.Concat(
            parts.Skip(1)
                .Prepend($"'{player.Nickname}'")
                .Prepend(parts[0])
                .Aggregate((n, s) => n + " " + s)
        );
    }
}