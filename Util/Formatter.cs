namespace CBLoLManager.Util;

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
}