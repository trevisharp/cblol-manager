using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace CBLoLManager.Views;

public abstract class BaseView
{
    public PointF? Location { get; set; }

    public abstract void draw(Bitmap bmp, Graphics g);

    public void Draw(Bitmap bmp, Graphics g)
    {
        this._bmp = bmp;
        this._g = g;
        this.draw(this._bmp, this._g);
    }

    private Graphics _g;
    private Bitmap _bmp;
    private Dictionary<string, PointF[]> structures = null;

    protected PointF[] register(string code, PointF[] structure)
    {
        if (code == "")
            return structure;
            
        if (structures == null)
            structures = new Dictionary<string, PointF[]>();

        structures.Add(code, structure);

        return structure;
    }

    protected PointF[] loadOrStore(string code, Func<PointF[]> create)
    {
        if (structures != null && structures.ContainsKey(code))
            return this.structures[code];
        return register(code, create());
    }

    protected void poly(Brush brush,
        Func<int, PointF> sel, int len, string code = "")
    {
        PointF[] pts = loadOrStore(code, () =>
            Enumerable.Range(0, len)
                .Select(sel)
                .ToArray()
        );
        _g?.FillPolygon(brush, pts);
    }
    
    protected void poly(Pen pen,
        Func<int, PointF> sel, int len, string code = "")
    {
        PointF[] pts = loadOrStore(code, () =>
            Enumerable.Range(0, len)
                .Select(sel)
                .ToArray()
        );
        _g?.DrawPolygon(pen, pts);
    }
}