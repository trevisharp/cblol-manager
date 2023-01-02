using System;

namespace CBLoLManager.GameRule;

using Model;

[Serializable]
public class Game
{
    private static Game crr = new Game();
    public static Game Current => crr;
    private Game() { }

    public Team Team { get; set;}

    public static void New()
        => crr = new Game();
    
    public static void Save()
    {

    }

    public static void Load()
    {

    }
}