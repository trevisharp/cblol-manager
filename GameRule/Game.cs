using System;

namespace CBLoLManager.GameRule;

using System.Collections.Generic;
using Model;

[Serializable]
public class Game
{
    private static Game crr = new Game();
    public static Game Current => crr;
    private Game() { }

    public Team Team { get; set;}
    public List<Team> Others { get; set; } = new List<Team>();
    
    public List<Player> FreeAgent { get; set; } = new List<Player>();
    public List<Player> EndContract { get; set; } = new List<Player>();
    public List<Player> SeeingProposes { get; set; } = new List<Player>();

    public static void New()
        => crr = new Game();
    
    public static void Save()
    {

    }

    public static void Load()
    {

    }
}