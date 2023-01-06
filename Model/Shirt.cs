using System;
using System.Drawing;

namespace CBLoLManager.Model;

[Serializable]
public class Shirt
{
    public bool Alternative { get; set; }

    public int BodyStyle { get; set; }
    public int BodyColor { get; set; }

    public int CollarStyle { get; set; }
    public int CollarColor { get; set; }

    public int SleeveStyle { get; set; }
    public int SleeveColor { get; set; }
    
    public int HemStyle { get; set; }
    public int HemColor { get; set; }

    public int Color { get; set; }

    public Image Photo { get; set; }
}