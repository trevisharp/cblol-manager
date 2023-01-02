using System;

namespace CBLoLManager.Model;

[Serializable]
public class Champion
{
    public string Name { get; set; }
    public Position Role { get; set; }
    public string Photo { get; set; }
    public Style Style { get; set; }
}