using System;
using System.Drawing;

namespace CBLoLManager.Views;

using Model;

public class MatchView : BaseView
{
    DraftResult draft = null;
    Image game = null;

    public MatchView(DraftResult draft)
    {
        this.draft = draft;
    }

    protected override void draw(Bitmap bmp, Graphics g)
    {
        g.DrawImage(game, new Rectangle(0, 0, bmp.Width, bmp.Height),
            new RectangleF(0, 0, game.Width, game.Height),
            GraphicsUnit.Pixel);
    }

    public override void Load(Bitmap bmp, Graphics g)
    {
        g.Clear(Color.Black);

        game = Bitmap.FromFile("Img/game.png");
    }

    public event Action Exit;
}