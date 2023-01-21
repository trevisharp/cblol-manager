using System;
using System.Drawing;

namespace CBLoLManager.Views;

using Model;

public class MainScreen : BaseView
{
    RectangleF logoRect;
    Image logo = null;

    ButtonView btPlay = null;
    ButtonView btLoad = null;

    string message = string.Empty;

    int frame = 0;

    protected override void draw(Bitmap bmp, Graphics g)
    {
        frame++;
        
        int stepSize = 40;
        int stepVelocity = 2;
        int step = (stepVelocity * frame / stepSize) % 2;
        int dx = (2 * step - 1) * (stepVelocity * frame % stepSize) 
            + ((2 * step - 1) < 0 ? stepSize : 0);

        Font font = new Font(FontFamily.GenericMonospace, 20f);

        g.Clear(Color.Black);
        g.DrawImage(logo, logoRect, 
            new Rectangle(0, 0, logo.Width, logo.Height), 
            GraphicsUnit.Pixel);
        g.DrawString(message, font,
            Brushes.White, 
            logoRect.X + logoRect.Width / 2 - 200 + dx,
            logoRect.Y + logoRect.Height - 50);
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
        btPlay.Rect = new RectangleF(wid * .3f, hei * .6f, wid * .4f, 80);

        btPlay.OnClick += delegate
        {
            OnPlay();
        };

        btLoad = new ButtonView();
        btLoad.Label = "Carregar";
        btLoad.Color = Brushes.Black;
        btLoad.SelectedColor = Brushes.White;
        btLoad.Rect = new RectangleF(wid * .3f, hei * .6f + 100, wid * .4f, 80);

        btLoad.OnClick += delegate
        {
            OnLoad();
        };

        logo = Image.FromFile("Img/logo2.png");
        logoRect = new RectangleF(
            wid * .25f, 40, wid * .5f, hei * .6f - 80
        );

        message = MenuMessages
            .GetRandom()?
            .Message ??
            "Tivemos um bug no sistemas de mensagens aleatórias...";
    }

    public event Action OnPlay;
    public event Action OnLoad;
}