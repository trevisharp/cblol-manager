namespace CBLoLManager.Model;

public class Propose
{
    public Player Player { get; set; }
    public Team Team { get; set; }
    public float Wage { get; set; }
    public float Time { get; set; }
    public float RescissionFee { get; set; }
    public int Round { get; set; }
}