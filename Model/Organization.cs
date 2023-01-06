using System;
using System.Drawing;

namespace CBLoLManager.Model;

[Serializable]
public class Organization
{
    public string Name { get; set; }
    public string Photo { get; set; }
    public Color MainColor { get; set; }
    public Color SecondColor { get; set; }
    public Color ThirdColor { get; set; }
}