using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CBLoLManager.Util;

public static class Serializer
{
    public static void Save(string path, object obj)
    {
        if (!Directory.Exists("Data"))
            Directory.CreateDirectory("Data");

        var file = File.Open(path, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();

        #pragma warning disable
        formatter.Serialize(file, obj);
        #pragma warning restore

        file.Close();
    }

    public static T Load<T>(string path)
        where T : class, new()
    {
        if (!File.Exists(path))
            return new T();
        
        var file = File.Open(path, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();

        #pragma warning disable
        T obj = formatter.Deserialize(file) as T;
        #pragma warning restore
        
        file.Close();

        return obj;
    }
}