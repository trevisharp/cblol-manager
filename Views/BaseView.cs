using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace CBLoLManager.Views;

public abstract class BaseView
{
    public PointF? Location { get; set; }

    public abstract void draw(Bitmap bmp, Graphics g);
    public virtual void MoseMove(PointF cursor, bool down) { }


    public void Draw(Bitmap bmp, Graphics g)
    {
        this._bmp = bmp;
        this._g = g;
        this.draw(this._bmp, this._g);
    }

    public virtual void Reset()
    {
        this._g = null;
        this._bmp = null;
        this.structures = null;
    }

    private Graphics _g;
    private Bitmap _bmp;
    private Dictionary<string, object> structures = null;

    protected object register(string code, object structure)
    {
        if (code == "")
            return structure;
            
        if (structures == null)
            structures = new Dictionary<string, object>();

        structures.Add(code, structure);

        return structure;
    }

    protected object loadOrStore(string code, Func<object> create)
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
        ) as PointF[];
        _g?.FillPolygon(brush, pts);
    }
    
    protected void poly(Pen pen,
        Func<int, PointF> sel, int len, string code = "")
    {
        PointF[] pts = loadOrStore(code, () =>
            Enumerable.Range(0, len)
                .Select(sel)
                .ToArray()
        ) as PointF[];
        _g?.DrawPolygon(pen, pts);
    }

    protected void lines(Pen pen,
        Func<PointF[]> sel, string code = "")
    {
        PointF[] pts = loadOrStore(code, sel) as PointF[];
        _g?.DrawLines(pen, pts);
    }
    

    protected void img(float x, float y, float height, Func<Bitmap> create, string code)
    {
        var bmp = loadOrStore(code, create) as Bitmap;
        _g.DrawImage(bmp, new RectangleF(x, y, height / bmp.Height * bmp.Width, height), 
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            GraphicsUnit.Pixel);
    }
}