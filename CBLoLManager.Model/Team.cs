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
    public float Popularity { get; set; }
    public List<TeamHistory> History { get; set; } = new List<TeamHistory>();

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

    public void Remove(Player player)
    {
        if (player.Role == Position.TopLaner)
        {
            if (player.Nickname == this.TopLaner.Nickname)
                this.TopLaner = null;
            
            return;
        }
        
        if (player.Role == Position.Jungler)
        {
            if (player.Nickname == this.Jungler.Nickname)
                this.Jungler = null;
            
            return;
        }
        
        if (player.Role == Position.MidLaner)
        {
            if (player.Nickname == this.MidLaner.Nickname)
                this.MidLaner = null;
            
            return;
        }
        
        if (player.Role == Position.Support)
        {
            if (player.Nickname == this.Support.Nickname)
                this.Support = null;
            
            return;
        }
        
        if (player.Role == Position.AdCarry)
        {
            if (player.Nickname == this.AdCarry.Nickname)
                this.AdCarry = null;
            
            return;
        }
    }

    public List<Player> GetAll()
    {
        var list = new List<Player>();

        if (TopLaner != null)
            list.Add(TopLaner);

        if (Jungler != null)
            list.Add(Jungler);

        if (MidLaner != null)
            list.Add(MidLaner);

        if (AdCarry != null)
            list.Add(AdCarry);

        if (Support != null)
            list.Add(Support);

        return list;
    }
}