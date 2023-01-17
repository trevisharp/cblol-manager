using System.Linq;

namespace CBLoLManager.Util;

public static class Formatter
{
    public static string FormatMoney(float number)
    {
        bool negative = false;
        if (number < 0)
        {
            negative = true;
            number *= -1;
        }

        int value = (int)number * 100;
        string result = $",{(value % 100).ToString("00")} R$";
        value /= 100;
        if (value < 0)
            value *= -1;

        result = $"{(value % 1000).ToString("000")}" + result;
        value /= 1000;

        while (value > 999)
        {
            result = $"{(value % 1000).ToString("000")}'" + result;
            value /= 1000;
        }
        
        result = $"{(value % 1000).ToString("###")}'" + result;
        value /= 1000;

        if (negative)
            result = "-" + result;

        return result;
    }

    public static string FormatPlayer(
        string nickName,
        string name
    )
    {
        var result = string.Empty;
        var parts = name.Split(' ');

        return string.Concat(
            parts.Skip(1)
                .Prepend($"'{nickName}'")
                .Prepend(parts[0])
                .Aggregate((n, s) => n + " " + s)
        );
    }
}