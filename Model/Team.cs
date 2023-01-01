namespace CBLoLManager.Model;

public class Team
{
    public string Name { get; set; }
    public string Photo { get; set; }

    public Player TopLaner { get; set; }
    public Player MidLaner { get; set; }
    public Player AdCarry { get; set; }
    public Player Jungler { get; set; }
    public Player Support { get; set; }
}