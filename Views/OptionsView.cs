using System;
using System.Collections.Generic;
using System.Drawing;

namespace CBLoLManager.Views;

public class OptionsView : BaseView
{
    private List<string> options = null;
    private PointF cursor = PointF.Empty;
    private bool down = false;
    private string selected = string.Empty;

    public OptionsView(params string[] opts)
    {
        options = new List<string>(opts);
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        g.FillRectangle(Brushes.Navy, new RectangleF(
            0f, bmp.Height - 50f, bmp.Width, 50f
        ));

        var font = new Font(FontFamily.GenericMonospace, 10f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        float x = 0f;
        foreach (var opt in this.options)
        {
            var rect = new RectangleF(
                x + 10f, bmp.Height - 40f, 180f, 30f
            );
            if (rect.Contains(cursor))
            {
                g.FillRectangle(Brushes.White, rect);
                g.DrawString(opt, font, Brushes.Navy, rect, format);

                if (down)
                    selected = opt;
                
                if (!down && selected == opt && OnOptionClick != null)
                    OnOptionClick(selected);
            }
            else
            {
                g.DrawString(opt, font, Brushes.White, rect, format);
            }
        }
        if (!down)
            selected = string.Empty;
    }

    public override void MoseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;
    }

    public event Action<string> OnOptionClick;
}