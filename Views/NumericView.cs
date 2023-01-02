using System.Drawing;

namespace CBLoLManager.Views;

using Util;

public class NumericView : BaseView
{
    public string Label { get; set; }
    public bool IsMoney { get; set; }
    public float Value { get; set; }
    public float Step { get; set; }
    public RectangleF Rect { get; set; }

    PointF cursor = PointF.Empty;
    bool down = false;
    int selected = 0;

    protected override void draw(Bitmap bmp, Graphics g)
    {
        var font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        g.DrawString(Label, font, Brushes.White,
            new RectangleF(Rect.X, Rect.Y, Rect.Width, 20f), format);
        var rect = new RectangleF(
            Rect.X + 40f, Rect.Y + 25f, Rect.Width - 80f, Rect.Height - 30f);
        g.FillRectangle(Brushes.Navy, rect);
        var text = IsMoney ? Formatter.FormatMoney(Value) : Value.ToString();
        g.DrawString(text, font, Brushes.White, rect, format);
        g.FillPolygon(Brushes.Navy, new PointF[]
        {
            new PointF(Rect.X + 30f, Rect.Y + 35f),
            new PointF(Rect.X + 5f, Rect.Y + Rect.Height / 2 + 25f / 2),
            new PointF(Rect.X + 30f, Rect.Y + Rect.Height - 10f)
        });
        g.FillPolygon(Brushes.Navy, new PointF[]
        {
            new PointF(Rect.X + Rect.Width - 30f, Rect.Y + 35f),
            new PointF(Rect.X + Rect.Width - 5f, Rect.Y + Rect.Height / 2 + 25f / 2),
            new PointF(Rect.X + Rect.Width - 30f, Rect.Y + Rect.Height - 10f)
        });

        if (Rect.Contains(cursor) && down)
        {
            if (cursor.X < Rect.X + 40f)
                selected = -1;
            else if (cursor.X > Rect.X + Rect.Width - 40f)
                selected = 1;
            else selected = 0;
        }
        else if (down)
            selected = 0;

        if (!down && selected != 0)
        {
            Value += selected * Step;
            selected = 0;
        }
    }

    public override void MouseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;   
    }
}