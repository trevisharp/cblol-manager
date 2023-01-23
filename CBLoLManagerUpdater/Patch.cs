public class Patch
{
    public int Version { get; set; }
    public List<string> Modifications { get; set; }

    public static Patch Open(string folder)
    {
        FileStream file = null;
        if (File.Exists(folder + "/release.info"))
            file = File.Open(folder + "/release.info", FileMode.Open);
        else if (File.Exists(folder + "/patch"))
            file = File.Open(folder + "/patch", FileMode.Open);
        else if (File.Exists(folder + "/patch.info"))
            file = File.Open(folder + "/patch.info", FileMode.Open);
        
        if (file == null)
            return null;

        Patch patch = new Patch();
        StreamReader reader = new StreamReader(file);

        patch.Version = reader.ReadLine().ToVersion();
        
        while (!reader.EndOfStream)
            patch.Modifications.Add(reader.ReadLine());
        
        return patch;
    }
}