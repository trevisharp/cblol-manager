using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace CBLoLManager.Model;

[Serializable]
public class Organizations : IEnumerable<Organization>
{
    #region Singleton

    private static Organizations current = null;
    public static Organizations All
    {
        get
        {
            if (current == null)
                current = Load();
            return current;
        }
    }
    private static Organizations Load()
    {
        if (!File.Exists("Data/organizations.dat"))
            return new Organizations();
        
        var file = File.Open("Data/organizations.dat", FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        Organizations orgs = formatter.Deserialize(file) as Organizations;
        file.Close();
        return orgs;
    }
    private Organizations() { }

    #endregion

    private List<Organization> organizations = new List<Organization>();
    private void Save()
    {
        var file = File.Open("Data/organizations.dat", FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, this);
        file.Close();
    }

    public void Add(Organization org)
    {
        this.organizations.Add(org);
        this.Save();
    }

    public void Remove(Organization org)
    {
        this.organizations.Remove(org);
        this.Save();
    }

    public IEnumerator<Organization> GetEnumerator()
        => organizations.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}