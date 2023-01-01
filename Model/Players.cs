using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace CBLoLManager.Model;

[Serializable]
public class Players : IEnumerable<Player>
{
    #region Singleton

    private static Players current = null;
    public static Players All
    {
        get
        {
            if (current == null)
                current = Load();
            return current;
        }
    }
    private static Players Load()
    {
        if (!File.Exists("Data/players.dat"))
            return new Players();
        
        var file = File.Open("Data/players.dat", FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        Players players = formatter.Deserialize(file) as Players;
        file.Close();
        return players;
    }
    private Players() { }

    #endregion

    private List<Player> players = new List<Player>();
    private void Save()
    {
        var file = File.Open("Data/players.dat", FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, this);
        file.Close();
    }

    public void Add(Player player)
    {
        this.players.Add(player);
        this.Save();
    }

    public void Remove(Player player)
    {
        this.players.Remove(player);
        this.Save();
    }

    public IEnumerator<Player> GetEnumerator()
        => players.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}