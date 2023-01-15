using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CBLoLManager.Util;

public static class Serializer
{
    public static void Save(string path, object obj)
    {
        var file = File.Open(path, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, obj);
        file.Close();
    }

    public static T Load<T>(string path)
        where T : class, new()
    {
        if (!File.Exists(path))
            return new T();
        
        var file = File.Open(path, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        T obj = formatter.Deserialize(file) as T;
        file.Close();

        return obj;
    }
}