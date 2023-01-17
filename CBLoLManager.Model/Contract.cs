using System;

namespace CBLoLManager.Model;

[Serializable]
public class Contract
{
    public Team Team { get; set; }
    public Player Player { get; set; }
    public bool Accepted { get; set; } = false;
    public bool Closed { get; set; } = false;
    public float Wage { get; set; }
    public float RescissionFee { get; set; }
    public float End { get; set; }
}