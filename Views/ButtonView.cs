using System;
using System.Drawing;

namespace CBLoLManager.Views;

public class ButtonView : BaseView
{
    public string Label { get; set; }
    public Brush Color { get; set; }
    public Brush SelectedColor { get; set; }
    public RectangleF Rect { get; set; }

    private PointF cursor = PointF.Empty;
    private bool down = false;
    private bool isDown = false;

    protected override void draw(Bitmap bmp, Graphics g)
    {
        Brush back = Color;
        Brush fore = SelectedColor;

        var font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        if (Rect.Contains(cursor))
        {
            back = SelectedColor;
            fore = Color;

            if (down)
                isDown = true;
            
            if (!down && isDown && OnClick != null)
            {
                isDown = false;
                OnClick();
            }
        }
        else isDown = false;

        g.FillRectangle(back, Rect);
        g.DrawString(Label, font, fore, Rect, format);
    }

    public override void MouseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;
    }

    public event Action OnClick;
}