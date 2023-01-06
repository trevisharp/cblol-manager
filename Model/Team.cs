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
    public Sponsorship MainSponsorship { get; set; }
    public Sponsorship SecondSponsorship { get; set; }
    public Shirt Shirt { get; set; }
    public float Money { get; set; }

    public void Add(Player player)
    {
        if (player.Role == Position.TopLaner)
            this.TopLaner = player;
            
        if (player.Role == Position.Jungler)
            this.Jungler = player;
            
        if (player.Role == Position.MidLaner)
            this.MidLaner = player;
            
        if (player.Role == Position.AdCarry)
            this.AdCarry = player;
            
        if (player.Role == Position.Support)
            this.Support = player;
    }

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