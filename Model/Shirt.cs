using System;

namespace CBLoLManager.Model;

[Serializable]
public class Shirt
{
    public int BodyStyle { get; set; }
    public int BodyColor { get; set; }

    public int CollarStyle { get; set; }
    public int CollarColor { get; set; }

    public int SleeveStyle { get; set; }
    public int SleeveColor { get; set; }
    
    public int HemStyle { get; set; }
    public int HemColor { get; set; }
}