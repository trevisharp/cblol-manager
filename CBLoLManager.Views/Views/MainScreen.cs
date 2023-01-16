using System;
using System.Drawing;

namespace CBLoLManager.Views;

public class MainScreen : BaseView
{
    ButtonView btPlay = null;
    ButtonView btLoad = null;

    protected override void draw(Bitmap bmp, Graphics g)
    {
        btPlay.Draw(bmp, g);
        btLoad.Draw(bmp, g);
    }

    public override void MouseMove(PointF cursor, bool down)
    {
        btPlay.MouseMove(cursor, down);
        btLoad.MouseMove(cursor, down);
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        float wid = bmp.Width;
        float hei = bmp.Height;

        btPlay = new ButtonView();
        btPlay.Label = "Jogar";
        btPlay.Color = Brushes.Black;
        btPlay.SelectedColor = Brushes.White;
        btPlay.Rect = new RectangleF(wid * .3f, hei * .4f, wid * .4f, 80);

        btPlay.OnClick += delegate
        {
            OnPlay();
        };

        btLoad = new ButtonView();
        btLoad.Label = "Carregar";
        btLoad.Color = Brushes.Black;
        btLoad.SelectedColor = Brushes.White;
        btLoad.Rect = new RectangleF(wid * .3f, hei * .4f + 100, wid * .4f, 80);

        btLoad.OnClick += delegate
        {
            OnLoad();
        };
    }

    public event Action OnPlay;
    public event Action OnLoad;
}