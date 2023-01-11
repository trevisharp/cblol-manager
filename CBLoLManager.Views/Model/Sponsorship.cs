using System;

namespace CBLoLManager.Model;

[Serializable]
public class Sponsorship
{
    public Sponsor Sponsor { get; set; }
    public int Duration { get; set; }
    public int Start { get; set; }
    public float Value { get; set; }
}