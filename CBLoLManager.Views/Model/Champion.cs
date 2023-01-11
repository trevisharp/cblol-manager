using System;

namespace CBLoLManager.Model;

[Serializable]
public class Champion
{
    public string Name { get; set; }
    public Position Role { get; set; }
    public Scaling Scaling { get; set; }
    public string Photo { get; set; }

    public bool AD { get; set; }
    public int Range { get; set;}
    public int Defence { get; set; }
    public int Damage { get; set; }
    public int Mobility { get; set; }
    public int Control { get; set; }
    public int Support { get; set; }
}