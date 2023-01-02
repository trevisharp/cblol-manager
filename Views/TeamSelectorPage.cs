using System;
using System.Linq;
using System.Drawing;

namespace CBLoLManager.Views;

using Model;

public class TeamSelectorPage : BaseView
{
    PointF cursor = PointF.Empty;
    bool down = false;
    Organization selected = null;

    int frame = 0;
    protected override void draw(Bitmap bmp, Graphics g)
    {
        float dx = bmp.Width * .05f,
              size = bmp.Width * .14f;

        float x = dx, 
              y = bmp.Height * .20f;
        
        int count = 0;
        Font font = new Font(FontFamily.GenericMonospace, 12f);
        Font title = new Font(FontFamily.GenericMonospace, 30f);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        g.DrawString("Escolha seu time:", title, Brushes.White, 
            new RectangleF(0f, 0f, bmp.Width, y), format);

        foreach (var org in Organizations.All
            .OrderBy(o => o.Name))
        {
            var orgrect = new RectangleF(x, y, size, size);
            if (orgrect.Contains(cursor))
            {
                var brush = new SolidBrush(
                    Color.FromArgb(255 - 32, 255 - 42, 255 - 68)
                );
                rect(brush, () => orgrect, "i" + org.Name + "back");
                img(x + 5, y + 5, size - 10, size - 10, 
                    () => Bitmap.FromFile("Img/" + org.Photo) as Bitmap, "i" + org.Name);
            }
            else
            {
                var brush = new SolidBrush(
                    Color.FromArgb(32, 42, 68)
                );
                rect(brush, () => orgrect, org.Name + "back");
                img(x + 30, y + 30, size - 60, size - 60, 
                    () => Bitmap.FromFile("Img/" + org.Photo) as Bitmap, org.Name);
            }
            g.DrawString(org.Name, font, Brushes.White,
                new RectangleF(x, y + size + 5f, size, 20f), format);

            if (down)
                selected = org;

            if (!down && selected != null)
            {
                if (OnSelect != null)
                    OnSelect(selected);
            }

            x += size + dx;
            count++;
            if (count == 5)
            {
                x = dx;
                y += size + dx;
            }
        }

        if (!down)
            selected = null;

        frame++;
        if (frame < 40)
        {
            var fadeColor = Color.FromArgb(240 - frame * 6, 0, 0, 0);
            var fadeBrush = new SolidBrush(fadeColor);
            g.FillRectangle(fadeBrush, new Rectangle(0, 0, bmp.Width, bmp.Height));
        }
    }

    public override void MoseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;   
    }

    public event Action<Organization> OnSelect;
}