using System;

namespace CBLoLManager.Model;

[Serializable]
public class Sponsor
{
    public string Name { get; set; }
    public string Photo { get; set; }
    public bool isMain { get; set; }
    public int Strong { get; set; }
}