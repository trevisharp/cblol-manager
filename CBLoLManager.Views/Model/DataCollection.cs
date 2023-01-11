using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace CBLoLManager.Model;

[Serializable]
public abstract class DataCollection<T, E> : IEnumerable<E>
    where T : DataCollection<T, E>, new()
{
    #region Singleton

    private static readonly string path;
    private static DataCollection<T, E> current = null;
    public static DataCollection<T, E> All
    {
        get
        {
            if (current == null)
                current = Load();
            return current;
        }
    }
    private static DataCollection<T, E> Load()
    {
        if (!File.Exists(path))
            return new T();
        
        var file = File.Open(path, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        T coll = formatter.Deserialize(file) as T;
        file.Close();
        return coll;
    }
    static DataCollection()
    {
        path = $"Data/{typeof(E).Name.ToLower()}.dat";
    }

    #endregion
    
    public void Add(E element)
    {
        this.elements.Add(element);
        this.Save();
    }

    public void Remove(E sponsor)
    {
        this.elements.Remove(sponsor);
        this.Save();
    }

    protected List<E> elements = new List<E>();

    protected void Save()
    {
        var file = File.Open(path, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, this);
        file.Close();
    }

    public IEnumerator<E> GetEnumerator()
        => elements.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}