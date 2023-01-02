using System;
using System.Collections.Generic;

namespace CBLoLManager.Model;

[Serializable]
public class Team
{
    public Organization Organization { get; set; }
    public Player TopLaner { get; set; }
    public Player MidLaner { get; set; }
    public Player AdCarry { get; set; }
    public Player Jungler { get; set; }
    public Player Support { get; set; }
    public float Money { get; set; } = 100000;

    public List<Player> GetAll()
    {
        var list = new List<Player>();

        if (TopLaner != null)
            list.Add(TopLaner);

        if (MidLaner != null)
            list.Add(MidLaner);

        if (AdCarry != null)
            list.Add(AdCarry);

        if (Jungler != null)
            list.Add(Jungler);

        if (Support != null)
            list.Add(Support);

        return list;
    }
}