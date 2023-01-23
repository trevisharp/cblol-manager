using System.Linq;

public static class VersionExtension
{
    public static int ToVersion(this string text)
    {
        if (text is null)
            return 0;
        
        if (text.Contains(' ') && text.Contains('v'))
            text = string.Concat(
                text.SkipWhile(c => c != ' ')
                    .SkipWhile(c => c != 'v')
                    .SkipWhile(c => c < '0' || c > '9')
            );
        else if (text.Contains(' '))
            text = string.Concat(
                text.SkipWhile(c => c != ' ')
                    .SkipWhile(c => c < '0' || c > '9')
            );
        else if (text.Contains('v'))
            text = string.Concat(
                text.SkipWhile(c => c != 'v')
                    .SkipWhile(c => c < '0' || c > '9')
            );
        else 
            text = string.Concat(
                text.SkipWhile(c => c != 'v')
                .SkipWhile(c => c < '0' || c > '9')
            );
            
        if (text.Length == 0)
            return 0;
        
        try
        {
            return text
                .Split('.')
                .Select(s => int.Parse(s))
                .Zip(new int[] {100 * 100, 100, 1 })
                .Select(t => t.First * t.Second)
                .Sum();
        }
        catch
        {
            return 0;
        }
    }
}