using System;
using System.Linq;
using System.Drawing;

namespace CBLoLManager.Views;

using Model;
using GameRule;

public class AdministrativeView : BaseView
{
    OptionsView options = null;
    PointF cursor = PointF.Empty;
    bool down = false;

    protected override void draw(Bitmap bmp, Graphics g)
    {
        options.MouseMove(cursor, down);
        options.Draw(bmp, g);
    }

    public override void MouseMove(PointF cursor, bool down)
    {
        this.cursor = cursor;
        this.down = down;
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);

        options = new OptionsView("Sair");
        options.OnOptionClick += delegate
        {
            Exit();
        };
    }

    public event Action Exit;
}