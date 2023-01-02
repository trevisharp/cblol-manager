using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace CBLoLManager.Model;

[Serializable]
public class Champions : IEnumerable<Champion>
{
    #region Singleton

    private static Champions current = null;
    public static Champions All
    {
        get
        {
            if (current == null)
                current = Load();
            return current;
        }
    }
    private static Champions Load()
    {
        if (!File.Exists("Data/champions.dat"))
            return new Champions();
        
        var file = File.Open("Data/champions.dat", FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        Champions champions = formatter.Deserialize(file) as Champions;
        file.Close();
        return champions;
    }
    private Champions() { }

    #endregion

    private List<Champion> champions = new List<Champion>();
    
    public void Save()
    {
        var file = File.Open("Data/champions.dat", FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, this);
        file.Close();
    }

    public void Add(Champion champion)
    {
        this.champions.Add(champion);
        this.Save();
    }

    public void Remove(Champion champion)
    {
        this.champions.Remove(champion);
        this.Save();
    }

    public IEnumerator<Champion> GetEnumerator()
        => champions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}