public class Release
{
    public string? Name { get; set; }
    public string? Url { get; set; }
    public int Id { get; set; }

    public int Version => Name?.ToVersion() ?? 0;
}